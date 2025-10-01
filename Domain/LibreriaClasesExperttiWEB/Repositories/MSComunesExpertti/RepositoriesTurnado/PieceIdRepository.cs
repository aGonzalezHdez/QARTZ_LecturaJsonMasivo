using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesTurnadoImpo;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado
{
    public class PieceIdRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public PieceIdRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public bool ExistePorGuia(string MyGuiaHouse)
        {
            bool result = false;

            var conn = new SqlConnection(sConexion);
            conn.Open();
            try
            {

                SqlCommand cmd;
                SqlDataReader dr;
                cmd = new SqlCommand("SELECT 1 FROM CASAEI.dbo.PIECEID WHERE Search = @GUIA", conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@GUIA", SqlDbType.VarChar);
                cmd.Parameters["@GUIA"].Value = MyGuiaHouse;


                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    result = true;

                }

                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
            }

            return result;

        }

        public List<PieceId> PendientesPorGuia(string MyGuiaHouse)
        {
            var result = new List<PieceId>();

            var conn = new SqlConnection(sConexion);
            conn.Open();
            try
            {

                SqlCommand cmd;
                SqlDataReader dr;
                cmd = new SqlCommand("SELECT [Search],[Piece ID] FROM PIECEID WHERE Search = @GUIA AND [Piece ID] NOT IN (SELECT DE.PieceId FROM DETALLEXPOPIECEIDS AS DE INNER JOIN REFERENCIAS AS RE ON RE.IdReferencia = DE.IdReferencia WHERE RE.NumeroDeReferencia = @GUIA);", conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@GUIA", SqlDbType.VarChar);
                cmd.Parameters["@GUIA"].Value = MyGuiaHouse;


                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var pieceIdObj = new PieceId();
                        pieceIdObj.NumeroDeGuia = dr["Search"].ToString();
                        pieceIdObj.PieceID = dr["Piece ID"].ToString();
                        result.Add(pieceIdObj);
                    }

                }

                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
            }

            return result;

        }

        public PieceId BuscarPieceId(string PieceId)
        {
            bool result = false;
            var pieceIdObj = new PieceId();
            var conn = new SqlConnection(sConexion);
            conn.Open();
            try
            {
                string pieceIdPre = PieceId.Remove(0, 1);
                string pieceIdPre2 = pieceIdPre.Remove(0, 1);

                SqlCommand cmd;
                SqlDataReader dr;
                cmd = new SqlCommand("SELECT * FROM CASAEI.dbo.PIECEID WHERE [Piece ID] = @PieceId OR [Piece ID] = @PieceIdPre OR [Piece ID] = @PieceIdPre2", conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@PieceId", SqlDbType.VarChar);
                cmd.Parameters["@PieceId"].Value = PieceId;

                cmd.Parameters.Add("@PieceIdPre", SqlDbType.VarChar);
                cmd.Parameters["@PieceIdPre"].Value = pieceIdPre;

                cmd.Parameters.Add("@PieceIdPre2", SqlDbType.VarChar);
                cmd.Parameters["@PieceIdPre2"].Value = pieceIdPre2;

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();

                    pieceIdObj.NumeroDeGuia = dr["Search"].ToString();
                    pieceIdObj.PieceID = dr["Piece ID"].ToString();

                }

                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
            }

            return pieceIdObj;

        }
        public int GetNumeroPieceIdsTotal(string NumeroReferencia)
        {
            int result = 0;
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_COUNT_PIECE_IDS_TOTAL";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@NumeroReferencia", SqlDbType.VarChar, 15);
                    @param.Value = NumeroReferencia;

                    using (SqlDataReader reader = dap.SelectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = Convert.ToInt32(reader["NumberPieceIds"]);
                        }
                    }

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + " NET_COUNT_PIECE_IDS_TOTAL");
                }

            }
            return result;
        }


        public bool InsertarPieceIDExpo(int IdReferencia, string PieceId)
        {
            bool result = false;
            string query = string.Empty;
            query += "INSERT INTO [dbo].[DETALLEXPOPIECEIDS] ([IdReferencia],[PieceId],[FechaEscaneo])";
            query += " VALUES (@IdReferencia,@PieceId, @FechaEscaneo)";

            using (var conn = new SqlConnection(sConexion))
            {
                using (var comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandType = CommandType.Text;
                    comm.CommandText = query;
                    comm.Parameters.AddWithValue("@IdReferencia", IdReferencia);
                    comm.Parameters.AddWithValue("@PieceId", PieceId);
                    comm.Parameters.AddWithValue("@FechaEscaneo", DateTime.Now);
                    try
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        result = false;
                    }
                }
            }

            return result;

        }
        public bool ImpoInsertPieceID(string Guia, string PieceId, int IdOficina, int IdUsuario, int IdReferencia)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            bool result = false;

            // Validar que PieceId tenga formato correcto
            if (PieceId.Length < 10)
            {
                throw new Exception("Formato del PieceId Incorrecto");
            }
            if (PieceId.Contains("+"))
            {
                throw new Exception("Formato del PieceId Incorrecto");
            }
            // Dim firsLetter = Mid(PieceId, 1, 1)
            // If firsLetter <> "J" Then
            // Throw New Exception("Formato del PieceId Incorrecto")
            // End If

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.CommandText = "POCKET_IMPO_INSERT_PIECEID";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@Guia", SqlDbType.VarChar, 255);
                @param.Value = Guia;

                @param = cmd.Parameters.Add("@PieceId", SqlDbType.VarChar, 255);
                @param.Value = PieceId;

                @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
                @param.Value = IdOficina;

                @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
                @param.Value = IdUsuario;

                @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                @param.Value = IdReferencia;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;


                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                result = true;
            }
            catch (Exception ex)
            {

                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " POCKET_IMPO_INSERT_PIECEID");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return result;
        }
        public DataTable ObtenerPieceIDsPorGuia(string Guia)
        {
            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "POCKET_IMPO_GET_PIECES_ID";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@Guia", SqlDbType.VarChar, 20);
                    @param.Value = Guia;

                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + " POCKET_IMPO_GET_PIECES_ID");
                }

            }
            return dtb;
        }
        public int ValidarPieceID(string Guia, string PieceID, int IdUsuario)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "POCKET_IMPO_VALIDAR_PIECEID";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@Guia", SqlDbType.VarChar, 21);
            @param.Value = Guia;

            @param = cmd.Parameters.Add("@PieceId", SqlDbType.VarChar, 500);
            @param.Value = PieceID;

            @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
            @param.Value = IdUsuario;

            @param = cmd.Parameters.Add("@new_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@new_registro"].Value;
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cmd.Parameters.Clear();
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " POCKET_IMPO_VALIDAR_PIECEID");
                return 0;
            }
            cn.Close();
            cn.Dispose();
            return id;
        }


        public int ValidarTodasLosPiecesID(string Guia, bool Validado, int IdUsuario)
        {
            // Dim id As Integer
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "POCKET_IMPO_VALIDADAR_TODO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@NUM_REFE  varchar
            @param = cmd.Parameters.Add("@Guia", SqlDbType.VarChar, 15);
            @param.Value = Guia;

            // ,@validado  boolean
            @param = cmd.Parameters.Add("@validado", SqlDbType.Bit);
            @param.Value = Validado;

            // ,@ID_USUARIO  varchar
            @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int);
            @param.Value = IdUsuario;


            try
            {
                cmd.ExecuteNonQuery();
                // id = CInt(cmd.Parameters("@new_registro").Value)
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cmd.Parameters.Clear();
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " [POCKET_IMPO_VALIDADAR_TODO]");
                return 0;
            }
            cn.Close();
            cn.Dispose();
            return 1;
        }
        public bool ExistPieceIDExpo(int IdReferencia, string PieceId)
        {
            bool result = false;

            var conn = new SqlConnection(sConexion);
            conn.Open();
            try
            {

                string pieceIdPre = PieceId.Remove(0, 1);
                string pieceIdPre2 = pieceIdPre.Remove(0, 1);

                SqlCommand cmd;
                SqlDataReader dr;
                cmd = new SqlCommand("SELECT 1 FROM CASAEI.dbo.DETALLEXPOPIECEIDS WHERE IdReferencia = @IdReferencia AND (PieceId = @PieceId OR PieceId = @PieceIdPre OR PieceId = @PieceIdPre2) ", conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@IdReferencia", SqlDbType.Int);
                cmd.Parameters["@IdReferencia"].Value = IdReferencia;

                cmd.Parameters.Add("@PieceId", SqlDbType.VarChar);
                cmd.Parameters["@PieceId"].Value = PieceId;

                cmd.Parameters.Add("@PieceIdPre", SqlDbType.VarChar);
                cmd.Parameters["@PieceIdPre"].Value = pieceIdPre;

                cmd.Parameters.Add("@PieceIdPre2", SqlDbType.VarChar);
                cmd.Parameters["@PieceIdPre2"].Value = pieceIdPre2;

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    result = true;

                }

                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
            }

            return result;

        }

        public bool ExistSinonimoDeRiesgoTextilesZapatos(string Descripcion)
        {
            bool result = false;

            var conn = new SqlConnection(sConexion);
            conn.Open();
            try
            {

                SqlCommand cmd;
                SqlDataReader dr;

                cmd = new SqlCommand("SELECT 1 FROM SINONIMOSDERIESGO SR INNER JOIN CATALOGODECATEGORIASSR C ON SR.IdCategoriaSR=C.IdCategoriaSR WHERE SR.IdCategoriaSR IN (95,112) AND @Descripcion LIKE SR.SINONIMO", conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar);
                cmd.Parameters["@Descripcion"].Value = Descripcion;

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    result = true;

                }

                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
            }

            return result;

        }
        public bool IsSubdivision(string ReferenceNumber)
        {
            bool result = false;
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_SEARCH_VALIDATE_PIECE_IDS";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@ReferenceNumber", SqlDbType.VarChar, 20);
                    @param.Value = ReferenceNumber;

                    int hasPieceIds = Convert.ToInt32(dap.SelectCommand.ExecuteScalar());
                    result = Convert.ToBoolean(hasPieceIds);

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + " NET_SEARCH_VALIDATE_PIECE_IDS");
                }

            }
            return result;
        }
        public DataTable GetPiecesIdsByGuiaHouse(string ReferenceNumber)
        {
            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_SEARCH_PIECE_IDS_BY_REFERENCE_NUMBER";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@ReferenceNumber", SqlDbType.VarChar, 20);
                    @param.Value = ReferenceNumber;

                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + " NET_SEARCH_PIECE_IDS_BY_REFERENCE_NUMBER");
                }

            }
            return dtb;
        }

        public bool DeletePieceID(string Guia, string PieceId)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            bool result = false;



            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.CommandText = "POCKET_IMPO_DELETE_PIECEID";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@Guia", SqlDbType.VarChar, 255);
                @param.Value = Guia;

                @param = cmd.Parameters.Add("@PieceId", SqlDbType.VarChar, 255);
                @param.Value = PieceId;


                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                result = true;
            }
            catch (Exception ex)
            {

                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " POCKET_IMPO_DELETE_PIECEID");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return result;
        }

        public DataTable GetPocketRastreoPieceId(string ReferenceNumber)
        {
            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "POCKET_RASTREO_PIECEID";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@NumerodeGuia", SqlDbType.VarChar, 20);
                    @param.Value = ReferenceNumber;

                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + " POCKET_RASTREO_PIECEID");
                }

            }
            return dtb;
        }

        public DataTable PieceIdsOficinaGrupo(string GuiaHouse)
        {
            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_PIECE_IDS_OFICINAS";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@ReferenceNumber", SqlDbType.VarChar, 20);
                    @param.Value = GuiaHouse;

                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + " NET_PIECE_IDS_OFICINAS");
                }

            }
            return dtb;
        }

        public string GetPieceIDsByReference(int IdReferencia)
        {
            string concatenatedPieceIDs = "";
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "POCKET_GET_PIECEID_REFERENCIA";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                    @param.Value = IdReferencia;

                    using (SqlDataReader reader = dap.SelectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            concatenatedPieceIDs = Convert.ToString(reader["ConcatenatedPieceIDs"]);
                        }
                    }

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + " NET_PIECE_IDS_OFICINAS");
                }

            }
            return concatenatedPieceIDs;
        }

        public int Update(string PieceIdOld, string PieceId)
        {
            int id = 0;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "POCKET_IMPO_UPDATE_PIECEID";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@PieceIdOld", SqlDbType.VarChar, 50);
            @param.Value = PieceIdOld;

            @param = cmd.Parameters.Add("@PieceId", SqlDbType.VarChar, 50);
            @param.Value = PieceId;

            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "POCKET_IMPO_UPDATE_PIECEID");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public string BuscarReferenciaPorPieceId(string pieceId)
        {
            string Referencia = "";
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {
                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "POCKET_SEARCH_REFERENCIA_BY_PIECEID";
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    @param = dap.SelectCommand.Parameters.Add("@PieceId", SqlDbType.VarChar, 25);
                    @param.Value = pieceId;

                    using (SqlDataReader reader = dap.SelectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Referencia = Convert.ToString(reader["Referencia"]);
                        }
                    }

                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + " POCKET_SEARCH_REFERENCIA_BY_PIECEID");
                }

            }
            return Referencia;
        }
    }
}
