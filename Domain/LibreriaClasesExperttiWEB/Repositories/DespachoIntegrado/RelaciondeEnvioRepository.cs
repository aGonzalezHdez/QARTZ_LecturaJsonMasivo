using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class RelaciondeEnvioRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public RelaciondeEnvioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<PredodaDetalle> CargarDetalleDoda(int idRelaciondeEnvio)
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
                cmd.CommandText = "NET_LOAD_RELACIONDEENVIO_DODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRelaciondeEnvio", idRelaciondeEnvio);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["idReferencia"] != DBNull.Value)
                        {
                            detalles.Add(new PredodaDetalle
                            {
                                IdReferencia =  Convert.ToInt32(reader["idReferencia"]),
                                Referencia = reader["Referencia"] != DBNull.Value ? reader["Referencia"].ToString() : "",
                                Pedimento = reader["Pedimento"] != DBNull.Value ? reader["Pedimento"].ToString() : "",
                                Remesa = 0,
                                NumeroDeCOVE = ""
                            });
                        }
                    }
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_LOAD_RELACIONDEENVIO_DODA");

            }
            cn.Close();
            cn.Dispose();
            return detalles;
        }

        public RelaciondeEnvio Buscar(int idRelaciondeEnvio)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            var dtb = new DataTable();
            SqlDataReader reader;
            RelaciondeEnvio relaciondeEnvio = new RelaciondeEnvio();
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "dbo.NET_SEARCH_RelaciondeEnvio";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRelaciondeEnvio", idRelaciondeEnvio);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    relaciondeEnvio = new RelaciondeEnvio()
                    {
                        IdRelaciondeEnvio = Convert.ToInt32(reader["idRelaciondeEnvio"]),
                        NumerodeEnvio = reader["NumerodeEnvio"].ToString(),
                        ClavePedimento = Convert.ToInt32(reader["ClavePedimento"]),
                        Fecha = Convert.ToDateTime(reader["fecha"]),
                        IdUsuario = Convert.ToInt32(reader["idUsuario"]),
                        Patente = Convert.ToInt32(reader["Patente"]),
                        Estatus = Convert.ToBoolean(reader["Estatus"]),
                        IdDepartamento = Convert.ToInt32(reader["idDepartamento"]),
                        CAAT = reader["CAAT"].ToString(),
                        Placas = reader["Placas"].ToString(),
                        IdOficina = Convert.ToInt32(reader["IDOficina"]),
                        FechaDeImpresion = Convert.ToDateTime(reader["FechaDeImpresion"]),
                        Integracion = Convert.ToString(reader["Integracion"]),
                        UbicacionDODA = Convert.ToString(reader["UbicacionDODA"]),
                        IdRelacionBitacora = Convert.ToInt32(reader["IdRelacionBitacora"]),
                        IdTramitador = Convert.ToInt32(reader["IdTramitador"]),
                        IdEmpTransportista = reader["IdEmpTransportista"].ToString(),
                        IdDoda = Convert.ToInt32(reader["IdDoda"]),
                        IdPreDoda = Convert.ToInt32(reader["IdPreDoda"]),
                        LimiteDePedimentos = Convert.ToInt32(reader["LimiteDePedimentos"]),
                        AduanaEntrada = reader["Aduana"].ToString(),
                        AduanaDespacho = reader["Aduana"].ToString()
                    };
                    
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_SEARCH_RelaciondeEnvio");

            }
            cn.Close();
            cn.Dispose();
            return relaciondeEnvio;
        }
        public int ActualizarPredoda(int IdRelaciondeEnvio, int idPredoda)
        {
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            int num = 0;
            try
            {
                sqlConnection.ConnectionString = SConexion;
                sqlConnection.Open();
                sqlCommand.CommandText = "dbo.NET_UPDATE_RELACIONDEENVIO_IDPREDODA";
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@IdRelaciondeEnvio", IdRelaciondeEnvio);
                sqlCommand.Parameters.AddWithValue("@IdPredoda", idPredoda);
                SqlParameter sqlParameter = sqlCommand.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                sqlParameter.Direction = ParameterDirection.Output;
                sqlCommand.ExecuteNonQuery();
                num = Convert.ToInt32(sqlCommand.Parameters["@newid_registro"].Value);
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_RELACIONDEENVIO_IDPREDODA");
            }

            return num;
        }

        public List<PredodaDetalle> CargarDoda(int IdRelaciondeEnvio)
        {
            List<PredodaDetalle> detalles = new List<PredodaDetalle>();
            var cmd = new SqlCommand();
            SqlDataReader reader;
            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.ConnectionString = SConexion;
                    cn.Open();
                    cmd.CommandText = "NET_LOAD_RELACIONDEENVIO";
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdRelaciondeEnvio", IdRelaciondeEnvio);

                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            detalles.Add(new PredodaDetalle
                            {
                                IdReferencia = Convert.ToInt32(reader["IdReferencia"]),
                                Remesa = 0,
                                NumeroDeCOVE = "",
                                Referencia = reader["Referencia"].ToString(),
                                Pedimento = reader["Pedimento"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex.Message} NET_LOAD_RELACIONDEENVIO");
                }
                finally
                {
                    cn.Close();
                    cn.Dispose();
                }
            }

            return detalles;
        }
        public int Modificar_IDDODA(int IdRelaciondeEnvio, int IdDoda)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;

            try
            {
                cmd.CommandText = "Pocket.NET_UPDATE_RelaciondeEnvio_DODA_Pocket";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@IdDoda", SqlDbType.Int);
                param.Value = IdDoda;

                param = cmd.Parameters.Add("@IdRelaciondeEnvio", SqlDbType.Int);
                param.Value = IdRelaciondeEnvio;


                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message + " Pocket.NET_UPDATE_RelaciondeEnvio_DODA_Pocket");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }
    }
}
