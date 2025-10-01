using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPrevalidador
{
    public class PrevalidadorRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;


        public PrevalidadorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<string> CargarPrevalidadores(string MyPatente, string MyAduana)
        {

            List<string> list = new List<string>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_PREVALIDADORES";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @@IdDepartamento int 

                cmd.Parameters.Add("@PAT_AA", SqlDbType.VarChar, 4).Value = MyPatente;
                cmd.Parameters.Add("@CVE_ADUA", SqlDbType.VarChar, 3).Value = MyAduana;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var prevalidador = dr["CVE_PREV"].ToString();
                        list.Add(prevalidador);
                    }
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
            return list;
        }

        public List<Representante> CargarRepresentantes(string MyPatente)
        {

            List<Representante> list = new List<Representante>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_OBTENER_REPRESENTANTE";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @@IdDepartamento int 

                cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4).Value = MyPatente;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var representante = new Representante();
                        representante.IdDetalleMandatarios = Convert.ToInt32(dr["IdDetalleMandatarios"].ToString());
                        representante.Patente = dr["Patente"].ToString();
                        representante.CveRep = dr["CveRep"].ToString();
                        representante.usarDefault = Convert.ToBoolean(dr["usarDefault"]);
                        list.Add(representante);
                    }
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
            return list;
        }
    }

}
