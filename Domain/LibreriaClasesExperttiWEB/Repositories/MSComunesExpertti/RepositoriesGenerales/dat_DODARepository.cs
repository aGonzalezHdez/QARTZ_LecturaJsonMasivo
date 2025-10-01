using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales
{
    public class dat_DODARepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public dat_DODARepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public ent_DODA BuscarRemesa(int IdReferencia, int NoRemesa)
        {
            var objdoda = new ent_DODA();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_SEARCH_DODA_REMESA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                @param.Value = IdReferencia;

                @param = cmd.Parameters.Add("@NoRemesa", SqlDbType.Int, 4);
                @param.Value = NoRemesa;


                cn.ConnectionString = SConexion;
                cn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
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
                }
                else
                {
                    objdoda = default;
                }
                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objdoda;
        }

        public ent_DODA ExisteenDODA(int idReferencia, int NoRemesa)
        {
            ent_DODA objdoda = null;
            var cmd = new SqlCommand();
            var cn = new SqlConnection();
            SqlDataReader reader;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_SEARCH_DODA_XIdReferencia_REMESA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdReferencia", idReferencia);
                cmd.Parameters.AddWithValue("@NoRemesa", NoRemesa);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    objdoda = new ent_DODA
                    {
                        _IdDODA = Convert.ToInt32(reader["idDODA"]),
                        _N_Ticket = reader["N_Ticket"].ToString(),
                        _N_Integracion = reader["N_Integracion"].ToString(),
                        _Patente = reader["Patente"].ToString(),
                        _FechaEmision = Convert.ToDateTime(reader["FechaEmision"]),
                        _N_Pedimentos = Convert.ToInt32(reader["N_Pedimentos"]),
                        _NAduana = Convert.ToInt32(reader["NAduana"]),
                        _NoAduana = reader["NoAduana"].ToString(),
                        _CadenaOriginal = reader["CadenaOriginal"].ToString(),
                        _NSerieCertificado = reader["NSerieCertificado"].ToString(),
                        _SelloDigital = reader["SelloDigital"].ToString(),
                        _NSerieCertificadoSAT = reader["NSerieCertificadoSAT"].ToString(),
                        _SelloDigitalSAT = reader["SelloDigitalSAT"].ToString(),
                        _CadenaOriginalSAT = reader["CadenaOriginalSAT"].ToString(),
                        _UsuarioCiec = reader["UsuarioCiec"].ToString(),
                        _Link = reader["Link"].ToString(),
                        _RutaArchivo = reader["RutaArchivo"].ToString()
                    };
                }


                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_SEARCH_DODA_XIdReferencia_REMESA");

            }
            cn.Close();
            cn.Dispose();
            return objdoda;
        }
        public ent_DODA ExisteenDODACOVE(int idReferencia, string NumeroDeCOVE)
        {
            ent_DODA objdoda = null;
            var cmd = new SqlCommand();
            var cn = new SqlConnection();
            SqlDataReader reader;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_SEARCH_DODA_xCOVE";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdReferencia", idReferencia);
                cmd.Parameters.AddWithValue("@NumeroDeCOVE", NumeroDeCOVE);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    objdoda = new ent_DODA
                    {
                        _IdDODA = Convert.ToInt32(reader["idDODA"]),
                        _N_Ticket = reader["N_Ticket"].ToString(),
                        _N_Integracion = reader["N_Integracion"].ToString(),
                        _Patente = reader["Patente"].ToString(),
                        _FechaEmision = Convert.ToDateTime(reader["FechaEmision"]),
                        _N_Pedimentos = Convert.ToInt32(reader["N_Pedimentos"]),
                        _NAduana = Convert.ToInt32(reader["NAduana"]),
                        _NoAduana = reader["NoAduana"].ToString(),
                        _CadenaOriginal = reader["CadenaOriginal"].ToString(),
                        _NSerieCertificado = reader["NSerieCertificado"].ToString(),
                        _SelloDigital = reader["SelloDigital"].ToString(),
                        _NSerieCertificadoSAT = reader["NSerieCertificadoSAT"].ToString(),
                        _SelloDigitalSAT = reader["SelloDigitalSAT"].ToString(),
                        _CadenaOriginalSAT = reader["CadenaOriginalSAT"].ToString(),
                        _UsuarioCiec = reader["UsuarioCiec"].ToString(),
                        _Link = reader["Link"].ToString(),
                        _RutaArchivo = reader["RutaArchivo"].ToString(),
                        Detener = Convert.ToBoolean(reader["Detener"])
                    };
                }


                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_SEARCH_DODA_xCOVE");

            }
            cn.Close();
            cn.Dispose();
            return objdoda;
        }
        public List<CPedimento> ObtenerPedimento(int idRelacionEnvio, int remesa)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();

            List<CPedimento> listado = new List<CPedimento>();
            SqlDataReader reader;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_PEDIMENTO_DODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdReferencia", idRelacionEnvio);
                cmd.Parameters.AddWithValue("@NoRemesa", remesa);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listado.Add(new CPedimento
                        {
                            _Patente = reader["PATENTE"].ToString(),
                            _NumeroRemesa = reader["REMESA"].ToString(),
                            _dtaNiu = reader["NIU"].ToString(),
                            _ImporteDifDolares = reader["IMPORTE_NO_EFECTIVO"].ToString(),
                            _ImporteEfectivoDolares = reader["IMPORTE_DOLARES"].ToString(),
                            _umc = reader["CANTIDAD"].ToString(),
                            _Campo12Apendice = reader["ARTICULO7"].ToString(),
                            _Cove = reader["COVE"].ToString(),
                            _Documento = reader["Documento"].ToString()
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
                throw new Exception(ex.Message.ToString() + " NET_LOAD_PEDIMENTO_DODA");
            }
            return listado;
        }
        public List<string> ObtenerAduana(int idAduana)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();

            List<string> listado = new List<string>();
            SqlDataReader reader;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_ADUANA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Aduana", idAduana);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listado.Add(reader["DESCRIP"].ToString());
                    }
                }

                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_LOAD_PEDIMENTO_DODA");
            }
            return listado;
        }
        public int dt_InsertarDoda(ent_DODA ent_DODA, int idPredoda)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();

            int dtrow = 0;

            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "NET_INSERT_DATOSDODAPITA_Seccion_ExpertitWEB";
                cmd.Connection =   cn; 
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@N_Integracion", SqlDbType.VarChar)).Value = ent_DODA._N_Integracion;
                cmd.Parameters.Add(new SqlParameter("@N_Ticket", SqlDbType.VarChar)).Value = ent_DODA._N_Ticket;
                cmd.Parameters.Add(new SqlParameter("@Patente", SqlDbType.VarChar)).Value = ent_DODA._Patente;
                cmd.Parameters.Add(new SqlParameter("@N_Pedimentos", SqlDbType.Int)).Value = ent_DODA._N_Pedimentos;
                cmd.Parameters.Add(new SqlParameter("@N_Aduana", SqlDbType.Int)).Value = ent_DODA._NAduana;
                cmd.Parameters.Add(new SqlParameter("@Nombre_Aduana", SqlDbType.VarChar)).Value = ent_DODA._NoAduana;
                cmd.Parameters.Add(new SqlParameter("@CadenaOriginal", SqlDbType.VarChar)).Value = ent_DODA._CadenaOriginal;
                cmd.Parameters.Add(new SqlParameter("@NSerieCertificado", SqlDbType.VarChar)).Value = ent_DODA._NSerieCertificado;
                cmd.Parameters.Add(new SqlParameter("@SelloDigital", SqlDbType.VarChar)).Value = ent_DODA._SelloDigital;
                cmd.Parameters.Add(new SqlParameter("@NSerieCertificadoSAT", SqlDbType.VarChar)).Value = ent_DODA._NSerieCertificadoSAT;
                cmd.Parameters.Add(new SqlParameter("@SelloDigitalSAT", SqlDbType.VarChar)).Value = ent_DODA._SelloDigitalSAT;
                cmd.Parameters.Add(new SqlParameter("@CadenaOriginalSAT", SqlDbType.VarChar)).Value = ent_DODA._CadenaOriginalSAT;
                cmd.Parameters.Add(new SqlParameter("@UsuarioCiec", SqlDbType.VarChar)).Value = ent_DODA._UsuarioCiec;
                cmd.Parameters.Add(new SqlParameter("@Link", SqlDbType.VarChar)).Value = ent_DODA._Link;
                cmd.Parameters.Add(new SqlParameter("@Ruta", SqlDbType.VarChar)).Value = ent_DODA._FechaEmision.ToString("yyyy") + ent_DODA._FechaEmision.ToString("MM") + "\\" + ent_DODA._FechaEmision.ToString("dd") + "\\";
                // dat_conexion.SqlCommand.Parameters.Add(new SqlParameter("@Img", SqlDbType.Image)).Value = ent_DODA._Img;
                cmd.Parameters.Add(new SqlParameter("@DespachoAduanero", SqlDbType.TinyInt)).Value = ent_DODA._DespachoAduanero;

                if (ent_DODA._NumeroGafeteUnico == null)
                {
                    cmd.Parameters.Add(new SqlParameter("@Gafeteunico", SqlDbType.VarChar)).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@Gafeteunico", SqlDbType.VarChar)).Value = ent_DODA._NumeroGafeteUnico;
                }

                cmd.Parameters.Add(new SqlParameter("@Placas", SqlDbType.VarChar, 10)).Value = ent_DODA.Placas;
                cmd.Parameters.Add(new SqlParameter("@CAAT", SqlDbType.VarChar, 4)).Value = ent_DODA.CAAT;
                cmd.Parameters.Add(new SqlParameter("@Seccion", SqlDbType.VarChar, 4)).Value = ent_DODA._Seccion;
                cmd.Parameters.Add(new SqlParameter("@idPredoda", SqlDbType.Int, 4)).Value = idPredoda;
                cmd.Parameters.Add(new SqlParameter("@Operacion", SqlDbType.Int, 4)).Value = ent_DODA.Operacion;

                dtrow = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
            }
            return dtrow;
        }
        public int dt_InsertarPedimentDoda(DataTable dt, int IdUsuario)
        {

            int dtrow = 0;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();

            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "NET_INSERT_DATOSTABLADODA_COVE";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@Tabla", SqlDbType.Structured)).Value = dt;
                cmd.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int, 4)).Value = IdUsuario;
                dtrow = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            return dtrow;
        }
        public int neg_InsertarDODANew(ent_DODA entDODA, int IdUsuario, int idPredoda)
        {
            
            int rows;
            int idDoda = 0;

            try
            {

                idDoda = dt_InsertarDoda(entDODA, idPredoda);

                DataTable dtpedimentos = new DataTable();
                dtpedimentos.Columns.Add("IdDODA", typeof(int));
                dtpedimentos.Columns.Add("IdReferencia", typeof(int));
                dtpedimentos.Columns.Add("NoRemesa", typeof(int));
                dtpedimentos.Columns.Add("COVE", typeof(string));

                foreach (DodaRemesa item in entDODA._ent_Lista_DODANew._ListaIdReferencia)
                {
                    DataRow row = dtpedimentos.NewRow();
                    row["IdDODA"] = idDoda;
                    row["IdReferencia"] = item.IdReferencia;
                    row["NoRemesa"] = item.Remesa;
                    row["COVE"] = item.Cove??"";
                    dtpedimentos.Rows.Add(row);
                }

                rows = dt_InsertarPedimentDoda( dtpedimentos, IdUsuario);
                
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
            finally
            {
              
            }

            return idDoda;
        }
        public int neg_UpdateIntegracion(ent_DODA entDoda, bool ERRORES, string DESCRIPCIONERROR)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            var dtb = new DataTable();
            SqlDataReader reader;
            int dtrow = 0;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_UPDATE_DODA_INTEGRACION";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@IDDODA", SqlDbType.Int)).Value = entDoda._IdDODA;
                cmd.Parameters.Add(new SqlParameter("@N_Integracion", SqlDbType.VarChar, 30)).Value = entDoda._N_Integracion;
                cmd.Parameters.Add(new SqlParameter("@ERRORES", SqlDbType.Bit)).Value = ERRORES;
                cmd.Parameters.Add(new SqlParameter("@DESCRIPCIONERROR", SqlDbType.VarChar, 400)).Value = DESCRIPCIONERROR;
                cmd.Parameters.Add(new SqlParameter("@CadenaOriginalSAT", SqlDbType.VarChar, -1)).Value = entDoda._CadenaOriginalSAT;
                cmd.Parameters.Add(new SqlParameter("@SelloDigitalSAT", SqlDbType.VarChar, -1)).Value = entDoda._SelloDigitalSAT;
                cmd.Parameters.Add(new SqlParameter("@NSerieCertificadoSAT", SqlDbType.VarChar, -1)).Value = entDoda._NSerieCertificadoSAT;
                cmd.Parameters.Add(new SqlParameter("@Link", SqlDbType.VarChar, -1)).Value = entDoda._Link;
                cmd.Parameters.Add(new SqlParameter("@FechaEmision", SqlDbType.DateTime)).Value = entDoda._FechaEmision;
                cmd.Parameters.Add(new SqlParameter("@Img", SqlDbType.Image)).Value = entDoda._Img;

                dtrow = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_DODA_INTEGRACION");
            }
            cn.Close();
            cn.Dispose();
            return dtrow;
        }
        public DataTable Cargar(int IdDODA)
        {
            DataTable dtb = new DataTable();
            SqlParameter param;

            try
            {
                using (SqlConnection cn = new SqlConnection(SConexion))
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand
                        {
                            Connection = cn,
                            CommandText = "NET_LOAD_DODA",
                            CommandType = CommandType.StoredProcedure
                        }
                    };

                    param = dap.SelectCommand.Parameters.Add("@IdDODA", SqlDbType.Int, 4);
                    param.Value = IdDODA;

                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message+ " NET_LOAD_DODA");
            }

            return dtb;
        }

        public ent_DODA Buscar(int IdDODA)
        {
            ent_DODA objdoda = new ent_DODA();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_SEARCH_DODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@IdDODA", SqlDbType.Int, 4);
                param.Value = IdDODA;

                cn.ConnectionString = SConexion;
                cn.Open();
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
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
                    objdoda._DespachoAduanero = Convert.ToInt32(dr["DespachoAduanero"]);
                    objdoda._NumeroGafeteUnico = dr["NumeroGafeteUnico"].ToString();
                    objdoda.Placas = dr["Placas"].ToString();
                    objdoda.CAAT = dr["CAAT"].ToString();
                    objdoda.IdPredoda = Convert.ToInt32(dr["IdPredoda"]);
                    // objdoda._ACTIVO = Convert.ToBoolean(dr["ACTIVO"]);
                }
                else
                {
                    objdoda = null;
                }

                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return objdoda;
        }


    }

}
