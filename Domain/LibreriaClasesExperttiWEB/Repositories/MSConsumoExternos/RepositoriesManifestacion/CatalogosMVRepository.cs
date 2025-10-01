using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion
{
    public class CatalogosMVRepository : ICatalogosMVRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogosMVRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<DropDownListDatos> Cargar(int idCatalogoMV)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CatalogosMV", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idCatalogoMV", SqlDbType.Int, 4).Value = idCatalogoMV;
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
