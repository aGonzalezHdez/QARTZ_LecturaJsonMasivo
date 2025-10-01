using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class DocumentosporGuiaRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public DocumentosporGuiaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int BuscarRuta(string RutaS3)
        {
            int CUANTOS = 0;

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_DOCUMENTOSPORGUIA_RUTAS3";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@RUTAS3", SqlDbType.VarChar, 250);
            param.Value = RutaS3;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    CUANTOS = Convert.ToInt32(dr["CUANTOS"].ToString());
                }

                else
                {
                    CUANTOS = 0;
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
            return CUANTOS;
        }

        public int InsertarConsecutivo(DocumentosporGuia lDocumentosPorGuia)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_DOCUMENTOSPORGUIA_CONSECUTIVO_S3";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@idTipoDocumento  int
            param = cmd.Parameters.Add("@idTipoDocumento", SqlDbType.Int, 4);
            param.Value = lDocumentosPorGuia.idTipoDocumento;

            // ,@idReferencia  int
            param = cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4);
            param.Value = lDocumentosPorGuia.idReferencia;

            // ,@RutaFecha  varchar
            param = cmd.Parameters.Add("@RutaFecha", SqlDbType.VarChar, 10);
            param.Value = lDocumentosPorGuia.RutaFecha;

            // @extension
            param = cmd.Parameters.Add("@extension", SqlDbType.VarChar, 5);
            param.Value = lDocumentosPorGuia.Extension;

            if (lDocumentosPorGuia.IdUsuario == null)
            {
                lDocumentosPorGuia.IdUsuario = 0;
            }

            // @IdUsuario
            param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
            param.Value = lDocumentosPorGuia.IdUsuario;

            param = cmd.Parameters.Add("@Consecutivo", SqlDbType.Int, 4);
            param.Value = lDocumentosPorGuia.Consecutivo;

            param = cmd.Parameters.Add("@RutaS3", SqlDbType.VarChar, 250);
            param.Value = Interaction.IIf(lDocumentosPorGuia.RutaS3 == null, "", lDocumentosPorGuia.RutaS3);


            param = cmd.Parameters.Add("@S3", SqlDbType.Bit);
            param.Value = Interaction.IIf(lDocumentosPorGuia.S3 == null, false, lDocumentosPorGuia.S3);

            // @Complemento
            param = cmd.Parameters.Add("@Complemento", SqlDbType.VarChar, 25);
            param.Value = Interaction.IIf(lDocumentosPorGuia.Complemento == null, "", lDocumentosPorGuia.Complemento);

            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value.ToString()) != -1)
                {
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value.ToString());
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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_DOCUMENTOSPORGUIA_CONSECUTIVO_S3");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }
        public int ExisteUltimoDocumentoSubido(int IdReferencia, int idTipoDocumento, int Consecutivo)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;

            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_SEARCH_SABER_SI_EXISTE_CONSECUTIVO_DE_DOCUMENTO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                // @IdReferencia INT
                param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
                param.Value = IdReferencia;

                // @idTipoDocumento  INTEGER ,
                param = cmd.Parameters.Add("@idTipoDocumento", SqlDbType.Int, 4);
                param.Value = idTipoDocumento;

                // @Consecutivo INTEGER ,
                param = cmd.Parameters.Add("@Consecutivo", SqlDbType.Int, 4);
                param.Value = Consecutivo;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_SEARCH_SABER_SI_EXISTE_CONSECUTIVO_DE_DOCUMENTO");
            }
            cn.Close();
            cn.Dispose();
            return id;

        }
        public DocumentosporGuia Buscar(int idReferencia, int IdTipoDocumento)
        {
            var objDOCUMENTOSPORGUIA = new DocumentosporGuia();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_DOCUMENTOSPORGUIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4);
            param.Value = idReferencia;

            param = cmd.Parameters.Add("@IdTipoDocumento", SqlDbType.Int, 4);
            param.Value = IdTipoDocumento;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objDOCUMENTOSPORGUIA.idDocumento = Convert.ToInt32(dr["idDocumento"]);
                    objDOCUMENTOSPORGUIA.idTipoDocumento = Convert.ToInt32(dr["idTipoDocumento"]);
                    objDOCUMENTOSPORGUIA.idReferencia = Convert.ToInt32(dr["idReferencia"]);
                    objDOCUMENTOSPORGUIA.Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                    objDOCUMENTOSPORGUIA.RutaFecha = dr["RutaFecha"].ToString();
                    objDOCUMENTOSPORGUIA.Extension = dr["Extension"].ToString();
                    objDOCUMENTOSPORGUIA.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                    objDOCUMENTOSPORGUIA.RutaS3 = dr["RutaS3"].ToString();
                }
                else
                {
                    objDOCUMENTOSPORGUIA = default;
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

            return objDOCUMENTOSPORGUIA;
        }

        public DocumentosporGuia BuscarPorId(int idDocumento)
        {
            var objDOCUMENTOSPORGUIA = new DocumentosporGuia();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_DOCUMENTOSPORGUIAxidDocumento";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@idDocumento", SqlDbType.Int, 4);
            param.Value = idDocumento;

       

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objDOCUMENTOSPORGUIA.idDocumento = Convert.ToInt32(dr["idDocumento"]);
                    objDOCUMENTOSPORGUIA.idTipoDocumento = Convert.ToInt32(dr["idTipoDocumento"]);
                    objDOCUMENTOSPORGUIA.idReferencia = Convert.ToInt32(dr["idReferencia"]);
                    objDOCUMENTOSPORGUIA.Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                    objDOCUMENTOSPORGUIA.RutaFecha = dr["RutaFecha"].ToString();
                    objDOCUMENTOSPORGUIA.Extension = dr["Extension"].ToString();
                    objDOCUMENTOSPORGUIA.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                    objDOCUMENTOSPORGUIA.RutaS3 = dr["RutaS3"].ToString();
                }
                else
                {
                    objDOCUMENTOSPORGUIA = default;
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

            return objDOCUMENTOSPORGUIA;
        }
        public int Eliminar(int idDocumento)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_DELETE_DOCUMENTOSPORGUIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@idTipoDocumento  int
            param = cmd.Parameters.Add("@idDocumento", SqlDbType.Int, 4);
            param.Value = idDocumento;

            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                if (int.Parse(cmd.Parameters["@newid_registro"].Value.ToString()) != -1)
                {
                    id = int.Parse(cmd.Parameters["@newid_registro"].Value.ToString());
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
                throw new Exception(ex.Message.ToString() + "NET_DELETE_DOCUMENTOSPORGUIA");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }


    }
}
