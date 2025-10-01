using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesTurnadoImpo;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Entities.EntitiesWs;
using LibreriaClasesAPIExpertti.Services.Turnado;
using Microsoft.VisualBasic;
using OfficeOpenXml.FormulaParsing.Excel.Operators;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSGlobal;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesCortes;
using NPOI.SS.Formula.Functions;

namespace LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesImportacion
{
    public class DespachoRepositoy
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        private readonly UbicaciondeArchivosRepository _ubicacionDeArchivosRepository;


        public DespachoRepositoy(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<Corte> CargarCortes(int IdUsuario, int Operacion )
        {
            var list = new List<Corte>();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_LOAD_CORTES_POR_OFICINA", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDUsuario", IdUsuario);
                cmd.Parameters.AddWithValue("@Operacion", Operacion);

                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new Corte
                            {
                                IdCorte = Convert.ToInt32(dr["IdCorte"]),
                                NoCorte = dr["NoCorte"].ToString(),
                                IdRegion = Convert.ToInt32(dr["IdRegion"]),
                                Region = dr["Region"].ToString(),
                                IdSalidaConsol = Convert.ToInt32(dr["IdSalidaConsol"]),
                                CerradoDespachos = Convert.ToBoolean(dr["CerradoDespachos"]),
                                CerradoConsol = Convert.ToBoolean(dr["CerradoConsol"]),
                                IdRelacionBitacora = dr["IdRelacionBitacora"] != DBNull.Value ? Convert.ToInt32(dr["IdRelacionBitacora"]) : 0,
                                Placas = dr["Placas"] != DBNull.Value ? dr["Placas"].ToString() : string.Empty,
                                Tramitador = dr["Tramitador"] != DBNull.Value ? dr["Tramitador"].ToString() : string.Empty,
                                IdTramitador = dr["IdTramitador"] != DBNull.Value ? Convert.ToInt32(dr["IdTramitador"]) : 0,
                                IdEmpTransportista = dr["IdEmpTransportista"] != DBNull.Value ? Convert.ToInt32(dr["IdEmpTransportista"]) : 0,
                                IdDoda = dr["IdDoda"] != DBNull.Value ? Convert.ToInt32(dr["IdDoda"]) : 0,
                                RelacionBitacora = dr["RelacionBitacora"] != DBNull.Value ? dr["RelacionBitacora"].ToString() : string.Empty,
                            };
                            list.Add(obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return list;
        }

        public TotalGuias CargarTotalGuias(int IdSalidaConsol)
        {
            var result = new TotalGuias();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("[Pocket].[NET_LOAD_CONSOLSALIDAS_TOTALGUIAS]", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDSALIDACONSOL", IdSalidaConsol);

                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            result = new TotalGuias
                            {
                                TotaldeGuias = Convert.ToInt32(dr["TotaldeGuias"]),
                                EnUnidad = Convert.ToInt32(dr["EnUnidad"]),
                                Faltan = Convert.ToInt32(dr["Faltan"]),
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }

        public TotalButos CargarTotalBultos(int IdSalidaConsol)
        {
            var result = new TotalButos();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("[Pocket].[NET_LOAD_CONSOLSALIDAS_TOTALBULTOS]", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDSALIDACONSOL", IdSalidaConsol);

                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            result = new TotalButos
                            {
                                TotaldeBultos = Convert.ToInt32(dr["TotaldeBultos"]),
                                EnUnidad = Convert.ToInt32(dr["EnUnidad"]),
                                Faltan = Convert.ToInt32(dr["Faltan"]),
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }
        public List<BultosPedimento> CargarPedimentosBultos(int IdSalidaConsol)
        {
            var result = new List<BultosPedimento>();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("[Pocket].[NET_LOAD_CONSOLSALIDAS_BULTOS]", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDSALIDACONSOL", IdSalidaConsol);
                cmd.Parameters.AddWithValue("@Estatus", 1);
                cmd.Parameters.AddWithValue("@NumPagina", 1);
                cmd.Parameters.AddWithValue("@pagesize", 100);
                
                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new BultosPedimento
                            {
                                BultosenPedimento = !dr.IsDBNull(dr.GetOrdinal("BultosenPedimento")) ? Convert.ToInt32(dr["BultosenPedimento"]) : 0,
                                EnUnidad = !dr.IsDBNull(dr.GetOrdinal("EnUnidad")) ? Convert.ToInt32(dr["EnUnidad"]) : 0,
                                Faltan = !dr.IsDBNull(dr.GetOrdinal("Faltan")) ? Convert.ToInt32(dr["Faltan"]) : 0,
                                Pedimento = dr.GetString("Pedimento").ToString()
                            };
                            result.Add(obj);

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }
        public List<BultosPedimento> CargarGuiasBultos(int IdSalidaConsol)
        {
            var result = new List<BultosPedimento>();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("[Pocket].[NET_LOAD_CONSOLSALIDAS_GUIAS]", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDSALIDACONSOL", IdSalidaConsol);
                cmd.Parameters.AddWithValue("@Estatus", 1);
                cmd.Parameters.AddWithValue("@NumPagina", 1);
                cmd.Parameters.AddWithValue("@pagesize", 100);

                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj  = new BultosPedimento
                            {
                                BultosenPedimento = !dr.IsDBNull(dr.GetOrdinal("BultosenPedimento")) ? Convert.ToInt32(dr["BultosenPedimento"]) : 0,
                                EnUnidad = !dr.IsDBNull(dr.GetOrdinal("EnUnidad")) ? Convert.ToInt32(dr["EnUnidad"]) : 0,
                                Faltan = !dr.IsDBNull(dr.GetOrdinal("Faltan")) ? Convert.ToInt32(dr["Faltan"]) : 0,
                                Pedimento = dr.GetString("Pedimento").ToString()
                            };
                            result.Add(obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return result;
        }

        public ProcesarGuiaRespuesta ProcesarGuia(string GuiaHouse, int IdCorte, int IDSalidaConsol, int IdUsuario, int IDDatosDeEmpresa, int IdOficina, int IdDepartamento,  int Proceso, int IdEstacion)
        {
            var objSalidasR = new ProcesarGuiaRespuesta();
            try
            {
                var objAnexos = new ConsolAnexos();
                var objAnexosD = new ConsolAnexosRepository(_configuration);
                string GuiaBabyHouse = "";
                bool guiaBabyCustomAlert = false;

                objAnexos = objAnexosD.Buscar(GuiaHouse.Trim(), IdCorte, IDDatosDeEmpresa);
                if (objAnexos == null)
                {
                    if (IDDatosDeEmpresa == 1)
                    {
                        throw new ArgumentException($"La guía {GuiaHouse.Trim()} no existe en pedimento consolidado o no tiene la categoría correspondiente, favor de regresar al área de consolidado.");
                    }
                    if (IDDatosDeEmpresa == 2)
                    {
                        objAnexos = objAnexosD.BuscarBaby(GuiaHouse.Trim(), IdCorte, IDDatosDeEmpresa);

                        if (objAnexos == null)
                        {
                            throw new ArgumentException("La guía " + GuiaHouse.Trim() + " no existe en pedimento consolidado o no tiene la categoría correspondiente, favor de regresar al área de consolidado.");
                        }

                        guiaBabyCustomAlert = true;
                        GuiaBabyHouse = GuiaHouse.Trim();
                        GuiaHouse = objAnexos.GuiaHouse;
                    }
                }

                var objReferencia1 = new Referencias();
                var objReferenciaD1 = new ReferenciasRepository(_configuration);
                objReferencia1 = objReferenciaD1.Buscar(GuiaHouse, IDDatosDeEmpresa);

                var objBloque = new ConsolBloques();
                var objBloqueD = new ConsolBloquesRepository(_configuration);
                objBloque = objBloqueD.Buscar(objAnexos.IdBloque);

                if (objBloque == null)
                {
                    throw new ArgumentException("Hubo un problema en la generación del pedimento, favor de regresar al área de consolidado.");
                }

                var objCorte = new ConsolCortes();
                var objCorteD = new ConsolCortesRepository(_configuration);
                string noCorte;

                objCorte = objCorteD.Buscar(objBloque.IdCorte);
                noCorte = objCorte.NoCorte;

                var objOficina = new CatalogoDeOficinas();
                var objOficinaD = new CatalogoDeOficinasRepository(_configuration);
                objOficina = objOficinaD.Buscar(objCorte.IdOficina);

                if (objBloque.IdCorte != IdCorte)
                {
                    throw new ArgumentException($"La guía {GuiaHouse.Trim()} pertenece al corte : {noCorte.Trim()} de {objOficina.nombre.Trim()}");
                }

               

                var objSalida = new SalidasConsol();
                var objSalidaD = new SalidasConsolRepository(_configuration);
                objSalida = objSalidaD.Buscar(objBloque.idSalidasConsol);
                if (objSalida == null)
                {
                    throw new ArgumentException($"No existe salida para la guía: {GuiaHouse.Trim()}");
                }

                if (objSalida.IdRelacionBitacora == 0 && IDDatosDeEmpresa == 2 )
                {
                    var objRelacionBitacoraData = new RelacionBitacoraRepository(_configuration);
                    var idRelacionBitacora = objRelacionBitacoraData.Insertar(IdEstacion, IdUsuario, Proceso, IDDatosDeEmpresa);

                    if (idRelacionBitacora != 0)
                    {
                        objSalida.IdRelacionBitacora = idRelacionBitacora;
                        objSalidaD.ActualizarBitacora(objSalida);
                        objSalidasR.IdRelacionBitacora = idRelacionBitacora;
                    }
                }

                var objRegion = new CatalogodeRegiones();
                var objRegionD = new CatalogodeRegionesRepository(_configuration);
                objRegion = objRegionD.Buscar(objBloque.IdRegion, IdOficina);
                if (objRegion == null)
                {
                    throw new ArgumentException($"No se pudo encontrar una región para la oficina. Guía: {GuiaHouse.Trim()}");
                }

                if (IDSalidaConsol != objSalida.IDSalidaConsol)
                {
                    throw new ArgumentException($"La guía house {GuiaHouse.Trim()} manifestada pertenece al corte: {noCorte.Trim()} de la región: {objRegion.Region}");
                }

                var objDetalleD = new DetalleSalidasConsolRepository(_configuration);
                int Contar = objDetalleD.Contar(objAnexos.IDAnexos);

                if (Contar >= objAnexos.Bultos)
                {
                    throw new ArgumentException($"Está excediendo los bultos de la guía {GuiaHouse.Trim()}");
                }
                

                int IdEvento = 0;
                var objEventosD = new ControldeEventosConsolRepository(_configuration);
                var objEventos = new ControldeEventosConsol(122, objAnexos.IDAnexos, IdUsuario, DateTime.Parse("01/01/1900"));
                IdEvento = objEventosD.InsertarEvento(objEventos, IdDepartamento, IdOficina);

                var objDetalle = new DetalleSalidasConsol
                {
                    IDAnexo = objAnexos.IDAnexos,
                    IDSalidasConsol = objBloque.idSalidasConsol
                };
                int idDetalle = objDetalleD.Insertar(objDetalle);

                if (idDetalle == 0)
                {
                    throw new ArgumentException($"No se pudo insertar el despacho de la guía: {GuiaHouse.Trim()}");
                }
                if (IDDatosDeEmpresa == 2) {
                    if (guiaBabyCustomAlert)
                    {
                        var guiaHouse2d = new CatalogoDeUsuarios2Repository(_configuration);
                        guiaHouse2d.UpdateGuiaBaby(GuiaHouse, DateTime.Now);
                        guiaHouse2d.UpdateGuiaBaby(GuiaBabyHouse, DateTime.Now);
                    }

                }

                Contar = objDetalleD.Contar(objAnexos.IDAnexos);
                objSalidasR.EnUnidad = Contar;
                objSalidasR.GuiaHouse = GuiaHouse;
                objSalidasR.Total = objAnexos.Bultos;
                objSalidasR.Pendientes = (objAnexos.Bultos - Contar);
                objSalidasR.IATA = objAnexos.IataDestino;

                if (IDDatosDeEmpresa == 2 )
                {
                    InsertarBitacoraDetalle(GuiaHouse, objSalida.IdRelacionBitacora, IdUsuario, "", IDDatosDeEmpresa);
                }
            }
            catch (Exception ex)
            {
                var objSalidasError = new ProcesarGuiaRespuesta();
                objSalidasError.GuiaHouse = GuiaHouse;
                objSalidasError.Error = ex.Message.ToString();
                objSalidasError.HasError = true;
                return objSalidasError;
            }

            return objSalidasR;
        }
        public bool InsertarBitacoraDetalle(string GuiaHouse, int idRelacionBitacora, int IdUsuario, string Guia2d, int IDDatosDeEmpresa)
        {
            bool result = false;
            try
            {
                var guia2Data = new Guia2dData();


                var BitacoraD = new BitacoraDetalleRepository(_configuration);
                var Bitacora = BitacoraD.Buscar(GuiaHouse.Trim());

                if (Bitacora == null)
                {
                    var objBitacoraNueva = new BitacoraDetalle
                    {
                        IdRelacionBitacora = idRelacionBitacora,
                        IDDatosDeEmpresa = IDDatosDeEmpresa,
                        GuiaHouse = GuiaHouse.Trim(),
                        ValorDolares = string.IsNullOrEmpty(Guia2d) ? 1 : guia2Data.Valor,
                        CantidadDeBultos = string.IsNullOrEmpty(Guia2d) ? 1 : guia2Data.Bultos,
                        Peso = string.IsNullOrEmpty(Guia2d) ? 1 : guia2Data.Peso,
                        DescripcionIngles = string.IsNullOrEmpty(Guia2d) ? "MENSAJERIA Y PAQUETERIA" : guia2Data.Descripcion,
                        Descripcion = string.IsNullOrEmpty(Guia2d) ? "MENSAJERIA Y PAQUETERIA" : guia2Data.Descripcion,
                        Moneda = string.IsNullOrEmpty(Guia2d) ? "USD" : guia2Data.ClaveDeMoneda,
                        FechaLlegada = DateTime.Now,
                        FechaAsignacion = new DateTime(1900, 1, 1),
                        FechaSalida = DateTime.Now,
                        IdUsuario = IdUsuario,
                        Tipo = 2,
                        Latitud = "",
                        Longitud = "",
                        CodigoPostal = "",
                        Cliente = ""
                    };

                    var BitacoraDTipo = new RecoleccionRepository(_configuration);
                    BitacoraDTipo.InsertarBitacora(objBitacoraNueva);
                    result = true;
                }
                else
                {
                    if (idRelacionBitacora == Bitacora.IdRelacionBitacora)
                    {
                        Bitacora.FechaLlegada = DateTime.Now;
                        Bitacora.FechaSalida = DateTime.Now;
                        Bitacora.IDDatosDeEmpresa = IDDatosDeEmpresa;
                        BitacoraD.ModificarporGuia(Bitacora);
                        result = true;
                    }
                    else
                    {
                        var objBitacoraNueva = new BitacoraDetalle
                        {
                            IdRelacionBitacora = idRelacionBitacora,
                            GuiaHouse = GuiaHouse.Trim(),
                            IDDatosDeEmpresa = IDDatosDeEmpresa,
                            ValorDolares = string.IsNullOrEmpty(Guia2d) ? 1 : guia2Data.Valor,
                            CantidadDeBultos = string.IsNullOrEmpty(Guia2d) ? 1 : guia2Data.Bultos,
                            Peso = string.IsNullOrEmpty(Guia2d) ? 1 : guia2Data.Peso,
                            DescripcionIngles = string.IsNullOrEmpty(Guia2d) ? "MENSAJERIA Y PAQUETERIA" : guia2Data.Descripcion,
                            Descripcion = string.IsNullOrEmpty(Guia2d) ? "MENSAJERIA Y PAQUETERIA" : guia2Data.Descripcion,
                            Moneda = string.IsNullOrEmpty(Guia2d) ? "USD" : guia2Data.ClaveDeMoneda,
                            Tipo = 2,
                            FechaLlegada = DateTime.Now,
                            FechaAsignacion = new DateTime(1900, 1, 1),
                            FechaSalida = DateTime.Now,
                            IdUsuario = IdUsuario,
                            Latitud = "",
                            Longitud = "",
                            CodigoPostal = "",
                            Cliente = ""
                        };

                        var BitacoraDTipo = new RecoleccionRepository(_configuration);
                        BitacoraDTipo.InsertarBitacora(objBitacoraNueva);
                        result = true;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }




    }

}
