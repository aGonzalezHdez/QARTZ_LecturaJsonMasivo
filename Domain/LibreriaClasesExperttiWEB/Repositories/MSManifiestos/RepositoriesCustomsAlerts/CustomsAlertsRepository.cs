using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesSifty;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado
{
    public class CustomsAlertsRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public CustomsAlertsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CustomsAlerts BuscarPorGuiaHouse(string GuiaHouse, int IDDatosDeEmpresa)
        {

            var objGuiaHouse = new CustomsAlerts();
            bool objGuia;

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CUSTOMSALERTS_IDDatosDeEmpresa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = cn;

            @param = cmd.Parameters.Add("@GUIAHOUSE", SqlDbType.VarChar, 15);
            @param.Value = GuiaHouse;

            // @IDDatosDeEmpresa INT
            @param = cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4);
            @param.Value = IDDatosDeEmpresa;

            try
            {

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    if (dr.Read())
                    {

                        objGuia = true;
                        objGuiaHouse.IdCustomAlert = Convert.ToInt32(dr["IdCustomAlert"]);
                        objGuiaHouse.GuiaHouse = dr["GuiaHouse"].ToString();
                        objGuiaHouse.ValorEnDolares = Convert.ToDouble(dr["ValorEnDolares"]);
                        objGuiaHouse.PesoTotal = Convert.ToDouble(dr["PesoTotal"]);
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

        public CustomsAlerts BuscarPorId(int idCustomsAlerts)
        {

            var objGuiaHouse = new CustomsAlerts();
          
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CUSTOMSALERTS";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = cn;

            @param = cmd.Parameters.Add("@IdCustomAlert", SqlDbType.Int, 4);
            @param.Value = idCustomsAlerts;

           try
            {

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {

                    if (dr.Read())
                    {

                        objGuiaHouse.IdCustomAlert = Convert.ToInt32(dr["IdCustomAlert"]);
                        objGuiaHouse.GuiaHouse = dr["GuiaHouse"].ToString();
                        objGuiaHouse.ValorEnDolares = Convert.ToDouble(dr["ValorEnDolares"]);
                        objGuiaHouse.PesoTotal = Convert.ToDouble(dr["PesoTotal"]);
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
        public CustomsAlerts ModificarporIDCategoria3(string GuiaHouse)
        {

            var objGuiaHouse = new CustomsAlerts();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_UPDATE_CUSTOMSALERTSPORGUIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // ,@GuiaHouse  varchar
            @param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 10);
            @param.Value = GuiaHouse;

            try
            {
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();


                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CUSTOMSALERTS_POR_GUIA");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return objGuiaHouse;

        }
        public int modificarSifty(int idCA, int sifty)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMESALERTS_NoAbrir";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@IdCA", SqlDbType.Int, 4);
            param.Value = idCA;

            param = cmd.Parameters.Add("@NoAbrir", SqlDbType.Int, 4);
            param.Value = sifty;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_PIECEID_CMF");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public async Task<int> modificarSiftyAsync(int idCA, int sifty)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMESALERTS_NoAbrir";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@IdCA", SqlDbType.Int, 4);
            param.Value = idCA;

            param = cmd.Parameters.Add("@NoAbrir", SqlDbType.Int, 4);
            param.Value = sifty;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = await cmd.ExecuteReaderAsync();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_PIECEID_CMF");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }




        public List<CustomsAlerts> CargarParaSifty()
        {
            List<CustomsAlerts> lstGuiaHouse = new ();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CUSTOMSALERTS";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = cn;

            try
            {

                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
               
                    while (dr.Read())
                    {
                        CustomsAlerts objGuiaHouse = new();

                        objGuiaHouse.IdCustomAlert = Convert.ToInt32(dr["IdCustomAlert"]);
                        objGuiaHouse.GuiaHouse = dr["GuiaHouse"].ToString();
                        objGuiaHouse.ValorEnDolares = Convert.ToDouble(dr["ValorEnDolares"]);
                        objGuiaHouse.PesoTotal = Convert.ToDouble(dr["PesoTotal"]);
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

                        lstGuiaHouse.Add(objGuiaHouse);
                    }
                 
                }
                else
                {
                    lstGuiaHouse.Clear();
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

            return lstGuiaHouse;

        }

        public List<CustomsAlerts> SiftyFedEx()
        {
            List<CustomsAlerts> lstGuiaHouse = new();


            lstGuiaHouse = CargarParaSifty();
            ApiSiftyRepository apiSifty = new(_configuration);

            foreach (CustomsAlerts item in lstGuiaHouse)
            {
                try
                {
                    apiSifty.EnviarSiftyCA(item.IdCustomAlert);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }

            return lstGuiaHouse;
        }

        public async Task< List<CustomsAlerts>> SiftyFedExAsync()
        {
            List<CustomsAlerts> lstGuiaHouse = new();
             

            lstGuiaHouse = CargarParaSifty();
            ApiSiftyRepository apiSifty = new(_configuration);

            //foreach (CustomsAlerts item in lstGuiaHouse)
            //{
            //    try
            //    {
                  await apiSifty.EnviarSiftyAsync(lstGuiaHouse);
            //    }
            //    catch (Exception ex)
            //    {
            //        continue;
            //    }

            //}

            return lstGuiaHouse;
        }
        public int modificarPreviosJCJF(int idCA, bool PrevioJCJF)
        {
            int id = 0;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CUSTOMESALERTS_PrevioJCJF";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;



            param = cmd.Parameters.Add("@IdCA", SqlDbType.Int, 4);
            param.Value = idCA;

            param = cmd.Parameters.Add("@PrevioJCJF", SqlDbType.Int, 4);
            param.Value = PrevioJCJF;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {

                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_CUSTOMESALERTS_PrevioJCJF");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }
    }
}
