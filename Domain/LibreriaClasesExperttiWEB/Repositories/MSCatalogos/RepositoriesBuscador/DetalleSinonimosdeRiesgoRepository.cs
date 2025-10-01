using LibreriaClasesAPIExpertti.Entities.EntitiesBuscador;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador
{
    public class DetalleSinonimosdeRiesgoRepository : IDetalleSinonimosdeRiesgoRepository
    {
        public string SConexion { get; set; }
        string IDetalleSinonimosdeRiesgoRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public DetalleSinonimosdeRiesgoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(DetalleSinonimosdeRiesgo_Bsc objDetalleSinonimosdeRiesgo_Bsc)
        {
            int idDetalleSR;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_INSERT_CASAEI_DetalleSinonimosdeRiesgo_Bsc", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdSinonimoRiesgo", SqlDbType.Int, 4).Value = objDetalleSinonimosdeRiesgo_Bsc.IdSinonimoRiesgo;
                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = objDetalleSinonimosdeRiesgo_Bsc.idBuscar;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                    {
                        idDetalleSR = (int)cmd.Parameters["@newid_registro"].Value;
                    }
                    else
                    {
                        idDetalleSR = 0;
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
            return idDetalleSR;
        }

        public DetalleSinonimosdeRiesgo_Bsc Buscar(int idDetalleSR)
        {
            DetalleSinonimosdeRiesgo_Bsc objDetalleSinonimosdeRiesgo_Bsc = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_SEARCH_CASAEI_DetalleSinonimosdeRiesgo_Bsc", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idDetalleSR", SqlDbType.Int, 4).Value = idDetalleSR;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objDetalleSinonimosdeRiesgo_Bsc.idDetalleSR = Convert.ToInt32(dr["idDetalleSR"]);
                        objDetalleSinonimosdeRiesgo_Bsc.idBuscar = Convert.ToInt32(dr["idBuscar"]);
                        objDetalleSinonimosdeRiesgo_Bsc.IdSinonimoRiesgo = Convert.ToInt32(dr["IdSinonimoRiesgo"]);
                        objDetalleSinonimosdeRiesgo_Bsc.Sinonimo = dr["Sinonimo"].ToString();
                        objDetalleSinonimosdeRiesgo_Bsc.Categoria = dr["Categoria"].ToString();
                        objDetalleSinonimosdeRiesgo_Bsc.Requiere = dr["Requiere"].ToString();
                        objDetalleSinonimosdeRiesgo_Bsc.Nombre = dr["nombre"].ToString();

                    }
                    else
                    {
                        objDetalleSinonimosdeRiesgo_Bsc = null;

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

            return objDetalleSinonimosdeRiesgo_Bsc;
        }


        public int Modificar(DetalleSinonimosdeRiesgo_Bsc objDetalleSinonimosdeRiesgo_Bsc)
        {
            int idDetalleSR = 0;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_UPDATE_CASAEI_DetalleSinonimosdeRiesgo_Bsc", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idDetalleSR", SqlDbType.Int, 4).Value = objDetalleSinonimosdeRiesgo_Bsc.idDetalleSR;
                    cmd.Parameters.Add("@IdSinonimoRiesgo", SqlDbType.Int, 4).Value = objDetalleSinonimosdeRiesgo_Bsc.IdSinonimoRiesgo;
                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = objDetalleSinonimosdeRiesgo_Bsc.idBuscar;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                    {
                        idDetalleSR = (int)cmd.Parameters["@newid_registro"].Value;
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
            return idDetalleSR;
        }

        public bool Eliminar(int idDetalleSR)
        {
            bool result = false;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_DELETE_CASAEI_DetalleSinonimosdeRiesgo_Bsc", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idDetalleSR", SqlDbType.Int, 4).Value = idDetalleSR;
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
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return result;
        }

        public List<DetalleSinonimosdeRiesgo_Bsc> Load(int idBuscar)
        {
            List<DetalleSinonimosdeRiesgo_Bsc>? listDetalleSinonimosdeRiesgo_Bsc = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_LOAD_CASAEI_DetalleSinonimosdeRiesgo_Bsc", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = idBuscar;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DetalleSinonimosdeRiesgo_Bsc? objDetalleSinonimosdeRiesgo_Bsc = new();
                            objDetalleSinonimosdeRiesgo_Bsc.idDetalleSR = Convert.ToInt32(dr["idDetalleSR"]);
                            objDetalleSinonimosdeRiesgo_Bsc.idBuscar = Convert.ToInt32(dr["idBuscar"]);
                            objDetalleSinonimosdeRiesgo_Bsc.IdSinonimoRiesgo = Convert.ToInt32(dr["IdSinonimoRiesgo"]);

                            listDetalleSinonimosdeRiesgo_Bsc.Add(objDetalleSinonimosdeRiesgo_Bsc);
                        }
                    }
                    else
                    {
                        listDetalleSinonimosdeRiesgo_Bsc = null;
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
            return listDetalleSinonimosdeRiesgo_Bsc;
        }



        public bool EliminarBuscador(int idBuscar)
        {
            bool result = false;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_DELETE_CASAEI_Buscador_DetalleSinonimosdeRiesgo_Bsc", con))
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
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return result;
        }


        public List<ImagenesRutaS3> CargarRutaS3(int IdSinonimoRiesgo)
        {
            List<ImagenesRutaS3>? listImagenesRutaS3 = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_LOAD_CASAEI_SinonimodeRiesgoFotoURI", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdSinonimoRiesgo", SqlDbType.Int, 4).Value = IdSinonimoRiesgo;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            ImagenesRutaS3 objImagenesRutaS3 = new()
                            {
                                idImagen = Convert.ToInt32(dr["idImagen"]),
                                RutaS3 = dr["RutaS3"].ToString()
                            };
                            listImagenesRutaS3.Add(objImagenesRutaS3);
                        }

                    }
                    else
                    {
                        listImagenesRutaS3 = null;

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
            return listImagenesRutaS3;
        }

        public async Task<List<SinonimodeRiesgoFotoURI>> CargarUriFotos(int IdSinonimoRiesgo)
        {
            List<SinonimodeRiesgoFotoURI> listSinonimodeRiesgoFotoURI = new();

            List<ImagenesRutaS3> listImagenesRutaS3 = CargarRutaS3(IdSinonimoRiesgo);
            SinonimodeRiesgoFotoURI objSinonimodeRiesgoFotoURI = new();          
            
            try
            {           

                if (listImagenesRutaS3 == null || !listImagenesRutaS3.Any())
                {
                    objSinonimodeRiesgoFotoURI.IdSinonimoRiesgo = IdSinonimoRiesgo;
                    objSinonimodeRiesgoFotoURI.NoFotos = 0;
                    objSinonimodeRiesgoFotoURI.FotosUri = new List<FotosUri>();
                }
                else 
                {
                    AWSS3Repository objS3BucketsRepository = new(_configuration);
                    objSinonimodeRiesgoFotoURI.IdSinonimoRiesgo = IdSinonimoRiesgo;
                    objSinonimodeRiesgoFotoURI.NoFotos = listImagenesRutaS3.Count;
                    objSinonimodeRiesgoFotoURI.FotosUri = new List<FotosUri>();
                    foreach (ImagenesRutaS3 item in listImagenesRutaS3)
                    {
                        if (item.RutaS3 != null)
                        {
                            objSinonimodeRiesgoFotoURI.FotosUri.Add(new FotosUri { Link = await objS3BucketsRepository.BuscarLinkS3(item.RutaS3, 3) });
                        }
                    }
                } 
                
                listSinonimodeRiesgoFotoURI.Add(objSinonimodeRiesgoFotoURI);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listSinonimodeRiesgoFotoURI;

        }
    }
}
