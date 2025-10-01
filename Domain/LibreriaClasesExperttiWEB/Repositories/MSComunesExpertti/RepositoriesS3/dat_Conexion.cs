using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3
{
    public class dat_Conexion
    {
        private readonly SqlConnection SqlConnection;
        public SqlCommand SqlCommand { get; set; }
        private SqlTransaction SqlTransaction;
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public dat_Conexion(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;

            SqlConnection = new SqlConnection();

            try
            {
                SqlConnection.ConnectionString = SConexion;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public void AbrirConexion(bool transaccion)
        {
            if (SqlConnection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    SqlConnection.Open();
                    SqlCommand = SqlConnection.CreateCommand();
                    SqlCommand.CommandTimeout = 0;

                    if (transaccion)
                    {
                        SqlTransaction = SqlConnection.BeginTransaction();
                        SqlCommand.Transaction = SqlTransaction;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
        }

        public void CerrarConexion()
        {
            try
            {
                if (SqlConnection.State == System.Data.ConnectionState.Open)
                {
                    SqlConnection.Close();
                    SqlConnection.Dispose();

                    if (SqlCommand != null)
                        SqlCommand.Dispose();

                    if (SqlTransaction != null)
                        SqlTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public void Dispose()
        {
            SqlConnection.Dispose();
        }


    }
}
