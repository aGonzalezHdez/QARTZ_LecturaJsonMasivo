using System.Data.SqlClient;
using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Amazon.S3.IO;
using LibreriaClasesAPIExpertti.Entities.EntitiesS3;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3
{
    public class BucketsS3Repository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con = null!;

        private IAmazonS3 conexionAmazon;
        public BucketsS3Repository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            conexionAmazon = CrearConexionAmazon();
        }


        ///// <summary>
        /////     ''' Llama a metodo que accede a DB para obtener credenciales y construye el cliente de conexión para AmazonS3 
        /////     ''' <br></br>
        /////     ''' Recomendable instanciarlo dentro de un TryCatch
        /////     ''' </summary>
        //public SurroundingClass(string lMyConexionString)
        //{
        //    MyConexionString = lMyConexionString;
        //    conexionAmazon = CrearConexionAmazon();
        //}

        /// <summary>
        ///     ''' Obtiene las credenciales de DB y construye el cliente de conexion con AmazonS3.
        ///     ''' </summary>
        ///     ''' <returns>AmazonS3Client</returns>
        private AmazonS3Client CrearConexionAmazon()
        {
            neg_AmazonS3 neg_AmazonS3 = new(_configuration);
            ent_CredencialesAmazonS3 ent_CredencialesAmazonS3 = neg_AmazonS3.NegObtenerCredenciales();
            BasicAWSCredentials objCreden = new(ent_CredencialesAmazonS3.AccessKeyID, ent_CredencialesAmazonS3.SecretKey);
            AmazonS3Client conexionAmazonS3 = new(objCreden, SeleccionarEndpoint(ent_CredencialesAmazonS3.RegionEndpoint));
            return conexionAmazonS3;
        }

        /// <summary>
        ///     ''' Select Case para convertir Enum a RegionEndpoint de AmazonS3.
        ///     ''' </summary>
        ///     ''' <param name="region">Enum ent_RegionesAmazonS3</param>
        ///     ''' <returns>RegionEndpoint de AmazonS3</returns>
        private RegionEndpoint SeleccionarEndpoint(ent_RegionesAmazonS3 region)
        {
            switch (region)
            {
                case ent_RegionesAmazonS3.USEast1:
                    {
                        return RegionEndpoint.USEast1;
                    }

                case ent_RegionesAmazonS3.USEast2:
                    {
                        return RegionEndpoint.USEast2;
                    }

                case ent_RegionesAmazonS3.USWest1:
                    {
                        return RegionEndpoint.USWest1;
                    }

                case ent_RegionesAmazonS3.USWest2:
                    {
                        return RegionEndpoint.USWest2;
                    }

                case ent_RegionesAmazonS3.AFSouth1:
                    {
                        return RegionEndpoint.AFSouth1;
                    }

                case ent_RegionesAmazonS3.MESouth1:
                    {
                        return RegionEndpoint.MESouth1;
                    }

                case ent_RegionesAmazonS3.CACentral1:
                    {
                        return RegionEndpoint.CACentral1;
                    }

                case ent_RegionesAmazonS3.CNNorthWest1:
                    {
                        return RegionEndpoint.CNNorthWest1;
                    }

                case ent_RegionesAmazonS3.CNNorth1:
                    {
                        return RegionEndpoint.CNNorth1;
                    }

                case ent_RegionesAmazonS3.USGovCloudWest1:
                    {
                        return RegionEndpoint.USGovCloudWest1;
                    }

                case ent_RegionesAmazonS3.USGovCloudEast1:
                    {
                        return RegionEndpoint.USGovCloudEast1;
                    }

                case ent_RegionesAmazonS3.APSoutheast2:
                    {
                        return RegionEndpoint.APSoutheast2;
                    }

                case ent_RegionesAmazonS3.APSoutheast1:
                    {
                        return RegionEndpoint.APSoutheast1;
                    }

                case ent_RegionesAmazonS3.APSouth1:
                    {
                        return RegionEndpoint.APSouth1;
                    }

                case ent_RegionesAmazonS3.APNortheast3:
                    {
                        return RegionEndpoint.APNortheast3;
                    }

                case ent_RegionesAmazonS3.SAEast1:
                    {
                        return RegionEndpoint.SAEast1;
                    }

                case ent_RegionesAmazonS3.APNortheast1:
                    {
                        return RegionEndpoint.APNortheast1;
                    }

                case ent_RegionesAmazonS3.APNortheast2:
                    {
                        return RegionEndpoint.APNortheast2;
                    }

                case ent_RegionesAmazonS3.EUNorth1:
                    {
                        return RegionEndpoint.EUNorth1;
                    }

                case ent_RegionesAmazonS3.EUWest1:
                    {
                        return RegionEndpoint.EUWest1;
                    }

                case ent_RegionesAmazonS3.EUWest3:
                    {
                        return RegionEndpoint.EUWest3;
                    }

                case ent_RegionesAmazonS3.EUCentral1:
                    {
                        return RegionEndpoint.EUCentral1;
                    }

                case ent_RegionesAmazonS3.EUSouth1:
                    {
                        return RegionEndpoint.EUSouth1;
                    }

                case ent_RegionesAmazonS3.APEast1:
                    {
                        return RegionEndpoint.APEast1;
                    }

                case ent_RegionesAmazonS3.EUWest2:
                    {
                        return RegionEndpoint.EUWest2;
                    }

                default:
                    {
                        return RegionEndpoint.USWest2;
                    }
            }
        }

        /// <summary>
        ///     ''' Metodo asincrono para crear un bucket
        ///     ''' </summary>
        ///     ''' <param name="nombreBucket">Nombre del bucket a crear</param>
        ///     ''' <returns>Mensaje de respuesta</returns>
        public async Task<string> CrearBucketAsync(string nombreBucket)
        {
            RespuestaS3 respuestaS3 = new();
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistV2Async(conexionAmazon, nombreBucket) == false)
                {
                    var solicitudCrearBucket = new PutBucketRequest()
                    {
                        BucketName = nombreBucket,
                        UseClientRegion = true
                    };
                    var respuestaCrearBucket = await conexionAmazon.PutBucketAsync(solicitudCrearBucket);
                    respuestaS3.Mensaje = "OK";
                    respuestaS3.Estatus = respuestaCrearBucket.HttpStatusCode;
                    respuestaS3.IdSolicitud = respuestaCrearBucket.ResponseMetadata.RequestId;
                }
                else
                {
                    respuestaS3.Mensaje = "Error, Este nombre de bucket ya existe";
                    respuestaS3.Estatus = HttpStatusCode.Conflict;
                }
            }
            catch (AmazonS3Exception e)
            {
                respuestaS3.Mensaje = e.ErrorCode;
                respuestaS3.Estatus = e.StatusCode;
                respuestaS3.IdSolicitud = e.RequestId;
            }
            catch (Exception e)
            {
                respuestaS3.Mensaje = e.Message;
                respuestaS3.Estatus = HttpStatusCode.InternalServerError;
            }
            return respuestaS3.Mensaje;
        }

        /// <summary>
        ///     ''' Metodo asincrono para borrar un bucket
        ///     ''' </summary>
        ///     ''' <param name="nombreBucket">Nombre del bucket a borrar</param>
        ///     ''' <returns>Mensaje de respuesta</returns>
        public async Task<string> BorrarBucketAsync(string nombreBucket)
        {
            RespuestaS3 respuestaS3 = new();
            try
            {
                // //<=================Función temporal para evitar el borrado/reemplazo de estos buckets/archivos===================//
                if (nombreBucket == "grupoei.respaldos" || nombreBucket == "zenit.expedientesdigitales")
                    throw new Exception("Función temporal para evitar el borrado/reemplazo de estos buckets/archivos");
                // //<=================Función temporal para evitar el borrado/reemplazo de estos buckets/archivos===================//

                if (await AmazonS3Util.DoesS3BucketExistV2Async(conexionAmazon, nombreBucket))
                {
                    var respuestaBorrarBucket = await conexionAmazon.DeleteBucketAsync(nombreBucket);
                    respuestaS3.Mensaje = "OK";
                    respuestaS3.Estatus = respuestaBorrarBucket.HttpStatusCode;
                    respuestaS3.IdSolicitud = respuestaBorrarBucket.ResponseMetadata.RequestId;
                }
                else
                {
                    respuestaS3.Mensaje = "Error, este bucket no existe";
                    respuestaS3.Estatus = HttpStatusCode.Gone;
                }
            }
            catch (AmazonS3Exception e)
            {
                respuestaS3.Mensaje = e.ErrorCode;
                respuestaS3.Estatus = e.StatusCode;
                respuestaS3.IdSolicitud = e.RequestId;
            }
            catch (Exception e)
            {
                respuestaS3.Mensaje = e.Message;
                respuestaS3.Estatus = HttpStatusCode.InternalServerError;
            }
            return respuestaS3.Mensaje;
        }

        /// <summary>
        ///     ''' Metodo asincrono para cargar un archivo a AmazonS3.
        ///     ''' <br></br>
        ///     ''' Recomendable antes verificar que no exista ya un archivo en la rutaS3 a usar si no se tiene activo el versionado, ya que se sobreescribirá.
        ///     ''' </summary>
        ///     ''' <param name="rutaLocal">Ruta del archivo local a subir. Ejemplo: "X:\directorio\archivo.ext". </param>
        ///     ''' <param name="nombreBucket">Nombre del bucket existente en AmazonS3.</param>
        ///     ''' <param name="rutaS3">Ruta completa de guardado(incl. extensión) en AmazonS3, después del Bucket. Ejemplo: "Subdirectorio/archivo.ext".</param>
        ///     ''' <returns>Mensaje de respuesta</returns>
        public async Task<string> SubirObjetoAsync(string rutaLocal, string nombreBucket, string rutaS3)
        {
            RespuestaS3 respuestaS3 = new();

            try
            {
                // //<=================Función temporal para evitar el borrado/reemplazo de estos buckets/archivos===================//
                if (nombreBucket == "grupoei.respaldos" || nombreBucket == "zenit.expedientesdigitales")
                    throw new Exception("Función temporal para evitar el borrado/reemplazo de estos buckets/archivos");
                // //<=================Función temporal para evitar el borrado/reemplazo de estos buckets/archivos===================//

                var fileTransferUtility = new TransferUtility(conexionAmazon);
                await fileTransferUtility.UploadAsync(rutaLocal, nombreBucket, rutaS3);
                respuestaS3.Mensaje = "OK";
                respuestaS3.Estatus = HttpStatusCode.OK;
            }
            catch (AmazonS3Exception e)
            {
                respuestaS3.Mensaje = e.ErrorCode;
                respuestaS3.Estatus = e.StatusCode;
                respuestaS3.IdSolicitud = e.RequestId;
            }
            catch (Exception e)
            {
                respuestaS3.Mensaje = e.Message;
                respuestaS3.Estatus = HttpStatusCode.InternalServerError;
            }
            return respuestaS3.Mensaje;
        }



        public string SubirObjeto(string rutaLocal, string nombreBucket, string rutaS3)
        {
            RespuestaS3 respuestaS3 = new();

            try
            {
                // //<=================Función temporal para evitar el borrado/reemplazo de estos buckets/archivos===================//
                if (nombreBucket == "grupoei.respaldos" || nombreBucket == "zenit.expedientesdigitales")
                    throw new Exception("Función temporal para evitar el borrado/reemplazo de estos buckets/archivos");
                // //<=================Función temporal para evitar el borrado/reemplazo de estos buckets/archivos===================//

                var fileTransferUtility = new TransferUtility(conexionAmazon);
                fileTransferUtility.Upload(rutaLocal, nombreBucket, rutaS3);
                respuestaS3.Mensaje = "OK";
                respuestaS3.Estatus = HttpStatusCode.OK;
            }
            catch (AmazonS3Exception e)
            {
                respuestaS3.Mensaje = e.ErrorCode;
                respuestaS3.Estatus = e.StatusCode;
                respuestaS3.IdSolicitud = e.RequestId;
            }
            catch (Exception e)
            {
                respuestaS3.Mensaje = e.Message;
                respuestaS3.Estatus = HttpStatusCode.InternalServerError;
            }
            return respuestaS3.Mensaje;
        }
        /// <summary>
        ///     ''' Metodo asincrono para descargar un objeto/archivo de AmazonS3
        ///     ''' <br></br>
        ///     ''' Recomendable hacer validaciones de System.IO antes de usar este método para evitar errores de escritura y sobreescritura
        ///     ''' </summary>
        ///     ''' <param name="rutaLocal">Ruta local completa donde guardar el archivo. Ejemplo: "C:\directorio\archivo.ext"</param>
        ///     ''' <param name="nombreBucket">Bucket donde se encuentra alojado el archivo</param>
        ///     ''' <param name="rutaS3">Ruta completa (despues del bucket) donde buscar el archivo</param>
        ///     ''' <returns>Mensaje de respuesta</returns>
        public async Task<string> DescargarObjetoAsync(string rutaLocal, string nombreBucket, string rutaS3)
        {
            RespuestaS3 respuestaS3 = new();
            try
            {
                var solicitudDescargarObjeto = new GetObjectRequest()
                {
                    BucketName = nombreBucket,
                    Key = rutaS3
                };
                var respuetaDescargarObjeto = await conexionAmazon.GetObjectAsync(solicitudDescargarObjeto);
                respuestaS3.Mensaje = "Objeto descargado (sin guardar)";
                respuestaS3.IdSolicitud = respuetaDescargarObjeto.ResponseMetadata.RequestId;
                respuestaS3.Estatus = respuetaDescargarObjeto.HttpStatusCode;
                RespuestaGuardadoLocal respuestaGuardadoLocal = await GuardarAArchivoAsync(respuetaDescargarObjeto, rutaLocal);
                if (respuestaGuardadoLocal.Estatus)
                    respuestaS3.Mensaje = "OK";
                else
                    respuestaS3.Mensaje = respuestaGuardadoLocal.Mensaje;
            }
            catch (AmazonS3Exception e)
            {
                respuestaS3.Mensaje = e.ErrorCode;
                respuestaS3.Estatus = e.StatusCode;
                respuestaS3.IdSolicitud = e.RequestId;
            }
            catch (Exception e)
            {
                respuestaS3.Mensaje = e.Message;
                respuestaS3.Estatus = HttpStatusCode.InternalServerError;
            }
            return respuestaS3.Mensaje;
        }

        /// <summary>
        ///     ''' Metodo asincrono para guardar el objeto descargado desde AmazonS3 a un archivo local
        ///     ''' <br></br>
        ///     ''' Recomendable hacer validaciones de System.IO antes de usar este método para evitar errores de escritura y sobreescritura
        ///     ''' </summary>
        ///     ''' <param name="respuestaS3Objeto">Objeto devuelto por el metodo "DescargarObjetoAsync"</param>
        ///     ''' <param name="rutaLocal">Ruta local completa donde guardar el archivo. Ejemplo: "C:\directorio\archivo.ext"</param>
        ///     ''' <returns>Objeto respuesta que contiene un mensaje y un estatus: "True" si se ha guardado el archivo correctamente o "False" si hubo un error</returns>
        private async Task<RespuestaGuardadoLocal> GuardarAArchivoAsync(GetObjectResponse respuestaS3Objeto, string rutaLocal)
        {
            RespuestaGuardadoLocal respuestaGuardadoLocal = new();
            try
            {
                await respuestaS3Objeto.WriteResponseStreamToFileAsync(rutaLocal, false, CancellationToken.None);
                respuestaGuardadoLocal.Estatus = true;
                respuestaGuardadoLocal.Mensaje = "OK";
            }
            catch (Exception e)
            {
                respuestaGuardadoLocal.Estatus = false;
                respuestaGuardadoLocal.Mensaje = e.Message;
            }
            return respuestaGuardadoLocal;
        }

        /// <summary>
        ///     ''' Método asíncrono para borrar un objeto/archivo en AmazonS3
        ///     ''' </summary>
        ///     ''' <param name="nombreBucket">Nombre del bucket donde se encuentra el objeto a borrar</param>
        ///     ''' <param name="rutaS3">Ruta completa (despues del bucket) en AmazonS3 al objeto a borrar</param>
        ///     ''' <returns>Mensaje de respuesta</returns>
        public async Task<string> BorrarObjectoAsync(string nombreBucket, string rutaS3)
        {
            RespuestaS3 respuestaS3 = new();
            try
            {
                // //<=================Función temporal para evitar el borrado/reemplazo de estos buckets/archivos===================//
                // Throw New NotImplementedException("Excepcion producida explicitamente (Temporal)")
                if (nombreBucket == "grupoei.respaldos" || nombreBucket == "zenit.expedientesdigitales")
                    throw new Exception("Función temporal para evitar el borrado/reemplazo de estos buckets/archivos");
                // //<=================Función temporal para evitar el borrado/reemplazo de estos buckets/archivos===================//
                DeleteObjectResponse respuestaBorrarObjeto = await conexionAmazon.DeleteObjectAsync(nombreBucket, rutaS3);
                respuestaS3.Mensaje = "OK";
                respuestaS3.Estatus = respuestaBorrarObjeto.HttpStatusCode;
                respuestaS3.IdSolicitud = respuestaBorrarObjeto.ResponseMetadata.RequestId;
            }
            catch (AmazonS3Exception e)
            {
                respuestaS3.Mensaje = e.ErrorCode;
                respuestaS3.Estatus = e.StatusCode;
                respuestaS3.IdSolicitud = e.RequestId;
            }
            catch (Exception e)
            {
                respuestaS3.Mensaje = e.Message;
                respuestaS3.Mensaje = Convert.ToString(HttpStatusCode.InternalServerError)!;
            }
            return respuestaS3.Mensaje;
        }

        /// <summary>
        ///     ''' Metodo asincrono para validar si ya existe un archivo en una rutaS3
        ///     ''' </summary>
        ///     ''' <param name="nombreBucket">Nombre del bucket donde validar la rutaS3</param>
        ///     ''' <param name="rutaS3">Ruta a validar</param>
        ///     ''' <returns>Mensaje de respuesta</returns>
        public async Task<string> ValidarArchivoExiste(string nombreBucket, string rutaS3)
        {
            RespuestaS3 respuestaS3 = new();
            await Task.Run(() =>
            {
                try
                {
                    var s3FileInfo = new S3FileInfo(conexionAmazon, nombreBucket, rutaS3);
                    if (s3FileInfo.Exists)
                    {
                        respuestaS3.Mensaje = "Existe";
                        respuestaS3.Estatus = HttpStatusCode.OK;
                    }
                    else
                    {
                        respuestaS3.Mensaje = "No existe";
                        respuestaS3.Estatus = HttpStatusCode.Gone;
                    }
                }
                catch (AmazonS3Exception e)
                {
                    respuestaS3.Mensaje = e.ErrorCode;
                    respuestaS3.Estatus = e.StatusCode;
                    respuestaS3.IdSolicitud = e.RequestId;
                }
                catch (Exception e)
                {
                    respuestaS3.Mensaje = e.Message;
                    respuestaS3.Estatus = HttpStatusCode.InternalServerError;
                }
            });
            return respuestaS3.Mensaje;
        }

        public string URL(string RutaAmazon, string BucketName)
        {
            neg_AmazonS3 neg_AmazonS3 = new(_configuration);
            ent_CredencialesAmazonS3 ent_CredencialesAmazonS3 = neg_AmazonS3.NegObtenerCredenciales();


            var bucketRegion1 = RegionEndpoint.USWest2;
            var s3Client1 = new AmazonS3Client(ent_CredencialesAmazonS3.AccessKeyID, ent_CredencialesAmazonS3.SecretKey, bucketRegion1);
            GetPreSignedUrlRequest request1 = new()
            {
                BucketName = BucketName,
                Key = RutaAmazon,
                Expires = DateTime.UtcNow.AddHours(6)
            };
            var urlString = s3Client1.GetPreSignedURL(request1);
            s3Client1.Dispose();
            return urlString;
        }

        public string DescargarObjeto(string rutaLocal, string nombreBucket, string rutaS3)
        {
            RespuestaS3 respuestaS3 = new RespuestaS3();
            try
            {
                var solicitudDescargarObjeto = new GetObjectRequest()
                {
                    BucketName = nombreBucket,
                    Key = rutaS3
                };
                var respuetaDescargarObjeto = conexionAmazon.GetObject(solicitudDescargarObjeto);
                respuestaS3.Mensaje = "Objeto descargado (sin guardar)";
                respuestaS3.IdSolicitud = respuetaDescargarObjeto.ResponseMetadata.RequestId;
                respuestaS3.Estatus = respuetaDescargarObjeto.HttpStatusCode;
                RespuestaGuardadoLocal respuestaGuardadoLocal = GuardarAArchivo(respuetaDescargarObjeto, rutaLocal);
                if (respuestaGuardadoLocal.Estatus)
                    respuestaS3.Mensaje = "OK";
                else
                    respuestaS3.Mensaje = respuestaGuardadoLocal.Mensaje;
            }
            catch (AmazonS3Exception e)
            {
                respuestaS3.Mensaje = e.ErrorCode;
                respuestaS3.Estatus = e.StatusCode;
                respuestaS3.IdSolicitud = e.RequestId;
            }
            catch (Exception e)
            {
                respuestaS3.Mensaje = e.Message;
                respuestaS3.Estatus = HttpStatusCode.InternalServerError;
            }
            return respuestaS3.Mensaje;
        }

        private RespuestaGuardadoLocal GuardarAArchivo(GetObjectResponse respuestaS3Objeto, string rutaLocal)
        {
            RespuestaGuardadoLocal respuestaGuardadoLocal = new RespuestaGuardadoLocal();
            try
            {
                respuestaS3Objeto.WriteResponseStreamToFile(rutaLocal, false);
                respuestaGuardadoLocal.Estatus = true;
                respuestaGuardadoLocal.Mensaje = "OK";
            }
            catch (Exception e)
            {
                respuestaGuardadoLocal.Estatus = false;
                respuestaGuardadoLocal.Mensaje = e.Message;
            }
            return respuestaGuardadoLocal;
        }
    }
}
