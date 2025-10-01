using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesControldeAsignacion;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControldeAsignacion.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControldeAsignacion
{
    public class ControldeAsignacionRepository : IControldeAsignacionRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public ControldeAsignacionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<ControldeAsignacion> Cargar(ControldeAsignacionRequest objRequest)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            List<ControldeAsignacion> lstControldeAsignacion = new List<ControldeAsignacion>();


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_REPORT_CONTROL_ASIGNACION_JAS";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@IDOFICINA", SqlDbType.Int, 4);
            @param.Value = objRequest.idOficina;

            @param = cmd.Parameters.Add("@ADUANA", SqlDbType.VarChar, 3);
            @param.Value = objRequest.Aduana;

            @param = cmd.Parameters.Add("@OPERACION", SqlDbType.Int, 4);
            @param.Value = objRequest.Operacion;

            @param = cmd.Parameters.Add("@IDDEPARTAMENTO", SqlDbType.Int, 4);
            @param.Value = objRequest.idDepartamento;


            @param = cmd.Parameters.Add("@IDUSUARIO", SqlDbType.Int, 4);
            @param.Value = objRequest.idUsuario;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ControldeAsignacion objControldeAsignacion = new ControldeAsignacion();

                        objControldeAsignacion.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                        objControldeAsignacion.Nombre = (dr["Nombre"]).ToString();
                        objControldeAsignacion.Inventario = Convert.ToInt32(dr["Inventario"]);
                        objControldeAsignacion.DiasAnteriores = Convert.ToInt32(dr["DA"]);
                        objControldeAsignacion.HoyMismo = Convert.ToInt32(dr["HoyMismo"]);
                        objControldeAsignacion.PendientesHoy = Convert.ToInt32(dr["PendientesHoy"]);
                        objControldeAsignacion.NotificadasHoy = Convert.ToInt32(dr["NotificadasHoy"]);
                        objControldeAsignacion.AltaPrioridad = Convert.ToInt32(dr["AltaPrioridad"]);

                        lstControldeAsignacion.Add(objControldeAsignacion);
                    }

                }


                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_REPORT_CONTROL_ASIGNACION_JAS");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return lstControldeAsignacion;
        }

        public List<ControldeAsignaciónDetalleJas> CargarDetalleJasForwarding(ControldeAsignacionRequest objRequest)
        {
            List<ControldeAsignaciónDetalleJas> lstControldeAsignaciónDetalleJas = new List<ControldeAsignaciónDetalleJas>();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_REPORT_CONTROL_ASIGNACION_JAS_DETALLE";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@IDOFICINA", SqlDbType.Int, 4);
            @param.Value = objRequest.idOficina;

            @param = cmd.Parameters.Add("@ADUANA", SqlDbType.VarChar, 3);
            @param.Value = objRequest.Aduana;

            @param = cmd.Parameters.Add("@OPERACION", SqlDbType.Int, 4);
            @param.Value = objRequest.Operacion;


            @param = cmd.Parameters.Add("@IDDEPARTAMENTO", SqlDbType.Int, 4);
            @param.Value = objRequest.idDepartamento;

            @param = cmd.Parameters.Add("@IDUSUARIOFILA", SqlDbType.Int, 4);
            @param.Value = objRequest.idUsuarioFila;

            @param = cmd.Parameters.Add("@COLUMNA", SqlDbType.Int, 4);
            @param.Value = objRequest.Columna;


            @param = cmd.Parameters.Add("@PAGE", SqlDbType.Int, 4);
            @param.Value = objRequest.page;

            @param = cmd.Parameters.Add("@PAGESIZE", SqlDbType.Int, 4);
            @param.Value = objRequest.pageSize;

            @param = cmd.Parameters.Add("@IDUSUARIO", SqlDbType.Int, 4);
            @param.Value = objRequest.idUsuario;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ControldeAsignaciónDetalleJas objControldeAsignacion = new ControldeAsignaciónDetalleJas();

                        objControldeAsignacion.TOperacion = (dr["TOperacion"]).ToString();
                        objControldeAsignacion.AduanaDespacho = (dr["AduanaDespacho"]).ToString();
                        objControldeAsignacion.Aduana = (dr["Aduana"]).ToString();
                        objControldeAsignacion.NumeroDeReferencia = (dr["NumeroDeReferencia"]).ToString();
                        objControldeAsignacion.FechaAsignacion = Convert.ToDateTime(dr["FechaAsignacion"]);
                        objControldeAsignacion.Cliente = (dr["Cliente"]).ToString();
                        objControldeAsignacion.Pedimento = (dr["Pedimento"]).ToString();
                        objControldeAsignacion.GuiasHouse = (dr["GuiasHouse"]).ToString();
                        objControldeAsignacion.Patente = (dr["Patente"]).ToString();
                        objControldeAsignacion.UltimaLlamada = (dr["UltimaLlamada"]).ToString();
                        objControldeAsignacion.UltimoCheck = (dr["UltimoCheck"]).ToString();
                        objControldeAsignacion.Prioridad = Convert.ToInt32(dr["Prioridad"]);

                        lstControldeAsignaciónDetalleJas.Add(objControldeAsignacion);

                    }

                }


                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_REPORT_CONTROL_ASIGNACION_JAS_DETALLE");
            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return lstControldeAsignaciónDetalleJas;
        }
    }
}
