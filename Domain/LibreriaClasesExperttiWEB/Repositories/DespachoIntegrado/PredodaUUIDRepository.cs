using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado.PredodaDHL;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class PredodaUUIDRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public PredodaUUIDRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI");
        }
        public PredodaUUIDs Insertar(PredodaUUIDs predodaUUIDs)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CASAEI_PREDODA_UUIDS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPredoda", predodaUUIDs.IdPredoda);
                cmd.Parameters.AddWithValue("@UUID", predodaUUIDs.UUID);
                
                param=cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                predodaUUIDs.IdUUID = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_PREDODA_UUIDS");
            }
            return predodaUUIDs;
        }

        public bool Eliminar(int idpredodaUUIDs)
        {

            bool result = false;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_DELETE_CASAEI_PREDODA_UUIDS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPredoda", idpredodaUUIDs);

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                int id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                result = id > 0;

                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_DELETE_CASAEI_PREDODA_UUIDS");
            }
            return result;
        }

        public List<PredodaUUIDs> Cargar(int idPredoda)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();

            List<PredodaUUIDs> listado = new List<PredodaUUIDs>();
            SqlDataReader reader;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_CASAEI_PREDODA_UUIDS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdPredoda", idPredoda);

                reader = cmd.ExecuteReader();

                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listado.Add(new PredodaUUIDs
                        {
                            IdUUID = Convert.ToInt32(reader["IdUUID"]),
                            IdPredoda = Convert.ToInt32(reader["IdPredoda"]),
                            UUID = reader["UUID"].ToString()
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
                throw new Exception(ex.Message.ToString() + " NET_LOAD_CASAEI_PREDODA_UUIDS");
            }
            return listado;
        }
    }
}
