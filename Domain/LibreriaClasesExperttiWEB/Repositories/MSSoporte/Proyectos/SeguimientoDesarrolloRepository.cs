using LibreriaClasesAPIExpertti.Entities.EntitiesProyectos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Proyectos
{
    public class SeguimientoDesarrolloRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public SeguimientoDesarrolloRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(SeguimientoDesarrollo objSeguimientoDesarrollo)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CASAEI_SEGUIMIENTODESARROLLO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdDetalle", SqlDbType.Int,4);
                @param.Value = objSeguimientoDesarrollo.IdDetalle;

                @param = cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 500);
                @param.Value = objSeguimientoDesarrollo.Descripcion;

                @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4);
                @param.Value = objSeguimientoDesarrollo.IdUsuario;

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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CASAEI_SEGUIMIENTODESARROLLO");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public int Modificar(SeguimientoDesarrollo objSeguimientoDesarrollo)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_UPDATE_CASAEI_SEGUIMIENTODESARROLLO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdSeguimiento", SqlDbType.Int, 4);
                @param.Value = objSeguimientoDesarrollo.IdSeguimiento;

                @param = cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 500);
                @param.Value = objSeguimientoDesarrollo.Descripcion;


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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CASAEI_SEGUIMIENTODESARROLLO");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public List<SeguimientoDesarrollo> Cargar(int IdDetalle)
        {
            List<SeguimientoDesarrollo> lista = new List<SeguimientoDesarrollo>();
          
            var objREFERENCIAS = new Referencias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CASAEI_SEGUIMIENTODESARROLLO";
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

                    while (dr.Read())
                    {
                        SeguimientoDesarrollo objSeguimientoDesarrollo = new();
                        objSeguimientoDesarrollo.IdSeguimiento = Convert.ToInt32(dr["IdSeguimiento"]);
                        objSeguimientoDesarrollo.Descripcion = (dr["Descripcion"]).ToString();
                        objSeguimientoDesarrollo.Fecha = Convert.ToDateTime(dr["Fecha"]);
                        objSeguimientoDesarrollo.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                        objSeguimientoDesarrollo.NombreUsuario = (dr["NombreUsuario"]).ToString();

                        lista.Add(objSeguimientoDesarrollo);
                    }
                }
                else
                {
                    lista.Clear();
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
            return lista;
        }



    }
}
