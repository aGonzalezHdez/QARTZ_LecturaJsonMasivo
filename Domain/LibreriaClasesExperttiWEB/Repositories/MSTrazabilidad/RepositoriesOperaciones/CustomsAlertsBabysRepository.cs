using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones
{
    public class CustomsAlertsBabysRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public CustomsAlertsBabysRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(CustomsAlertsBabys lCustomsAlertsBabys)
        {
            int id;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            try
            {
                cmd.CommandText = "NET_INSERT_CASAEI_CUSTOMSALERTSBABY";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // ,@IdCustomAlert  int
                param = cmd.Parameters.Add("@IdCustomAlert", SqlDbType.Int, 4);
                param.Value = lCustomsAlertsBabys.IdCustomAlert;

                // ,@GuiaBaby  varchar
                param = cmd.Parameters.Add("@GuiaBaby", SqlDbType.VarChar, 15);
                param.Value = lCustomsAlertsBabys.GuiaBaby;


                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;


                //cn.ConnectionString = MyConnectionString;
                cn.Open();
                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_CUSTOMSALERTSBABY");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }
        public CustomsAlerts BuscarPorGuiaBaby(string GuiaHouse)
        {

            var objGuiaHouse = new CustomsAlerts();
            bool objGuia;

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "fdx.NET_SEARCH_CUSTOMSALERTSBABY";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = cn;

            @param = cmd.Parameters.Add("@GUIAHOUSE", SqlDbType.VarChar, 15);
            @param.Value = GuiaHouse;

            try
            {

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    if (dr.Read())
                    {
                        objGuia = true;
                        objGuiaHouse.IdCustomAlertsBaby = Convert.ToInt32(dr["IdCustomAlertsBaby"]);
                        objGuiaHouse.IdCustomAlert = Convert.ToInt32(dr["IdCustomAlert"]);
                        objGuiaHouse.GuiaHouse = dr["GuiaHouse"].ToString();
                        objGuiaHouse.ValorEnDolares = Convert.ToDouble(dr["ValorEnDolares"]);
                        objGuiaHouse.PesoTotal = Convert.ToDouble(dr["PesoTotal"].ToString());
                        objGuiaHouse.Descripcion = dr["Descripcion"].ToString();
                        objGuiaHouse.Cliente = dr["Cliente"].ToString();
                        objGuiaHouse.OrigenIata = dr["OrigenIata"].ToString();
                        objGuiaHouse.DestinoIata = dr["DestinoIata"].ToString();
                        objGuiaHouse.GuiaMaster = dr["GuiaMaster"].ToString();
                        objGuiaHouse.FechaDeEntrada = Convert.ToDateTime(dr["FechaDeEntrada"]);
                        objGuiaHouse.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                        objGuiaHouse.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                        objGuiaHouse.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                        objGuiaHouse.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        objGuiaHouse.DescripcionEspanol = dr["DescripcionEspanol"].ToString();
                        objGuiaHouse.IdTipodePedimento = Convert.ToInt32(dr["IdTipodePedimento"]);
                        objGuiaHouse.Remitente = dr["Remitente"].ToString();
                        objGuiaHouse.Piezas = Convert.ToInt32(dr["Piezas"]);
                        objGuiaHouse.ProveedorConfiable = Convert.ToBoolean(dr["ProveedorConfiable"]);
                        objGuiaHouse.Detener = Convert.ToBoolean(dr["Detener"]);
                        objGuiaHouse.DetenidaporCliente = Convert.ToBoolean(dr["DetenidaporCliente"]);
                        objGuiaHouse.HoyMismo = Convert.ToBoolean(dr["HoyMismo"]);
                        objGuiaHouse.ClavedePedimento = dr["ClavedePedimento"].ToString();
                        objGuiaHouse.IdNotificador = Convert.ToInt32(dr["IdNotificador"]);
                        objGuiaHouse.Patente = Convert.ToInt32(dr["Patente"]);
                    }
                    else
                    {
                        objGuiaHouse = default;
                    }
                }
                else
                {
                    objGuiaHouse = default;
                }



                cmd.Parameters.Clear();
                cn.Close();
            }

            catch (Exception ex)
            {

                Interaction.MsgBox(ex.Message.ToString());

            }

            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return objGuiaHouse;

        }
    }
}
