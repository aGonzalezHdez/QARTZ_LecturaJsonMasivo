using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesCasa
{
    public class SaaioFacparRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn = null!;

        public SaaioFacparRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public Entities.EntitiesPedimentos.SaaioFacpar Buscar(string MyNum_refe, int MyCons_Fact, int MCons_Part, string MyConnectionString)
        {
            Entities.EntitiesPedimentos.SaaioFacpar objSAAIO_FACPAR = new Entities.EntitiesPedimentos.SaaioFacpar();

            using (SqlConnection cn = new SqlConnection(MyConnectionString))
            using (SqlCommand cmd = new SqlCommand("NET_SEARCH_SAAIO_FACPAR", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = MyNum_refe;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = MyCons_Fact;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = MCons_Part;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objSAAIO_FACPAR.NUM_REFE = dr["NUM_REFE"].ToString();
                            objSAAIO_FACPAR.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                            objSAAIO_FACPAR.CONS_PART = Convert.ToInt32(dr["CONS_PART"]);
                            objSAAIO_FACPAR.NUM_PART = dr["NUM_PART"].ToString();
                            objSAAIO_FACPAR.PAI_ORIG = dr["PAI_ORIG"].ToString();
                            objSAAIO_FACPAR.PAI_VEND = dr["PAI_VEND"].ToString();
                            objSAAIO_FACPAR.FRACCION = dr["FRACCION"].ToString();
                            objSAAIO_FACPAR.CAS_TLCS = dr["CAS_TLCS"].ToString();
                            objSAAIO_FACPAR.COM_TLC =  dr["COM_TLC"].ToString();
                            objSAAIO_FACPAR.ADVAL =    Convert.ToDouble(dr["ADVAL"]);
                            objSAAIO_FACPAR.PORC_IVA = Convert.ToDouble(dr["PORC_IVA"]);
                            objSAAIO_FACPAR.MON_FACT = Convert.ToDouble(dr["MON_FACT"]);
                            objSAAIO_FACPAR.TIP_MONE = dr["TIP_MONE"].ToString();
                            objSAAIO_FACPAR.TIP_MERC = dr["TIP_MERC"].ToString();
                            objSAAIO_FACPAR.USO_CARA = dr["USO_CARA"].ToString();
                            objSAAIO_FACPAR.CVE_VALO = dr["CVE_VALO"].ToString();
                            objSAAIO_FACPAR.CVE_VINC = dr["CVE_VINC"].ToString();
                            objSAAIO_FACPAR.PES_UNIT = Convert.ToDouble(dr["PES_UNIT"]);
                            objSAAIO_FACPAR.UNI_PESO = dr["UNI_PESO"].ToString();
                            objSAAIO_FACPAR.PES_NETO = Convert.ToDouble(dr["PES_NETO"]);
                            objSAAIO_FACPAR.CAN_FACT = Convert.ToDouble(dr["CAN_FACT"]);
                            objSAAIO_FACPAR.UNI_FACT = dr["UNI_FACT"].ToString();
                            objSAAIO_FACPAR.CAN_TARI = Convert.ToDouble(dr["CAN_TARI"]);
                            objSAAIO_FACPAR.UNI_TARI = dr["UNI_TARI"].ToString();
                            objSAAIO_FACPAR.DES_MERC = dr["DES_MERC"].ToString();
                            objSAAIO_FACPAR.OBS_FRAC = dr["OBS_FRAC"].ToString();
                            objSAAIO_FACPAR.VAL_AGRE = Convert.ToDouble(dr["VAL_AGRE"]);
                            objSAAIO_FACPAR.VAL_UNIT = Convert.ToDouble(dr["VAL_UNIT"]);
                            objSAAIO_FACPAR.FEC_TLC = dr["FEC_TLC"].ToString();
                            objSAAIO_FACPAR.NUM_PEDIDO = dr["NUM_PEDIDO"].ToString();
                            objSAAIO_FACPAR.POS_PEDIDO = Convert.ToInt32(dr["POS_PEDIDO"]);
                            objSAAIO_FACPAR.OBS_ADIC = dr["OBS_ADIC"].ToString();
                            objSAAIO_FACPAR.DECR_TLC = dr["DECR_TLC"].ToString();
                            objSAAIO_FACPAR.MAR_MERC = dr["MAR_MERC"].ToString();
                            objSAAIO_FACPAR.SUB_MODE = dr["SUB_MODE"].ToString();
                            objSAAIO_FACPAR.NUM_SERI = dr["NUM_SERI"].ToString();
                            objSAAIO_FACPAR.CANT_COVE = Convert.ToDouble(dr["CANT_COVE"]);
                            objSAAIO_FACPAR.UNI_COVE = dr["UNI_COVE"].ToString();
                            objSAAIO_FACPAR.DESC_COVE = dr["DESC_COVE"].ToString();
                            objSAAIO_FACPAR.SUB_PART = dr["SUB_PART"].ToString();
                            objSAAIO_FACPAR.Ultimo = dr["Ultimo"].ToString();
                            objSAAIO_FACPAR.Primero = dr["Primero"].ToString();
                            objSAAIO_FACPAR.Siguiente = dr["Siguiente"].ToString();
                            objSAAIO_FACPAR.Anterior = dr["Anterior"].ToString();
                        }
                        else
                        {
                            objSAAIO_FACPAR = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objSAAIO_FACPAR;
        }

        public List<Entities.EntitiesPedimentos.SaaioFacpar> Cargar(string MyNum_refe, int MyCons_Fact)
        {
            List<Entities.EntitiesPedimentos.SaaioFacpar> lstSAAIO_FACPAR = new List<Entities.EntitiesPedimentos.SaaioFacpar>();

            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_LOAD_SAAIO_FACPAR", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = MyNum_refe;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = MyCons_Fact;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var obj = new Entities.EntitiesPedimentos.SaaioFacpar
                                {
                                    NUM_REFE = dr["NUM_REFE"].ToString(),
                                    CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]),
                                    CONS_PART = Convert.ToInt32(dr["CONS_PART"]),
                                    NUM_PART = dr["NUM_PART"].ToString(),
                                    PAI_ORIG = dr["PAI_ORIG"].ToString(),
                                    PAI_VEND = dr["PAI_VEND"].ToString(),
                                    FRACCION = dr["FRACCION"].ToString(),
                                    CAS_TLCS = dr["CAS_TLCS"].ToString(),
                                    COM_TLC = dr["COM_TLC"].ToString(),
                                    ADVAL = Convert.ToDouble(dr["ADVAL"]),
                                    PORC_IVA = Convert.ToDouble(dr["PORC_IVA"]),
                                    MON_FACT = Convert.ToDouble(dr["MON_FACT"]),
                                    TIP_MONE = dr["TIP_MONE"].ToString(),
                                    TIP_MERC = dr["TIP_MERC"].ToString(),
                                    USO_CARA = dr["USO_CARA"].ToString(),
                                    CVE_VALO = dr["CVE_VALO"].ToString(),
                                    CVE_VINC = dr["CVE_VINC"].ToString(),
                                    PES_UNIT = Convert.ToDouble(dr["PES_UNIT"]),
                                    UNI_PESO = dr["UNI_PESO"].ToString(),
                                    PES_NETO = Convert.ToDouble(dr["PES_NETO"]),
                                    CAN_FACT = Convert.ToDouble(dr["CAN_FACT"]),
                                    UNI_FACT = dr["UNI_FACT"].ToString(),
                                    CAN_TARI = Convert.ToDouble(dr["CAN_TARI"]),
                                    UNI_TARI = dr["UNI_TARI"].ToString(),
                                    DES_MERC = dr["DES_MERC"].ToString(),
                                    OBS_FRAC = dr["OBS_FRAC"].ToString(),
                                    VAL_AGRE = Convert.ToDouble(dr["VAL_AGRE"]),
                                    VAL_UNIT = Convert.ToDouble(dr["VAL_UNIT"]),
                                    FEC_TLC = dr["FEC_TLC"].ToString(),
                                    NUM_PEDIDO = dr["NUM_PEDIDO"].ToString(),
                                    POS_PEDIDO = Convert.ToInt32(dr["POS_PEDIDO"]),
                                    OBS_ADIC = dr["OBS_ADIC"].ToString(),
                                    DECR_TLC = dr["DECR_TLC"].ToString(),
                                    MAR_MERC = dr["MAR_MERC"].ToString(),
                                    SUB_MODE = dr["SUB_MODE"].ToString(),
                                    NUM_SERI = dr["NUM_SERI"].ToString(),
                                    CANT_COVE = Convert.ToDouble(dr["CANT_COVE"]),
                                    UNI_COVE = dr["UNI_COVE"].ToString(),
                                    DESC_COVE = dr["DESC_COVE"].ToString()
                                };

                                lstSAAIO_FACPAR.Add(obj);
                            }
                        }
                        else
                        {
                            lstSAAIO_FACPAR = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return lstSAAIO_FACPAR;
        }


    }
}
