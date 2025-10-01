using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class PartidasDePedimentoParaDataGridViewRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public PartidasDePedimentoParaDataGridViewRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<PartidasDePedimentoParaDataGridView> CargarVistaPreviaDeAgrupacion(string MyNum_Refe, int MyMetodo, int MyConfirma, string lCve_impo, int lIMP_EXPO, decimal lTipoDeCambio, int lIdCliente)
        {

            var lstPartidas = new List<PartidasDePedimentoParaDataGridView>();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            double ValorAduana;

            try
            {


                // cmd.CommandText = "NET_HACER_PARTIDAS_DE_PEDIMENTO_DACOR"
                //cmd.CommandText = "NET_HACER_PARTIDAS_DE_PEDIMENTO_NEW_2";
                cmd.CommandText = "NET_HACER_PARTIDAS_DE_PEDIMENTO";

                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @NUM_REFE VARCHAR(15) 
                @param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                @param.Value = MyNum_Refe;

                // @METODO INT ,
                @param = cmd.Parameters.Add("@METODO", SqlDbType.Int, 4);
                @param.Value = MyMetodo;

                // @CONFIRMA INT 
                @param = cmd.Parameters.Add("@CONFIRMA", SqlDbType.Int, 4);
                @param.Value = MyConfirma;
                cn.ConnectionString = SConexion;
                cn.Open();

                // Insertar Parametro de busqueda


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        var objPartidas = new PartidasDePedimentoParaDataGridView();
                        objPartidas.Num_Part = Convert.ToInt32(dr["Num_Part"]);
                        objPartidas.FRACCION = dr["FRACCION"].ToString();
                        objPartidas.SUB_PART = dr["SUB_PART"].ToString();
                        objPartidas.DES_MERC = dr["DES_MERC"].ToString();
                        objPartidas.MON_FACT = Convert.ToDecimal(dr["MON_FACT"]);
                        objPartidas.VAL_AGRE = Convert.ToDecimal(dr["VAL_AGRE"]);
                        objPartidas.TIP_MONE = dr["TIP_MONE"].ToString();
                        objPartidas.CAN_FACT = Convert.ToDecimal(dr["CAN_FACT"]);
                        objPartidas.UNI_FACT = dr["UNI_FACT"].ToString();
                        objPartidas.CAN_TARI = Convert.ToDecimal(dr["CAN_TARI"]);
                        objPartidas.UNI_TARI = dr["UNI_TARI"].ToString();
                        objPartidas.CVE_VINC = dr["CVE_VINC"].ToString();
                        objPartidas.CVE_VALO = dr["CVE_VALO"].ToString();
                        objPartidas.PAI_ORIG = dr["PAI_ORIG"].ToString();
                        objPartidas.PAI_VEND = dr["PAI_VEND"].ToString();
                        objPartidas.CAS_TLCS = dr["CAS_TLCS"].ToString();
                        objPartidas.PORC_IVA = Convert.ToDecimal(dr["PORC_IVA"]);

                        ValorAduana = 0;
                        ValorAduana = Math.Round(Convert.ToDouble(dr["MON_FACT"].ToString()) * Convert.ToDouble(dr["EQU_DLLS"].ToString()) * (double)lTipoDeCambio, 0);


                        lstPartidas.Add(objPartidas);

                        if (MyConfirma == 1)
                        {
                            switch (lIdCliente)
                            {
                                case var @case when @case == 51788:
                                    {
                                        if (lIMP_EXPO == 2)
                                        {

                                            NET_LOAD_IDENTIFICADOR_XP_POR_FRACCION(MyNum_Refe, dr["FRACCION"].ToString(), Convert.ToInt32(dr["Num_Part"]), dr["SUB_PART"].ToString());
                                            NET_CONOCER_SI_APLICA_LFPIORPI_O_ANEXO30(MyNum_Refe, dr["FRACCION"].ToString(), Convert.ToInt32(dr["Num_Part"]), Convert.ToDouble(dr["MON_FACT"].ToString()), Convert.ToDouble(dr["CAN_FACT"].ToString()), ValorAduana, lIMP_EXPO, lIdCliente);

                                        }

                                        break;
                                    }

                                default:
                                    {
                                        NET_CONOCER_SI_APLICA_LFPIORPI_O_ANEXO30(MyNum_Refe, dr["FRACCION"].ToString(), Convert.ToInt32(dr["Num_Part"]), Convert.ToDouble(dr["MON_FACT"].ToString()), Convert.ToDouble(dr["CAN_FACT"].ToString()), ValorAduana, lIMP_EXPO, lIdCliente);
                                        break;
                                    }

                            }

                        }


                    }
                }

                else
                {
                    lstPartidas = null;
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

            return lstPartidas;
        }


        public bool NET_LOAD_IDENTIFICADOR_XP_POR_FRACCION(string MyNum_Refe, string MyFraccion, int MyNum_Part, string MyNico)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            bool MyRegreso = false;

            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_IDENTIFICADOR_XP_POR_FRACCION_NICO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @Fraccion VARCHAR(8) 
                @param = cmd.Parameters.Add("@Fraccion", SqlDbType.VarChar, 8);
                @param.Value = MyFraccion;

                // , @Nico VARCHAR(2)
                @param = cmd.Parameters.Add("@Nico", SqlDbType.VarChar, 2);
                @param.Value = MyNico;

                // Insertar Parametro de busqueda


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {

                        var SAAIOIDEFRADATA = new SaaioIdeFraRepository(_configuration);
                        var SAAIOIDEFRA = new SaaioIdeFra();
                        SAAIOIDEFRA.NUM_REFE = MyNum_Refe;
                        SAAIOIDEFRA.NUM_PART = MyNum_Part;
                        SAAIOIDEFRA.NUM_IDE = 0;
                        SAAIOIDEFRA.CVE_PERM = Convert.ToString(dr["CVE_PERM"]).ToString();
                        SAAIOIDEFRA.NUM_PERM = Convert.ToString(dr["NUM_PERM"]).ToString();
                        SAAIOIDEFRA.NUM_PERM2 = Convert.ToString(dr["NUM_PERM2"]).ToString();
                        SAAIOIDEFRA.NUM_PERM3 = Convert.ToString(dr["NUM_PERM3"]).ToString();

                        SAAIOIDEFRADATA.Insertar(SAAIOIDEFRA);

                    }
                }

                else
                {
                    MyRegreso = false;
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

            return MyRegreso;
        }

        public bool NET_CONOCER_SI_APLICA_LFPIORPI_O_ANEXO30(string MyNum_Refe, string MyFraccion, int MyNum_Part, double ValorComercialMN, double MyCantidadComerciaFactura, double MyVALOR_ADUANA, int MyOperacion, int lIdCliente)

        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;
            bool MyRegreso = false;

            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_CONOCER_SI_APLICA_LFPIORPI_O_ANEXO30";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @Fraccion VARCHAR(8) 
                @param = cmd.Parameters.Add("@Codigo", SqlDbType.VarChar, 8);
                @param.Value = MyFraccion;

                // Insertar Parametro de busqueda


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    if (dr.Read())
                    {


                        if (Convert.ToBoolean(dr["LFPIORPI"]))
                        {

                            string Complemento;
                            if (ValorComercialMN >= (double)dr["BaseParaDeterminar"])
                            {
                                Complemento = "1";
                            }
                            else
                            {
                                Complemento = "2";
                            }
                            if ((int)dr["Unidad"] != 1)
                            {
                                if (MyVALOR_ADUANA / MyCantidadComerciaFactura >= (double)dr["BaseParaDeterminar"])
                                {
                                    Complemento = "1";
                                }
                                else
                                {
                                    Complemento = "2";
                                }
                            }

                            var SAAIOIDEFRADATA = new SaaioIdeFraRepository(_configuration);
                            var SAAIOIDEFRA = new SaaioIdeFra();
                            SAAIOIDEFRA.NUM_REFE = MyNum_Refe;
                            SAAIOIDEFRA.NUM_PART = MyNum_Part;
                            SAAIOIDEFRA.NUM_IDE = 0;
                            SAAIOIDEFRA.CVE_PERM = "OV";

                            if (lIdCliente == 21452)
                                Complemento = "3";
                            // Por solicitud de Marcela y autorización de Héctor Garcia de la Cadena en el correo del Jueves 2 de Febrero de 2018 a las 14:29
                            // se pone el complemento 3 en todas las operaciones del cliente Siemens con identificador OV de ley anti lavado, estan copiadas en el correo
                            // Carlos Quinto, Janet Peñafiel, Oscar Chavez Mediana, Luis Sanchez, Juan Manuel Galarza, Alfredo Garcia, Paulina Vazque, rlopez.mex y Arnulfo. 

                            SAAIOIDEFRA.NUM_PERM = Complemento;
                            SAAIOIDEFRA.FEC_PERM = null;
                            if (dr["Sector"].ToString() != "99") // El Sector 99 no existe, aproveche el campo para diferenciar cuando debo meter el identificador y cuando no, la peticion 
                            {
                                // viene de Janet Peñafiel ya que el día de hoy 13 de Enero de 2021 ya se conocen las fracciones nuevas que también aplicaran ley anti-lavado
                                // sin embargo aun no se conoce la fecha de su aplicación, pero Janet desea que ya se empiecen a bloquear pero sin poner el identificador OV 
                                // Cuando ya se conozca la fecha de aplicación, simplemente todas la fracciones de la tabla tgie que tenga LFPIORPI = 1 y Sector 99, les cambio el sector por 1
                                if (SAAIOIDEFRADATA.BuscarIdentificadorDePartida(MyNum_Refe, MyNum_Part, "OV") == 0)
                                    SAAIOIDEFRADATA.Insertar(SAAIOIDEFRA);
                            }


                        }

                        if (Convert.ToBoolean(dr["Anexo30"].ToString()))
                        {
                            if (MyOperacion == 1)
                            {
                                var SAAIOIDEFRADATA = new SaaioIdeFraRepository(_configuration);
                                var SAAIOIDEFRA = new SaaioIdeFra();
                                SAAIOIDEFRA.NUM_REFE = MyNum_Refe;
                                SAAIOIDEFRA.NUM_PART = MyNum_Part;
                                SAAIOIDEFRA.NUM_IDE = 0;
                                SAAIOIDEFRA.CVE_PERM = "MC";
                                SAAIOIDEFRA.FEC_PERM = null;

                                if (SAAIOIDEFRADATA.BuscarIdentificadorDePartida(MyNum_Refe, MyNum_Part, "MC") == 0)
                                    SAAIOIDEFRADATA.Insertar(SAAIOIDEFRA);

                            }
                        }
                    }
                }

                else
                {
                    MyRegreso = false;
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

            return MyRegreso;
        }

    }
}
