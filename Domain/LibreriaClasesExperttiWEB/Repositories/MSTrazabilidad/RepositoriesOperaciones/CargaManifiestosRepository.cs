using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones
{
    public class CargaManifiestosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CargaManifiestosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public List<CustomsAlerts> Buscar(string GuiaHouse)
        {
            List<CustomsAlerts> lst = new();


            try
            {
                using (con = new SqlConnection(SConexion))

                using (SqlCommand cmd = new("NET_SEARCH_CustomsAlertsXGuiaHouse_WEB", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25).Value = GuiaHouse;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CustomsAlerts objCustomsAlerts = new CustomsAlerts();
                            objCustomsAlerts.IdCustomAlert = Convert.ToInt32(dr["IdCustomAlert"]);
                            objCustomsAlerts.GuiaHouse = dr["GuiaHouse"].ToString();
                            objCustomsAlerts.ValorEnDolares = Convert.ToDouble(dr["ValorEnDolares"]);
                            objCustomsAlerts.PesoTotal = Convert.ToDouble(dr["PesoTotal"]);
                            objCustomsAlerts.Descripcion = dr["Descripcion"].ToString();
                            objCustomsAlerts.Cliente = dr["Cliente"].ToString();
                            objCustomsAlerts.OrigenIata = dr["OrigenIata"].ToString();
                            objCustomsAlerts.DestinoIata = dr["DestinoIata"].ToString();
                            objCustomsAlerts.GuiaMaster = dr["GuiaMaster"].ToString();
                            objCustomsAlerts.FechaDeEntrada = Convert.ToDateTime(dr["FechaDeEntrada"]);
                            objCustomsAlerts.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                            objCustomsAlerts.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objCustomsAlerts.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                            objCustomsAlerts.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                            objCustomsAlerts.DescripcionEspanol = dr["DescripcionEspanol"].ToString();
                            objCustomsAlerts.IdTipodePedimento = Convert.ToInt32(dr["IdTipodePedimento"]);
                            objCustomsAlerts.Remitente = dr["Remitente"].ToString();
                            objCustomsAlerts.Piezas = Convert.ToInt32(dr["Piezas"]);
                            objCustomsAlerts.ProveedorConfiable = Convert.ToBoolean(dr["ProveedorConfiable"]);
                            objCustomsAlerts.Detener = Convert.ToBoolean(dr["Detener"]);
                            objCustomsAlerts.DetenidaporCliente = Convert.ToBoolean(dr["DetenidaporCliente"]);
                            objCustomsAlerts.HoyMismo = Convert.ToBoolean(dr["HoyMismo"]);
                            objCustomsAlerts.ClavedePedimento = dr["ClavedePedimento"].ToString();
                            objCustomsAlerts.IdNotificador = Convert.ToInt32(dr["IdNotificador"]);
                            objCustomsAlerts.Patente = Convert.ToInt32(dr["Patente"]);
                            objCustomsAlerts.IdRiel = Convert.ToInt32(dr["IdRiel"]);
                            objCustomsAlerts.ValorDolaresCA = Convert.ToDouble(dr["ValorDolaresCA"]);
                            objCustomsAlerts.IdRielWEC = Convert.ToInt32(dr["IdRielWEC"]);
                            objCustomsAlerts.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                            objCustomsAlerts.ValorMe = Convert.ToDouble(dr["ValorMe"]);
                            objCustomsAlerts.Moneda = dr["Moneda"].ToString();
                            objCustomsAlerts.Fletes = Convert.ToDouble(dr["Fletes"]);
                            objCustomsAlerts.FacturacionFx = dr["FacturacionFx"].ToString();
                            objCustomsAlerts.RFCCLIENTE = dr["RFCCLIENTE"].ToString();
                            objCustomsAlerts.PrevioJCJF = Convert.ToBoolean(dr["PrevioJCJF"]);


                            lst.Add(objCustomsAlerts);
                        }
                    }
                    else
                    {
                        lst.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lst;
        }

        public List<CustomsAlerts> BuscarJCJF(string GuiaHouse)
        {
            List<CustomsAlerts> lst = new();


            try
            {
                using (con = new SqlConnection(SConexion))

                using (SqlCommand cmd = new("NET_SEARCH_CustomsAlertsXGuiaHouse_WEB_JCJF", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25).Value = GuiaHouse;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CustomsAlerts objCustomsAlerts = new CustomsAlerts();
                            objCustomsAlerts.IdCustomAlert = Convert.ToInt32(dr["IdCustomAlert"]);
                            objCustomsAlerts.GuiaHouse = dr["GuiaHouse"].ToString();
                            objCustomsAlerts.ValorEnDolares = Convert.ToDouble(dr["ValorEnDolares"]);
                            objCustomsAlerts.PesoTotal = Convert.ToDouble(dr["PesoTotal"]);
                            objCustomsAlerts.Descripcion = dr["Descripcion"].ToString();
                            objCustomsAlerts.Cliente = dr["Cliente"].ToString();
                            objCustomsAlerts.OrigenIata = dr["OrigenIata"].ToString();
                            objCustomsAlerts.DestinoIata = dr["DestinoIata"].ToString();
                            objCustomsAlerts.GuiaMaster = dr["GuiaMaster"].ToString();
                            objCustomsAlerts.FechaDeEntrada = Convert.ToDateTime(dr["FechaDeEntrada"]);
                            objCustomsAlerts.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                            objCustomsAlerts.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objCustomsAlerts.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                            objCustomsAlerts.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                            objCustomsAlerts.DescripcionEspanol = dr["DescripcionEspanol"].ToString();
                            objCustomsAlerts.IdTipodePedimento = Convert.ToInt32(dr["IdTipodePedimento"]);
                            objCustomsAlerts.Remitente = dr["Remitente"].ToString();
                            objCustomsAlerts.Piezas = Convert.ToInt32(dr["Piezas"]);
                            objCustomsAlerts.ProveedorConfiable = Convert.ToBoolean(dr["ProveedorConfiable"]);
                            objCustomsAlerts.Detener = Convert.ToBoolean(dr["Detener"]);
                            objCustomsAlerts.DetenidaporCliente = Convert.ToBoolean(dr["DetenidaporCliente"]);
                            objCustomsAlerts.HoyMismo = Convert.ToBoolean(dr["HoyMismo"]);
                            objCustomsAlerts.ClavedePedimento = dr["ClavedePedimento"].ToString();
                            objCustomsAlerts.IdNotificador = Convert.ToInt32(dr["IdNotificador"]);
                            objCustomsAlerts.Patente = Convert.ToInt32(dr["Patente"]);
                            objCustomsAlerts.IdRiel = Convert.ToInt32(dr["IdRiel"]);
                            objCustomsAlerts.ValorDolaresCA = Convert.ToDouble(dr["ValorDolaresCA"]);
                            objCustomsAlerts.IdRielWEC = Convert.ToInt32(dr["IdRielWEC"]);
                            objCustomsAlerts.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                            objCustomsAlerts.ValorMe = Convert.ToDouble(dr["ValorMe"]);
                            objCustomsAlerts.Moneda = dr["Moneda"].ToString();
                            objCustomsAlerts.Fletes = Convert.ToDouble(dr["Fletes"]);
                            objCustomsAlerts.FacturacionFx = dr["FacturacionFx"].ToString();
                            objCustomsAlerts.RFCCLIENTE = dr["RFCCLIENTE"].ToString();

                            lst.Add(objCustomsAlerts);
                        }
                    }
                    else
                    {
                        lst.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lst;
        }
        public CustomsAlerts ModificarporIDCategoria3(string GuiaHouse)
        {

            var objGuiaHouse = new CustomsAlerts();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = SConexion;
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

        public CustomsAlerts BuscarIndividual(string GuiaHouse)
        {
            CustomsAlerts objCustomsAlerts = new CustomsAlerts();

            try
            {
                using (con = new SqlConnection(SConexion))

                using (SqlCommand cmd = new("NET_SEARCH_CustomsAlertsXGuiaHouse_WEB", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25).Value = GuiaHouse;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            objCustomsAlerts.IdCustomAlert = Convert.ToInt32(dr["IdCustomAlert"]);
                            objCustomsAlerts.GuiaHouse = dr["GuiaHouse"].ToString();
                            objCustomsAlerts.ValorEnDolares = Convert.ToDouble(dr["ValorEnDolares"]);
                            objCustomsAlerts.PesoTotal = Convert.ToDouble(dr["PesoTotal"]);
                            objCustomsAlerts.Descripcion = dr["Descripcion"].ToString();
                            objCustomsAlerts.Cliente = dr["Cliente"].ToString();
                            objCustomsAlerts.OrigenIata = dr["OrigenIata"].ToString();
                            objCustomsAlerts.DestinoIata = dr["DestinoIata"].ToString();
                            objCustomsAlerts.GuiaMaster = dr["GuiaMaster"].ToString();
                            objCustomsAlerts.FechaDeEntrada = Convert.ToDateTime(dr["FechaDeEntrada"]);
                            objCustomsAlerts.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                            objCustomsAlerts.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objCustomsAlerts.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                            objCustomsAlerts.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                            objCustomsAlerts.DescripcionEspanol = dr["DescripcionEspanol"].ToString();
                            objCustomsAlerts.IdTipodePedimento = Convert.ToInt32(dr["IdTipodePedimento"]);
                            objCustomsAlerts.Remitente = dr["Remitente"].ToString();
                            objCustomsAlerts.Piezas = Convert.ToInt32(dr["Piezas"]);
                            objCustomsAlerts.ProveedorConfiable = Convert.ToBoolean(dr["ProveedorConfiable"]);
                            objCustomsAlerts.Detener = Convert.ToBoolean(dr["Detener"]);
                            objCustomsAlerts.DetenidaporCliente = Convert.ToBoolean(dr["DetenidaporCliente"]);
                            objCustomsAlerts.HoyMismo = Convert.ToBoolean(dr["HoyMismo"]);
                            objCustomsAlerts.ClavedePedimento = dr["ClavedePedimento"].ToString();
                            objCustomsAlerts.IdNotificador = Convert.ToInt32(dr["IdNotificador"]);
                            objCustomsAlerts.Patente = Convert.ToInt32(dr["Patente"]);
                            objCustomsAlerts.IdRiel = Convert.ToInt32(dr["IdRiel"]);
                            objCustomsAlerts.ValorDolaresCA = Convert.ToDouble(dr["ValorDolaresCA"]);
                            objCustomsAlerts.IdRielWEC = Convert.ToInt32(dr["IdRielWEC"]);
                            objCustomsAlerts.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                            objCustomsAlerts.ValorMe = Convert.ToDouble(dr["ValorMe"]);
                            objCustomsAlerts.Moneda = dr["Moneda"].ToString();
                            objCustomsAlerts.Fletes = Convert.ToDouble(dr["Fletes"]);
                            objCustomsAlerts.FacturacionFx = dr["FacturacionFx"].ToString();
                            objCustomsAlerts.RFCCLIENTE = dr["RFCCLIENTE"].ToString();


                        }
                    }
                    else
                    {
                        objCustomsAlerts = null;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCustomsAlerts;
        }

        public CustomsAlerts BuscarPorGuiaHouse(string GuiaHouse, int IDDatosDeEmpresa)
        {

            var objGuiaHouse = new CustomsAlerts();
            bool objGuia;

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CUSTOMSALERTS_IDDatosDeEmpresa";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = cn;

            param = cmd.Parameters.Add("@GUIAHOUSE", SqlDbType.VarChar, 15);
            param.Value = GuiaHouse;

            // @IDDatosDeEmpresa INT
            param = cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4);
            param.Value = IDDatosDeEmpresa;

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
                        objGuiaHouse.PesoTotal = double.Parse(dr["PesoTotal"].ToString());
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

        public CustomsAlerts Buscar(int IdCustomAlert)
        {
            CustomsAlerts objCustomsAlerts = new CustomsAlerts();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            //cn.ConnectionString = MyConnectionString;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CustomsAlerts";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@IdCustomAlert", SqlDbType.Int, 4);
            param.Value = IdCustomAlert;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objCustomsAlerts.IdCustomAlert = Convert.ToInt32(dr["IdCustomAlert"]);
                    objCustomsAlerts.GuiaHouse = dr["GuiaHouse"].ToString();
                    objCustomsAlerts.ValorEnDolares = Convert.ToDouble(dr["ValorEnDolares"]);
                    objCustomsAlerts.PesoTotal = (double)Convert.ToDecimal(dr["PesoTotal"]);
                    objCustomsAlerts.Descripcion = dr["Descripcion"].ToString();
                    objCustomsAlerts.Cliente = dr["Cliente"].ToString();
                    objCustomsAlerts.OrigenIata = dr["OrigenIata"].ToString();
                    objCustomsAlerts.DestinoIata = dr["DestinoIata"].ToString();
                    objCustomsAlerts.GuiaMaster = dr["GuiaMaster"].ToString();
                    objCustomsAlerts.FechaDeEntrada = Convert.ToDateTime(dr["FechaDeEntrada"]);
                    objCustomsAlerts.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                    objCustomsAlerts.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                    objCustomsAlerts.IdCategoria = Convert.ToInt32(dr["IdCategoria"]);
                    objCustomsAlerts.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                    objCustomsAlerts.DescripcionEspanol = dr["DescripcionEspanol"].ToString();
                    objCustomsAlerts.IdTipodePedimento = Convert.ToInt32(dr["IdTipodePedimento"]);
                    objCustomsAlerts.Remitente = dr["Remitente"].ToString();
                    objCustomsAlerts.Piezas = Convert.ToInt32(dr["Piezas"]);
                    objCustomsAlerts.ProveedorConfiable = Convert.ToBoolean(dr["ProveedorConfiable"]);
                    objCustomsAlerts.Detener = Convert.ToBoolean(dr["Detener"]);
                    objCustomsAlerts.DetenidaporCliente = Convert.ToBoolean(dr["DetenidaporCliente"]);
                    objCustomsAlerts.HoyMismo = Convert.ToBoolean(dr["HoyMismo"]);
                    objCustomsAlerts.ClavedePedimento = dr["ClavedePedimento"].ToString();
                    objCustomsAlerts.IdNotificador = Convert.ToInt32(dr["IdNotificador"]);
                    objCustomsAlerts.Patente = Convert.ToInt32(dr["Patente"]);
                    objCustomsAlerts.IdRielWEC = Convert.ToInt32(dr["idRielWEC"]);
                }
                else
                    objCustomsAlerts = null/* TODO Change to default(_) if this is not a reference type */;
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

            return objCustomsAlerts;
        }


       
    }//Clase
}//NameSpace
