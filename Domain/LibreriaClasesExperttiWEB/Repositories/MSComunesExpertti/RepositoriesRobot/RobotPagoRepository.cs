using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesRobot;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesRobot
{
    public class RobotPagoRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public RobotPagoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DataTable GetRobotConfigByClave(string Clave, int idOficina)
        {

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_FIND_CONFIG_ROBOT_PAGO";

                    @param = dap.SelectCommand.Parameters.Add("@Clave", SqlDbType.VarChar);
                    @param.Value = Clave;

                    @param = dap.SelectCommand.Parameters.Add("@IdOficina", SqlDbType.Int);
                    @param.Value = idOficina;

                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_FIND_CONFIG_ROBOT_PAGO");
                }
            }
            return dtb;
        }

        public void SendEmail(int idReferencia, bool success)
        {
            SqlConnection cn = new();
            SqlCommand cmd = new();
            SqlParameter @param;
            try
            {
                cmd.CommandText = "NET_REPORT_PEDIMENTO_VALIDADO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@idReferencia", SqlDbType.Int);
                @param.Value = idReferencia;

                @param = cmd.Parameters.Add("@Success", SqlDbType.Bit);
                @param.Value = success;

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + " NET_REPORT_PEDIMENTO_VALIDADO");
            }
            finally { cn.Close(); cn.Dispose(); }
        }

        public DataTable GetRobotPagoListByStatus(string Estatus, int numberMaxItems, int IdOficina, int AduanaDespacho, string Prevalidador, string Patente)
        {

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_ROBOT_PAGO_LIST_BY_STATUS";

                    @param = dap.SelectCommand.Parameters.Add("@Estatus", SqlDbType.VarChar);
                    @param.Value = Estatus;

                    @param = dap.SelectCommand.Parameters.Add("@NumberMaxItems", SqlDbType.VarChar);
                    @param.Value = numberMaxItems;

                    @param = dap.SelectCommand.Parameters.Add("@IdOficina", SqlDbType.Int);
                    @param.Value = IdOficina;

                    @param = dap.SelectCommand.Parameters.Add("@AduanaDespacho", SqlDbType.Int);
                    @param.Value = AduanaDespacho;

                    @param = dap.SelectCommand.Parameters.Add("@Prevalidador", SqlDbType.VarChar);
                    @param.Value = Prevalidador;

                    @param = dap.SelectCommand.Parameters.Add("@Patente", SqlDbType.VarChar);
                    @param.Value = Patente;

                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_ROBOT_PAGO_LIST_BY_STATUS");
                }
            }
            return dtb;
        }

        public bool RobotPagoInsert(RobotPago robotPago)
        {
            bool result = new bool();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            result = false;

            try
            {
                robotPago.Estatus = robotPago.Estatus.Trim();

                if (!RobotPago.ValidateStatus(robotPago.Estatus))
                {
                    throw new Exception("El Estatus no es un valor válido ");
                }

                cmd.CommandText = "NET_ROBOT_PAGO_INSERT";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
                @param.Value = robotPago.IdOficina;

                @param = cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4);
                @param.Value = robotPago.Patente;

                @param = cmd.Parameters.Add("@AduanaDespacho", SqlDbType.VarChar, 3);
                @param.Value = robotPago.AduanaDespacho;

                @param = cmd.Parameters.Add("@Prevalidador", SqlDbType.VarChar, 3);
                @param.Value = robotPago.Prevalidador;

                @param = cmd.Parameters.Add("@ClaveMandatario", SqlDbType.VarChar, 2);
                @param.Value = robotPago.ClaveMandatario;

                @param = cmd.Parameters.Add("@Estatus", SqlDbType.VarChar, 20);
                @param.Value = robotPago.Estatus.Trim().ToUpper();

                @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                @param.Value = robotPago.IdReferencia;

                @param = cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4);
                @param.Value = robotPago.IdCliente;

                @param = cmd.Parameters.Add("@FECHA_ARRIBO", SqlDbType.VarChar, 10);
                @param.Value = robotPago.FechaArribo;

                @param = cmd.Parameters.Add("@FECHA_TERMINO", SqlDbType.VarChar, 10);
                @param.Value = robotPago.FechaTermino;

                @param = cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 255);
                @param.Value = robotPago.Descripcion;

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                result = true;
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_ROBOT_PAGO_INSERT");
            }
            cn.Close();
            cn.Dispose();
            return result;
        }
        public bool RobotPagoUpdateStatus(int IDNetRobotPago, string Estatus, string Description)
        {
            bool result = new bool();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            result = false;

            try
            {
                Estatus = Estatus.Trim();

                if (!RobotPago.ValidateStatus(Estatus))
                {
                    throw new Exception("El Estatus no es un valor válido ");
                }

                cmd.CommandText = "NET_ROBOT_PAGO_UPDATE_STATUS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IDNetRobotPago", SqlDbType.Int, 8);
                @param.Value = IDNetRobotPago;

                @param = cmd.Parameters.Add("@Estatus", SqlDbType.VarChar, 20);
                @param.Value = Estatus.Trim().ToUpper();

                @param = cmd.Parameters.Add("@Description", SqlDbType.VarChar, 500);
                @param.Value = Description.Trim().ToUpper();

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                result = true;
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_ROBOT_PAGO_UPDATE_STATUS");
            }
            cn.Close();
            cn.Dispose();
            return result;
        }

        public bool RobotPagoUpdateExecutionAttemps(int IDNetRobotPago, int IntentosEjecucion)
        {
            bool result = new bool();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            result = false;

            try
            {
                cmd.CommandText = "NET_ROBOT_PAGO_UPDATE_EXECUTION_ATTEMPS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IDNetRobotPago", SqlDbType.Int);
                @param.Value = IDNetRobotPago;

                @param = cmd.Parameters.Add("@IntentosEjecucion", SqlDbType.Int);
                @param.Value = IntentosEjecucion;

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + " NET_ROBOT_PAGO_UPDATE_STATUS");
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }
            return result;
        }
        //public DataTable GetIdsOficina(string SConexion)
        public DataTable GetIdsOficina()
        {
            var dtb = new DataTable();

            using (var cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_GET_IDS_OFICINA_ROBOT_PAGO";


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_GET_IDS_OFICINA_ROBOT_PAGO");
                }
            }
            return dtb;
        }
        public DataTable ValidateFirPago(string NumeroDeReferencia)
        {

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_ROBOT_PAGO_VALIDATE_FIR_PAGO";

                    @param = dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar);
                    @param.Value = NumeroDeReferencia;

                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_ROBOT_PAGO_VALIDATE_FIR_PAGO");
                }
            }
            return dtb;
        }

    }
}
