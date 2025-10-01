using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPrevalidador
{
    public class CatalogoDeUsuariosITCERepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public CatalogoDeUsuariosITCERepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogoDeUsuariosITCE Buscar(int MiIdUsuario, string MiAduana, string MiPatente)
        {
            var objCATALOGODEUSUARIOSITCE = new CatalogoDeUsuariosITCE();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            try
            {

                cmd.CommandText = "NET_SEARCH_CASAEI_CATALOGODEUSUARIOSITCE";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @Idusuario INT
                @param = cmd.Parameters.Add("@Idusuario", SqlDbType.Int, 4);
                @param.Value = MiIdUsuario;

                // ,@Aduana VARCHAR(3)
                @param = cmd.Parameters.Add("@Aduana", SqlDbType.VarChar, 3);
                @param.Value = MiAduana;

                // ,@Patente VARCHAR(4)
                @param = cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4);
                @param.Value = MiPatente;

                cn.ConnectionString = sConexion;
                cn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODEUSUARIOSITCE.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                    objCATALOGODEUSUARIOSITCE.Aduana = dr["Aduana"].ToString();
                    objCATALOGODEUSUARIOSITCE.Patente = dr["Patente"].ToString();
                    objCATALOGODEUSUARIOSITCE.Usuario = dr["Usuario"].ToString();
                    objCATALOGODEUSUARIOSITCE.Psw = dr["Psw"].ToString();
                }
                else
                {
                    objCATALOGODEUSUARIOSITCE = default;
                }
                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCATALOGODEUSUARIOSITCE;
        }

    }
}
