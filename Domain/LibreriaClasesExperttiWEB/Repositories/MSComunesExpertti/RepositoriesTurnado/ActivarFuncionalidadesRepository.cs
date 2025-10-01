using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado
{
    public class ActivarFuncionalidadesRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public ActivarFuncionalidadesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public ActivarFuncionalidades BuscarPorOficina(string Funcionalidad, int IdOficina)
        {
            var objActivarFuncionalidades = new ActivarFuncionalidades();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_SEARCH_CASAEI_ActivarFuncionalidadesxOficina";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@Funcionalidad", SqlDbType.VarChar, 250);
                @param.Value = Funcionalidad;

                @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
                @param.Value = IdOficina;

                cn.ConnectionString = SConexion;
                cn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objActivarFuncionalidades.idFuncionalidad = Convert.ToInt32(dr["idFuncionalidad"]);
                    objActivarFuncionalidades.Funcionalidad = dr["Funcionalidad"].ToString();
                    objActivarFuncionalidades.Activo = Convert.ToBoolean(dr["Activo"]);
                    if (!(dr["Idoficina"] == null))
                    {
                        objActivarFuncionalidades.Idoficina = Convert.ToInt32(dr["Idoficina"]);
                    }
                }

                else
                {
                    objActivarFuncionalidades = default;
                }
                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objActivarFuncionalidades;
        }

        public ActivarFuncionalidades Buscar(int idFuncionalidad)
        {
            var objActivarFuncionalidades = new ActivarFuncionalidades();

            using (var cn = new SqlConnection(SConexion))
            using (var cmd = new SqlCommand("NET_SEARCH_CASAEI_ActivarFuncionalidades", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@idFuncionalidad", SqlDbType.Int) { Value = idFuncionalidad });

                try
                {
                    cn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objActivarFuncionalidades.idFuncionalidad = Convert.ToInt32(dr["idFuncionalidad"]);
                            objActivarFuncionalidades.Funcionalidad = dr["Funcionalidad"].ToString();
                            objActivarFuncionalidades.Activo = Convert.ToBoolean(dr["Activo"]);
                        }
                        else
                        {
                            objActivarFuncionalidades = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error en Buscar: {ex.Message}");
                }
            }

            return objActivarFuncionalidades;
        }
    }
}
