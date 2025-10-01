using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos
{


    public class BitacoraDePrevalRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public BitacoraDePrevalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public DataTable LlenarPorReferenciaGrid(string NumeroDeReferencia)
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
                    dap.SelectCommand.CommandText = "NET_LLENAR_BITACORADEPREVAL_POR_REFERENCIA_EN_DATEGRIDVIEW '" + NumeroDeReferencia + "'";
                    dap.SelectCommand.CommandType = CommandType.Text;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + "NET_LLENAR_BITACORADEPREVAL_POR_REFERENCIA_EN_DATEGRIDVIEW");
                }
            }
            return dtb;
        }
        public DataTable LlenarPorJulianoGrid(string Juliano)
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
                    dap.SelectCommand.CommandText = "NET_LLENAR_BITACORADEPREVAL_POR_JULIANO_EN_DATEGRIDVIEW '" + Juliano + "'";
                    dap.SelectCommand.CommandType = CommandType.Text;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + "NET_LLENAR_BITACORADEPREVAL_POR_JULIANO_EN_DATEGRIDVIEW");
                }
            }
            return dtb;
        }


        public DataTable LlenarPorReferenciaErroresGrid(string NumeroDeReferencia)
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
                    dap.SelectCommand.CommandText = "NET_LLENAR_ERROES_EN_BITACORADEPREVAL_POR_REFERENCIA '" + NumeroDeReferencia + "'";
                    dap.SelectCommand.CommandType = CommandType.Text;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + "NET_LLENAR_ERROES_EN_BITACORADEPREVAL_POR_REFERENCIA");
                }
            }
            return dtb;
        }
        public DataTable LlenarPorJulianoErroresGrid(string Juliano)
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
                    dap.SelectCommand.CommandText = "NET_LLENAR_ERROES_EN_BITACORADEPREVAL_POR_JULIANO '" + Juliano + "'";
                    dap.SelectCommand.CommandType = CommandType.Text;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + "NET_LLENAR_ERROES_EN_BITACORADEPREVAL_POR_JULIANO");
                }
            }
            return dtb;
        }





        public void Delete(string NumeroDeReferencia)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_PREVAL_DELETE_BITACORADEPREVAL";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15)
                param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                param.Value = NumeroDeReferencia;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }


        }

        public int InsertarJustificaRef(int IdReferencia, int IdError, int IdUsuario, string Juliano)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            try
            {
                cmd.CommandText = "NET_INSERT_CASAEI_JUSTIFICACIONDEREFERENCIAS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // ,@IdReferencia  int
                param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                param.Value = IdReferencia;

                // ,@IdError  int
                param = cmd.Parameters.Add("@IdError", SqlDbType.Int, 4);
                param.Value = IdError;

                // ,@IdUsuario  int
                param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
                param.Value = IdUsuario;

                param = cmd.Parameters.Add("@Juliano", SqlDbType.VarChar, 50);
                param.Value = Juliano;


                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;


                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = int.Parse(cmd.Parameters["@newid_registro"].Value.ToString());
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_JUSTIFICACIONDEREFERENCIAS");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }


    }
}
