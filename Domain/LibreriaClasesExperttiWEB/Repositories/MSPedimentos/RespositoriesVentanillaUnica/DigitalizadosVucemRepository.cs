using DocumentFormat.OpenXml.Drawing.Charts;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica
{
   
    public class DigitalizadosVucemRepository : IDigitalizadosVucemRepository
    {
        public string SConexion { get; set; }
        string IDigitalizadosVucemRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;       

        //public string SConexion { get; set; }
        //public IConfiguration _configuration;
        public SqlConnection con;
        public ControldeEventosRepository eventosRepository;

        public DigitalizadosVucemRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DigitalizadosVucem Buscar(int IdDigitalizadosVucem)
        {
            var objDigitalizadosVucem = new DigitalizadosVucem();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_DIGITALIZADOSVUCEM_Id";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @IDReferencia INT,

            @param = cmd.Parameters.Add("@IdDigitalizadosVucem", SqlDbType.Int, 4);
            @param.Value = IdDigitalizadosVucem;



            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objDigitalizadosVucem.IdDigitalizadosVucem = Convert.ToInt32(dr["IdDigitalizadosVucem"]);
                    objDigitalizadosVucem.IDDocumento = Convert.ToInt32(dr["IDDocumento"]);
                    objDigitalizadosVucem.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                    objDigitalizadosVucem.Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                    objDigitalizadosVucem.eDocument = dr["eDocument"].ToString();
                    objDigitalizadosVucem.numeroDeTramite = dr["numeroDeTramite"].ToString();
                    objDigitalizadosVucem.NoOperacion = Convert.ToInt32(dr["NoOperacion"]);
                    objDigitalizadosVucem.RFCSello = dr["RFCSello"].ToString();
                    objDigitalizadosVucem.ErrorArchivo = Convert.ToBoolean(dr["ErrorArchivo"]);
                    objDigitalizadosVucem.EnviadoSAT = Convert.ToBoolean(dr["EnviadoSAT"]);
                    objDigitalizadosVucem.Extension = dr["Extension"].ToString();
                    objDigitalizadosVucem.Archivo = dr["Archivo"].ToString();
                    objDigitalizadosVucem.URL = dr["URL"].ToString();
                    objDigitalizadosVucem.HashDoc = dr["HashDoc"].ToString();
                    objDigitalizadosVucem.FirmaBase64 = dr["FirmaBase64"].ToString();
                    objDigitalizadosVucem.URLviejo = dr["URLviejo"].ToString();
                }
                else
                {
                    objDigitalizadosVucem = default;
                }
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objDigitalizadosVucem;
        }
        public DigitalizadosVucem Buscar(int IDReferencia, int IdDocumento)
        {
            var objDigitalizadosVucem = new DigitalizadosVucem();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_DIGITALIZADOSVUCEM";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @IDReferencia INT,

            @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
            @param.Value = IDReferencia;

            // @IdDocumento INT
            @param = cmd.Parameters.Add("@IdDocumento", SqlDbType.Int, 4);
            @param.Value = IdDocumento;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objDigitalizadosVucem.IdDigitalizadosVucem = Convert.ToInt32(dr["IdDigitalizadosVucem"]);
                    objDigitalizadosVucem.IDDocumento = Convert.ToInt32(dr["IDDocumento"]);
                    objDigitalizadosVucem.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                    objDigitalizadosVucem.Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                    objDigitalizadosVucem.eDocument = dr["eDocument"].ToString();
                    objDigitalizadosVucem.numeroDeTramite = dr["numeroDeTramite"].ToString();
                    objDigitalizadosVucem.NoOperacion = Convert.ToInt32(dr["NoOperacion"]);
                    objDigitalizadosVucem.RFCSello = dr["RFCSello"].ToString();
                    objDigitalizadosVucem.ErrorArchivo = Convert.ToBoolean(dr["ErrorArchivo"]);
                    objDigitalizadosVucem.EnviadoSAT = Convert.ToBoolean(dr["EnviadoSAT"]);
                    objDigitalizadosVucem.Extension = dr["Extension"].ToString();
                    objDigitalizadosVucem.Archivo = dr["Archivo"].ToString();
                    objDigitalizadosVucem.URL = dr["URL"].ToString();
                    objDigitalizadosVucem.HashDoc = dr["HashDoc"].ToString();
                    objDigitalizadosVucem.FirmaBase64 = dr["FirmaBase64"].ToString();
                    objDigitalizadosVucem.URLviejo = dr["URLviejo"].ToString();
                }
                else
                {
                    objDigitalizadosVucem = default;
                }
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objDigitalizadosVucem;
        }
        public int Modificar(DigitalizadosVucem ldigitalizadosvucem)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_DIGITALIZADOSVUCEM_NEW";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@IdDigitalizadosVucem  int
            @param = cmd.Parameters.Add("@IdDigitalizadosVucem", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.IdDigitalizadosVucem;

            // ,@Consecutivo  int
            @param = cmd.Parameters.Add("@Consecutivo", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.Consecutivo;

            // ,@eDocument  varchar
            @param = cmd.Parameters.Add("@eDocument", SqlDbType.VarChar, 30);
            @param.Value = ldigitalizadosvucem.eDocument;

            // ,@numeroDeTramite  varchar
            @param = cmd.Parameters.Add("@numeroDeTramite", SqlDbType.VarChar, 30);
            @param.Value = ldigitalizadosvucem.numeroDeTramite;

            // ,@NoOperacion  int
            @param = cmd.Parameters.Add("@NoOperacion", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.NoOperacion;



            // ,@RFCSello  varchar
            @param = cmd.Parameters.Add("@RFCSello", SqlDbType.VarChar, 50);
            @param.Value = ldigitalizadosvucem.RFCSello;

            // ,@ErrorArchivo  bit
            @param = cmd.Parameters.Add("@ErrorArchivo", SqlDbType.Bit, 4);
            @param.Value = ldigitalizadosvucem.ErrorArchivo;

            // ,@EnviadoSAT  bit
            @param = cmd.Parameters.Add("@EnviadoSAT", SqlDbType.Bit, 4);
            @param.Value = ldigitalizadosvucem.EnviadoSAT;


            // ,@NoHojas int
            @param = cmd.Parameters.Add("@NoHojas", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.NoHojas;

            // ,@Extension varchar(5)
            @param = cmd.Parameters.Add("@Extension", SqlDbType.VarChar, 5);
            @param.Value = ldigitalizadosvucem.Extension;

            //// ,@FechaEnvio bit
            //@param = cmd.Parameters.Add("@FechaEnvio", SqlDbType.Bit, 1);
            //@param.Value = ldigitalizadosvucem.FechaEnvio;


            //// ,@FechaRecibido bit
            //@param = cmd.Parameters.Add("@FechaRecibido", SqlDbType.Bit, 1);
            //@param.Value = ldigitalizadosvucem.FechaRecibido;

            // ,@FechaEnvio datatime
            @param = cmd.Parameters.Add("@FechaEnvio", SqlDbType.DateTime);
            @param.Value = ldigitalizadosvucem.FechaEnvio;


            // ,@FechaRecibido datatime
            @param = cmd.Parameters.Add("@FechaRecibido", SqlDbType.DateTime);
            @param.Value = ldigitalizadosvucem.FechaRecibido;

            // ,@HashDoc VARCHAR(42)
            @param = cmd.Parameters.Add("@HashDoc", SqlDbType.VarChar, 42);
            @param.Value = ldigitalizadosvucem.HashDoc;

            // ,@FirmaBase64 VARCHAR(345)
            @param = cmd.Parameters.Add("@FirmaBase64", SqlDbType.VarChar, 345);
            @param.Value = ldigitalizadosvucem.FirmaBase64;


            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_DIGITALIZADOSVUCEM_NEW");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }
        public int ModificarS3(int IdDigitalizadosVucem, int idDocumentoS3)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_DIGITALIZADOSVUCEM_S3";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@IdDigitalizadosVucem  int
            @param = cmd.Parameters.Add("@IdDigitalizadosVucem", SqlDbType.Int, 4);
            @param.Value = IdDigitalizadosVucem;

            // ,@Consecutivo  int
            @param = cmd.Parameters.Add("@idDocumentoS3", SqlDbType.Int, 4);
            @param.Value = idDocumentoS3;



            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_DIGITALIZADOSVUCEM_S3");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }
        public int Insertar(DigitalizadosVucem ldigitalizadosvucem)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_DIGITALIZADOSVUCEM";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // ,@IDDocumento  int
            @param = cmd.Parameters.Add("@IDDocumento", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.IDDocumento;

            // ,@IDReferencia  int
            @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.IDReferencia;

            // ,@Consecutivo  int
            @param = cmd.Parameters.Add("@Consecutivo", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.Consecutivo;

            // ,@eDocument  varchar
            @param = cmd.Parameters.Add("@eDocument", SqlDbType.VarChar, 30);
            @param.Value = ldigitalizadosvucem.eDocument;

            // ,@numeroDeTramite  varchar
            @param = cmd.Parameters.Add("@numeroDeTramite", SqlDbType.VarChar, 30);
            @param.Value = ldigitalizadosvucem.numeroDeTramite;

            // ,@NoOperacion  int
            @param = cmd.Parameters.Add("@NoOperacion", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.NoOperacion;

            // ,@RFCSello  varchar
            @param = cmd.Parameters.Add("@RFCSello", SqlDbType.VarChar, 50);
            @param.Value = ldigitalizadosvucem.RFCSello;

            // ,@ErrorArchivo  bit
            @param = cmd.Parameters.Add("@ErrorArchivo", SqlDbType.Bit, 4);
            @param.Value = ldigitalizadosvucem.ErrorArchivo;

            // ,@EnviadoSAT  bit
            @param = cmd.Parameters.Add("@EnviadoSAT", SqlDbType.Bit, 4);
            @param.Value = ldigitalizadosvucem.EnviadoSAT;

            // ,@NoHojas int
            @param = cmd.Parameters.Add("@NoHojas", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.NoHojas;

            // ,@Extension varchar(5)
            @param = cmd.Parameters.Add("@Extension", SqlDbType.VarChar, 5);
            @param.Value = ldigitalizadosvucem.Extension;



            @param = cmd.Parameters.Add("@Complemento", SqlDbType.VarChar, 250);
            @param.Value = ldigitalizadosvucem.Complemento;


            @param = cmd.Parameters.Add("@idDocumentoPorGuia", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.idDocumentoPorGuia;


            @param = cmd.Parameters.Add("@IdTipoDocumento", SqlDbType.Int, 4);
            @param.Value = ldigitalizadosvucem.IdTipoDocumento;



            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_DIGITALIZADOSVUCEM");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public List<DigitalizadosVucem> Cargar(int IdReferencia)
        {
            List<DigitalizadosVucem> response = new List<DigitalizadosVucem>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_DIGITALIZADOSVUCEM";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            @param = cmd.Parameters.Add("@IDREFERENCIA", SqlDbType.Int, 4);
            @param.Value = IdReferencia;

            try
            {
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var objDigitalizadosVucem = new DigitalizadosVucem
                        {
                            IdDigitalizadosVucem = Convert.ToInt32(dr["IdDigitalizadosVucem"]),
                            IDDocumento = Convert.ToInt32(dr["IDDocumento"]),
                            IDReferencia = Convert.ToInt32(dr["Id"]),
                            eDocument = dr["eDocument"].ToString(),
                            numeroDeTramite = dr["numeroDeTramite"].ToString(),
                            NoOperacion = Convert.ToInt32(dr["NoOperacion"]),
                            
                            Extension = dr["Extension"].ToString(),
                            Archivo = dr["Archivo"].ToString(),
                            URL = dr["URL"].ToString(),
                            
                            
                            
                            Complemento = dr["Complemento"].ToString(),
                        };
                        response.Add(objDigitalizadosVucem);
                    }
                }
                else
                {
                    response = default;
                }

                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_LOAD_DIGITALIZADOSVUCEM");
            }
            return response;
        }

        public List<DigitalizadosVucemVerificacion> CargarParaVerificacion(int idReferencia)
        {
            var resultado = new List<DigitalizadosVucemVerificacion>();

            try
            {
                using (SqlConnection cn = new SqlConnection(SConexion))
                using (SqlCommand cmd = new SqlCommand("NET_LOAD_DIGITALIZADOSVUCEM_DESDE_VERIFICACION", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDREFERENCIA", SqlDbType.Int).Value = idReferencia;

                    cn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new DigitalizadosVucemVerificacion
                            {
                                IdDigitalizadosVucem = Convert.ToInt32(reader["IdDigitalizadosVucem"]),
                                Id = Convert.ToInt32(reader["Id"]),
                                Documento = reader["Documento"].ToString(),
                                NoOperacion = Convert.ToInt32(reader["NoOperacion"]),
                                eDocument = reader["eDocument"].ToString(),
                                numeroDeTramite = Convert.ToInt32(reader["numeroDeTramite"]),
                                Archivo = reader["Archivo"].ToString(),
                                URL = reader["URL"].ToString(),
                                Encriptado = reader["Encriptado"].ToString()
                            };

                            resultado.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message} NET_LOAD_DIGITALIZADOSVUCEM");
            }

            return resultado;
        }

        //atg 
        public List<DigitalizadosVucem> CargarMV(string NumerodeReferencia)
        {
            int idReferencia = 0;
            

            List<DigitalizadosVucem> lstDigitalizadosVucem = new();

            try
            {
              

              

                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_DIGITALIZADOSVUCEM_MV", con);
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NumerodeReferencia;

                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DigitalizadosVucem objDigitalizadosVucem = new();

                            objDigitalizadosVucem.IdDigitalizadosVucem = Convert.ToInt32(dr["IdDigitalizadosVucem"]);
                            objDigitalizadosVucem.IDDocumento = Convert.ToInt32(dr["IDDocumento"]);
                            objDigitalizadosVucem.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                            objDigitalizadosVucem.Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                            objDigitalizadosVucem.eDocument = Convert.ToString(dr["eDocument"]);
                            objDigitalizadosVucem.numeroDeTramite = Convert.ToString(dr["numeroDeTramite"]);
                            objDigitalizadosVucem.NoOperacion = Convert.ToInt32(dr["NoOperacion"]);
                            objDigitalizadosVucem.FechaEnvioDate = Convert.ToDateTime(dr["FechaEnvio"]);
                            objDigitalizadosVucem.FechaRecibidoDate = Convert.ToDateTime(dr["FechaRecibido"]);
                            objDigitalizadosVucem.RFCSello = Convert.ToString(dr["RFCSello"]);
                            objDigitalizadosVucem.ErrorArchivo = Convert.ToBoolean(dr["ErrorArchivo"]);
                            objDigitalizadosVucem.EnviadoSAT = Convert.ToBoolean(dr["EnviadoSAT"]);
                            objDigitalizadosVucem.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                            objDigitalizadosVucem.NoHojas = Convert.ToInt32(dr["NoHojas"]);
                            objDigitalizadosVucem.Extension = Convert.ToString(dr["Extension"]);
                            objDigitalizadosVucem.HashDoc = Convert.ToString(dr["HashDoc"]);
                            objDigitalizadosVucem.FirmaBase64 = Convert.ToString(dr["FirmaBase64"]);
                            objDigitalizadosVucem.IdDigitalizadosVucemPermiso = Convert.ToInt32(dr["IdDigitalizadosVucemPermiso"]);
                            objDigitalizadosVucem.idDocumentoS3 = Convert.ToInt32(dr["idDocumentoS3"]);
                            objDigitalizadosVucem.idDocumentoAcuse = Convert.ToInt32(dr["idDocumentoAcuse"]);
                            objDigitalizadosVucem.Complemento = dr["Complemento"] != DBNull.Value ? Convert.ToString(dr["Complemento"]) : string.Empty;
                            objDigitalizadosVucem.idDocumentoPorGuia = dr["idDocumentoPorGuia"] != DBNull.Value ? Convert.ToInt32(dr["idDocumentoPorGuia"]) : 0;
                            objDigitalizadosVucem.IdTipoDocumento = Convert.ToInt32(dr["IdTipoDocumento"]);

                           
                                if (objDigitalizadosVucem.idDocumentoS3 != 0)
                                {
                                    DocumentosporGuia documentosporGuia = new DocumentosporGuia();
                                    DocumentosporGuiaRepository documentosporGuiaRepository = new(_configuration);
                                    documentosporGuia = documentosporGuiaRepository.BuscarPorId(objDigitalizadosVucem.idDocumentoS3);

                                    string vURL = string.Empty;
                                    BucketsS3Repository objS3 = new BucketsS3Repository(_configuration);

                                    if (documentosporGuia == null)
                                    {
                                        vURL = string.Empty;
                                    }
                                    else
                                    {
                                        vURL = objS3.URL(documentosporGuia.RutaS3, "grupoei.documentos");
                                    }

                                objDigitalizadosVucem.linkS3 = vURL;
                                }
                                else { objDigitalizadosVucem.linkS3 = string.Empty; }

                        



                            lstDigitalizadosVucem.Add(objDigitalizadosVucem);
                        }
                    }
                    else
                    {
                        lstDigitalizadosVucem.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }           

            return lstDigitalizadosVucem;
        }
        //atg
    }
}
