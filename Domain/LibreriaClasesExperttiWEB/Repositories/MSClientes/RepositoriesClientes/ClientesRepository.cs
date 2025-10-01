using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class ClientesRepository : IClientesRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public ClientesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<ClientesProveedores> CargarProveedores(int IDCliente, int Operacion)
        {
            //DataTable dtb = new();
            List<ClientesProveedores> ListCtracProved = new();

            using (con = new(SConexion))
            using (SqlCommand cmd = new("NET_LOAD_CTRAC_PROVED_DELCLIENTE_NEW", con))

            {
                try
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = IDCliente;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int).Value = Operacion;

                    //var da = new SqlDataAdapter(cmd);
                    //da.Fill(dtb);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClientesProveedores CtracProved = new();

                            CtracProved.CVE_PRO = string.Format("{0}", dr["CVE_PRO"]);
                            CtracProved.NOM_PRO = string.Format("{0}", dr["NOM_PRO"]);
                            CtracProved.DIR_PRO = string.Format("{0}", dr["DIR_PRO"]);
                            CtracProved.POB_PRO = string.Format("{0}", dr["POB_PRO"]);
                            CtracProved.ZIP_PRO = string.Format("{0}", dr["ZIP_PRO"]);
                            CtracProved.TAX_PRO = string.Format("{0}", dr["TAX_PRO"]);
                            CtracProved.PAI_PRO = string.Format("{0}", dr["PAI_PRO"]);
                            CtracProved.CTA_PRO = string.Format("{0}", dr["CTA_PRO"]);
                            CtracProved.EFE_PRO = string.Format("{0}", dr["EFE_PRO"]);
                            CtracProved.NOI_PRO = string.Format("{0}", dr["NOI_PRO"]);
                            CtracProved.NOE_PRO = string.Format("{0}", dr["NOE_PRO"]);
                            CtracProved.VIN_PRO = string.Format("{0}", dr["VIN_PRO"]);
                            CtracProved.EFE_DESP = string.Format("{0}", dr["EFE_DESP"]);
                            CtracProved.TEL_PRO = string.Format("{0}", dr["TEL_PRO"]);
                            CtracProved.AFE_PREC = string.Format("{0}", dr["AFE_PREC"]);
                            CtracProved.CVE_PROC = string.Format("{0}", dr["CVE_PROC"]);

                            if (dr["FEC_BAJA"] != DBNull.Value)
                            {
                                CtracProved.FEC_BAJA = Convert.ToDateTime(dr["FEC_BAJA"]);
                            }
                            else
                            {
                                CtracProved.FEC_BAJA = Convert.ToDateTime("01/01/1900");
                            }

                            //CtracProved.FEC_BAJA = Convert.ToDateTime(dr["FEC_BAJA"]);
                            CtracProved.INT_PRO = Convert.ToInt32(dr["INT_PRO"]);
                            CtracProved.EXP_CONF = string.Format("{0}", dr["EXP_CONF"]);
                            CtracProved.APE_PATE = string.Format("{0}", dr["APE_PATE"]);
                            CtracProved.APE_MATE = string.Format("{0}", dr["APE_MATE"]);
                            CtracProved.COL_PRO = string.Format("{0}", dr["COL_PRO"]);
                            CtracProved.LOC_PRO = string.Format("{0}", dr["LOC_PRO"]);
                            CtracProved.REFE_PRO = string.Format("{0}", dr["REFE_PRO"]);
                            CtracProved.NOM_COVE = string.Format("{0}", dr["NOM_COVE"]);
                            CtracProved.MUN_COVE = string.Format("{0}", dr["MUN_COVE"]);
                            CtracProved.MAIL_COVE = string.Format("{0}", dr["MAIL_COVE"]);
                            CtracProved.RUTA_USPPI = string.Format("{0}", dr["RUTA_USPPI"]);
                            CtracProved.TIP_OPER = string.Format("{0}", dr["TIP_OPER"]);



                            ListCtracProved.Add(CtracProved);
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            return ListCtracProved;
        }

        public DataTable CargarProveedores(int IDCLIENTE, int Operacion, string myConnectionString)
        {
            DataTable dtb = new DataTable();
            SqlParameter param;

            using (SqlConnection cn = new SqlConnection(myConnectionString))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_CTRAC_PROVED_DELCLIENTE_NEW";

                    param = dap.SelectCommand.Parameters.Add("@IDCLIENTE", SqlDbType.Int);
                    param.Value = IDCLIENTE;

                    param = dap.SelectCommand.Parameters.Add("@Operacion", SqlDbType.Int);
                    param.Value = Operacion;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_LOAD_CTRAC_PROVED_DELCLIENTE");
                }
            }
            return dtb;
        }

        public Clientes Buscar(int IdCliente)
        {
            var objCliente = new Clientes();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_CLIENTES";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4);
                @param.Value = IdCliente;
                // Insertar Parametro de busqueda

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCliente.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                    objCliente.Clave = dr["Clave"].ToString();
                    objCliente.Nombre = dr["Nombre"].ToString();
                    objCliente.ApellidoPaterno = dr["ApellidoPaterno"].ToString();
                    objCliente.ApellidoMaterno = dr["ApellidoMaterno"].ToString();
                    objCliente.RFCGenerico = Convert.ToBoolean(dr["RFCGenerico"]);
                    objCliente.RFC = dr["RFC"].ToString();
                    objCliente.CURP = dr["CURP"].ToString();
                    objCliente.IDCapturo = Convert.ToInt32(dr["IDCapturo"]);
                    objCliente.Activo = Convert.ToBoolean(dr["Activo"]);
                    objCliente.Telefono = dr["Telefono"].ToString();
                    objCliente.EmailContacto = dr["EmailContacto"].ToString();
                    objCliente.Atencion = dr["Atencion"].ToString();
                    objCliente.VerificarProductos = Convert.ToBoolean(dr["VerificarProductos"]);
                    objCliente.IDNotificador = Convert.ToInt32(dr["IDNotificador"]);
                    objCliente.IDNotificadorBK = Convert.ToInt32(dr["IDNotificadorBK"]);
                    objCliente.eMailCFD = dr["eMailCFD"].ToString();
                    objCliente.VerificaProveedor = Convert.ToBoolean(dr["VerificaProveedor"]);
                    objCliente.OperacionesRT = Convert.ToBoolean(dr["OperacionesRT"]);
                    objCliente.SectorComercial = dr["SectorComercial"].ToString();
                    objCliente.TipoDeFigura = Convert.ToInt32(dr["TipoDeFigura"]);
                    objCliente.MandanEdocuments = Convert.ToBoolean(dr["MandanEdocuments"]);
                    objCliente.CambianFacturas = Convert.ToBoolean(dr["CambianFacturas"]);
                    objCliente.ForzarSOP = Convert.ToBoolean(dr["ForzarSOP"]);
                    objCliente.EnPadronDeIMP = Convert.ToBoolean(dr["EnPadronDeIMP"]);
                    objCliente.EmailPdfCASA = dr["EmailPdfCASA"].ToString();
                    objCliente.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                    objCliente.RfcParaConsulta = dr["RfcParaConsulta"].ToString();
                    objCliente.EmailManifiesto = dr["EmailManifiesto"].ToString();
                    objCliente.IDVendedor = Convert.ToInt32(dr["IDVendedor"]);
                    objCliente.SoloDeGrupoEI = Convert.ToBoolean(dr["SoloDeGrupoEI"]);
                    objCliente.FechaDeInicio = Convert.ToDateTime(dr["FechaDeInicio"]);
                    objCliente.NoFacturar = Convert.ToBoolean(dr["NoFacturar"]);
                    objCliente.SoloExportacion = Convert.ToBoolean(dr["SoloExportacion"]);
                    objCliente.Prospecto = Convert.ToBoolean(dr["Prospecto"]);
                    objCliente.IdUsuarioManif = Convert.ToInt32(dr["IdUsuarioManif"]);
                    objCliente.IDDireccion = (int)dr["IDDireccion"];
                    objCliente.IDCliente = (int)dr["IDCliente"];
                    objCliente.Direccion = dr["Direccion"].ToString();
                    objCliente.Colonia = dr["Colonia"].ToString();
                    objCliente.Poblacion = dr["Poblacion"].ToString();
                    objCliente.CodigoPostal = dr["CodigoPostal"].ToString();
                    objCliente.NumeroExt = dr["NumeroExt"].ToString();
                    objCliente.NumeroInt = dr["NumeroInt"].ToString();
                    objCliente.EntreLaCalleDe = dr["EntreLaCalleDe"].ToString();
                    objCliente.YDe = dr["YDe"].ToString();
                    objCliente.ClaveEntidadFederativa = dr["ClaveEntidadFederativa"].ToString();
                    objCliente.IdEncriptado = dr["IdEncriptado"].ToString();
                }
                else
                {
                    objCliente = default;
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

            return objCliente;
        }

    }
}
