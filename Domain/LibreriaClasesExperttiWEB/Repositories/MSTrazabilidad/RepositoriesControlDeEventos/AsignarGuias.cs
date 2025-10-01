using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;


namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos
{
    public class AsignarGuias
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public AsignarGuias(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public async Task<AsignarGuiasRespuesta> Validaciones(CatalogodeCheckPoints objCheck, Referencias objRefe, int IdUsuario, bool Extraordinario, string Observaciones, int IDDatosDeEmpresa)
        {
            AsignarGuiasRespuesta objRespuesta = new();



            SaaioPedime objPedime = new();
            SaaioPedimeRepository objPedimeD = new(_configuration);

            objPedime = objPedimeD.Buscar(objRefe.NumeroDeReferencia);

            if (objPedime != null)
            {
                if (objCheck.AsignarPedimento)
                {
                    if (objPedime.NUM_PEDI != null)
                    {
                        if (objPedime.NUM_PEDI == "")
                        {
                            ControldePedimentosRepository objCtrlPedi = new ControldePedimentosRepository(_configuration);
                            ControldePedimentos lControldePedimentos = new ControldePedimentos();
                            lControldePedimentos.IdReferencia = objRefe.IDReferencia;
                            lControldePedimentos.Patente = objPedime.PAT_AGEN.Trim();
                            lControldePedimentos.IdOficina = objRefe.IdOficina;
                            lControldePedimentos.IdUsuario = IdUsuario;

                            objRespuesta = await objCtrlPedi.AsignarPedimento(lControldePedimentos, Extraordinario, objPedime.ADU_DESP.Trim());
                            objPedime.NUM_PEDI = objRespuesta.Pedimento;
                        }
                    }
                }

            }

            if (objCheck.ValidaPedimento)
            {
                if (objPedime == null)
                    throw new ArgumentException("La referencia no existe en el sistema de pedimentos");

                if (objPedime.NUM_PEDI == null)
                    throw new ArgumentException("La referencia no existe en el sistema de pedimentos");

                if (objPedime.NUM_PEDI.Trim() == "")
                    throw new ArgumentException(objCheck.Descripcion.Trim() + ":Requiere numero de Pedimento");
            }

            if (objPedime != null)
            {
                objRespuesta.Pedimento = objPedime.NUM_PEDI;
            }

            if (objCheck.ValidaProforma)
            {
                await ValidarProforma(objRefe.IDReferencia);
            }

            objRespuesta.Prevalidar = objCheck.Prevalida;


            if (objCheck.ObservacionObligatoria)
            {
                if (Observaciones.Trim().Length < 15)
                    throw new ArgumentException("Es Necesario insertar una observación mas especifica");
            }

            return objRespuesta;
        }

        public int getUsuarioDeGrupo(int IdGrupo)
        {


            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            int idUsuario = 0;

            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_SEARCH_USUARIO_DE_GRUPO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // @IdUsuario INT
            @param = cmd.Parameters.Add("@IdGrupo", SqlDbType.Int, 4);
            @param.Value = IdGrupo;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    idUsuario = Convert.ToInt32(dr["IdUsuario"]);
                }


                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_SEARCH_USUARIO_DE_GRUPO");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return idUsuario;
        }

        public async Task<bool> ValidarProforma(int IdReferencia)
        {
            bool vResultado = true;


            DocumentosRepository objDocD = new(_configuration);
            if (await objDocD.ExisteProforma(IdReferencia))
            {
                vResultado = false;
                throw new ArgumentException("IMPOSIBLE RELACIONAR ESTA GUÍA. USTED DEBERÁ  GENERAR LA PROFORMA DEL PEDIMENTO EN FORMATO PDF DESDE CSAAIWIN");
            }


            return vResultado;
        }

        public async Task<AsignarGuiasRespuesta> ReasignarGuia(int IdUsuarioAsigna, int IdUsuarioAsignado, int IdReferencia)
        {
            var objRespuesta = new AsignarGuiasRespuesta();
            int IdEvento = 0;

            var ObjUsuario = new CatalogoDeUsuarios();
            var ObjUsuarioD = new CatalogoDeUsuariosRepository(_configuration);

            ObjUsuario = ObjUsuarioD.BuscarPorId(IdUsuarioAsignado);
            if (ObjUsuario == null)
            {
                throw new ArgumentException("Hubo un problema con el usuario al que se quiere re-asignar la guía");
            }

            var lEventosD = new ControldeEventosRepository(_configuration);

            var idDepartamentoActual = await lEventosD.BuscaDepartamentoActual(IdReferencia, ObjUsuario.IdOficina);

            if (idDepartamentoActual == 0)
            {
                throw new Exception("La referencia no está asignada al departamento. Ingrese un evento de entrada. ");
            }

            var dtb = new DataTable();
            var usuariosDisponibles = new UsuariosDisponibles();
            var UsuariosDisponiblesD = new UsuariosDisponiblesRepository(_configuration);
            dtb = UsuariosDisponiblesD.DepartamentoAsignado(ObjUsuario.IdUsuario, idDepartamentoActual);
            if (dtb.Rows.Count == 0)
            {
                throw new Exception("El usuario seleccionado debe estar asignado al departamento actual de la referencia");
            }

            var objAsignarD = new AsignacionDeGuiasRepository(_configuration);

            objRespuesta = objAsignarD.ReasignarGuia(IdUsuarioAsigna, IdUsuarioAsignado, IdReferencia);

            return objRespuesta;

        }


    }//Clase
}// NameSpace
