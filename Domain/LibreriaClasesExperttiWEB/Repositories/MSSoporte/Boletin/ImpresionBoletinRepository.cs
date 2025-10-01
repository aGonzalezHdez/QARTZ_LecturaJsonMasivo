using LibreriaClasesAPIExpertti.Entities.EntitiesBoletin;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesS3;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Repositories.MSSoporte.Boletin.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Boletin
{
    public class ImpresionBoletinRepository : IImpresionBoletinRepository
    {

        public string SConexion { get; set; }
        string IImpresionBoletinRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public IUbicaciondeArchivosRepository _ubicaciondeArchivosRepository;
        BucketsS3Repository _bucketRepo;       

        public ImpresionBoletinRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            _ubicaciondeArchivosRepository = new UbicaciondeArchivosRepository(_configuration);
            _bucketRepo = new(_configuration);
        }      

        public async Task<string> GenerarBoletin(int IdBoletin)
        {
            clReportes objRpr = new(_configuration);
            int IDDatosDeEmpresa = 0; 
            string rutaS3;
            string filePath = "";
            string LinkS3 = "";
            string RutaLocal;   
            try
            {
                // 0 Imagenes del boletin     
                UbicaciondeArchivos objRuta = _ubicaciondeArchivosRepository.Buscar(254);
                if (!Directory.Exists(objRuta.Ubicacion))
                {
                    Directory.CreateDirectory(objRuta.Ubicacion);
                }
               
                List<ImagenesporIdBoletin> lisImagenes = CargarporIdBoletin(IdBoletin);
                if (lisImagenes != null && lisImagenes.Count > 0)
                {
                    foreach (var imagen in lisImagenes)
                    {
                        string resultadoS3Img = await _bucketRepo.DescargarObjetoAsync(Path.Combine(objRuta.Ubicacion, Path.GetFileName(imagen.RutaS3)), "grupoei.proyectos", imagen.RutaS3);
                        if (resultadoS3Img.Equals("OK", StringComparison.OrdinalIgnoreCase))
                        {
                            RutaLocal = Path.Combine(objRuta.Ubicacion, Path.GetFileName(imagen.RutaS3));
                            InsertarImagenEnSQL(imagen.IdBoletin, imagen.IdBoletinDetalle, imagen.IdBoletinImagen, RutaLocal);
                        }
                    }                       
                }

                //InsertarImagenFondo("\\\\172.24.32.57\\Rutas\\Boletin\\Imagenes\\FondoDesarrollo.png", 0);

                // 1 Nombre del archivo
                BoletinesRepository objBoletinesRepository = new(_configuration);
                Boletines objBoletines = new();

                objBoletines = objBoletinesRepository.GetBoletinById(IdBoletin);
                string nombreArchivo = $"Boletin_{objBoletines.NoVersion}.pdf";

                // 2 Generar el PDF
                var paramList = new List<string> { $"IdBoletin={IdBoletin}" };
                byte[] bytes = await objRpr.GenerarReportePdf(97, paramList, IDDatosDeEmpresa);

                //3 Guardar el archivo
                filePath = Path.Combine(objRuta.Ubicacion, nombreArchivo);

                // 4 Guardar el archivo localmente
                await File.WriteAllBytesAsync(filePath, bytes);

                // 5 Subir a S3
                rutaS3 = $"Boletines/{IdBoletin}/{nombreArchivo}";

                string resultadoS3 = _bucketRepo.SubirObjeto(filePath, "grupoei.proyectos", rutaS3.Trim());

                if (!resultadoS3.Equals("OK", StringComparison.OrdinalIgnoreCase))
                {
                    rutaS3 = "";
                    throw new Exception("Error al subir el archivo a S3.");
                }

                // 6 Actualizar la base de datos con la ruta S3
                _ = Modificar(IdBoletin, rutaS3);

                // 7 Regresa link
                LinkS3 = _bucketRepo.URL(rutaS3, "grupoei.proyectos");

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
            finally
            {
                // 8 Eliminar el archivo local después de subirlo a S3
                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    File.Delete(filePath);
                }                
            }
            return LinkS3;
        }


        public int Modificar(int IdBoletin, string RutaS3)
        {
            int Id = 0;

            try
            {
                using SqlConnection con = new(SConexion);
                using var cmd = new SqlCommand("NET_UPDATE_CASAEI_BOLETINES_RUTAS3", con)
                {
                    CommandType = CommandType.StoredProcedure
                };  
                cmd.Parameters.Add("@IdBoletin", SqlDbType.Int, 4).Value = IdBoletin;
                cmd.Parameters.Add("@RutaS3", SqlDbType.VarChar, 500).Value = RutaS3;
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();

                Id = cmd.Parameters["@newid_registro"].Value != DBNull.Value ? Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) : 0;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Id;
        }

        public void InsertarImagenEnSQL(int IdBoletin, int IdBoletinDetalle, int IdBoletinImagen, string rutaImagen)
        {
            try
            {
                byte[] imagenBytes = File.ReadAllBytes(rutaImagen);

                using SqlConnection con = new(SConexion);
                using var cmd = new SqlCommand("NET_INSERT_CASAEI_BOLETINES_IMG_BINARIO", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@IdBoletin", SqlDbType.Int, 4).Value = IdBoletin;
                cmd.Parameters.Add("@IdBoletinDetalle", SqlDbType.Int, 4).Value = IdBoletinDetalle;
                cmd.Parameters.Add("@IdBoletinImagen", SqlDbType.Int, 4).Value = IdBoletinImagen;
                cmd.Parameters.Add("@Imagen", SqlDbType.Image ).Value = imagenBytes;            
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();            
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al insertar la imagen en la base de datos.", ex);
            }
        }

        public void InsertarImagenFondo(string rutaImagen, int IdDepartamento)
        {
            try
            {
                byte[] imagenBytes = File.ReadAllBytes(rutaImagen);

                using SqlConnection con = new(SConexion);
                using var cmd = new SqlCommand("NET_INSERT_CASAEI_BOLETINES_IMG_CATALOGO_BINARIO", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@Imagen", SqlDbType.Image).Value = imagenBytes;           
                cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = IdDepartamento;               
                cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = 1;             
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al insertar la imagen en la base de datos.", ex);
            }
        }

        public List<ImagenesporIdBoletin> CargarporIdBoletin(int IdBoletin)
        {
            
            List<ImagenesporIdBoletin> listImagenesporIdBoletin = new();         

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CASAEI_BOLETINESIMAGENES_IDBOLETIN", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdBoletin", SqlDbType.Int, 4).Value = IdBoletin;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ImagenesporIdBoletin img = new()
                            {
                                IdBoletin = Convert.ToInt32(dr["IdBoletin"]),
                                IdBoletinDetalle = Convert.ToInt32(dr["IdBoletinDetalle"]),
                                IdBoletinImagen = dr["IdBoletinImagen"] != DBNull.Value ? Convert.ToInt32(dr["IdBoletinImagen"]) : 0,
                                RutaS3 = dr["RUTAS3"] != DBNull.Value ? dr["RUTAS3"].ToString() : null,
                            };
                            if (img.RutaS3 != null)
                            {
                                listImagenesporIdBoletin.Add(img);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listImagenesporIdBoletin;
        }
    }
}
