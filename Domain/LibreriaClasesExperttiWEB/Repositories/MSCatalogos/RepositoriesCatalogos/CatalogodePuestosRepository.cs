using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{    
    public class CatalogodePuestosRepository : ICatalogodePuestosRepository
    {
        public string SConexion { get; set; }
        string ICatalogodePuestosRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogodePuestosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<DropDownListDatos> CargarPuestos()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODEPUESTOS", con))
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
        public CatalogodePuestos GetPuesto(int IdPuesto)
        {
            CatalogodePuestos puesto = null;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_GET_CATALOGODEPUESTO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdPuesto", IdPuesto);

                    using SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            puesto = new CatalogodePuestos
                            {
                                IDPuesto = Convert.ToInt32(reader["IDPuesto"]),
                                Puesto = reader["Puesto"].ToString(),
                                IdPuestoJefe = Convert.ToInt32(reader["idPuestoJefe"]),
                                IdNivelPuesto = Convert.ToInt32(reader["idNivelPuesto"]),
                                TodasOficinas = Convert.ToBoolean(reader["TodasOficinas"]),
                                TodosDepartamentos = Convert.ToBoolean(reader["TodosDepartamentos"]),
                                TodasAduanas = Convert.ToBoolean(reader["TodasAduanas"]),
                                AmbasOperaciones = Convert.ToBoolean(reader["AmbasOperaciones"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString()+ " NET_GET_CATALOGODEPUESTO");
            }
            return puesto;
        }
    }
}
