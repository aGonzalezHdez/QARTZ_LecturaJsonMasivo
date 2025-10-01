using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using wsVentanillaUnica;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales
{
    public class FacturasCoveRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con = null!;

        public FacturasCoveRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(FacturasCove lfacturascove)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_INSERT_FACTURASCOVE", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros
                cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4).Value = lfacturascove.IDReferencia;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = lfacturascove.CONS_FACT;
                cmd.Parameters.Add("@CadenaOriginal", SqlDbType.Text).Value = lfacturascove.CadenaOriginal ?? (object)DBNull.Value;
                cmd.Parameters.Add("@NumeroDeCOVE", SqlDbType.VarChar, 13).Value = lfacturascove.NumeroDeCOVE ?? (object)DBNull.Value;
                cmd.Parameters.Add("@FirmaDigital", SqlDbType.VarChar, 8000).Value = lfacturascove.FirmaDigital ?? (object)DBNull.Value;
                cmd.Parameters.Add("@COVE", SqlDbType.Bit).Value = lfacturascove.COVE;
                cmd.Parameters.Add("@NumeroDeOperacion", SqlDbType.Int, 4).Value = lfacturascove.NumeroDeOperacion;
                cmd.Parameters.Add("@FirmaDigitalOperacion", SqlDbType.VarChar, 8000).Value = lfacturascove.FirmaDigitalOperacion ?? (object)DBNull.Value;
                cmd.Parameters.Add("@CadenaOriginalOperacion", SqlDbType.VarChar, 8000).Value = lfacturascove.CadenaOriginalOperacion ?? (object)DBNull.Value;
                cmd.Parameters.Add("@EnviadoSAT", SqlDbType.Bit).Value = lfacturascove.EnviadoSAT;
                cmd.Parameters.Add("@numeroAdenda", SqlDbType.VarChar, 20).Value = lfacturascove.numeroAdenda ?? (object)DBNull.Value;

                var outputParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                outputParam.Direction = ParameterDirection.Output;

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    id = Convert.ToInt32(outputParam.Value);
                    if (id == -1) id = 0;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " NET_INSERT_FACTURASCOVE");
                }
            }

            return id;
        }

        public int ModificarDocAcuse(int IdReferencia, int CONS_FACT, int IdDocumento)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_FACTURASCOVE_idDocumento";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@IDReferencia  int
            @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
            @param.Value = IdReferencia;

            // ,@CONS_FACT  int
            @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
            @param.Value = CONS_FACT;


            // ,@IdDocumento  int
            @param = cmd.Parameters.Add("@IdDocumento", SqlDbType.Int, 4);
            @param.Value = IdDocumento;


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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_FACTURASCOVE_idDocumentoXML");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public int Eliminar(int Idreferencia, int ConsFact)
        {
            int id = 0;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_DELETE_FACTURASCOVE";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@IDReferencia  int
            @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
            @param.Value = Idreferencia;

            // ,@CONS_FACT  int
            @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
            @param.Value = ConsFact;


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
                throw new Exception(ex.Message.ToString() + "NET_DELETE_FACTURASCOVE");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;

        }
        public int ModificarDocCOVE(int IdReferencia, int CONS_FACT, int IdDocumento)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_FACTURASCOVE_idDocumentoCOVE";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@IDReferencia  int
            @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
            @param.Value = IdReferencia;

            // ,@CONS_FACT  int
            @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
            @param.Value = CONS_FACT;


            // ,@IdDocumento  int
            @param = cmd.Parameters.Add("@idDocumentoCove", SqlDbType.Int, 4);
            @param.Value = IdDocumento;


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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_FACTURASCOVE_idDocumentoCOVE");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public FacturasCove Buscar(int IDReferencia, int CONS_FACT)
        {
            var objFACTURASCOVE = new FacturasCove();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_FACTURASCOVE";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @IDReferencia INT,
            @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
            @param.Value = IDReferencia;

            // @CONS_FACT INT
            @param = cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4);
            @param.Value = CONS_FACT;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objFACTURASCOVE.IDFacturaCOVE = Convert.ToInt32(dr["IDFacturaCOVE"]);
                    objFACTURASCOVE.IDReferencia = Convert.ToInt32(dr["IDReferencia"]);
                    objFACTURASCOVE.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                    objFACTURASCOVE.CadenaOriginal = dr["CadenaOriginal"].ToString();
                    objFACTURASCOVE.NumeroDeCOVE = dr["NumeroDeCOVE"].ToString();
                    objFACTURASCOVE.FirmaDigital = dr["FirmaDigital"].ToString();
                    objFACTURASCOVE.COVE = Convert.ToBoolean(dr["COVE"]);
                    objFACTURASCOVE.NumeroDeOperacion = Convert.ToInt32(dr["NumeroDeOperacion"]);
                    objFACTURASCOVE.FirmaDigitalOperacion = dr["FirmaDigitalOperacion"].ToString();
                    objFACTURASCOVE.CadenaOriginalOperacion = dr["CadenaOriginalOperacion"].ToString();
                    objFACTURASCOVE.FechaDeEnvio = Convert.ToDateTime(dr["FechaDeEnvio"]);
                    objFACTURASCOVE.FechaDeRecibido = Convert.ToDateTime(dr["FechaDeRecibido"]);
                    objFACTURASCOVE.EnviadoSAT = Convert.ToBoolean(dr["EnviadoSAT"]);
                    objFACTURASCOVE.numeroAdenda = dr["numeroAdenda"].ToString();
                    objFACTURASCOVE.idDocumentoCove = Convert.ToInt32(dr["idDocumentoCove"]);
                }
                else
                {
                    objFACTURASCOVE = default;
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

            return objFACTURASCOVE;
        }

        public int EliminarRemesa(int Idreferencia, int NumReme)
        {
            int id = 0;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_DELETE_FACTURASCOVE_REMESA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@IDReferencia  int
            @param = cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4);
            @param.Value = Idreferencia;

            // ,@CONS_FACT  int
            @param = cmd.Parameters.Add("@NUM_REM", SqlDbType.Int, 4);
            @param.Value = NumReme;


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
                throw new Exception(ex.Message.ToString() + "NET_DELETE_FACTURASCOVE_REMESA");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;

        }
        public int Modificar(FacturasCove lfacturascove, int IdUsuario, int IdUsuarioAutoriza)
        {
            int id = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_UPDATE_FACTURASCOVE_BITACORA", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IDFacturaCOVE", SqlDbType.Int).Value = lfacturascove.IDFacturaCOVE;
                cmd.Parameters.Add("@IDReferencia", SqlDbType.Int).Value = lfacturascove.IDReferencia;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int).Value = lfacturascove.CONS_FACT;
                cmd.Parameters.Add("@CadenaOriginal", SqlDbType.Text).Value = lfacturascove.CadenaOriginal ?? (object)DBNull.Value;
                cmd.Parameters.Add("@NumeroDeCOVE", SqlDbType.VarChar, 13).Value = lfacturascove.NumeroDeCOVE ?? "";
                cmd.Parameters.Add("@FirmaDigital", SqlDbType.VarChar, -1).Value = lfacturascove.FirmaDigital ?? "";
                cmd.Parameters.Add("@COVE", SqlDbType.Bit).Value = lfacturascove.COVE;
                cmd.Parameters.Add("@NumeroDeOperacion", SqlDbType.Int).Value = lfacturascove.NumeroDeOperacion;
                cmd.Parameters.Add("@FirmaDigitalOperacion", SqlDbType.VarChar, -1).Value = lfacturascove.FirmaDigitalOperacion ?? "";
                cmd.Parameters.Add("@CadenaOriginalOperacion", SqlDbType.VarChar, -1).Value = lfacturascove.CadenaOriginalOperacion ?? "";
                cmd.Parameters.Add("@EnviadoSAT", SqlDbType.Bit).Value = lfacturascove.EnviadoSAT;
                cmd.Parameters.Add("@numeroAdenda", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(lfacturascove.numeroAdenda) ? "" : lfacturascove.numeroAdenda;
                cmd.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario;
                cmd.Parameters.Add("@IdUsuarioAutoriza", SqlDbType.Int).Value = IdUsuarioAutoriza;

                SqlParameter outputParam = new SqlParameter("@newid_registro", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    if (outputParam.Value != DBNull.Value && Convert.ToInt32(outputParam.Value) != -1)
                    {
                        id = Convert.ToInt32(outputParam.Value);
                    }
                    else
                    {
                        id = 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " NET_UPDATE_FACTURASCOVE");
                }
            }

            return id;
        }
        public int ModificarDocNOIA(int IdReferencia, int NUM_REM, int IdDocumento, int TIPO)
        {
            int id = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_UPDATE_FACTURASCOVE_Docs", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IDReferencia", SqlDbType.Int).Value = IdReferencia;
                cmd.Parameters.Add("@NUM_REM", SqlDbType.Int).Value = NUM_REM;
                cmd.Parameters.Add("@TIPO", SqlDbType.Int).Value = TIPO;
                cmd.Parameters.Add("@IdDocumento", SqlDbType.Int).Value = IdDocumento;

                SqlParameter outputParam = new SqlParameter("@newid_registro", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    id = outputParam.Value != DBNull.Value && Convert.ToInt32(outputParam.Value) != -1
                        ? Convert.ToInt32(outputParam.Value)
                        : 0;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " NET_UPDATE_FACTURASCOVE_idDocumentoXML");
                }
            }

            return id;
        }

        public int ModificarDocAcuseXML(int IdReferencia, int CONS_FACT, int IdDocumento)
        {
            int id = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_UPDATE_FACTURASCOVE_idDocumentoAcuseXML", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4).Value = IdReferencia;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@idDocumentoAcuseXML", SqlDbType.Int, 4).Value = IdDocumento;

                // Parámetro de salida
                SqlParameter outputParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                outputParam.Direction = ParameterDirection.Output;

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    if (Convert.ToInt32(outputParam.Value) != -1)
                    {
                        id = Convert.ToInt32(outputParam.Value);
                    }
                    else
                    {
                        id = 0;
                    }

                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " NET_UPDATE_FACTURASCOVE_idDocumentoAcuseXML");
                }
            }

            return id;
        }

        public int ModificarDocXML(int IdReferencia, int CONS_FACT, int IdDocumento)
        {
            int id = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_UPDATE_FACTURASCOVE_idDocumentoXML", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.Add("@IDReferencia", SqlDbType.Int, 4).Value = IdReferencia;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@IdDocumentoXML", SqlDbType.Int, 4).Value = IdDocumento;

                // Parámetro de salida
                SqlParameter outputParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                outputParam.Direction = ParameterDirection.Output;

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();

                    id = Convert.ToInt32(outputParam.Value) != -1
                        ? Convert.ToInt32(outputParam.Value)
                        : 0;

                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " NET_UPDATE_FACTURASCOVE_idDocumentoXML");
                }
            }

            return id;
        }



    }
}
