using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class DetalleApoderadosSiemensRepository : IDetalleApoderadosSiemensRepository
    {
        public string SConexion { get; set; }
        string IDetalleApoderadosSiemensRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public DetalleApoderadosSiemensRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(DetalleApoderadosSiemens objDetalleApoderadosSiemens)
        {
            int IdDetalle = 0;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CASAEI_DETALLEAPODERADOSSIEMENS", con))
                { 
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdApoderado", SqlDbType.Int, 4).Value = objDetalleApoderadosSiemens.IdApoderado;
                    cmd.Parameters.Add("@IataDestino", SqlDbType.VarChar , 3).Value = objDetalleApoderadosSiemens.IataDestino;
                    cmd.Parameters.Add("@IdNotificador", SqlDbType.VarChar, 4).Value = objDetalleApoderadosSiemens.IdNotificador;
                    //cmd.Parameters.Add("@Fecha", SqlDbType.DateTime, 4).Value = objDetalleApoderadosSiemens.Fecha;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objDetalleApoderadosSiemens.IdUsuario;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        IdDetalle = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return IdDetalle;
        }

        public DetalleApoderadosSiemens Buscar(int IdDetalle)
        {
            DetalleApoderadosSiemens objDetalleApoderadosSiemens = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CASAEI_DETALLEAPODERADOSSIEMENS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdDetalle", SqlDbType.Int ).Value = IdDetalle;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objDetalleApoderadosSiemens.IdDetalle = Convert.ToInt32(dr["IdDetalle"]);
                        objDetalleApoderadosSiemens.IdApoderado = Convert.ToInt32(dr["IdApoderado"]);
                        objDetalleApoderadosSiemens.Apoderado = (dr["Apoderado"]).ToString();
                        objDetalleApoderadosSiemens.IataDestino = dr["IataDestino"].ToString();
                        objDetalleApoderadosSiemens.IdNotificador = Convert.ToInt32(dr["IdNotificador"]);
                        objDetalleApoderadosSiemens.Notificador = dr["Notificador"].ToString();
                        objDetalleApoderadosSiemens.Fecha = Convert.ToDateTime(dr["Fecha"]);
                        objDetalleApoderadosSiemens.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                        objDetalleApoderadosSiemens.UsuarioCapturo = dr["UsuarioCapturo"].ToString();
                    }
                    else
                    {
                        objDetalleApoderadosSiemens = null!;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objDetalleApoderadosSiemens;
        }

        public List<DetalleApoderadosSiemens> Cargar()
        {
            List<DetalleApoderadosSiemens> listDetalleApoderadosSiemens = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CASAEI_DETALLEAPODERADOSSIEMENS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DetalleApoderadosSiemens objDetalleApoderadosSiemens = new();
                            objDetalleApoderadosSiemens.IdDetalle = Convert.ToInt32(dr["IdDetalle"]);
                            objDetalleApoderadosSiemens.IdApoderado = Convert.ToInt32(dr["IdApoderado"]);
                            objDetalleApoderadosSiemens.Apoderado = (dr["Apoderado"]).ToString();
                            objDetalleApoderadosSiemens.IataDestino = dr["IataDestino"].ToString();
                            objDetalleApoderadosSiemens.IdNotificador = Convert.ToInt32(dr["IdNotificador"]);
                            objDetalleApoderadosSiemens.Notificador = dr["Notificador"].ToString();
                            objDetalleApoderadosSiemens.Fecha = Convert.ToDateTime(dr["Fecha"]);
                            objDetalleApoderadosSiemens.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objDetalleApoderadosSiemens.UsuarioCapturo = dr["UsuarioCapturo"].ToString();

                            listDetalleApoderadosSiemens.Add(objDetalleApoderadosSiemens);
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listDetalleApoderadosSiemens;
        }

        public bool Modificar(DetalleApoderadosSiemens objDetalleApoderadosSiemens)
        {
            bool SiNo = false;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_CASAEI_DETALLEAPODERADOSSIEMENS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDetalle", SqlDbType.Int, 4).Value = objDetalleApoderadosSiemens.IdDetalle;
                    cmd.Parameters.Add("@IdApoderado", SqlDbType.Int, 4).Value = objDetalleApoderadosSiemens.IdApoderado;
                    cmd.Parameters.Add("@IataDestino", SqlDbType.VarChar, 3).Value = objDetalleApoderadosSiemens.IataDestino;
                    cmd.Parameters.Add("@IdNotificador", SqlDbType.Int, 4).Value = objDetalleApoderadosSiemens.IdNotificador;
                    //cmd.Parameters.Add("@Fecha", SqlDbType.DateTime, 4).Value = objDetalleApoderadosSiemens.Fecha;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objDetalleApoderadosSiemens.IdUsuario;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int Id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        
                        if (Id == 1)
                        { 
                            SiNo = true; 
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
                throw new Exception(ex.Message);
            }
            return SiNo;
        }
      
        public bool Eliminar(int IdDetalle)
        {
            bool SiNo = false;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_DELETE_CASAEI_DETALLEAPODERADOSSIEMENS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdDetalle", SqlDbType.Int, 4).Value = IdDetalle;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int Id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                        if (Id == 1)
                        {
                            SiNo = true;
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
                throw new Exception(ex.Message);
            }           
            return SiNo;
        }
    }
}     

