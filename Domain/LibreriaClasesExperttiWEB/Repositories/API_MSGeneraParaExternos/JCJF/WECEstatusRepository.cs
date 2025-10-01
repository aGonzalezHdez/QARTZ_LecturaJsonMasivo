using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesJCJF;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.API_MSGeneraParaExternos.JCJF
{
    public class WECEstatusRepository : IWECEstatusRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public WECEstatusRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public int Insertar(WECEstatus objWECEstatus)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CASAEI_WECEstatus_JCJF";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
                @param.Value = objWECEstatus.GuiaHouse;

                @param = cmd.Parameters.Add("@GuiaMaster", SqlDbType.VarChar, 25);
                @param.Value = objWECEstatus.GuiaMaster;

                @param = cmd.Parameters.Add("@Tipo", SqlDbType.SmallInt);
                @param.Value = objWECEstatus.Tipo;


                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CASAEI_WECEstatus");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public WECEstatus Buscar(GuiaHouseRequest guiaHouseRequest)
        {
            WECEstatus wECEstatus = new WECEstatus();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            try
            {

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_WECEstatus";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) 
                @param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
                @param.Value = guiaHouseRequest.GuiaHouse;

                @param = cmd.Parameters.Add("@GuiaMaster", SqlDbType.VarChar, 25);
                @param.Value = guiaHouseRequest.GuiaMaster;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    //objWECEstatus.IdEstatusWEC = Convert.ToInt32(dr["IdEstatusWEC"]);
                    wECEstatus.GuiaHouse = (dr["GuiaHouse"]).ToString();
                    wECEstatus.GuiaMaster = (dr["GuiaMaster"]).ToString();
                    wECEstatus.Tipo = Convert.ToInt32(dr["Tipo"]);
                    //objWECEstatus.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                    //objWECEstatus.JCJF = Convert.ToBoolean(dr["JCJF"]);
                }
                else
                {
                    wECEstatus = null;
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

            return wECEstatus;
        }

    }
}
