using LibreriaClasesAPIExpertti.Entities.EntitiesS3;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3
{
    public class dat_AmazonS3
    {
        public ent_CredencialesAmazonS3 Dat_ObtenerCredenciales(dat_Conexion dat_conexion)
        {
            ent_CredencialesAmazonS3 credenciales = new();
            try
            {
                dat_conexion.SqlCommand.CommandType = CommandType.StoredProcedure;
                dat_conexion.SqlCommand.CommandText = "NET_LOAD_AMAZONS3_CREDENCIALES";
                SqlDataReader reader = dat_conexion.SqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    credenciales.AccessKeyID = string.Format("{0}", reader[0]);
                    credenciales.SecretKey = string.Format("{0}", reader[1]);
                    credenciales.RegionEndpoint = (ent_RegionesAmazonS3)Enum.Parse(typeof(ent_RegionesAmazonS3), string.Format("{0}", reader[2]));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return credenciales;
        }

    }
}
