using LibreriaClasesAPIExpertti.Entities.EntitiesBuscador;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesS3;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador
{
    public class Imagenes_BscRepository : IImagenes_BscRepository
    {

        public string SConexion { get; set; }
        string IImagenes_BscRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public Imagenes_BscRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<int> InsertarImagenes(Imagenes_Bsc objImagenes_Bsc)
        {
            int idImagen = 0;

            if (Path.GetExtension(objImagenes_Bsc.NombreArchivo) == ".jpg")
                {
                    Imagenes_BscRepository objImagenes_BscRepository = new(_configuration);
                    int consecutivo = objImagenes_BscRepository.BuscarConsecutivo(objImagenes_Bsc.idBuscar);
                    string rutaS3 = await SubirImagen(objImagenes_Bsc.idBuscar, objImagenes_Bsc.NombreArchivo, objImagenes_Bsc.ArchivoBase64, consecutivo);

                    if (!string.IsNullOrEmpty(rutaS3))
                    {

                    objImagenes_Bsc.RutaS3 = rutaS3;
                    objImagenes_Bsc.Consecutivo = consecutivo;
               
                    idImagen = objImagenes_BscRepository.Insertar(objImagenes_Bsc);  // Inserta la imagen en la base de datos
                    }
                }
            return idImagen;
        }      

        public int Insertar(Imagenes_Bsc objIimagenes)
        {
            int idImagen;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_INSERT_CASAEI_Imagenes_Bsc", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = objIimagenes.idBuscar;
                    cmd.Parameters.Add("@RutaS3", SqlDbType.VarChar, -1).Value = objIimagenes.RutaS3;
                    cmd.Parameters.Add("@Consecutivo", SqlDbType.Int, 4).Value = objIimagenes.Consecutivo;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                    {
                        idImagen = (int)cmd.Parameters["@newid_registro"].Value;
                    }
                    else
                    {
                        idImagen = 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return idImagen;
        }

        public int Modificar(Imagenes_Bsc objIimagenes)
        {
            int idImagen = 0;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_UPDATE_CASAEI_Imagenes_Bsc", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idImagen", SqlDbType.Int, 4).Value = objIimagenes.idImagen;
                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = objIimagenes.idBuscar;
                    cmd.Parameters.Add("@RutaS3", SqlDbType.VarChar, -1).Value = objIimagenes.RutaS3;
                    cmd.Parameters.Add("@Consecutivo", SqlDbType.Int, 4).Value = objIimagenes.Consecutivo;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                    {
                        idImagen = (int)cmd.Parameters["@newid_registro"].Value;
                    }                   
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return idImagen;
        }

        public bool EliminarBuscador(int idBuscar)
        {
            bool result = false;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_DELETE_CASAEI_Buscador_Imagenes_Bsc", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = idBuscar;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return result;
        }

        public async Task<bool> EliminarImagenes(int idBuscar)
        {

            Imagenes_BscRepository imagenes_BscRepository = new(_configuration);
            List<Imagenes_Bsc> listImagenes_Bsc = imagenes_BscRepository.Load(idBuscar);

            foreach (Imagenes_Bsc item in listImagenes_Bsc)
            {
                BucketsS3Repository objS3 = new(_configuration);
                await objS3.BorrarObjectoAsync("3", item.RutaS3);
            }

            EliminarBuscador(idBuscar);


            return true;
        }

        public  bool Eliminar(int idImagen)
        {
            Imagenes_Bsc objImagenes_Bsc = Buscar(idImagen);

            bool SiNo = false;
            
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_DELETE_CASAEI_Imagenes_Bsc", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idImagen", SqlDbType.Int, 4).Value = idImagen;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                        {
                            SiNo = true;
                           
                                BucketsS3Repository objS3 = new(_configuration);
                                objS3.BorrarObjectoAsync("3", objImagenes_Bsc.RutaS3);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return SiNo;
        }

        public List<Imagenes_Bsc> Load(int idBuscar)
        {
            List<Imagenes_Bsc>? listImagenes_Bsc = new();

            try
            {
                using (SqlConnection con = new(SConexion))                
                using (SqlCommand cmd = new("buscador.NET_LOAD_CASAEI_Imagenes_Bsc", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int).Value = idBuscar;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Imagenes_Bsc objImagenes_Bsc = new()
                            {
                                idImagen = dr["idImagen"] != DBNull.Value ? Convert.ToInt32(dr["idImagen"]) : 0,
                                idBuscar = dr["idBuscar"] != DBNull.Value ? Convert.ToInt32(dr["idBuscar"]) : 0,
                                RutaS3 = dr["RutaS3"] != DBNull.Value ? dr["RutaS3"].ToString() : string.Empty,
                                Consecutivo = dr["Consecutivo"] != DBNull.Value ? Convert.ToInt32(dr["Consecutivo"]) : 0
                            };
                            listImagenes_Bsc.Add(objImagenes_Bsc);
                        }
                    }
                    else
                    {
                        listImagenes_Bsc = null;
                    }
                }               
            }
            catch (SqlException sqlEx)
            {               
                throw new Exception("Database error: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general: " + ex.Message, ex);
            }

            return listImagenes_Bsc;
        }       

        public Imagenes_Bsc Buscar(int idImagen)
        {
            Imagenes_Bsc objImagenes_Bsc = new();           

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_SEARCH_CASAEI_Imagenes_Bsc", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idImagen", SqlDbType.Int, 4).Value = idImagen;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objImagenes_Bsc.idImagen = Convert.ToInt32(dr["idImagen"]);
                        objImagenes_Bsc.idBuscar = Convert.ToInt32(dr["idBuscar"]);
                        objImagenes_Bsc.RutaS3 = dr["RutaS3"].ToString();
                        objImagenes_Bsc.Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                    }
                    else
                    {
                        objImagenes_Bsc = null;

                    }
                }              
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objImagenes_Bsc;
        }

        public int BuscarConsecutivo(int idBuscar)
        {
            int Consecutivo = 0;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_SEARCH_IMAGENES_BSC_CONSECUTIVO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = idBuscar;
                  

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        Consecutivo = Convert.ToInt32(dr["Consecutivo"]);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Consecutivo;
        }

        public async Task<string> SubirImagen(int idBuscar, string NombreArchivo, string ArchivoBase64, int Consecutivo)
        {
            string RutaS3 = string.Empty;

            UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
            UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(180);
            if (Directory.Exists(objRuta.Ubicacion) == false)
            {
                Directory.CreateDirectory(objRuta.Ubicacion);
            }

            try
            {
                string rutaArchivo = objRuta.Ubicacion + NombreArchivo;
                byte[] bytes = Convert.FromBase64String(ArchivoBase64);
                File.WriteAllBytes(rutaArchivo, bytes);

                if (Path.GetExtension(rutaArchivo) == ".jpg")
                {
                    RutaS3 = await CopiaArchivoAlDestinoS3(rutaArchivo, idBuscar, Consecutivo);   
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return RutaS3;
        }

        public async Task<string> CopiaArchivoAlDestinoS3(string MyRutadeOrigen, int idBuscar, int Consecutivo)
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

                string Extension = string.Empty;
                Extension = Path.GetExtension(MyRutadeOrigen);
                RutaS3 =  "ImagenesdelBuscador" + "/" + idBuscar + "/" + "Imagen_" + Consecutivo.ToString("000") + Extension.Trim();

                string subioS3 = string.Empty;
                BucketsS3Repository objS3 = new(_configuration);
                subioS3 = await objS3.SubirObjetoAsync(MyRutadeOrigen, objS3Buckets.Bucket, RutaS3.Trim());
                //subioS3 = "OK";
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

    


    }
}
