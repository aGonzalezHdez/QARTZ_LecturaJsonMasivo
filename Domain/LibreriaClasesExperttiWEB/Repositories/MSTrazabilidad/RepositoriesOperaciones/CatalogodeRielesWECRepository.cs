using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones
{
    public class CatalogodeRielesWECRepository
    {

        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodeRielesWECRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogodeRielesWEC Buscar(int idRielWEC)
        {
            CatalogodeRielesWEC objCATALOGODERIELESWEC = new CatalogodeRielesWEC();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            //cn.ConnectionString = MyConnectionString;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CATALOGODERIELESWEC";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@idRielWEC", SqlDbType.Int, 4);
            param.Value = idRielWEC;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objCATALOGODERIELESWEC.idRielWEC = Convert.ToInt32(dr["idRielWEC"]);
                    objCATALOGODERIELESWEC.NoRiel = Convert.ToInt32(dr["NoRiel"]);
                    objCATALOGODERIELESWEC.Descripcion = dr["Descripcion"].ToString();
                }
                else
                    objCATALOGODERIELESWEC = null/* TODO Change to default(_) if this is not a reference type */;
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

            return objCATALOGODERIELESWEC;
        }



    }
}
