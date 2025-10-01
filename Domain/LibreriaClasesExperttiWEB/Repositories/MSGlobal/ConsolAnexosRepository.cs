using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Conagtadu;
using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado
{
    public class ConsolAnexosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public ConsolAnexosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public ConsolAnexos Buscar(string GuiaHouse, int IDDatosDeEmpresa)
        {

            var objCONSOLANEXOS = new ConsolAnexos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CONSOLANEXOSPORGUIAHOUSE_IDDatosDeEmpresa";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("GuiaHouse", SqlDbType.VarChar, 13);
            @param.Value = GuiaHouse;

            @param = cmd.Parameters.Add("IDDatosDeEmpresa", SqlDbType.Int, 4);
            @param.Value = IDDatosDeEmpresa;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCONSOLANEXOS.IDAnexos = Convert.ToInt32(dr["IDAnexos"]);
                    objCONSOLANEXOS.GuiaHouse = dr["GuiaHouse"].ToString();
                    objCONSOLANEXOS.Descripcion = dr["Descripcion"].ToString();
                    objCONSOLANEXOS.ValorME = Convert.ToDouble(dr["ValorME"]);
                    objCONSOLANEXOS.ValorDlls = Convert.ToDouble(dr["ValorDlls"]);
                    objCONSOLANEXOS.ClaveDeMoneda = dr["ClaveDeMoneda"].ToString();
                    objCONSOLANEXOS.Equivalencia = Convert.ToDecimal(dr["Equivalencia"]);
                    objCONSOLANEXOS.IDVendedor = dr["IDVendedor"].ToString();
                    objCONSOLANEXOS.FechaDeOperacion = Convert.ToDateTime(dr["FechaDeOperacion"]);
                    objCONSOLANEXOS.Peso = Convert.ToDecimal(dr["Peso"]);
                    objCONSOLANEXOS.Bultos = Convert.ToInt32(dr["Bultos"]);
                    objCONSOLANEXOS.Estatus = Convert.ToInt32(dr["Estatus"]);
                    objCONSOLANEXOS.IdBloque = Convert.ToInt32(dr["IdBloque"]);
                    objCONSOLANEXOS.idusuario = Convert.ToInt32(dr["idusuario"]);
                    objCONSOLANEXOS.Piezas = Convert.ToInt32(dr["Piezas"]);
                    objCONSOLANEXOS.idusuarioClasificador = Convert.ToInt32(dr["idusuarioClasificador"]);
                    objCONSOLANEXOS.ParametroControl = dr["ParametroControl"].ToString();
                    objCONSOLANEXOS.idfraccion = Convert.ToInt32(dr["idfraccion"]);
                    objCONSOLANEXOS.unifac = Convert.ToInt32(dr["unifac"]);
                    objCONSOLANEXOS.SinLlegar = Convert.ToBoolean(dr["SinLlegar"]);
                    objCONSOLANEXOS.IdReferencia = Convert.ToInt32(dr["IdReferencia"]);
                    objCONSOLANEXOS.SALIO = Convert.ToBoolean(dr["SALIO"]);
                    objCONSOLANEXOS.IataDestino = dr["IataDestino"].ToString();
                }
                else
                {
                    objCONSOLANEXOS = default;
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

            return objCONSOLANEXOS;

        }

        public ConsolAnexos BuscarGuiayIdReferencia(string GuiaHouse, int idReferencia, int IDDatosDeEmpresa)
        {

            var objCONSOLANEXOS = new ConsolAnexos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CONSOLANEXOSPORGUIAHOUSE_idReferencia";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("GuiaHouse", SqlDbType.VarChar, 13);
            @param.Value = GuiaHouse;

            @param = cmd.Parameters.Add("IDDatosDeEmpresa", SqlDbType.Int, 4);
            @param.Value = IDDatosDeEmpresa;

            @param = cmd.Parameters.Add("idReferencia", SqlDbType.Int, 4);
            @param.Value = idReferencia;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCONSOLANEXOS.IDAnexos = Convert.ToInt32(dr["IDAnexos"]);
                    objCONSOLANEXOS.GuiaHouse = dr["GuiaHouse"].ToString();
                    objCONSOLANEXOS.Descripcion = dr["Descripcion"].ToString();
                    objCONSOLANEXOS.ValorME = Convert.ToDouble(dr["ValorME"]);
                    objCONSOLANEXOS.ValorDlls = Convert.ToDouble(dr["ValorDlls"]);
                    objCONSOLANEXOS.ClaveDeMoneda = dr["ClaveDeMoneda"].ToString();
                    objCONSOLANEXOS.Equivalencia = Convert.ToDecimal(dr["Equivalencia"]);
                    objCONSOLANEXOS.IDVendedor = dr["IDVendedor"].ToString();
                    objCONSOLANEXOS.FechaDeOperacion = Convert.ToDateTime(dr["FechaDeOperacion"]);
                    objCONSOLANEXOS.Peso = Convert.ToDecimal(dr["Peso"]);
                    objCONSOLANEXOS.Bultos = Convert.ToInt32(dr["Bultos"]);
                    objCONSOLANEXOS.Estatus = Convert.ToInt32(dr["Estatus"]);
                    objCONSOLANEXOS.IdBloque = Convert.ToInt32(dr["IdBloque"]);
                    objCONSOLANEXOS.idusuario = Convert.ToInt32(dr["idusuario"]);
                    objCONSOLANEXOS.Piezas = Convert.ToInt32(dr["Piezas"]);
                    objCONSOLANEXOS.idusuarioClasificador = Convert.ToInt32(dr["idusuarioClasificador"]);
                    objCONSOLANEXOS.ParametroControl = dr["ParametroControl"].ToString();
                    objCONSOLANEXOS.idfraccion = Convert.ToInt32(dr["idfraccion"]);
                    objCONSOLANEXOS.unifac = Convert.ToInt32(dr["unifac"]);
                    objCONSOLANEXOS.SinLlegar = Convert.ToBoolean(dr["SinLlegar"]);
                    objCONSOLANEXOS.IdReferencia = Convert.ToInt32(dr["IdReferencia"]);
                    objCONSOLANEXOS.SALIO = Convert.ToBoolean(dr["SALIO"]);
                    objCONSOLANEXOS.IataDestino = dr["IataDestino"].ToString();
                }
                else
                {
                    objCONSOLANEXOS = default;
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

            return objCONSOLANEXOS;

        }
        public ConsolAnexos BuscarBaby(string GuiaHouse, int IdCorte, int IDDatosDeEmpresa)
        {
            ConsolAnexos objCONSOLANEXOS = new ConsolAnexos();
            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("Pocket.NET_SEARCH_CONSOLANEXOSPORGUIABABYxCorte_IDDatosDeEmpresa", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("GuiaHouse", SqlDbType.VarChar, 13).Value = GuiaHouse;
                cmd.Parameters.Add("IdCorte", SqlDbType.Int, 4).Value = IdCorte;
                cmd.Parameters.Add("IDDatosDeEmpresa", SqlDbType.Int, 4).Value = IDDatosDeEmpresa;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCONSOLANEXOS.IDAnexos = Convert.ToInt32(dr["IDAnexos"]);
                            objCONSOLANEXOS.GuiaHouse = dr["GuiaHouse"].ToString();
                            objCONSOLANEXOS.Descripcion = dr["Descripcion"].ToString();
                            objCONSOLANEXOS.ValorME = Convert.ToDouble(dr["ValorME"]);
                            objCONSOLANEXOS.ValorDlls = Convert.ToDouble(dr["ValorDlls"]);
                            objCONSOLANEXOS.ClaveDeMoneda = dr["ClaveDeMoneda"].ToString();
                            objCONSOLANEXOS.Equivalencia = Convert.ToDecimal(dr["Equivalencia"]);
                            objCONSOLANEXOS.IDVendedor = dr["IDVendedor"].ToString();
                            objCONSOLANEXOS.FechaDeOperacion = Convert.ToDateTime(dr["FechaDeOperacion"]);
                            objCONSOLANEXOS.Peso = Convert.ToDecimal(dr["Peso"]);
                            objCONSOLANEXOS.Bultos = Convert.ToInt32(dr["Bultos"]);
                            objCONSOLANEXOS.Estatus = Convert.ToInt32(dr["Estatus"]);
                            objCONSOLANEXOS.IdBloque = Convert.ToInt32(dr["IdBloque"]);
                            objCONSOLANEXOS.idusuario = Convert.ToInt32(dr["idusuario"]);
                            objCONSOLANEXOS.Piezas = Convert.ToInt32(dr["Piezas"]);
                            objCONSOLANEXOS.idusuarioClasificador = Convert.ToInt32(dr["idusuarioClasificador"]);
                            objCONSOLANEXOS.ParametroControl = dr["ParametroControl"].ToString();
                            objCONSOLANEXOS.idfraccion = Convert.ToInt32(dr["idfraccion"]);
                            objCONSOLANEXOS.unifac = Convert.ToInt32(dr["unifac"]);
                            objCONSOLANEXOS.SinLlegar = Convert.ToBoolean(dr["SinLlegar"]);
                            objCONSOLANEXOS.IdReferencia = Convert.ToInt32(dr["IdReferencia"]);
                            objCONSOLANEXOS.SALIO = Convert.ToBoolean(dr["SALIO"]);
                            objCONSOLANEXOS.IataDestino = dr["IataDestino"].ToString();
                        }
                        else
                        {
                            objCONSOLANEXOS = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCONSOLANEXOS;
        }

        public ConsolAnexos Buscar(string GuiaHouse, int IdCorte, int IDDatosDeEmpresa)
        {
            var objCONSOLANEXOS = new ConsolAnexos();
            using (var cn = new SqlConnection(sConexion))
            {
                using (var cmd = new SqlCommand("Pocket.NET_SEARCH_CONSOLANEXOSPORGUIAHOUSExCorte_IDDatosDeEmpresa", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("GuiaHouse", SqlDbType.VarChar, 13) { Value = GuiaHouse });
                    cmd.Parameters.Add(new SqlParameter("IdCorte", SqlDbType.Int) { Value = IdCorte });
                    cmd.Parameters.Add(new SqlParameter("IDDatosDeEmpresa", SqlDbType.Int) { Value = IDDatosDeEmpresa });

                    try
                    {
                        cn.Open();
                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                objCONSOLANEXOS.IDAnexos = Convert.ToInt32(dr["IDAnexos"]);
                                objCONSOLANEXOS.GuiaHouse = dr["GuiaHouse"].ToString();
                                objCONSOLANEXOS.Descripcion = dr["Descripcion"].ToString();
                                objCONSOLANEXOS.ValorME = Convert.ToDouble(dr["ValorME"]);
                                objCONSOLANEXOS.ValorDlls = Convert.ToDouble(dr["ValorDlls"]);
                                objCONSOLANEXOS.ClaveDeMoneda = dr["ClaveDeMoneda"].ToString();
                                objCONSOLANEXOS.Equivalencia = Convert.ToDecimal(dr["Equivalencia"]);
                                objCONSOLANEXOS.IDVendedor = dr["IDVendedor"].ToString();
                                objCONSOLANEXOS.FechaDeOperacion = Convert.ToDateTime(dr["FechaDeOperacion"]);
                                objCONSOLANEXOS.Peso = Convert.ToDecimal(dr["Peso"]);
                                objCONSOLANEXOS.Bultos = Convert.ToInt32(dr["Bultos"]);
                                objCONSOLANEXOS.Estatus = Convert.ToInt32(dr["Estatus"]);
                                objCONSOLANEXOS.IdBloque = Convert.ToInt32(dr["IdBloque"]);
                                objCONSOLANEXOS.idusuario = Convert.ToInt32(dr["idusuario"]);
                                objCONSOLANEXOS.Piezas = Convert.ToInt32(dr["Piezas"]);
                                objCONSOLANEXOS.idusuarioClasificador = Convert.ToInt32(dr["idusuarioClasificador"]);
                                objCONSOLANEXOS.ParametroControl = dr["ParametroControl"].ToString();
                                objCONSOLANEXOS.idfraccion = Convert.ToInt32(dr["idfraccion"]);
                                objCONSOLANEXOS.unifac = Convert.ToInt32(dr["unifac"]);
                                objCONSOLANEXOS.SinLlegar = Convert.ToBoolean(dr["SinLlegar"]);
                                objCONSOLANEXOS.IdReferencia = Convert.ToInt32(dr["IdReferencia"]);
                                objCONSOLANEXOS.SALIO = Convert.ToBoolean(dr["SALIO"]);
                                objCONSOLANEXOS.IataDestino = dr["IataDestino"].ToString();
                            }
                            else
                            {
                                objCONSOLANEXOS = null; // Equivalent to Nothing in VB.NET
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return objCONSOLANEXOS;
        }



        public List<ConsolAnexos> Cargarlst( int idBloque)
        {

            var  lstCONSOLANEXOS = new List<ConsolAnexos>();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CONSOLANEXOSXBLOQUE_CONAGTADU";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdBloque", SqlDbType.Int, 4);
            @param.Value = idBloque;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        var objCONSOLANEXOS = new ConsolAnexos();
                        objCONSOLANEXOS.IDAnexos = Convert.ToInt32(dr["IDAnexos"]);
                        objCONSOLANEXOS.GuiaHouse = dr["GuiaHouse"].ToString();
                        objCONSOLANEXOS.Descripcion = dr["Descripcion"].ToString();
                        objCONSOLANEXOS.ValorME = Convert.ToDouble(dr["ValorME"]);
                        objCONSOLANEXOS.ValorDlls = Convert.ToDouble(dr["ValorDlls"]);
                        objCONSOLANEXOS.ClaveDeMoneda = dr["ClaveDeMoneda"].ToString();
                        objCONSOLANEXOS.Equivalencia = Convert.ToDecimal(dr["Equivalencia"]);
                        objCONSOLANEXOS.IDVendedor = dr["IDVendedor"].ToString();
                        objCONSOLANEXOS.FechaDeOperacion = Convert.ToDateTime(dr["FechaDeOperacion"]);
                        objCONSOLANEXOS.Peso = Convert.ToDecimal(dr["Peso"]);
                        objCONSOLANEXOS.Bultos = Convert.ToInt32(dr["Bultos"]);
                        objCONSOLANEXOS.Estatus = Convert.ToInt32(dr["Estatus"]);
                        objCONSOLANEXOS.IdBloque = Convert.ToInt32(dr["IdBloque"]);
                        objCONSOLANEXOS.idusuario = Convert.ToInt32(dr["idusuario"]);
                        objCONSOLANEXOS.Piezas = Convert.ToInt32(dr["Piezas"]);
                        objCONSOLANEXOS.idusuarioClasificador = Convert.ToInt32(dr["idusuarioClasificador"]);
                        objCONSOLANEXOS.ParametroControl = dr["ParametroControl"].ToString();
                        objCONSOLANEXOS.idfraccion = Convert.ToInt32(dr["idfraccion"]);
                        objCONSOLANEXOS.unifac = Convert.ToInt32(dr["unifac"]);
                        objCONSOLANEXOS.SinLlegar = Convert.ToBoolean(dr["SinLlegar"]);
                        objCONSOLANEXOS.IdReferencia = Convert.ToInt32(dr["IdReferencia"]);
                        objCONSOLANEXOS.SALIO = Convert.ToBoolean(dr["SALIO"]);
                        objCONSOLANEXOS.IataDestino = dr["IataDestino"].ToString();

                        lstCONSOLANEXOS.Add(objCONSOLANEXOS);
                    }  
                  
                }
                else
                {
                    lstCONSOLANEXOS.Clear();
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

            return lstCONSOLANEXOS;

        }


        public NuevoConagtadu BuscarProrateo(int idReferencia ,string GuiaHouse)
        {

            var objNuevoCon = new NuevoConagtadu();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_CONAGTADU_JSON_POR_GUIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("IDREFERENCIA", SqlDbType.Int, 4);
            @param.Value = idReferencia;

            @param = cmd.Parameters.Add("GUIAHOUSE", SqlDbType.VarChar, 10);
            @param.Value = GuiaHouse;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objNuevoCon.Fijo0 = dr["Fijo0"].ToString();
                    objNuevoCon.Brocker= dr["Brocker"].ToString();
                    objNuevoCon.Pediment = dr["Pediment"].ToString();
                    objNuevoCon.FijoUno = dr["Fijo1"].ToString();
                    objNuevoCon.DateCapture = Convert.ToDateTime(dr["DataCapture"]);
                    objNuevoCon.Guia = dr["Guia"].ToString();
                    objNuevoCon.FijoDos = dr["Fijo2"].ToString();
                    objNuevoCon.FijoTres = dr["Fijo3"].ToString();
                    objNuevoCon.Contenido = dr["Contenido"].ToString();
                    objNuevoCon.Valor = Convert.ToDouble(dr["Valor"]);
                    objNuevoCon.Piezas = Convert.ToInt32(dr["Piezas"]);
                    objNuevoCon.Peso = Convert.ToDouble(dr["Peso"]);
                    objNuevoCon.Mava = dr["Mava"].ToString();
                    objNuevoCon.GuiaMaster = dr["GuiaMaster"].ToString();
                    objNuevoCon.FijoCuatro = dr["Fijo4"].ToString();
                    objNuevoCon.ClavePedimento= dr["ClavePedimento"].ToString();
                    objNuevoCon.FijoCinco= dr["Fijo5"].ToString();
                    objNuevoCon.ImpuestosYDerechos = Convert.ToDouble(dr["ImpuestosYDerechos"]);
                    objNuevoCon.UserId = dr["UserId"].ToString();
                    objNuevoCon.ParametroContol = dr["ParametroControl"].ToString();
                    objNuevoCon.ProrateroPRV= Convert.ToDouble(dr["ProrateoPRV"]);
                    objNuevoCon.IVA = Convert.ToDouble(dr["IVA"]);

                }
                else
                {
                    objNuevoCon = default;
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

            return objNuevoCon;

        }


        public List<GuiasPendientes> ConagtadusPendientes()
        {
            List<GuiasPendientes> Guias = new List<GuiasPendientes>();

            var objCONSOLANEXOS = new ConsolAnexos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CONAGTADUS_PENDIENTES";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

          

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                  while  ( dr.Read())
                   {
                     GuiasPendientes guiasPendientes = new GuiasPendientes();

                    guiasPendientes.GuiaHouse= dr["GuiaHouse"].ToString();
                    guiasPendientes.idReferencia = Convert.ToInt32(dr["idReferencia"]);


                     Guias.Add(guiasPendientes);
                   }
                  
                }
                else
                {
                    Guias.Clear();
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

            return Guias;

        }


        public List<GuiasPendientes> ConagtadusPendientesHilos(int Hilo)
        {
            List<GuiasPendientes> Guias = new List<GuiasPendientes>();

            var objCONSOLANEXOS = new ConsolAnexos();
            SqlParameter @param;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CONAGTADUS_PENDIENTES_HILO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@HILO", SqlDbType.Int, 4);
            @param.Value = Hilo;


            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        GuiasPendientes guiasPendientes = new GuiasPendientes();

                        guiasPendientes.GuiaHouse = dr["GuiaHouse"].ToString();
                        guiasPendientes.idReferencia = Convert.ToInt32(dr["idReferencia"]);


                        Guias.Add(guiasPendientes);
                    }

                }
                else
                {
                    Guias.Clear();
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

            return Guias;

        }

        public List<string> ConagtadusporReferencia(int idReferencia)
        {
            List<string> Guias = new List<string>();

            var objCONSOLANEXOS = new ConsolAnexos();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_CONAGTADUS_REFERENCIA";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            @param = cmd.Parameters.Add("IDREFERENCIA", SqlDbType.Int, 4);
            @param.Value = idReferencia;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        string GuiaHouse = dr["GuiaHouse"].ToString();
                        Guias.Add(GuiaHouse);
                    }

                }
                else
                {
                    Guias.Clear();
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

            return Guias;

        }



    }
}
