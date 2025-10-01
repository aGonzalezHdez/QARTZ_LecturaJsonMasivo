using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using Microsoft.Extensions.Configuration;
using NPOI.HSSF.Record;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class RelacionBitacoraRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public RelacionBitacoraRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<PredodaDetalle> CargarDetalleDoda(int  idRelacionBitacora)
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
                cmd.CommandText = "fdx.NET_LOAD_BitacoraDetallePorRelacion_ID";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRelacionBitacora", idRelacionBitacora);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["idReferencia"] != DBNull.Value)
                        {
                            detalles.Add(new PredodaDetalle
                            {
                                IdReferencia = Convert.ToInt32(reader["idReferencia"]),
                                Remesa = reader["Remesa"]!=DBNull.Value?Convert.ToInt32(reader["Remesa"]):0,
                                NumeroDeCOVE = reader["Cove"]!=DBNull.Value?reader["Cove"].ToString():"",
                                Referencia = reader["Referencia"].ToString(),
                                Pedimento = reader["Pedimento"]!= DBNull.Value?reader["Pedimento"].ToString():""
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
                throw new Exception(ex.Message.ToString() + " NET_LOAD_BitacoraDetallePorRelacion_ID");

            }
            cn.Close();
            cn.Dispose();
            return detalles;
        }
        public int Insertar(int IdEstacion, int IdUsuario, int Proceso, int IDDatosDeEmpresa)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("fdx.NET_INSERT_RelacionBitacora", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdEstacion", SqlDbType.Int).Value = IdEstacion;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                cmd.Parameters.Add("@Proceso", SqlDbType.Int).Value = Proceso;
                cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int).Value = IDDatosDeEmpresa;

                SqlParameter newIdRegistroParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                newIdRegistroParam.Direction = ParameterDirection.Output;

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(newIdRegistroParam.Value);
                }
                catch (Exception ex)
                {
                    id = 0;
                    throw new Exception(ex.Message + " fdx.NET_INSERT_RelacionBitacora");
                }
            }

            return id;
        }

        public int ActualizarPredoda(int idRelacionBitacora,int idPredoda)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;

            int resultado = 0;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "fdx.NET_UPDATE_RELACIONBITACORA_IDPREDODADODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdRelacionBitacora", idRelacionBitacora);
                cmd.Parameters.AddWithValue("@IdPredoda", idPredoda);
                

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                resultado = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " fdx.NET_UPDATE_RELACIONBITACORA_IDPREDODADODA");
            }
            return resultado;
        }
        public RelacionBitacoraEntity Buscar(int idRealacionBitacora,int estatus)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader reader;
            RelacionBitacoraEntity item = null;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "fdx.NET_SEARCH_RelacionBitacora";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRelacionBitacora", idRealacionBitacora);
                cmd.Parameters.AddWithValue("@Estatus", estatus);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    item = new RelacionBitacoraEntity
                    {
                        IdRelacionBitacora = Convert.ToInt32(reader["IdRelacionBitacora"]),
                        RelacionBitacora = reader["RelacionBitacora"].ToString(),
                        IdEstacion = Convert.ToInt32(reader["IdEstacion"]),
                        IdUsuario = Convert.ToInt32(reader["IdUsuario"].ToString()),
                        Consecutivo = Convert.ToInt32(reader["Consecutivo"]),
                        Proceso = Convert.ToInt32(reader["Proceso"]),
                        FechaBitacora = Convert.ToDateTime(reader["FechaBitacora"]),
                        Estatus = Convert.ToInt32(reader["Estatus"]),
                        FechaCierre = Convert.ToDateTime(reader["FechaCierre"]),
                        IdEmpTransportista = reader["IdEmpTransportista"].ToString(),
                        IdTramitador = Convert.ToInt32(reader["IdTramitador"]),
                        RFC = reader["rfc"]== DBNull.Value?"": reader["rfc"].ToString(),

                    };
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " fdx.NET_SEARCH_RelacionBitacora");

            }
            cn.Close();
            cn.Dispose();
            return item;
        }
        public int Modificar_IDDODA(int IdRelacionBitacora, int IdDoda)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;

            try
            {
                cmd.CommandText = "fdx.NET_UPDATE_RelacionBitacora_IDDODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@IdRelacionBitacora", SqlDbType.Int);
                param.Value = IdRelacionBitacora;

                param = cmd.Parameters.Add("@IdDoda", SqlDbType.Int);
                param.Value = IdDoda;

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
                throw new Exception(ex.Message + " fdx.NET_UPDATE_RelacionBitacora_IDDODA");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public int ActualizarRfc(int idRelacionBitacora,int idSalidaConsol, string rfc)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;

            int resultado = 0;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "fdx.NET_UPDATE_RFC_RelacionBitacora_DespachoIntegrado";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdRelacionBitacora", idRelacionBitacora==0?DBNull.Value: idRelacionBitacora);
                cmd.Parameters.AddWithValue("@IdSalidasConsol", idSalidaConsol==0?DBNull.Value:idSalidaConsol);
                cmd.Parameters.AddWithValue("@rfc", rfc);

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
                throw new Exception(ex.Message.ToString() + " fdx.NET_UPDATE_RFC_RelacionBitacora_DespachoIntegrado");
            }
            return resultado;
        }

        public List<RelacionBitacoraEntity> CargaxFecha(int idEstacion,DateTime fecha)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            var dtb = new DataTable();
            SqlDataReader reader;
            List<RelacionBitacoraEntity> detalles = new List<RelacionBitacoraEntity>();
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "fdx.NET_LOAD_RelacionBitacora_Fecha";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdEstacion", idEstacion);
                cmd.Parameters.AddWithValue("@Fecha", fecha);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        detalles.Add(new RelacionBitacoraEntity
                        {
                            IdRelacionBitacora = Convert.ToInt32(reader["IdRelacionBitacora"]),
                            RelacionBitacora = reader["RelacionBitacora"].ToString(),
                            IdEstacion = Convert.ToInt32(reader["IdEstacion"]),
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            Consecutivo = Convert.ToInt32(reader["Consecutivo"]),
                            Proceso = Convert.ToInt32(reader["Proceso"]),
                            FechaBitacora = Convert.ToDateTime(reader["FechaBitacora"]),
                            Estatus = Convert.ToInt32(reader["Estatus"]),
                            FechaCierre = Convert.ToDateTime(reader["FechaCierre"]),
                            IdEmpTransportista = reader["idEmpTransportista"].ToString()
                        });
                        
                    }
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " fdx.NET_LOAD_RelacionBitacora_Fecha");

            }
            cn.Close();
            cn.Dispose();
            return detalles;
        }

        public List<DetalleRelacionBitacoraFX> GetBitacoraFx(int idRelacionBitacora)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            var dtb = new DataTable();
            SqlDataReader reader;
            List<DetalleRelacionBitacoraFX> detalles = new List<DetalleRelacionBitacoraFX>();
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_DETALLE_DESPACHO_INTEGRADO_FX";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRelacionBitacora", idRelacionBitacora);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        detalles.Add(new DetalleRelacionBitacoraFX
                        {
                            Tipo = reader["Tipo"].ToString(),
                            Cliente = reader["Cliente"].ToString(),
                            Guia = reader["Guia"].ToString(),
                            Referencia = reader["Referencia"].ToString(),
                            Valor = Convert.ToDouble(reader["Valor"]),
                            Peso = Convert.ToDouble(reader["Peso"]),
                            Bultos = Convert.ToInt32(reader["Bultos"]),
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
                throw new Exception(ex.Message.ToString() + " NET_DETALLE_DESPACHO_INTEGRADO_FX");

            }
            cn.Close();
            cn.Dispose();
            return detalles;
        }
        public List<DetalleRelacionBitacoraFX> GetCierreUnidadBitacoraFx(int idRelacionBitacora)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            var dtb = new DataTable();
            SqlDataReader reader;
            List<DetalleRelacionBitacoraFX> detalles = new List<DetalleRelacionBitacoraFX>();
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_DETALLE_DESPACHO_INTEGRADO_FX_SECCIONES";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRelacionBitacora", idRelacionBitacora);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        detalles.Add(new DetalleRelacionBitacoraFX
                        {
                            Tipo = reader["Tipo"].ToString(),
                            Cliente = reader["Cliente"].ToString(),
                            Guia = reader["Guia"].ToString(),
                            Referencia = reader["Referencia"].ToString(),
                            Valor = Convert.ToDouble(reader["Valor"]),
                            Peso = Convert.ToDouble(reader["Peso"]),
                            Bultos = Convert.ToInt32(reader["Bultos"]),
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
                throw new Exception(ex.Message.ToString() + " NET_DETALLE_DESPACHO_INTEGRADO_FX");

            }
            cn.Close();
            cn.Dispose();
            return detalles;
        }
        public ent_DODA CargarBitacoraDoda(int idRealacionBitacora)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;
            ent_DODA objdoda = new ent_DODA();
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "fdx.NET_SEARCH_RelacionBitacora_DODA_Pocket";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdRelacionBitacora", idRealacionBitacora);

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    if (dr.Read()) { 
                        objdoda._IdDODA = Convert.ToInt32(dr["IdDODA"]);
                        objdoda._N_Ticket = dr["N_Ticket"].ToString();
                        objdoda._N_Integracion = dr["N_Integracion"].ToString();
                        objdoda._Patente = dr["Patente"].ToString();
                        objdoda._FechaEmision = Convert.ToDateTime(dr["FechaEmision"]);
                        objdoda._N_Pedimentos = Convert.ToInt32(dr["N_Pedimentos"]);
                        objdoda._NAduana = Convert.ToInt32(dr["NAduana"]);
                        objdoda._NoAduana = dr["NoAduana"].ToString();
                        objdoda._CadenaOriginal = dr["CadenaOriginal"].ToString();
                        objdoda._NSerieCertificado = dr["NSerieCertificado"].ToString();
                        objdoda._SelloDigital = dr["SelloDigital"].ToString();
                        objdoda._NSerieCertificadoSAT = dr["NSerieCertificadoSAT"].ToString();
                        objdoda._SelloDigitalSAT = dr["SelloDigitalSAT"].ToString();
                        objdoda._CadenaOriginalSAT = dr["CadenaOriginalSAT"].ToString();
                        objdoda._UsuarioCiec = dr["UsuarioCiec"].ToString();
                        objdoda._Link = dr["Link"].ToString();
                        objdoda._RutaArchivo = dr["RutaArchivo"].ToString();

                        objdoda._DespachoAduanero = Convert.IsDBNull(dr["DespachoAduanero"]) ? 0 : Convert.ToInt32(dr["DespachoAduanero"]);
                        objdoda._NumeroGafeteUnico = Convert.IsDBNull(dr["NumeroGafeteUnico"]) ? "" : dr["NumeroGafeteUnico"].ToString();
                        objdoda.Placas = Convert.IsDBNull(dr["Placas"]) ? "" : dr["Placas"].ToString();
                        objdoda.CAAT = Convert.IsDBNull(dr["CAAT"]) ? "" : dr["CAAT"].ToString();
                        objdoda.IdPredoda = Convert.IsDBNull(dr["IdPredoda"]) ? 0 : Convert.ToInt32(dr["IdPredoda"]);
                        objdoda.NumerodeTag = Convert.IsDBNull(dr["NumerodeTag"]) ? "" : dr["NumerodeTag"].ToString();
                        objdoda.tipo_documento_id = Convert.IsDBNull(dr["tipo_documento_id"]) ? 0 : Convert.ToInt32(dr["tipo_documento_id"]);
                        objdoda.Fast_Id = Convert.IsDBNull(dr["Fast_Id"]) ? "" : dr["Fast_Id"].ToString();
                        objdoda.datos_adicionales = Convert.IsDBNull(dr["datos_adicionales"]) ? "" : dr["datos_adicionales"].ToString();
                        objdoda.ModalidadCruce = Convert.IsDBNull(dr["ModalidadCruce"]) ? 0 : Convert.ToInt32(dr["ModalidadCruce"]);
                        objdoda.Candados = Convert.IsDBNull(dr["Candados"]) ? "" : dr["Candados"].ToString();
                        if (!Convert.IsDBNull(dr["FechaVigencia"]))
                        {
                            objdoda.FechaVigencia = Convert.ToDateTime(dr["FechaVigencia"]);
                        }
                        objdoda.ValidacionAgencia = Convert.IsDBNull(dr["ValidacionAgencia"]) ? "" : dr["ValidacionAgencia"].ToString();
                        objdoda.Operacion = Convert.IsDBNull(dr["Operacion"]) ? 0 : Convert.ToInt32(dr["Operacion"]);
                    }
                }

                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " fdx.NET_SEARCH_RelacionBitacora_DODA_Pocket");

            }
            cn.Close();
            cn.Dispose();
            return objdoda;
        }
        public int EnviarReporteFX(int idRelacionBitacora)
        {

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;

            int resultado = 0;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_REPORT_DESPACHO_INTEGRADO_FEDEX";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdRelacion", idRelacionBitacora);

                cmd.ExecuteNonQuery();
                resultado = 1;
                cmd.Parameters.Clear();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cmd.Parameters.Clear();
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_REPORT_DESPACHO_INTEGRADO_FEDEX");
            }
            return resultado;
        }
    }
}
