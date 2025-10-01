using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesS3;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3
{
    public class neg_AmazonS3
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con = null!;

        public neg_AmazonS3(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public ent_CredencialesAmazonS3 NegObtenerCredenciales()
        {
            dat_Conexion dat_Conexion = null!/* TODO Change to default(_) if this is not a reference type */;
            ent_CredencialesAmazonS3 credenciales;
            try
            {
                var dat_AmazonS3 = new dat_AmazonS3();
                dat_Conexion = new dat_Conexion(_configuration);
                dat_Conexion.AbrirConexion(false);
                credenciales = dat_AmazonS3.Dat_ObtenerCredenciales(dat_Conexion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dat_Conexion.CerrarConexion();
            }
            return credenciales;
        }

    }
}
