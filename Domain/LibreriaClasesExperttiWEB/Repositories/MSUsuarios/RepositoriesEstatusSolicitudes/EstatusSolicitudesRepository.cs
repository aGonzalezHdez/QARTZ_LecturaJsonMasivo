using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.RepositoriesEstatusSolicitudes
{
    public class EstatusSolicitudesRepository : IEstatusSolicitudesRepository
    {
        public string SConexion { get; set; }
        string IEstatusSolicitudesRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public EstatusSolicitudesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<DropDownListDatos> Cargar()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("usuario.NET_LOAD_CASAEI_ESTATUSSOLICITUDES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
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
