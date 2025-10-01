using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeFtpsRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public CatalogoDeFtpsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int BuscarIdFtp(int IdDatosdeEmpresa, int IdOficina, string prevalidador)
        {
            var IdFTP = 0;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_ID_FTP";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdDatosdeEmpresa", SqlDbType.Int, 4);
                @param.Value = IdDatosdeEmpresa;

                @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
                @param.Value = IdOficina;

                @param = cmd.Parameters.Add("@prevalidador", SqlDbType.VarChar, 3);
                @param.Value = prevalidador;


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    IdFTP = Convert.ToInt32(dr["IdFTP"]);
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

            return IdFTP;
        }
        public CatalogoDeFtps Buscar(int id)
        {
            var objCATALOGODEFTPS = new CatalogoDeFtps();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_CATALOGODEFTPS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @IdFTP INT
                @param = cmd.Parameters.Add("@IdFTP", SqlDbType.Int, 4);
                @param.Value = id;
                // Insertar Parametro de busqueda

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODEFTPS.IdFTP = Convert.ToInt32(dr["IdFTP"]);
                    objCATALOGODEFTPS.FTP = dr["FTP"].ToString();
                    objCATALOGODEFTPS.Mode = dr["Mode"].ToString();
                    objCATALOGODEFTPS.UsuarioFTP = dr["UsuarioFTP"].ToString();
                    objCATALOGODEFTPS.PasswordFTP = dr["PasswordFTP"].ToString();
                    objCATALOGODEFTPS.Puerto = Convert.ToInt32(dr["Puerto"]);
                    objCATALOGODEFTPS.Estatus = Convert.ToBoolean(dr["Estatus"]);
                    objCATALOGODEFTPS.PathLocal = dr["PathLocal"].ToString();
                    objCATALOGODEFTPS.FTPRecibidos = dr["FTPRecibidos"].ToString();
                }
                else
                {
                    objCATALOGODEFTPS = default;
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

            return objCATALOGODEFTPS;
        }
    }
}
