using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;
using LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.RepositoriesReportes
{
    public class CatalogodeReportesRepository: ICatalogodeReportesRepository
    {

        public string SConexion { get; set; }
        string ICatalogodeReportesRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogodeReportesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<CatalogodeReportes> Cargar(int IdUsuario)
        {
            List<CatalogodeReportes>? lstCatalogodeReportes = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODEREPORTES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CatalogodeReportes objCatalogodeReportes = new();
                            objCatalogodeReportes.IDReporte = Convert.ToInt32(dr["IDReporte"]);
                            objCatalogodeReportes.Nombre = dr["Nombre"].ToString();

                            lstCatalogodeReportes.Add(objCatalogodeReportes);
                        }
                    }
                    else

                        lstCatalogodeReportes = null;
                }               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstCatalogodeReportes;
        }  
    }
}
