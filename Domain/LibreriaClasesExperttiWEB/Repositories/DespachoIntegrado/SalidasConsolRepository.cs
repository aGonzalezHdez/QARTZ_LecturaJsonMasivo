using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class SalidasConsolRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public SalidasConsolRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public SalidasConsol Buscar(int IDSalidaConsol)
        {
            SalidasConsol objSALIDASCONSOL = new SalidasConsol();

            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("NET_SEARCH_SALIDASCONSOL", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IDSalidaConsol", SqlDbType.Int) { Value = IDSalidaConsol });

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objSALIDASCONSOL.IDSalidaConsol = Convert.ToInt32(dr["IdSalidaConsol"]);
                            objSALIDASCONSOL.NumeroSalidaConsol = dr["NumeroSalidaConsol"].ToString();
                            objSALIDASCONSOL.Fecha = Convert.ToDateTime(dr["Fecha"]);
                            objSALIDASCONSOL.IDUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objSALIDASCONSOL.Estatus = Convert.ToBoolean(dr["Cerrado"]);
                            objSALIDASCONSOL.IdCorte = Convert.ToInt32(dr["IdCorte"]);
                            objSALIDASCONSOL.NoCorte = dr["Corte"].ToString();
                            objSALIDASCONSOL.IDRegion = Convert.ToInt32(dr["IdRegion"]);
                            objSALIDASCONSOL.IdMasterConsol = Convert.ToInt32(dr["IdMasterConsol"]);
                            objSALIDASCONSOL.IdRelacionBitacora = Convert.ToInt32(dr["IdRelacionBitacora"]);
                            objSALIDASCONSOL.IdEmpTransportista = Convert.ToInt32(dr["IdEmpTransportista"]);
                            objSALIDASCONSOL.IdTramitador = Convert.ToInt32(dr["IdTramitador"]);
                            objSALIDASCONSOL.Placas = dr["Placas"].ToString();
                            objSALIDASCONSOL.IdPreDoda = Convert.ToInt32(dr["IdPreDoda"]);
                            objSALIDASCONSOL.IdOficina = dr["IdOficina"].ToString();
                            objSALIDASCONSOL.RFC = dr["rfc"]==DBNull.Value?"": dr["rfc"].ToString();
                        }
                        else
                        {
                            objSALIDASCONSOL = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objSALIDASCONSOL;
        }
        public int ActualizarBitacora(SalidasConsol lsalidasconsol)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("[Pocket].[NET_UPDATE_SALIDASCONSOLBITACORA]", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdRelacionBitacora", SqlDbType.Int).Value = lsalidasconsol.IdRelacionBitacora;
                cmd.Parameters.Add("@IdSalidaConsol", SqlDbType.Int).Value = lsalidasconsol.IDSalidaConsol;

                SqlParameter newIdRegistroParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                newIdRegistroParam.Direction = ParameterDirection.Output;

                SqlParameter idRegionParam = cmd.Parameters.Add("@IdRegion", SqlDbType.Int);
                idRegionParam.Direction = ParameterDirection.Output;

                SqlParameter idCorteParam = cmd.Parameters.Add("@IdCorte", SqlDbType.Int);
                idCorteParam.Direction = ParameterDirection.Output;

                SqlParameter regionParam = cmd.Parameters.Add("@Region", SqlDbType.VarChar, 45);
                regionParam.Direction = ParameterDirection.Output;

                SqlParameter noCorteParam = cmd.Parameters.Add("@NoCorte", SqlDbType.VarChar, 45);
                noCorteParam.Direction = ParameterDirection.Output;

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(newIdRegistroParam.Value) != -1 ? Convert.ToInt32(newIdRegistroParam.Value) : 0;
                }
                catch (Exception ex)
                {
                    id = 0;
                    throw new Exception(ex.Message + " NET_UPDATE_SALIDASCONSOLBITACORA");
                }
            }

            return id;
        }

        public int ActualizarPredoda(int IdSalidaConsol, int idPredoda)
        {
            SqlConnection sqlConnection = new SqlConnection();
            SqlCommand sqlCommand = new SqlCommand();
            int num = 0;
            try
            {
                sqlConnection.ConnectionString = SConexion;
                sqlConnection.Open();
                sqlCommand.CommandText = "dbo.NET_UPDATE_SALIDASCONSOL_IDPREDODA";
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@IdSalidaConsol", IdSalidaConsol);
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
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_SALIDASCONSOL_IDPREDODA");
            }

            return num;
        }
        public List<PredodaDetalle> CargarDoda(int IdCorte,int IdRegion,int IdOficina)
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
                    cmd.CommandText = "NET_LOAD_GENERA_DODA_CONSOL_NEW";
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCorte", IdCorte);
                    cmd.Parameters.AddWithValue("@IdRegion", IdRegion);
                    cmd.Parameters.AddWithValue("@IdOficina", IdOficina);

                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader["IdReferencia"] != DBNull.Value)
                            {
                                detalles.Add(new PredodaDetalle
                                {
                                    IdReferencia = reader["IdReferencia"] != DBNull.Value ? Convert.ToInt32(reader["IdReferencia"]) : 0,
                                    Remesa = 0,
                                    NumeroDeCOVE = "",
                                    Referencia = reader["NumeroDeReferencia"] != DBNull.Value ? reader["NumeroDeReferencia"].ToString() : "",
                                    Pedimento = reader["DOCUMENTO"] != DBNull.Value ? reader["DOCUMENTO"].ToString() : ""
                                });
                            }
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
        public int ActualizarCierre(int idSalidaConsol, int IdTramitador,int IdEmpTransportista, string rfc)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;

            int resultado = 0;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_UPDATE_CIERRE_SALIDASCONSOL";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdSalidasConsol", idSalidaConsol );
                cmd.Parameters.AddWithValue("@IdTramitador", IdTramitador == 0 ? DBNull.Value : IdTramitador);
                cmd.Parameters.AddWithValue("@IdEmpTransportista", IdEmpTransportista == 0 ? DBNull.Value : IdEmpTransportista);
                cmd.Parameters.AddWithValue("@RFC", rfc);

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                resultado = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cmd.Parameters.Clear();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cmd.Parameters.Clear();
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_CIERRE_SALIDASCONSOL");
            }
            return resultado;
        }
        public int Modificar_IDDODA(int IdSalidasConsol, int IdDoda)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;

            try
            {
                cmd.CommandText = "Pocket.NET_UPDATE_SalidasConsol_DODA_Pocket";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@IdDoda", SqlDbType.Int);
                param.Value = IdDoda;

                param = cmd.Parameters.Add("@IdSalidasConsol", SqlDbType.Int);
                param.Value = IdSalidasConsol;


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
                throw new Exception(ex.Message + " Pocket.NET_UPDATE_SalidasConsol_DODA_Pocket");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }
    }
}
