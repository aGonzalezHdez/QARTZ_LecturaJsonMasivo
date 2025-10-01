using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales
{
    public class PrioridadSafranRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public PrioridadSafranRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public PrioridadSafran Buscar(string GuiaHouse)
        {
            var objPrioridadSafran = new PrioridadSafran();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_PRIORIDADSAFRAN";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 15);
            @param.Value = GuiaHouse;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objPrioridadSafran.Id = Convert.ToInt32(dr["IdPrioridadSafran"]);
                    objPrioridadSafran.GuiaHouse = dr["GuiaHouse"].ToString();
                    objPrioridadSafran.PO = dr["PO"].ToString();
                    objPrioridadSafran.Engine = dr["Engine"].ToString();
                    objPrioridadSafran.Fecha = Convert.ToDateTime(dr["Fecha"]);
                    objPrioridadSafran.Prioridad = dr["Prioridad"].ToString();
                }
                else
                {
                    objPrioridadSafran = default;
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

            return objPrioridadSafran;
        }

    }
}
