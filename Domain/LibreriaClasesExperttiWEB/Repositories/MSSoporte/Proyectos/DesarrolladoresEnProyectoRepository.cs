using LibreriaClasesAPIExpertti.Entities.EntitiesProyectos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Proyectos
{
    public class DesarrolladoresEnProyectoRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public DesarrolladoresEnProyectoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public List<DesarrolladoresEnProyecto> Cargar(int IdDetalle)
        {
           List< DesarrolladoresEnProyecto> lstDesarrolladoresEnProyecto = new();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CASAEI_DESARROLLADORESENPROYECTO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdDetalle", SqlDbType.Int, 4);
            @param.Value = IdDetalle;



            // @IDDatosDeEmpresa
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                  while  (dr.Read())
                    {
                        DesarrolladoresEnProyecto objDesarrolladoresEnProyecto = new();
                        objDesarrolladoresEnProyecto.idDesarrolladores = Convert.ToInt32(dr["idDesarrolladores"]);
                        objDesarrolladoresEnProyecto.IdDetalle = Convert.ToInt32(dr["IdDetalle"]);
                        objDesarrolladoresEnProyecto.IdDesarrollador = Convert.ToInt32(dr["IdDesarrollador"]);
                        objDesarrolladoresEnProyecto.FechadeAsignacion = Convert.ToDateTime(dr["FechadeAsignacion"]);
                        objDesarrolladoresEnProyecto.Nombre = dr["Nombre"].ToString();

                        lstDesarrolladoresEnProyecto.Add(objDesarrolladoresEnProyecto);
                    }

                   
                }
                else
                {
                    lstDesarrolladoresEnProyecto.Clear();
                }
                dr.Close();
                // cn.Close()
                // SqlConnection.ClearPool(cn)
                // cn.Dispose()
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            cn.Dispose();
            return lstDesarrolladoresEnProyecto;
        }

        public int Insertar(DesarrolladoresEnProyecto objDesarrolladoresEnProyecto)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CASAEI_DESARROLLADORESENPROYECTO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdDetalle", SqlDbType.SmallInt);
                @param.Value = objDesarrolladoresEnProyecto.IdDetalle;

                @param = cmd.Parameters.Add("@IdDesarrollador", SqlDbType.SmallInt);
                @param.Value = objDesarrolladoresEnProyecto.IdDesarrollador;

                @param = cmd.Parameters.Add("@IdTipo", SqlDbType.SmallInt);
                @param.Value = objDesarrolladoresEnProyecto.IdTipo;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CASAEI_DESARROLLADORESENPROYECTO");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public int Eliminar(int idDesarrolladores)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_DELETE_CASAEI_DESARROLLADORESENPROYECTO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@idDesarrolladores", SqlDbType.SmallInt);
                @param.Value = idDesarrolladores;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_DELETE_CASAEI_DESARROLLADORESENPROYECTO");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

    }
}
