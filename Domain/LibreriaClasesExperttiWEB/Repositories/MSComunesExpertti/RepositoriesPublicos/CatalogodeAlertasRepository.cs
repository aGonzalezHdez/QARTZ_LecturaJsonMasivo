namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos
{
    using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
    using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
    using LibreriaClasesAPIExpertti.Utilities.Converters;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualBasic;
    using System.Data;
    using System.Data.SqlClient;

    public class CatalogodeAlertasRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodeAlertasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = configuration.GetConnectionString("dbCASAEI");
        }


        public List<DropDownListDatos> Cargar(int IdOficina)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CatalogodeAlertas", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
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
