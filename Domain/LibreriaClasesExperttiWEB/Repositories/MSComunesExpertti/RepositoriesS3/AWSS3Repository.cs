using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesS3;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3
{
    public class AWSS3Repository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con = null!;

        public AWSS3Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public async Task<S3Buckets> Buscar(int IdBucket)
        {
            S3Buckets objS3Buckets = new();

            try
            {
                using (con = new(SConexion))

                using (var cmd = new SqlCommand("NET_SEARCH_CASAEI_S3Buckets", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdBucket", SqlDbType.Int, 4).Value = IdBucket;

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objS3Buckets.IdBucket = Convert.ToInt32(dr["IdBucket"]);
                        objS3Buckets.Bucket = string.Format("{0}", dr["Bucket"]);
                        objS3Buckets.idSolicitud = string.Format("{0}", dr["idSolicitud"]);
                    }
                    else
                    {
                        objS3Buckets = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objS3Buckets;
        }

        public S3Buckets BuscarSynchronous(int IdBucket)
        {
            S3Buckets objS3Buckets = new();

            try
            {
                using (con = new(SConexion))

                using (var cmd = new SqlCommand("NET_SEARCH_CASAEI_S3Buckets", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdBucket", SqlDbType.Int, 4).Value = IdBucket;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objS3Buckets.IdBucket = Convert.ToInt32(dr["IdBucket"]);
                        objS3Buckets.Bucket = string.Format("{0}", dr["Bucket"]);
                        objS3Buckets.idSolicitud = string.Format("{0}", dr["idSolicitud"]);
                    }
                    else
                    {
                        objS3Buckets = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objS3Buckets;
        }

        public async Task<string> SubirDocaS3(int IDCliente, string NombreFile, string rutaLocal)
        {

            string RutaS3;

            try
            {
                AWSS3Repository objS3BucketsRepository = new(_configuration);
                S3Buckets objS3Buckets = await objS3BucketsRepository.Buscar(3);
                if (objS3Buckets == null)
                {
                    throw new ArgumentException("No existe Bucket Configurado, favor de avisar al equipo de desarrollo");
                }
                RutaS3 = $"{IDCliente}/Documentacion/{NombreFile}";

                string subioS3 = string.Empty;
                BucketsS3Repository objS3 = new(_configuration);
                subioS3 = await objS3.SubirObjetoAsync(rutaLocal, objS3Buckets.Bucket, RutaS3.Trim());
               
                if (subioS3.ToUpper() != "OK")
                {
                    RutaS3 = "";
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return RutaS3;
        }

        public string SubirDocaS3Synchronous(int IDCliente, string NombreFile, string rutaLocal)
        {

            string RutaS3;

            try
            {
                AWSS3Repository objS3BucketsRepository = new(_configuration);
                S3Buckets objS3Buckets = objS3BucketsRepository.BuscarSynchronous(3);
                if (objS3Buckets == null)
                {
                    throw new ArgumentException("No existe Bucket Configurado, favor de avisar al equipo de desarrollo");
                }
                RutaS3 = $"{IDCliente}/Documentacion/{NombreFile}";

                string subioS3 = string.Empty;
                BucketsS3Repository objS3 = new(_configuration);
                subioS3 = objS3.SubirObjeto(rutaLocal, objS3Buckets.Bucket, RutaS3.Trim());
             
                if (subioS3.ToUpper() != "OK")
                {
                    RutaS3 = "";
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return RutaS3;
        }

        public async Task<string> BuscarLinkS3(string RutaS3, int IdBucket)
        {
            string Encriptado;

            try
            {

                S3Buckets objBucket = new();
                AWSS3Repository objBucketRepository = new(_configuration);

                objBucket = await objBucketRepository.Buscar(IdBucket);

                BucketsS3Repository objS3 = new(_configuration);
                Encriptado = objS3.URL(RutaS3, objBucket.Bucket);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return Encriptado;
        }

        public string BuscarLinkS3Synchronous(string RutaS3, int IdBucket)
        {
            string Encriptado;

            try
            {

                S3Buckets objBucket = new();
                AWSS3Repository objBucketRepository = new(_configuration);

                objBucket = objBucketRepository.BuscarSynchronous(IdBucket);

                BucketsS3Repository objS3 = new(_configuration);
                Encriptado = objS3.URL(RutaS3, objBucket.Bucket);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return Encriptado;
        }
       
        public async Task<string> EliminarDocS3(string RutaS3, int IdBucket)
        {
            string EliminoS3;

            try
            {

                S3Buckets objBucket = new();
                AWSS3Repository objBucketRepository = new(_configuration);

                objBucket = await objBucketRepository.Buscar(IdBucket);

                BucketsS3Repository objS3 = new(_configuration);
                EliminoS3 = await objS3.BorrarObjectoAsync(objBucket.Bucket, RutaS3);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return EliminoS3;
        }
    }
}
