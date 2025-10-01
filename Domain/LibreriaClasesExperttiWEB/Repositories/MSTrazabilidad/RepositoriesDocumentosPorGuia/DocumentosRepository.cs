using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities; 
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;


namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia
{
    public class DocumentosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public DocumentosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<bool> ExisteProforma(int idReferencia)
        {
            bool Existe = false;

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_DOCUMENTOS_EXISTEPROFORMA", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4).Value = idReferencia;

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                    {
                        Existe = true;
                    }
                    else
                        Existe = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return Existe;
        }
        public PathsDocumentos BuscarPath(string GuiaHouse, string Identificador)
        {
            var objPaths = new PathsDocumentos();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_DOCUMENTOS_UBICACION";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
            param.Value = GuiaHouse;

            // @IDENTIFICADOR
            param = cmd.Parameters.Add("@IDENTIFICADOR", SqlDbType.VarChar, 3);
            param.Value = Identificador;

            dr = cmd.ExecuteReader();
            try
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    objPaths.pathArchivo = dr["pathArchivo"].ToString();
                    objPaths.PathFecha = dr["PathFecha"].ToString();
                    objPaths.PathArchivoAnterior = dr["PathArchivoAnterior"].ToString();
                    objPaths.RutaS3 = dr["RutaS3"].ToString();
                    objPaths.S3 = Convert.ToBoolean(dr["S3"].ToString());
                    objPaths.RutaFisica = dr["RutaFisica"].ToString();
                    objPaths.IdDocumento = Convert.ToInt32(dr["IdDocumento"].ToString());
                }
                else
                {

                    objPaths.pathArchivo = string.Empty;
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
                objPaths.pathArchivo = string.Empty;

                throw new Exception(ex.Message.ToString());
            }

            return objPaths;
        }
        public Documento Buscar(string GuiaHouse, string Identificador, string Cons)
        {
            var objDoc = new Documento();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;
            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_DOCUMENTOS_NEW";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
                param.Value = GuiaHouse;

                // @IDENTIFICADOR
                param = cmd.Parameters.Add("@IDENTIFICADOR", SqlDbType.VarChar, 3);
                param.Value = Identificador;

                // @Cons Int
                param = cmd.Parameters.Add("@Cons", SqlDbType.VarChar, 2);
                param.Value = Cons;


                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    dr.Read();
                    objDoc.DirectorioVirtual = dr["DirectorioVirtual"].ToString();
                    objDoc.RutaFisica = dr["RutaFisica"].ToString();
                    objDoc.IdTipoDocumento = dr["TipodeDocumento"].ToString();
                    objDoc.Encriptado = dr["Encriptado"].ToString();
                    objDoc.IdDocumento = dr["IdDocumento"].ToString();
                    objDoc.DirectorioVirtualViejo = dr["DirectorioVirtualViejo"].ToString();
                    objDoc.S3 = dr["S3"].ToString();
                    objDoc.RutaS3 = dr["RutaS3"].ToString();
                }
                else
                {

                    objDoc = default;
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
                objDoc = default;

                throw new Exception(ex.Message.ToString());
            }

            return objDoc;
        }
        public Documento BuscarIdDoc(int IdDocumento)
        {
            var objDoc = new Documento();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_DOCUMENTOS_NEW_IDDOCUMENTO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdDocumento", SqlDbType.Int);
                @param.Value = IdDocumento;



                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    dr.Read();
                    objDoc.DirectorioVirtual = dr["DirectorioVirtual"].ToString();
                    objDoc.RutaFisica = dr["RutaFisica"].ToString();
                    objDoc.TipodeDocumento = dr["TipodeDocumento"].ToString();
                    objDoc.Encriptado = dr["Encriptado"].ToString();
                    objDoc.IdDocumento = dr["IdDocumento"].ToString();
                    objDoc.DirectorioVirtualViejo = dr["DirectorioVirtualViejo"].ToString();
                    objDoc.S3 = dr["S3"].ToString();
                    objDoc.RutaS3 = dr["RutaS3"].ToString();
                }
                else
                {

                    objDoc = default;
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
                objDoc = default;

                throw new Exception(ex.Message.ToString());
            }

            return objDoc;
        }


    }
}
