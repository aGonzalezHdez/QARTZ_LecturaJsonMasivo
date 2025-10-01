/********************************************************************************************
Fecha de Creación : 2025-06-12
Usuario Crea: Franklin Pereda - Cubits 
Descripción General: Validar la asignación de la referencia
********************************************************************************************/

using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Services.MsTrazabilidad
{
    public class ValidarReferenciaService
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;        
        public ControldeSalidasRepository _salidasRepository;
        public ReferenciasRepository _referenciasRepository;
        public CatalogoDeUsuariosRepository _catalogoDeUsuariosRepository;
        public ControldeEventosRepository _controldeEventosRepository;

        public ValidarReferenciaService(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            _salidasRepository = new ControldeSalidasRepository(_configuration);
            _referenciasRepository = new ReferenciasRepository(_configuration);
            _catalogoDeUsuariosRepository = new CatalogoDeUsuariosRepository(_configuration);
            _controldeEventosRepository = new ControldeEventosRepository(_configuration);
        }

        public async Task<ResultadoAsignacion> ValidarReferencia(int idReferencia, int idUsuario)
        {
            ResultadoAsignacion result = null;
            bool cmdSinCambiosVisible = false;
            Referencias objReferencia = new Referencias();
            CatalogoDeUsuarios objUsuario = new CatalogoDeUsuarios();
            objUsuario = _catalogoDeUsuariosRepository.BuscarPorId(idUsuario);
            
            if(objUsuario==null)
            {
                throw new Exception("No se encontró un usuario para el ID: "+idUsuario);
            }

            objReferencia = _referenciasRepository.Buscar(idReferencia,objUsuario.IDDatosDeEmpresa);

            if (objReferencia == null)
            {
                throw new Exception("No se encontró una referencia para el ID: " + idReferencia);
            }
            
            int departamentoActual = 0;

            
            departamentoActual = await _controldeEventosRepository.BuscaDepartamentoActual(objReferencia.IDReferencia, objUsuario.IdOficina);

            if (departamentoActual == 0)
            {
                departamentoActual = objUsuario.IdDepartamento;
            }

            if (await _controldeEventosRepository.BuscaSiExisteIDCheckpoint(523, objReferencia.IDReferencia.ToString()))
            {
                if (departamentoActual == 24)
                {
                    cmdSinCambiosVisible = true;
                }
            }

            result = await AsignadaaDepartamento(objReferencia,objUsuario);           
            return result;
        }

        public async Task<ResultadoAsignacion> AsignadaaDepartamento(Referencias objRefe, CatalogoDeUsuarios GObjUsuario)
        {
            int result = 0;
            string mensaje = string.Empty;

            try
            {
                if (!NoValidada(objRefe.NumeroDeReferencia))
                {
                    return new ResultadoAsignacion { Result = 2, Mensaje = "Guía Validada" };
                }

                AsignaciondeGuias objAsig = new AsignaciondeGuias();
                AsignacionDeGuiasRepository objAsigD = new AsignacionDeGuiasRepository(_configuration);
                CatalogodeCheckPointsRepository objCheckD = new CatalogodeCheckPointsRepository(_configuration);

                objAsig = objAsigD.BuscarUltimoDepartamento(objRefe.IDReferencia);

                if (objAsig != null)
                {
                    if (objAsig.idDepartamento != GObjUsuario.IdDepartamento)
                    {
                        DataTable dtb = new DataTable();
                        UsuariosDisponiblesRepository UsuariosDisponiblesD = new UsuariosDisponiblesRepository(_configuration);

                        dtb = UsuariosDisponiblesD.DepartamentoAsignado(GObjUsuario.IdUsuario, objAsig.idDepartamento);

                        if (dtb.Rows.Count == 0)
                        {
                            CatalogoDepartamentosRepository objDepD = new CatalogoDepartamentosRepository(_configuration);
                            var objDep = await objDepD.Buscar(objAsig.idDepartamento);

                            if (objDep == null)
                            {
                                throw new ArgumentException("Hubo un problema con el departamento asignado, favor de reportar al área de desarrollo");
                            }

                            if (GObjUsuario.IdDepartamento == 34)
                            {
                                return new ResultadoAsignacion { Result = 1, Mensaje = string.Empty };
                            }

                            return new ResultadoAsignacion
                            {
                                Result = 3,
                                Mensaje = "La guía se encuentra asignada al departamento " + objDep.NombreDepartamento.Trim()
                            };
                        }
                        else
                        {
                            return new ResultadoAsignacion { Result = 1, Mensaje = string.Empty };

                        }
                    }
                    else
                    {
                        result = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResultadoAsignacion { Result = -1, Mensaje = ex.Message };
            }

            return new ResultadoAsignacion { Result = result, Mensaje = mensaje };
        }


        public bool NoValidada(string Referencia)
        {
            bool Sinvalidacion = false;

            try
            {
                SaaioPedime objPedime = new SaaioPedime();
                SaaioPedimeRepository objPedimeD = new SaaioPedimeRepository(_configuration);
                objPedime = objPedimeD.Buscar(Referencia.Trim());

                if (objPedime != null)
                {
                    if (string.IsNullOrEmpty(objPedime.FIR_ELEC))
                    {
                        Sinvalidacion = true;
                    }
                    else
                    {
                        Sinvalidacion = false;
                    }
                }
                else
                {
                    Sinvalidacion = true;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return Sinvalidacion;
        }

    }
}
