using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesBoletin;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using NPOI.SS.Formula.Functions;
using LibreriaClasesAPIExpertti.Entities.EntitiesS3;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Boletin
{
    public class BoletinesImagenesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public IUbicaciondeArchivosRepository _ubicaciondeArchivosRepository;
        BucketsS3Repository _bucketRepo;
        public BoletinesImagenesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
            _ubicaciondeArchivosRepository = new UbicaciondeArchivosRepository(_configuration);
            _bucketRepo = new(_configuration);
        }

        public async Task<BoletinesImagenes> InsertBoletinImagen(BoletinesImagenesDTO img)
        {
            int newId = 0;
            string filePath = "";
            string rutaS3;
            BoletinesImagenes boletinesImagenes = new BoletinesImagenes();

            try {

                if (!ExisteBoletin(img.IdBoletin))
                {
                    throw new ArgumentException($"No se encontró un boletín con el ID: {img.IdBoletin}.");
                }

                byte[] bytes = Convert.FromBase64String(img.ArchivoBase64);
                string nombreArchivo = Guid.NewGuid().ToString("N");
                nombreArchivo = nombreArchivo.Replace("\\", "");
                nombreArchivo = nombreArchivo.Replace("/", "");
                nombreArchivo = nombreArchivo.Replace("+", "");
                nombreArchivo = nombreArchivo.Replace("&", "");
                nombreArchivo = nombreArchivo + '.' + img.Extencion;


                UbicaciondeArchivos objRuta = _ubicaciondeArchivosRepository.Buscar(254);

                if (!Directory.Exists(objRuta.Ubicacion))
                {
                    Directory.CreateDirectory(objRuta.Ubicacion);
                }

                filePath = Path.Combine(objRuta.Ubicacion, nombreArchivo);

                await File.WriteAllBytesAsync(filePath, bytes);

                AWSS3Repository s3Repo = new(_configuration);
                S3Buckets objS3Buckets = await s3Repo.Buscar(2)
                    ?? throw new ArgumentException("No existe Bucket Configurado, favor de avisar al equipo de desarrollo.");

                rutaS3 = $"Boletines/{img.IdBoletin}/{img.IdBoletinDetalle}/{nombreArchivo}";
               
                string resultadoS3 = await _bucketRepo.SubirObjetoAsync(filePath, objS3Buckets.Bucket, rutaS3.Trim());

                if (!resultadoS3.Equals("OK", StringComparison.OrdinalIgnoreCase))
                {
                    rutaS3 = "";
                    throw new Exception("Error al subir el archivo a S3.");
                }



                using (SqlConnection cn = new SqlConnection(sConexion))
                {
                    using (SqlCommand cmd = new SqlCommand("NET_INSERT_CASAEI_BOLETINESIMAGENES", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IdBoletinDetalle",  img.IdBoletinDetalle);
                        cmd.Parameters.AddWithValue("@RUTAS3", string.IsNullOrEmpty(rutaS3) ? DBNull.Value : rutaS3);
                        cmd.Parameters.AddWithValue("@activo", img.activo);
                        cmd.Parameters.AddWithValue("@idUsuario", img.idUsuario);

                        try
                        {
                            cn.Open();
                            newId = Convert.ToInt32(cmd.ExecuteScalar());

                            boletinesImagenes.IdBoletinImagen = newId;
                            boletinesImagenes.IdBoletinDetalle = img.IdBoletinDetalle;
                            boletinesImagenes.RUTAS3 = _bucketRepo.URL(rutaS3, "grupoei.proyectos");
                            boletinesImagenes.activo = img.activo;
                            boletinesImagenes.idUsuario = img.idUsuario;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error al insertar BoletinImagen: " + ex.Message);
                        }
                    }
                }
                

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }


            return boletinesImagenes;



        }

        private bool ExisteBoletin(int id)
        {
            bool exists = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(sConexion))
                {
                    using (SqlCommand command = new SqlCommand("NET_SEARCH_BY_ID_BOLETIN", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdBoletin", id);

                        SqlParameter existsParam = new SqlParameter("@Exists", SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(existsParam);

                        connection.Open();
                        command.ExecuteNonQuery();
                        exists = Convert.ToBoolean(existsParam.Value);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return exists;
        }

        public async Task<int>  DeleteBoletinImagen(int id)
        {
            int affectedRows = 0;
            BoletinesImagenes boletinesImagenes =  SearchBoletinImagenes(id);
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_DELETE_CASAEI_BOLETINESIMAGENES", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletinImagen", id);
                    try
                    {
                        cn.Open();
                        affectedRows = Convert.ToInt32(cmd.ExecuteScalar());
                        if (affectedRows > 0)
                        {
                            AWSS3Repository s3Repo = new(_configuration);
                            S3Buckets objS3Buckets = await s3Repo.Buscar(2);

                            await _bucketRepo.BorrarObjectoAsync(objS3Buckets.Bucket, boletinesImagenes.RUTAS3);
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al eliminar BoletinImagen: " + ex.Message);
                    }
                }
            }
            return affectedRows;
        }

        public List<BoletinesImagenes> LoadBoletinImagenesByDetalle(int idBoletinDetalle)
        {
            List<BoletinesImagenes> list = new List<BoletinesImagenes>();
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_LOAD_CASAEI_BOLETINESIMAGENES", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletinDetalle", idBoletinDetalle);
                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                BoletinesImagenes img = new BoletinesImagenes
                                {
                                    IdBoletinImagen = Convert.ToInt32(dr["IdBoletinImagen"]),
                                    IdBoletinDetalle = dr["IdBoletinDetalle"] != DBNull.Value ? Convert.ToInt32(dr["IdBoletinDetalle"]) : null,
                                    RUTAS3 = dr["RUTAS3"] != DBNull.Value ? _bucketRepo.URL(dr["RUTAS3"].ToString(), "grupoei.proyectos") : null,
                                    activo = dr["activo"] != DBNull.Value ? Convert.ToBoolean(dr["activo"]) : null,
                                    idUsuario = dr["idUsuario"] != DBNull.Value ? Convert.ToInt32(dr["idUsuario"]) : null,
                                    fechaAlta = dr["fechaAlta"] != DBNull.Value ? Convert.ToDateTime(dr["fechaAlta"]) : null
                                };
                                list.Add(img);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al cargar BoletinImagenes: " + ex.Message);
                    }
                }
            }

            return list;
        }


        public BoletinesImagenes SearchBoletinImagenes(int idImagen)
        {
            BoletinesImagenes boletinesImagenes = new BoletinesImagenes();
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_SEARCH_CASAEI_BOLETINESIMAGENES", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletinImagen", idImagen);
                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                boletinesImagenes = new BoletinesImagenes
                                {
                                    IdBoletinImagen = Convert.ToInt32(dr["IdBoletinImagen"]),
                                    IdBoletinDetalle = dr["IdBoletinDetalle"] != DBNull.Value ? Convert.ToInt32(dr["IdBoletinDetalle"]) : null,
                                    RUTAS3 = dr["RUTAS3"] != DBNull.Value ? dr["RUTAS3"].ToString() : null,
                                    activo = dr["activo"] != DBNull.Value ? Convert.ToBoolean(dr["activo"]) : null,
                                    idUsuario = dr["idUsuario"] != DBNull.Value ? Convert.ToInt32(dr["idUsuario"]) : null,
                                    fechaAlta = dr["fechaAlta"] != DBNull.Value ? Convert.ToDateTime(dr["fechaAlta"]) : null
                                };
                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al cargar BoletinImagenes: " + ex.Message);
                    }
                }
            }

            return boletinesImagenes;
        }
    }
}
