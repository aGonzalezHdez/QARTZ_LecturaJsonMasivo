using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;
using LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.RepositoriesPantallas
{
    public class PantallasYReportesPorUsuarioRepository: IPantallasYReportesPorUsuarioRepository
    {
        public string SConexion { get; set; }
        string IPantallasYReportesPorUsuarioRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public PantallasYReportesPorUsuarioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<PantallasYReportesPorUsuario> Cargar(int IdUsuario)
        {
            List<PantallasYReportesPorUsuario> listPantallasYReportesPorUsuario = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_PANTALLASYREPORTESPORUSUARIO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                       while (dr.Read())
                       {
                            PantallasYReportesPorUsuario objPantallasYReportesPorUsuario = new();
                            objPantallasYReportesPorUsuario.IdPantalla = Convert.ToInt32(dr["idpantalla"]);
                            objPantallasYReportesPorUsuario.Modulo = dr["modulo"].ToString();
                            objPantallasYReportesPorUsuario.Descripcion = dr["descripcion"].ToString();
                            objPantallasYReportesPorUsuario.NivelDeAcceso = dr["NivelDeAcceso"].ToString();


                            listPantallasYReportesPorUsuario.Add(objPantallasYReportesPorUsuario);
                        };
                    }
                    else
                    {
                        listPantallasYReportesPorUsuario = null!;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return listPantallasYReportesPorUsuario;
        }

        public void Insertar(Pantallas objPantallas)
        {   
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_PANTALLASYREPORTESPORUSUARIO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objPantallas.IdUsuario;
                    cmd.Parameters.Add("@IdPantalla", SqlDbType.Int, 4).Value = objPantallas.IdPantalla;
                    cmd.Parameters.Add("@NivelDeAcceso", SqlDbType.Int, 4).Value = objPantallas.IDNivel;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }          
        }

        public void Eliminar(Pantallas objPantallas)
        {            
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_DELETE_PANTALLASYREPORTESPORUSUARIO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objPantallas.IdUsuario;
                    cmd.Parameters.Add("@IdPantalla", SqlDbType.Int, 4).Value = objPantallas.IdPantalla;
                    cmd.Parameters.Add("@NivelDeAcceso", SqlDbType.Int, 4).Value = objPantallas.IDNivel;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }       
        }
    }
}
