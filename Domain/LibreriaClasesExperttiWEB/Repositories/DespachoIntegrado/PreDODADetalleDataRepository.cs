using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;

using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class PreDODADetalleDataRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public PreDODADetalleDataRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(PredodaDetalle predodaDetalle)
        {
            int id = 0;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;

            try
            {
                cn.ConnectionString = SConexion;
                cmd.CommandText = "NET_INSERT_CASAEI_PreDODADetalle";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idPreDoda", predodaDetalle.idPreDoda);
                cmd.Parameters.AddWithValue("@IdReferencia", predodaDetalle.IdReferencia);
                cmd.Parameters.AddWithValue("@NoRemesa", predodaDetalle.Remesa);
                cmd.Parameters.AddWithValue("@NumeroDeCOVE", predodaDetalle.NumeroDeCOVE);
                cmd.Parameters.AddWithValue("@RutaS3", predodaDetalle.RutaS3);

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cn.Open();
                cmd.ExecuteNonQuery();

                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cmd.Parameters.Clear();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message + "NET_INSERT_CASAEI_PreDODADetalle ");
            }
            return id;
        }
    }
}
