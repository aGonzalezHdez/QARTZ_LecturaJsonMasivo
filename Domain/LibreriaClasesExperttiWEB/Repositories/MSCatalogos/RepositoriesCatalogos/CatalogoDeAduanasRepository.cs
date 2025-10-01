using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeAduanasRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogoDeAduanasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<DropDownListDatos> Cargar(int IdOficina)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_ADUANAS_LOCALES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDOFICINA", SqlDbType.Int, 4).Value = IdOficina;
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


        public List<DropDownListDatos> CargarTodas(int IdOficina)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_ADUANAS_TODAS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDOFICINA", SqlDbType.Int, 4).Value = IdOficina;
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
