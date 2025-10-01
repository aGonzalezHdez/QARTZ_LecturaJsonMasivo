using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones
{
    public class SinonimosdeRiesgoRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public SinonimosdeRiesgoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public bool ExisteSinonimoDeRiesgoTextilesZapatos(string Descripcion)
        {
            bool result = false;

            var conn = new SqlConnection(sConexion);
            conn.Open();
            try
            {

                SqlCommand cmd;
                SqlDataReader dr;

                cmd = new SqlCommand("SELECT 1 FROM SINONIMOSDERIESGO SR INNER JOIN CATALOGODECATEGORIASSR C ON SR.IdCategoriaSR=C.IdCategoriaSR WHERE SR.IdCategoriaSR IN (95,112) AND @Descripcion LIKE SR.SINONIMO", conn);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar);
                cmd.Parameters["@Descripcion"].Value = Descripcion;

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    result = true;

                }

                conn.Close();
            }
            catch (Exception e)
            {
                conn.Close();
            }

            return result;

        }

    }
}
