using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogodeMasterRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public CatalogodeMasterRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogodeMaster Buscar(string GuiaMaster)
        {

            var objCATALOGODEMASTER = new CatalogodeMaster();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            if (GuiaMaster.Length <= 11)
            {
                GuiaMaster = Strings.Mid(GuiaMaster, 1, 3) + "-" + Strings.Mid(GuiaMaster, 4, 8);
            }
            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_CATALOGODEMASTER";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@GuiaMaster", SqlDbType.VarChar, 15);
                @param.Value = GuiaMaster;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODEMASTER.IDMasterConsol = Convert.ToInt32(dr["IDMasterConsol"]);
                    objCATALOGODEMASTER.GuiaMaster = dr["GuiaMaster"].ToString();
                    objCATALOGODEMASTER.IDOrigen = dr["IDOrigen"].ToString();
                    objCATALOGODEMASTER.IDUsuario = Convert.ToInt32(dr["IDUsuario"]);
                    objCATALOGODEMASTER.TipoDePedimento = dr["TipoDePedimento"].ToString();
                    objCATALOGODEMASTER.IDPrefijo = Convert.ToInt32(dr["IDPrefijo"]);
                    objCATALOGODEMASTER.FechaCaptura = Convert.ToDateTime(dr["FechaCaptura"]);
                    objCATALOGODEMASTER.FechaArribo = Convert.ToDateTime(dr["FechaArribo"]);
                    objCATALOGODEMASTER.FechaArriboUnitarias = Convert.ToDateTime(dr["FechaArriboUnitarias"]);
                    objCATALOGODEMASTER.Automatico = Convert.ToBoolean(dr["Automatico"]);
                    objCATALOGODEMASTER.AplicaSorting = Convert.ToBoolean(dr["AplicaSorting"]);
                    objCATALOGODEMASTER.Prefijo = dr["Prefijo"].ToString();
                    objCATALOGODEMASTER.NombreUsuario = dr["NombreUsuario"].ToString();
                    objCATALOGODEMASTER.AduanaDespacho = dr["AduanaDespacho"].ToString();
                    objCATALOGODEMASTER.IdOficina = Convert.ToInt32(dr["IdOficina"].ToString());
                }

                else
                {
                    objCATALOGODEMASTER = default;
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

            return objCATALOGODEMASTER;
        }

        public CatalogodeMaster Buscar(int IDMaster)
        {

            var objCATALOGODEMASTER = new CatalogodeMaster();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEARCH_CATALOGODEMASTERPORIDMASTERCONSOL";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IDMasterConsol", SqlDbType.Int);
                @param.Value = IDMaster;


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODEMASTER.IDMasterConsol = Convert.ToInt32(dr["IDMasterConsol"]);
                    objCATALOGODEMASTER.GuiaMaster = dr["GuiaMaster"].ToString();
                    objCATALOGODEMASTER.IDOrigen = dr["IDOrigen"].ToString();
                    objCATALOGODEMASTER.IDUsuario = Convert.ToInt32(dr["IDUsuario"]);
                    objCATALOGODEMASTER.TipoDePedimento = dr["TipoDePedimento"].ToString();
                    objCATALOGODEMASTER.IDPrefijo = Convert.ToInt32(dr["IDPrefijo"]);
                    objCATALOGODEMASTER.FechaCaptura = Convert.ToDateTime(dr["FechaCaptura"]);
                    objCATALOGODEMASTER.FechaArribo = Convert.ToDateTime(dr["FechaArribo"]);
                    objCATALOGODEMASTER.FechaArriboUnitarias = Convert.ToDateTime(dr["FechaArriboUnitarias"]);
                    objCATALOGODEMASTER.Automatico = Convert.ToBoolean(dr["Automatico"]);
                    objCATALOGODEMASTER.AplicaSorting = Convert.ToBoolean(dr["AplicaSorting"]);
                    objCATALOGODEMASTER.Prefijo = dr["Prefijo"].ToString();
                    objCATALOGODEMASTER.NombreUsuario = dr["NombreUsuario"].ToString();
                    objCATALOGODEMASTER.ImagenMasterizacion = Convert.ToBoolean(dr["ImagenMasterizacion"]);
                    objCATALOGODEMASTER.ImagenRevalidacion = Convert.ToBoolean(dr["ImagenRevalidacion"]);
                }
                else
                {
                    objCATALOGODEMASTER = default;
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

            return objCATALOGODEMASTER;
        }

        public CatalogodeMaster BuscarPorDia(int IdOficina)
        {

            var objCATALOGODEMASTER = new CatalogodeMaster();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "fdx.NET_SEARCH_CATALOGODEMASTERXOFICINAXPO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
                @param.Value = IdOficina;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODEMASTER.IDMasterConsol = Convert.ToInt32(dr["IDMasterConsol"]);
                    objCATALOGODEMASTER.GuiaMaster = dr["GuiaMaster"].ToString();
                    objCATALOGODEMASTER.IDOrigen = dr["IDOrigen"].ToString();
                    objCATALOGODEMASTER.IDUsuario = Convert.ToInt32(dr["IDUsuario"]);
                    objCATALOGODEMASTER.TipoDePedimento = dr["TipoDePedimento"].ToString();
                    objCATALOGODEMASTER.IDPrefijo = Convert.ToInt32(dr["IDPrefijo"]);
                    objCATALOGODEMASTER.FechaCaptura = Convert.ToDateTime(dr["FechaCaptura"]);
                    objCATALOGODEMASTER.FechaArribo = Convert.ToDateTime(dr["FechaArribo"]);
                    objCATALOGODEMASTER.FechaArriboUnitarias = Convert.ToDateTime(dr["FechaArriboUnitarias"]);
                    objCATALOGODEMASTER.Automatico = Convert.ToBoolean(dr["Automatico"]);
                    objCATALOGODEMASTER.AplicaSorting = Convert.ToBoolean(dr["AplicaSorting"]);
                    objCATALOGODEMASTER.Prefijo = dr["Prefijo"].ToString();
                    objCATALOGODEMASTER.NombreUsuario = dr["NombreUsuario"].ToString();
                }

                else
                {
                    objCATALOGODEMASTER = default;
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

            return objCATALOGODEMASTER;
        }

        public List<CatalogodeMaster> Cargar(DateTime FechaArribo)
        {

            var lstCATALOGODEMASTER = new List<CatalogodeMaster>();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_CATALOGODEMASTER";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@FechaArribo", SqlDbType.Date);
                @param.Value = FechaArribo;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        var objCATALOGODEMASTER = new CatalogodeMaster();
                        objCATALOGODEMASTER.IDMasterConsol = Convert.ToInt32(dr["IDMasterConsol"]);
                        objCATALOGODEMASTER.GuiaMaster = dr["GuiaMaster"].ToString();
                        objCATALOGODEMASTER.IDOrigen = dr["IDOrigen"].ToString();
                        objCATALOGODEMASTER.IDUsuario = Convert.ToInt32(dr["IDUsuario"]);
                        objCATALOGODEMASTER.TipoDePedimento = dr["TipoDePedimento"].ToString();
                        objCATALOGODEMASTER.IDPrefijo = Convert.ToInt32(dr["IDPrefijo"]);
                        objCATALOGODEMASTER.FechaCaptura = Convert.ToDateTime(dr["FechaCaptura"]);
                        objCATALOGODEMASTER.FechaArribo = Convert.ToDateTime(dr["FechaArribo"]);
                        objCATALOGODEMASTER.FechaArriboUnitarias = Convert.ToDateTime(dr["FechaArriboUnitarias"]);
                        objCATALOGODEMASTER.Automatico = Convert.ToBoolean(dr["Automatico"]);
                        objCATALOGODEMASTER.AplicaSorting = Convert.ToBoolean(dr["AplicaSorting"]);

                        lstCATALOGODEMASTER.Add(objCATALOGODEMASTER);
                    }
                }
                else
                {
                    lstCATALOGODEMASTER = null;
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

            return lstCATALOGODEMASTER;
        }

        public List<int> CargarCortes(DateTime FECHA)
        {
            var lstCortesMaster = new List<int>();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_CONSOLBLOQUES_CORTES";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@FECHA", SqlDbType.Date);
                @param.Value = FECHA;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                    lstCortesMaster.Add(Convert.ToInt32(dr["Corte"]));

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

            return lstCortesMaster;
        }

        public int Insertar(CatalogodeMaster lcatalogodemaster)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_CATALOGODEMASTER";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // ,@GuiaMaster  char
            @param = cmd.Parameters.Add("@GuiaMaster", SqlDbType.VarChar, 15);
            @param.Value = lcatalogodemaster.GuiaMaster;

            // ,@IDOrigen  int
            @param = cmd.Parameters.Add("@IDOrigen", SqlDbType.VarChar, 10);
            @param.Value = lcatalogodemaster.IDOrigen;

            // ,@IDUsuario  int
            @param = cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4);
            @param.Value = lcatalogodemaster.IDUsuario;

            // ,@TipoDePedimento  char
            @param = cmd.Parameters.Add("@TipoDePedimento", SqlDbType.Char, 20);
            @param.Value = lcatalogodemaster.TipoDePedimento;

            // ,@IDPrefijo  int
            @param = cmd.Parameters.Add("@IDPrefijo", SqlDbType.Int, 4);
            @param.Value = lcatalogodemaster.IDPrefijo;

            // ,@FechaArribo  datetime
            @param = cmd.Parameters.Add("@FechaArribo", SqlDbType.DateTime, 4);
            @param.Value = lcatalogodemaster.FechaArribo;

            // ,@FechaArriboUnitarias  datetime
            @param = cmd.Parameters.Add("@FechaArriboUnitarias", SqlDbType.DateTime, 4);
            @param.Value = lcatalogodemaster.FechaArriboUnitarias;

            // ,@Automatico  bit
            @param = cmd.Parameters.Add("@Automatico", SqlDbType.Bit, 4);
            @param.Value = lcatalogodemaster.Automatico;

            // ,@AplicaSorting  bit
            @param = cmd.Parameters.Add("@AplicaSorting", SqlDbType.Bit, 4);
            @param.Value = lcatalogodemaster.AplicaSorting;


            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;

            try
            {
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
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CATALOGODEMASTER");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return id;
        }

        public int Modificar(CatalogodeMaster lcatalogodemaster)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CATALOGODEMASTER_NEW";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // ,@IDMasterConsol  int
            @param = cmd.Parameters.Add("@IDMasterConsol", SqlDbType.Int, 4);
            @param.Value = lcatalogodemaster.IDMasterConsol;

            // ,@GuiaMaster  char
            @param = cmd.Parameters.Add("@GuiaMaster", SqlDbType.VarChar, 15);
            @param.Value = lcatalogodemaster.GuiaMaster;

            // ,@IDOrigen  int
            @param = cmd.Parameters.Add("@IDOrigen", SqlDbType.VarChar, 10);
            @param.Value = lcatalogodemaster.IDOrigen;

            // ,@IDUsuario  int
            @param = cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4);
            @param.Value = lcatalogodemaster.IDUsuario;

            // ,@TipoDePedimento  char
            @param = cmd.Parameters.Add("@TipoDePedimento", SqlDbType.Char, 20);
            @param.Value = lcatalogodemaster.TipoDePedimento;

            // ,@IDPrefijo  int
            @param = cmd.Parameters.Add("@IDPrefijo", SqlDbType.Int, 4);
            @param.Value = lcatalogodemaster.IDPrefijo;

            // ,@FechaArribo  datetime
            @param = cmd.Parameters.Add("@FechaArribo", SqlDbType.DateTime, 4);
            @param.Value = lcatalogodemaster.FechaArribo;

            // ,@FechaArriboUnitarias  datetime
            @param = cmd.Parameters.Add("@FechaArriboUnitarias", SqlDbType.DateTime, 4);
            @param.Value = lcatalogodemaster.FechaArriboUnitarias;

            // ,@Automatico  bit
            @param = cmd.Parameters.Add("@Automatico", SqlDbType.Bit, 4);
            @param.Value = lcatalogodemaster.Automatico;

            // ,@AplicaSorting  bit
            @param = cmd.Parameters.Add("@AplicaSorting", SqlDbType.Bit, 4);
            @param.Value = lcatalogodemaster.AplicaSorting;

            // ,@ImagenMasterizacion BIT
            @param = cmd.Parameters.Add("@ImagenMasterizacion", SqlDbType.Bit, 4);
            @param.Value = lcatalogodemaster.ImagenMasterizacion;

            // ,@ImagenRevalidacion BIT
            @param = cmd.Parameters.Add("@ImagenRevalidacion", SqlDbType.Bit, 4);
            @param.Value = lcatalogodemaster.ImagenRevalidacion;


            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;

            try
            {
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
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CATALOGODEMASTER");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }

        public List<CatalogodeMaster> CargarLista(string FechaInicial, string FechaFinal)
        {

            var lstcatalogodemaster = new List<CatalogodeMaster>();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CONSOLMASTER";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@FechaInicial", SqlDbType.VarChar, 10);
            @param.Value = FechaInicial;


            @param = cmd.Parameters.Add("@FechaFinal", SqlDbType.VarChar, 10);
            @param.Value = FechaFinal;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        var objcatalogodemaster = new CatalogodeMaster();

                        objcatalogodemaster.IDMasterConsol = Convert.ToInt32(dr["IDMasterConsol"]);
                        objcatalogodemaster.GuiaMaster = dr["GuiaMaster"].ToString();

                        lstcatalogodemaster.Add(objcatalogodemaster);
                    }
                }

                else
                {
                    lstcatalogodemaster = null;
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

            return lstcatalogodemaster;
        }

        public DataTable Cortes(int IdOficina, int IdMasterConsol)
        {

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {

                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_CONSOLMASTERCORTES";

                    // , @IDCONSOLMASTER INT
                    @param = dap.SelectCommand.Parameters.Add("@IdMasterConsol", SqlDbType.Int);
                    @param.Value = IdMasterConsol;

                    // @IDOFICINA INT
                    @param = dap.SelectCommand.Parameters.Add("@IdOficina", SqlDbType.Int);
                    @param.Value = IdOficina;



                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + "NET_LOAD_CONSOLMASTERCORTES");
                }

            }
            return dtb;
        }

        public int UpdateMasterXPO(ref int IdReferencia, string NumeroDeReferencia, int IdVuelo, int IdOficina)
        {
            int iStatus;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_RELACIONMASTER_XPO2";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            @param.Value = IdReferencia;

            @param = cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15);
            @param.Value = NumeroDeReferencia;

            @param = cmd.Parameters.Add("@IdVuelo", SqlDbType.Int, 4);
            @param.Value = IdVuelo;

            @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
            @param.Value = IdOficina;

            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    iStatus = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    iStatus = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (SqlException ex)
            {
                iStatus = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_RELACIONMASTER_XPO");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return iStatus;
        }
        public int UpdateMasterByIdXPO(ref int IdMaster, ref int IdReferencia, string NumeroDeReferencia)
        {
            int iStatus;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "Pocket.NET_UPDATE_MASTER_XPO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            @param.Value = IdReferencia;

            @param = cmd.Parameters.Add("@IdMasterConsol", SqlDbType.Int, 4);
            @param.Value = IdMaster;

            @param = cmd.Parameters.Add("@NumeroDeReferencia", SqlDbType.VarChar, 15);
            @param.Value = NumeroDeReferencia;

            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;

            try
            {
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    iStatus = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    iStatus = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (SqlException ex)
            {
                iStatus = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_MASTER_XPO");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return iStatus;
        }

        public DataTable GetGuiasMaster(int Operacion, int IdOficina, int IdDatosEmpresa)
        {

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {

                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LIST_GUIA_MASTER";

                    // , @IDCONSOLMASTER INT
                    @param = dap.SelectCommand.Parameters.Add("@Operacion", SqlDbType.Int);
                    @param.Value = Operacion;

                    // @IDOFICINA INT
                    @param = dap.SelectCommand.Parameters.Add("@Iddatosdeempresa", SqlDbType.Int);
                    @param.Value = IdDatosEmpresa;

                    @param = dap.SelectCommand.Parameters.Add("@IdOficina", SqlDbType.Int);
                    @param.Value = IdOficina;



                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + "NET_LIST_GUIA_MASTER");
                }

            }
            return dtb;
        }
        public DataTable GetReportControlAsignacionAifa(string GuiaMaster)
        {

            var dtb = new DataTable();
            SqlParameter @param;

            using (var cn = new SqlConnection(sConexion))
            {

                try
                {
                    cn.Open();
                    var dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_REPORT_CONTROLASIGNACIONAIFA";

                    // , @ide INT
                    @param = dap.SelectCommand.Parameters.Add("@ide", SqlDbType.Int);
                    @param.Value = 0;

                    // @GUIAMASTER VARCHAR
                    @param = dap.SelectCommand.Parameters.Add("@GUIAMASTER", SqlDbType.VarChar);
                    @param.Value = GuiaMaster;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();
                    cn.Dispose();
                }

                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();
                    throw new Exception(ex.Message.ToString() + "NET_REPORT_CONTROLASIGNACIONAIFA");
                }

            }
            return dtb;
        }
        public bool SendMailReportControlAsignacionAifa(string GuiaMaster)
        {
            bool result = false;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_REPORT_CONTROLASIGNACIONAIFA_MAIL";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("@GUIAMASTER", SqlDbType.VarChar, 15);
            @param.Value = GuiaMaster;

            try
            {
                cmd.ExecuteNonQuery();
                result = true;

                cmd.Parameters.Clear();
            }
            catch (SqlException ex)
            {
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_REPORT_CONTROLASIGNACIONAIFA_MAIL");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();

            return result;
        }
    }
}
