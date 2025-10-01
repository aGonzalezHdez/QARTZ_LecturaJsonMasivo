using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeRegexRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogoDeRegexRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CatalogoDeRegex Buscar(int id)
        {
            var objCATALOGODEREGEX = new CatalogoDeRegex();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {

                cmd.CommandText = "NET_SEARCH_CATALOGODEREGEX";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @IdRegex INTEGER 
                @param = cmd.Parameters.Add("@IdRegex", SqlDbType.Int, 4);
                @param.Value = id;

                cn.ConnectionString = sConexion;
                cn.Open();
                // Insertar Parametro de busqueda

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODEREGEX.IdRegex = Convert.ToInt32(dr["IdRegex"]);
                    objCATALOGODEREGEX.Regex = dr["Regex"].ToString();
                    objCATALOGODEREGEX.Descripcion = dr["Descripcion"].ToString();
                }
                else
                {
                    objCATALOGODEREGEX = default;
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

            return objCATALOGODEREGEX;
        }

    }
}
