using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.RepositoriesUsuarios
{
    public class PermisosExperttiRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public PermisosExperttiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<int> ObtenerPermisos(int IdUsuario, int IdPantalla)
        {
            int NivelDeAcceso = 0;

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_PANTALLASYREPORTESPORUSUARIO_NIVEL_DE_ACCESO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdPantalla", SqlDbType.Int, 4).Value = IdPantalla;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;
                    cmd.Parameters.Add("@NivelDeAcceso", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@NivelDeAcceso"].Value) != -1)
                        {
                            if (Convert.ToInt32(cmd.Parameters["@NivelDeAcceso"].Value) != 0)
                            {
                                NivelDeAcceso = Convert.ToInt32(cmd.Parameters["@NivelDeAcceso"].Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return NivelDeAcceso;
        }

    }
}
