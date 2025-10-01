using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador
{
    public class ExpedienteDigitalRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public ExpedienteDigitalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public bool GeneraLineaDeComandosEnvioDesftp(string myBat, string myFile)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    cn.ConnectionString = sConexion;
                    cn.Open();

                    // Asignar el Stored Procedure
                    cmd.CommandText = $"EXECUTE NET_ENVIO_SFTP '{myBat}','\"\"\"\"{myFile}\"\"\"\"'";
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.Text;

                    // Ejecutar el stored procedure
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Parameters.Clear();

                    if (cn.State == System.Data.ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }

            return true;
        }
        public void CopiarJulianosYenviaPorFtp(string misPedimentos)
        {
            using (SqlConnection cn = new SqlConnection())
            using (SqlCommand cmd = new SqlCommand())
            {
                SqlDataReader dr = null;
                SqlParameter param;

                try
                {
                    cmd.CommandText = "NET_ENVIAR_ARCHIVOS_JULIANOS_A_FTP";
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // @ArrayPedimentos VARCHAR(MAX)
                    param = cmd.Parameters.Add("@ArrayPedimentos", SqlDbType.VarChar, 8000);
                    param.Value = misPedimentos;

                    cn.ConnectionString = sConexion;
                    cn.Open();

                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (!string.IsNullOrEmpty(dr["RutaOrigen"].ToString()))
                            {
                                GeneraLineaDeComandosEnvioDesftp(dr["ArchivoBat"].ToString(), dr["RutaOrigen"].ToString());
                            }
                        }
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Parameters.Clear();

                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
        }
    }
}
