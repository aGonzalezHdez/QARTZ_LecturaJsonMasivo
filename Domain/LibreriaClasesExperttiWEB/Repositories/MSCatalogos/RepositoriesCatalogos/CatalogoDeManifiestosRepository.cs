using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using Microsoft.Extensions.Configuration;
namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{

    public class CatalogoDeManifiestosRepository
    {

        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogoDeManifiestosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<DropDownListDatos> Cargar(int IdOficina, int idDatosdeEmpresa)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODEMANIFIESTOS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@idDatosdeEmpresa", SqlDbType.Int, 4).Value = idDatosdeEmpresa;

                    using SqlDataReader reader = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return comboList;
        }


    }
}
