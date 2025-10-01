using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;
using LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.RepositoriesReportes
{
    public class PermisosyReportesporUsuarioRepository : IPermisosyReportesporUsuarioRepository

    {
        public string SConexion { get; set; }
        string IPermisosyReportesporUsuarioRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public PermisosyReportesporUsuarioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public int Insertar(int IDUsuario, int IDReporte)
        {
            int id; 

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_PERMISOSYREPORTESPORUSUARIO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4).Value = IDUsuario;
                    cmd.Parameters.Add("@IDReporte", SqlDbType.Int, 4).Value = IDReporte;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    if (System.Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        id = System.Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    else
                        id = 0;                   
                }
            }
            catch (Exception ex)
            {
               
                throw new Exception(ex.Message.ToString());
            }
          
            return id;
        }

        public int Eliminar(int IDUsuario, int IDPermisosyReportes)
        {
            int id = 0;  

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_DELETE_PERMISOSYREPORTESPORUSUARIO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDPermisosyReportes", SqlDbType.Int, 4).Value = IDPermisosyReportes;
                    cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4).Value = IDUsuario;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    if (System.Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        id = System.Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    else
                        id = 0;

                }
            }
            catch (Exception ex)
            {         
                throw new Exception(ex.Message.ToString() + "NET_DELETE_PERMISOSYREPORTESPORUSUARIO");            }
         
            return id;
        }

        public List<PermisosYReportesporUsuario> Cargar(int IDUsuario)
        {
            List<PermisosYReportesporUsuario>? lstPermisosyReportesporUsuario = new();
           
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_LOAD_PERMISOSYREPORTESPORUSUARIO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Idusuario", SqlDbType.Int).Value = IDUsuario;

                    using SqlDataReader dr = cmd.ExecuteReader();


                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            PermisosYReportesporUsuario objPermisosyReportesporUsuario = new();

                            objPermisosyReportesporUsuario.IDPermisosyReportes = Convert.ToInt32(dr["IDPermisosyReportes"]);
                            objPermisosyReportesporUsuario.Nombre = dr["Nombre"].ToString();
                            objPermisosyReportesporUsuario.IDUsuario = Convert.ToInt32(dr["IDUsuario"]);
                            objPermisosyReportesporUsuario.IDReporte = Convert.ToInt32(dr["IDReporte"]);

                            lstPermisosyReportesporUsuario.Add(objPermisosyReportesporUsuario);
                        }
                    }
                    else
                    {
                        lstPermisosyReportesporUsuario = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstPermisosyReportesporUsuario;
        }


    }
}
