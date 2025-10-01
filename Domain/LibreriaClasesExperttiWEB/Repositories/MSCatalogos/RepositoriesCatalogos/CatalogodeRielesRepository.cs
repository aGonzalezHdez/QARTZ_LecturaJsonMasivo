using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogodeRielesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public CatalogodeRielesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogodeRieles Buscar(int idRiel)
        {
            CatalogodeRieles obj = new();

            try
            {
                using (SqlConnection con = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODERIELES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDRiel", SqlDbType.Int, 4).Value = idRiel;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        obj.idRiel = Convert.ToInt32(dr["idRiel"]);
                        obj.NoRiel = Convert.ToInt32(dr["NoRiel"]);
                        obj.Descripcion = (dr["Descripcion"].ToString());
                        obj.Orden = Convert.ToInt32(dr["Orden"]);
                        obj.DescripcionWeb= (dr["DescripcionWeb"].ToString());
                        obj.ActivoWeb= Convert.ToBoolean(dr["ActivoWeb"]);

                    }
                    else
                    {
                        obj = null!;
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }


            return obj;
        }

    }
}
