using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM
{
    public class CatalogodeMonedasCOVERepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public CatalogodeMonedasCOVERepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogodeMonedasCOVE Buscar(string MonedaAnexo22)
        {
            CatalogodeMonedasCOVE objCATALOGODEMONEDASCOVE = new CatalogodeMonedasCOVE();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CATALOGODEMONEDASCOVE";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@Anexo22", SqlDbType.VarChar, 3);
            param.Value = MonedaAnexo22.Trim();

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objCATALOGODEMONEDASCOVE.IdMoneda = Convert.ToInt32(dr["IdMoneda"]);
                    objCATALOGODEMONEDASCOVE.Anexo22 = dr["Anexo22"].ToString();
                    objCATALOGODEMONEDASCOVE.OMA = dr["OMA"].ToString();
                }
                else
                {
                    objCATALOGODEMONEDASCOVE = null;
                }

                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return objCATALOGODEMONEDASCOVE;
        }

    }
}
