using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class DetalledeEnvioRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public DetalledeEnvioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<PredodaDetalle> GenerarDODAConsolWS(int idCorte,int idRegion,int idOficina)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            var dtb = new DataTable();
            SqlDataReader reader;
            List<PredodaDetalle> detalles = new List<PredodaDetalle>();
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_LOAD_GENERA_DODA_CONSOL_NEW";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCorte", idCorte);
                cmd.Parameters.AddWithValue("@IdRegion", idRegion);
                cmd.Parameters.AddWithValue("@IdOficina", idOficina);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        detalles.Add(new PredodaDetalle
                        {
                            IdReferencia = Convert.ToInt32(reader["idReferencia"]),
                            Referencia = reader["Referencia"].ToString(),
                            Pedimento = reader["Pedimento"].ToString()
                        });
                    }
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_LOAD_GENERA_DODA_CONSOL_NEW");

            }
            cn.Close();
            cn.Dispose();
            return detalles;
        }
        public bool VerificarPago(int IdCorte,int IdRegion,int IdOficina)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            var dtb = new DataTable();
            SqlDataReader reader;
            bool pagados = false;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_LOAD_GENERA_DODA_CONSOL_VERIFICARPAGO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCorte", IdCorte);
                cmd.Parameters.AddWithValue("@IdRegion", IdRegion);
                cmd.Parameters.AddWithValue("@IdOficina", IdOficina);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    pagados = Convert.ToBoolean(reader["PAGADOS"]);
                }
                reader.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_LOAD_GENERA_DODA_CONSOL_NEW");

            }
            cn.Close();
            cn.Dispose();
            return pagados;
        }
    }
}
