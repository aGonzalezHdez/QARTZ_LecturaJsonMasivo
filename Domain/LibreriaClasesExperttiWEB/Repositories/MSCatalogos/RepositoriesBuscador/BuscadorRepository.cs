using LibreriaClasesAPIExpertti.Entities.EntitiesBuscador;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;


namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesBuscador
{
    public class BuscadorRepository : IBuscadorRepository
    {
        public string SConexion { get; set; }
        string IBuscadorRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public BuscadorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(BuscadorBuscador objBuscador)
        {
            int idBuscar;          

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_INSERT_Buscador", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PalabraClave", SqlDbType.VarChar, 200).Value = objBuscador.PalabraClave;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, -1).Value = objBuscador.Descripcion;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objBuscador.Operacion;
                    cmd.Parameters.Add("@idTipodeMercancia", SqlDbType.Int, 4).Value = objBuscador.idTipodeMercancia;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {

                        if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                        {
                            idBuscar = (int)cmd.Parameters["@newid_registro"].Value;
                        }
                        else
                        {
                            idBuscar = 0;
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
            return idBuscar;
        }

        public int Modificar(BuscadorBuscador objBuscador)
        {
            int idBuscar;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_UPDATE_Buscador", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = objBuscador.idBuscar;
                    cmd.Parameters.Add("@PalabraClave", SqlDbType.VarChar, 200).Value = objBuscador.PalabraClave;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, -1).Value = objBuscador.Descripcion;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objBuscador.Operacion;
                    cmd.Parameters.Add("@idTipodeMercancia", SqlDbType.Int, 4).Value = objBuscador.idTipodeMercancia;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {

                        if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                        {
                            idBuscar = (int)cmd.Parameters["@newid_registro"].Value;
                        }
                        else
                        {
                            idBuscar = 0;
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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_Buscador");
            }           
            return idBuscar;
        }

        public BuscadorBuscador Buscar(int idBuscar)
        {
            BuscadorBuscador? objBuscador = new ();
            try
            {  
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("Buscador.NET_SEARCH_Buscador", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = idBuscar;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            objBuscador.idBuscar = Convert.ToInt32(dr["idBuscar"]);
                            objBuscador.PalabraClave = dr["PalabraClave"].ToString();
                            objBuscador.Descripcion = dr["Descripcion"].ToString();
                            objBuscador.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                            objBuscador.Operacion = dr["Operacion"] != DBNull.Value ? Convert.ToInt32(dr["Operacion"]) : 0;
                            objBuscador.idTipodeMercancia = dr["idTipodeMercancia"] != DBNull.Value ? Convert.ToInt32(dr["idTipodeMercancia"]) : 0;

                        }
                        else
                        {
                            objBuscador = default;
                        }
                    }
                    else
                    {
                        objBuscador = default;
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
            return objBuscador;
        }

        public List<BuscadorBuscador> Load(string PalabraClave)
        {
            List<BuscadorBuscador> lstBuscador = new();   

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_LOAD_Buscador", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@PalabraClave", SqlDbType.VarChar, 200).Value = PalabraClave;

                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            BuscadorBuscador objBuscador = new();
                            objBuscador.idBuscar = Convert.ToInt32(dr["idBuscar"]);
                            objBuscador.PalabraClave = (dr["PalabraClave"]).ToString();
                            objBuscador.Descripcion = (dr["Descripcion"]).ToString();
                            objBuscador.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                            objBuscador.Operacion = dr["Operacion"] != DBNull.Value ? Convert.ToInt32(dr["Operacion"]) : 0; 
                            objBuscador.idTipodeMercancia = dr["idTipodeMercancia"] != DBNull.Value ? Convert.ToInt32(dr["idTipodeMercancia"]) : 0; 
                           
                            lstBuscador.Add(objBuscador);
                        }
                    }
                    else
                    {
                    lstBuscador.Clear();
                    }               
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {

                Interaction.MsgBox(ex.Message.ToString());

            } 
            return lstBuscador;
        }      

        public List<SinonimosdeRiesgoTodos> SinonimosdeRiesgoTodos(string Descripcion)
        {
            List<SinonimosdeRiesgoTodos>? lstSinonimosdeRiesgoTodos = new();

            using (SqlConnection con = new(SConexion))
            using (SqlCommand cmd = new("NET_REPORT_SinonimosdeRiesgoTodos", con))
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 200).Value = Descripcion;

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SinonimosdeRiesgoTodos objSinonimosdeRiesgoTodos = new();

                        objSinonimosdeRiesgoTodos.IdSinonimoRiesgo = Convert.ToInt32( dr["IdSinonimoRiesgo"]);
                        objSinonimosdeRiesgoTodos.Sinonimo = dr["Sinonimo"].ToString();
                        objSinonimosdeRiesgoTodos.Categoria = dr["Categoria"].ToString();
                        objSinonimosdeRiesgoTodos.Requiere = dr["Requiere"].ToString();
                        objSinonimosdeRiesgoTodos.Nombre = dr["nombre"].ToString();

                        lstSinonimosdeRiesgoTodos.Add(objSinonimosdeRiesgoTodos);
                    }
                }
                else
                {
                    lstSinonimosdeRiesgoTodos = null;
                }

            }  
            return lstSinonimosdeRiesgoTodos;
        }

        public async Task<int> InsertarBuscador(BuscadorBuscador objBuscador)
        {

            //1.-
            int idBuscar = Insertar(objBuscador);
            //2.-
            Imagenes_BscRepository objImagenes_BscRepository = new(_configuration); 
            foreach (Imagenes_Bsc item in objBuscador.Imagenes)
            { 
                Imagenes_Bsc objImagenes_Bsc = new()
                {
                    idBuscar = idBuscar,
                    NombreArchivo = item.NombreArchivo,
                    ArchivoBase64 = item.ArchivoBase64
                };
                await objImagenes_BscRepository.InsertarImagenes(objImagenes_Bsc);               
            }
            //3.-
            DetalleSinonimosdeRiesgoRepository listDetalleSinonimosdeRiesgo_Bsc = new(_configuration);
            foreach (DetalleSinonimosdeRiesgo_Bsc item in objBuscador.SinonimosdeRiesgo)
            {
                DetalleSinonimosdeRiesgo_Bsc objDetalleSinonimosdeRiesgo_Bsc = new()
                {
                    idBuscar = idBuscar,
                    IdSinonimoRiesgo = item.IdSinonimoRiesgo
                };

                if (objDetalleSinonimosdeRiesgo_Bsc.IdSinonimoRiesgo != 0)
                {
                    listDetalleSinonimosdeRiesgo_Bsc.Insertar(objDetalleSinonimosdeRiesgo_Bsc);
                }

            }
            return idBuscar;
        }

        public List<BuscadorBuscador> LoadBuscador(string PalabraClave)
        {
            List<BuscadorBuscador>? lstBuscador = Load(PalabraClave);

            if (lstBuscador != null)
            {
                foreach (BuscadorBuscador item in lstBuscador)
                {
                    Imagenes_BscRepository? listImagenes_BscRepository = new(_configuration);
                    item.Imagenes = listImagenes_BscRepository.Load(item.idBuscar);

                    DetalleSinonimosdeRiesgoRepository listDetalleSinonimosdeRiesgo_Bsc = new(_configuration);
                    item.SinonimosdeRiesgo = listDetalleSinonimosdeRiesgo_Bsc.Load(item.idBuscar);
                }
            }
            return lstBuscador;
        }

        public BuscadorBuscador BuscarBuscador(int idBuscar)
        {
            BuscadorBuscador? objBuscador = Buscar(idBuscar);

            if (objBuscador != null)
            {
                Imagenes_BscRepository? listImagenes_BscRepository = new(_configuration);
                objBuscador.Imagenes = listImagenes_BscRepository.Load(idBuscar);

                DetalleSinonimosdeRiesgoRepository listDetalleSinonimosdeRiesgo_Bsc = new(_configuration);
                objBuscador.SinonimosdeRiesgo = listDetalleSinonimosdeRiesgo_Bsc.Load(idBuscar);
            }            
            return objBuscador;
        }

        public async Task<int> ModificarBuscador(BuscadorBuscador objBuscador)
        {           
            try
            {            
                Modificar(objBuscador);
                
                //2.-
                Imagenes_BscRepository objImagenes_BscRepository = new(_configuration);
                //Elimina Imagenes
                if (objBuscador.Imagenes?.Count > 0)
                {
                    await objImagenes_BscRepository.EliminarImagenes(objBuscador.idBuscar);
                    //Inserta Imagenes

                    foreach (Imagenes_Bsc item in objBuscador.Imagenes)
                    {
                        Imagenes_Bsc objImagenes_Bsc = new()
                        {
                            idBuscar = objBuscador.idBuscar,
                            NombreArchivo = item.NombreArchivo,
                            ArchivoBase64 = item.ArchivoBase64
                        };
                        await objImagenes_BscRepository.InsertarImagenes(objImagenes_Bsc);
                    }
                }

                //3.-
                DetalleSinonimosdeRiesgoRepository listDetalleSinonimosdeRiesgo_Bsc = new(_configuration);
                //Elimina
                listDetalleSinonimosdeRiesgo_Bsc.EliminarBuscador(objBuscador.idBuscar);
                //Inserta
                foreach (DetalleSinonimosdeRiesgo_Bsc item in objBuscador.SinonimosdeRiesgo)
                {
                    DetalleSinonimosdeRiesgo_Bsc objDetalleSinonimosdeRiesgo_Bsc = new()
                    {
                        idBuscar = objBuscador.idBuscar,
                        IdSinonimoRiesgo = item.IdSinonimoRiesgo
                    };
                    listDetalleSinonimosdeRiesgo_Bsc.Insertar(objDetalleSinonimosdeRiesgo_Bsc);
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
            return objBuscador.idBuscar;

        }
        //public bool ModificarBuscador(BuscadorBuscador objBuscador)
        //{
        //    bool SiNo = false;
        //    try
        //    {
        //        using (SqlConnection con = new(SConexion))
        //        using (SqlCommand cmd = new("buscador.NET_UPDATE_TODOS", con))                 
        //        {
        //            con.Open();
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            // //1.- Parámetros tabla [buscador].[Buscador]
        //            cmd.Parameters.AddWithValue("@idBuscar", objBuscador.idBuscar);
        //            cmd.Parameters.AddWithValue("@PalabraClave", (object)objBuscador.PalabraClave ?? DBNull.Value);
        //            cmd.Parameters.AddWithValue("@Descripcion", (object)objBuscador.Descripcion ?? DBNull.Value);
        //            cmd.Parameters.AddWithValue("@FechaAlta", (object)objBuscador.FechaAlta ?? DBNull.Value);

        //            // //2.- Parámetro para la lista de imágenes
        //            //var imagenesTable = new DataTable();
        //            //imagenesTable.Columns.Add("idImagen", typeof(int));
        //            //imagenesTable.Columns.Add("idBuscar", typeof(int));
        //            //imagenesTable.Columns.Add("RutaS3", typeof(string));
        //            //imagenesTable.Columns.Add("Consecutivo", typeof(int));

        //            //foreach (var imagen in objBuscador.Imagenes)
        //            //{
        //            //    imagenesTable.Rows.Add(imagen.idImagen, imagen.idBuscar, imagen.RutaS3, (object)imagen.Consecutivo ?? DBNull.Value);
        //            //}
        //            //var imagenesParam = cmd.Parameters.AddWithValue("@ImagenesList", imagenesTable);
        //            //imagenesParam.SqlDbType = SqlDbType.Structured;

        //            // //3.- Parámetro para la lista de sinónimos
        //            var sinonimosTable = new DataTable();
        //            sinonimosTable.Columns.Add("idDetalleSR", typeof(int));
        //            sinonimosTable.Columns.Add("idBuscar", typeof(int));
        //            sinonimosTable.Columns.Add("IdSinonimoRiesgo", typeof(int));

        //            foreach (var sinonimo in objBuscador.SinonimosdeRiesgo)
        //            {
        //                sinonimosTable.Rows.Add(sinonimo.idDetalleSR, sinonimo.idBuscar, (object)sinonimo.IdSinonimoRiesgo ?? DBNull.Value);
        //            }
        //            var sinonimosParam = cmd.Parameters.AddWithValue("@SinonimosRiesgoList", sinonimosTable);
        //            sinonimosParam.SqlDbType = SqlDbType.Structured;


        //            cmd.ExecuteNonQuery();
        //            //if ((int)cmd.Parameters["@newid_registro"].Value == 1)
        //            //{
        //            SiNo = true;
        //            //}
        //        }
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        throw new Exception("Database error: " + sqlEx.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message.ToString());
        //    }
        //    return SiNo;

        //}

        public async Task<bool> EliminarBuscador(int idBuscar)
        {
            bool SiNo = false;
            //Cargar la rutas de imagenes
            //Eliminar del RutaS3


            Imagenes_BscRepository imagenes_BscRepository = new(_configuration);
            List<Imagenes_Bsc>  listImagenes_Bsc = imagenes_BscRepository.Load(idBuscar);            
                
            //Eliminar de la tabla de [buscador].[Imagenes_Bsc]
            //Eliminar relación DetalleSinonimosdeRiesgo_Bsc
            //Eliminar la tabla [buscador].[Buscador]
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("buscador.NET_DELETE_CASAEI_BUSCADOR_BUSCADOR", con))
                {
                    con.Open();
            
                    cmd.CommandType = CommandType.StoredProcedure;
            
                    cmd.Parameters.Add("@idBuscar", SqlDbType.Int, 4).Value = idBuscar;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                        {
                            SiNo = true;

                            if (listImagenes_Bsc == null || !listImagenes_Bsc.Any())
                            {
                                return SiNo;
                            }
                         
                            foreach (Imagenes_Bsc item in listImagenes_Bsc)
                            {
                                BucketsS3Repository objS3 = new(_configuration);
                                await objS3.BorrarObjectoAsync("3", item.RutaS3);
                            }
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
            return SiNo;
        }
    }
}
