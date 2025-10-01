using DocumentFormat.OpenXml.Office2010.Excel;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDepartamentosRepository
    {

        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogoDepartamentosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<CatalogoDepartamentos> Buscar(int id)
        {
            CatalogoDepartamentos objCATALOGODEPARTAMENTOS = new CatalogoDepartamentos();
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODEPARTAMENTOS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = id;
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCATALOGODEPARTAMENTOS.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);
                            objCATALOGODEPARTAMENTOS.NombreDepartamento = string.Format("{0}", dr["NombreDepartamento"]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return objCATALOGODEPARTAMENTOS!;
        }

        public List<DropDownListDatos> CargarDepartamentos()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_DEPARTAMENTOS", con))
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

        public List<DropDownListDatos> DepartamentosACargo(int IdUsuario)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_DEPARTAMENTOS_ACARGO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IdUsuario;

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
