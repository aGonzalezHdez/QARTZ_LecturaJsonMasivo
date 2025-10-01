using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class PredodaCandadoRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public PredodaCandadoRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public PredodaCandado Insertar(PredodaCandado candado)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CASAEI_PREDODA_CANDADOS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPredoda", candado.IDPredoda);
                cmd.Parameters.AddWithValue("@Candado", candado.Candado);

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                candado.IdCandados = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_PREDODA_CANDADOS");
            }
            return candado;
        }


        public List<PredodaCandado> Cargar(int idPredoda)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();

            List<PredodaCandado> listado = new List<PredodaCandado>();
            SqlDataReader reader;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_CASAEI_PREDODA_CANDADOS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPredoda", idPredoda);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listado.Add(new PredodaCandado
                        {
                            IdCandados = Convert.ToInt32(reader["idCandados"]),
                            IDPredoda = Convert.ToInt32(reader["IdPredoda"]),
                            Candado = reader["Candado"].ToString()
                        });
                    }
                }

                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_LOAD_CASAEI_PREDODA_CANDADOS");
            }
            return listado;
        }
    }
}
