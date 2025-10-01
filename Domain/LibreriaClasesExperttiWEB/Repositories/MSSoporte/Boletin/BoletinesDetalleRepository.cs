using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesBoletin;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Boletin
{
    public class BoletinesDetalleRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public BoletinesDetalleRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public BoletinDetalle InsertBoletinDetalle(BoletinDetalleInsert bd)
        {
            int newId = 0;
            BoletinDetalle boletinDetalle = new BoletinDetalle();
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_INSERT_CASAEI_BOLETINESDETALLE", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletin", bd.IdBoletin.HasValue ? bd.IdBoletin.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrEmpty(bd.Descripcion) ? DBNull.Value : bd.Descripcion);
                    cmd.Parameters.AddWithValue("@activo", bd.activo.HasValue ? bd.activo.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@idUsuario", bd.idUsuario.HasValue ? bd.idUsuario.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Consecutivo", bd.Consecutivo.HasValue ? bd.Consecutivo.Value : DBNull.Value);

                    try
                    {
                        cn.Open();
                        newId = Convert.ToInt32(cmd.ExecuteScalar());
                        boletinDetalle = SearchBoletinDetalle(newId);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al insertar BoletinDetalle: " + ex.Message);
                    }
                }
            }
            return boletinDetalle;
        }

        public int UpdateBoletinDetalle(BoletinDetalleUpdate bd)
        {
            int affectedRows = 0;
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_UPDATE_CASAEI_BOLETINESDETALLE", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletinDetalle", bd.IdBoletinDetalle);
                    cmd.Parameters.AddWithValue("@IdBoletin", bd.IdBoletin.HasValue ? bd.IdBoletin.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Descripcion", string.IsNullOrEmpty(bd.Descripcion) ? DBNull.Value : bd.Descripcion);
                    cmd.Parameters.AddWithValue("@activo", bd.activo.HasValue ? bd.activo.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@idUsuario", bd.idUsuario.HasValue ? bd.idUsuario.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Consecutivo", bd.Consecutivo.HasValue ? bd.Consecutivo.Value : DBNull.Value);

                    try
                    {
                        cn.Open();
                        //affectedRows = Convert.ToInt32(cmd.ExecuteScalar());
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al actualizar BoletinDetalle: " + ex.Message);
                    }
                }
            }
            return affectedRows;
        }

        public int DeleteBoletinDetalle(int id)
        {
            int affectedRows = 0;
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_DELETE_CASAEI_BOLETINESDETALLE", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletinDetalle", id);

                    try
                    {
                        bool letContinue = DeleteBoletinDetalleImagenes(id);

                        if (letContinue)
                        {
                            cn.Open();
                            var result = cmd.ExecuteScalar();
                            affectedRows = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al eliminar BoletinDetalle : " + ex.Message);
                    }
                }
            }
            return affectedRows;
        }

        private bool DeleteBoletinDetalleImagenes(int id)
        {
            bool result = false;
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_DELETE_CASAEI_BOLETINESDETALLEIMAGENES",cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletinDetalle", id);

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al eliminar las imagenes del BoletinDetalle: " + ex.Message);
                    }
                }
            }
            return result;
        }


        public BoletinDetalle SearchBoletinDetalle(int idBoletinDetalle)
        {
            BoletinDetalle bd = null;
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_SEARCH_CASAEI_BOLETINESDETALLE", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletinDetalle", idBoletinDetalle);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                bd = new BoletinDetalle
                                {
                                    IdBoletinDetalle = Convert.ToInt32(dr["IdBoletinDetalle"]),
                                    IdBoletin = dr["IdBoletin"] != DBNull.Value ? Convert.ToInt32(dr["IdBoletin"]) : null,
                                    Descripcion = dr["Descripcion"] != DBNull.Value ? dr["Descripcion"].ToString() : null,
                                    activo = dr["activo"] != DBNull.Value ? Convert.ToBoolean(dr["activo"]) : null,
                                    idUsuario = dr["idUsuario"] != DBNull.Value ? Convert.ToInt32(dr["idUsuario"]) : null,
                                    fechaAlta = dr["fechaAlta"] != DBNull.Value ? Convert.ToDateTime(dr["fechaAlta"]) : null,
                                    Consecutivo = dr["Consecutivo"] != DBNull.Value ? Convert.ToInt32(dr["Consecutivo"]) : null
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al buscar BoletinDetalle por Id: " + ex.Message);
                    }
                }
            }
            return bd;
        }


        public List<BoletinDetalle> LoadBoletinDetallesByBoletin(int idBoletin)
        {
            List<BoletinDetalle> list = new List<BoletinDetalle>();
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_LOAD_CASAEI_BOLETINESDETALLE", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletin", idBoletin);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                BoletinDetalle bd = new BoletinDetalle
                                {
                                    IdBoletinDetalle = Convert.ToInt32(dr["IdBoletinDetalle"]),
                                    IdBoletin = dr["IdBoletin"] != DBNull.Value ? Convert.ToInt32(dr["IdBoletin"]) : null,
                                    Descripcion = dr["Descripcion"] != DBNull.Value ? dr["Descripcion"].ToString() : null,
                                    activo = dr["activo"] != DBNull.Value ? Convert.ToBoolean(dr["activo"]) : null,
                                    idUsuario = dr["idUsuario"] != DBNull.Value ? Convert.ToInt32(dr["idUsuario"]) : null,
                                    fechaAlta = dr["fechaAlta"] != DBNull.Value ? Convert.ToDateTime(dr["fechaAlta"]) : null,
                                    Consecutivo = dr["Consecutivo"] != DBNull.Value ? Convert.ToInt32(dr["Consecutivo"]) : null
                                };
                                list.Add(bd);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al cargar BoletinDetalles por IdBoletin: " + ex.Message);
                    }
                }
            }
            return list;
        }
    }
}
