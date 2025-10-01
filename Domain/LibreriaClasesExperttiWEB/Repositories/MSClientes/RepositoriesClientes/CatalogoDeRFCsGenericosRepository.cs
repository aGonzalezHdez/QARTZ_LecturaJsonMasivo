using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogoDeRFCsGenericosRepository : ICatalogoDeRFCsGenericosRepository
    {

        public string SConexion { get; set; }
        string ICatalogoDeRFCsGenericosRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogoDeRFCsGenericosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<CatalogoDeRFCsGenericos> Buscar()
        {
            List<CatalogoDeRFCsGenericos> rfcsGenericos = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CASAEI_CATALOGODERFCSGENERICOS", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    rfcsGenericos = SqlDataReaderToList.DataReaderMapToList<CatalogoDeRFCsGenericos>(reader);
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return rfcsGenericos.ToList();
        }
        public CatalogoDeRFCsGenericos Buscar(string lRfc)
        {
            var objCatalogodeRFCsGenericos = new CatalogoDeRFCsGenericos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CatalogodeRFCsGenericos";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @RFC as varchar(13)
            @param = cmd.Parameters.Add("@RFC", SqlDbType.VarChar, 13);
            @param.Value = lRfc;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCatalogodeRFCsGenericos.IdRfcgenerico = Convert.ToInt32(dr["IdRFCGenerico"]);
                    objCatalogodeRFCsGenericos.NOMBRE = dr["NOMBRE"].ToString();
                    objCatalogodeRFCsGenericos.RFC = dr["RFC"].ToString();
                    objCatalogodeRFCsGenericos.OrdenDeDespliegue = Convert.ToInt32(dr["OrdenDeDespliegue"]);
                }
                else
                {
                    objCatalogodeRFCsGenericos = default;
                }
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCatalogodeRFCsGenericos;
        }

    }
}
