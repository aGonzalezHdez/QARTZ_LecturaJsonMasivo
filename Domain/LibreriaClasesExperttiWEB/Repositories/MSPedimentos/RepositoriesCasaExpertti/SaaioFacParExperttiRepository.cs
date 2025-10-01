using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasaExpertti.Interfaces;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasaExpertti
{ 
    public class SaaioFacParExperttiRepository : ISaaioFacParRepository
    {
        public string SConexion { get; set; }
        string ISaaioFacParRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public SaaioFacParExperttiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public SaaioFacpar Buscar(string NUM_REFE, int CONS_FACT, int CONS_PART)
        {
            SaaioFacpar objSAAIO_FACPAR = new();

            try
            {
                using SqlConnection con = new(SConexion);
                using (SqlCommand cmd = new("NET_SEARCH_SAAIO_FACPAR_EXPERTTI", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                    cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                    cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = CONS_PART;

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
                            objSAAIO_FACPAR.COM_TLC = dr["COM_TLC"].ToString();
                            objSAAIO_FACPAR.ADVAL = Convert.ToDouble(dr["ADVAL"]);
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
                            objSAAIO_FACPAR.Validada = Convert.ToBoolean(dr["Validada"]);
                        }
                        else
                            objSAAIO_FACPAR = null/* TODO Change to default(_) if this is not a reference type */;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objSAAIO_FACPAR;
        }

        public int TraelElMaxConst_PartDelCasa(string NUM_REFE, int CONS_FACT)
        {
            int CONS_PART = 0;

            try
            {
                using SqlConnection con = new(SConexion);
                using (SqlCommand cmd = new("NET_EXTRAE_MAX_CONS_FACPAR_EXPERTTI", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                    cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                    cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Direction = ParameterDirection.Output; 
                    
                    cmd.ExecuteNonQuery();

                    CONS_PART = Convert.ToInt32(cmd.Parameters["@CONS_PART"].Value) + 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return CONS_PART;
        }

        public int Insertar(SaaioFacpar lsaaio_facpar)
        {
            int id;
           
            try
            {
                using SqlConnection con = new(SConexion);
                using (SqlCommand cmd = new("NET_INSERT_SAAIO_FACPAR_EXPERTTI_NICO_2", con))
                {  
                    con.Open(); 
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = lsaaio_facpar.NUM_REFE;
                    cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = lsaaio_facpar.CONS_FACT;
                    cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = lsaaio_facpar.CONS_PART;
                    cmd.Parameters.Add("@NUM_PART", SqlDbType.VarChar, 50).Value = lsaaio_facpar.NUM_PART;
                    cmd.Parameters.Add("@PAI_ORIG", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(lsaaio_facpar.PAI_ORIG) ? DBNull.Value : lsaaio_facpar.PAI_ORIG;
                    cmd.Parameters.Add("@PAI_VEND", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(lsaaio_facpar.PAI_VEND) ? DBNull.Value : lsaaio_facpar.PAI_VEND;
                    cmd.Parameters.Add("@FRACCION", SqlDbType.VarChar, 8).Value = string.IsNullOrEmpty(lsaaio_facpar.FRACCION) ? DBNull.Value : lsaaio_facpar.FRACCION;
                    cmd.Parameters.Add("@CAS_TLCS", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_facpar.CAS_TLCS) ? DBNull.Value : lsaaio_facpar.CAS_TLCS;
                    cmd.Parameters.Add("@COM_TLC", SqlDbType.VarChar, 35).Value = string.IsNullOrEmpty(lsaaio_facpar.COM_TLC) ? DBNull.Value : lsaaio_facpar.COM_TLC;
                    cmd.Parameters.Add("@ADVAL", SqlDbType.Float, 18).Value = lsaaio_facpar.ADVAL == 0.0 ? DBNull.Value : lsaaio_facpar.ADVAL;
                    cmd.Parameters.Add("@PORC_IVA", SqlDbType.Float, 18).Value = lsaaio_facpar.PORC_IVA == 0.0 ? DBNull.Value : lsaaio_facpar.PORC_IVA;
                    cmd.Parameters.Add("@MON_FACT", SqlDbType.Float, 18).Value = lsaaio_facpar.MON_FACT == 0.0 ? DBNull.Value : lsaaio_facpar.MON_FACT;
                    cmd.Parameters.Add("@TIP_MONE", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(lsaaio_facpar.TIP_MONE) ? DBNull.Value : lsaaio_facpar.TIP_MONE;
                    cmd.Parameters.Add("@CVE_VALO", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_facpar.CVE_VALO) ? DBNull.Value : lsaaio_facpar.CVE_VALO;
                    cmd.Parameters.Add("@CVE_VINC", SqlDbType.Char, 1).Value = string.IsNullOrEmpty(lsaaio_facpar.CVE_VINC) ? DBNull.Value : lsaaio_facpar.CVE_VINC;
                    cmd.Parameters.Add("@CAN_FACT", SqlDbType.Float, 18).Value = lsaaio_facpar.CAN_FACT == 0.0 ? DBNull.Value : lsaaio_facpar.CAN_FACT;
                    cmd.Parameters.Add("@UNI_FACT", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_facpar.UNI_FACT) ? DBNull.Value : lsaaio_facpar.UNI_FACT;
                    cmd.Parameters.Add("@CAN_TARI", SqlDbType.Float, 18).Value = lsaaio_facpar.CAN_TARI == 0.0 ? DBNull.Value : lsaaio_facpar.CAN_TARI;
                    cmd.Parameters.Add("@UNI_TARI", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_facpar.UNI_TARI) ? DBNull.Value : lsaaio_facpar.UNI_TARI;
                    cmd.Parameters.Add("@VAL_AGRE", SqlDbType.Float, 18).Value = lsaaio_facpar.VAL_AGRE == 0.0 ? DBNull.Value : lsaaio_facpar.VAL_AGRE;
                    cmd.Parameters.Add("@VAL_UNIT", SqlDbType.Float, 18).Value = lsaaio_facpar.VAL_UNIT == 0.0 ? DBNull.Value : lsaaio_facpar.VAL_UNIT;
                    cmd.Parameters.Add("@NUM_PEDIDO", SqlDbType.VarChar, 15).Value = string.IsNullOrEmpty(lsaaio_facpar.NUM_PEDIDO) ? DBNull.Value : lsaaio_facpar.NUM_PEDIDO;
                    cmd.Parameters.Add("@CANT_COVE", SqlDbType.Float, 18).Value = lsaaio_facpar.CANT_COVE == 0.0 ? DBNull.Value : lsaaio_facpar.CANT_COVE;
                    cmd.Parameters.Add("@UNI_COVE", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(lsaaio_facpar.UNI_COVE) ? DBNull.Value : lsaaio_facpar.UNI_COVE;
                    cmd.Parameters.Add("@DESC_COVE", SqlDbType.VarChar, 250).Value = string.IsNullOrEmpty(lsaaio_facpar.DESC_COVE) ? DBNull.Value : lsaaio_facpar.DESC_COVE;
                    cmd.Parameters.Add("@Validada", SqlDbType.Bit).Value = lsaaio_facpar.Validada ? false : lsaaio_facpar.Validada;
                    cmd.Parameters.Add("@PES_UNIT", SqlDbType.Float, 18).Value = lsaaio_facpar.PES_UNIT == 0.0 ? DBNull.Value : lsaaio_facpar.PES_UNIT;
                    cmd.Parameters.Add("@SUB_PART", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_facpar.SUB_PART) ? DBNull.Value : lsaaio_facpar.SUB_PART;
                    cmd.Parameters.Add("@MAR_MERC", SqlDbType.VarChar, 80).Value = string.IsNullOrEmpty(lsaaio_facpar.MAR_MERC) ? DBNull.Value : lsaaio_facpar.MAR_MERC;
                    cmd.Parameters.Add("@SUB_MODE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_facpar.SUB_MODE) ? DBNull.Value : lsaaio_facpar.SUB_MODE;
                    cmd.Parameters.Add("@NUM_SERI", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(lsaaio_facpar.NUM_SERI) ? DBNull.Value : lsaaio_facpar.NUM_SERI;
                    cmd.Parameters.Add("@OBS_FRAC", SqlDbType.VarChar, 8000).Value = string.IsNullOrEmpty(lsaaio_facpar.OBS_FRAC) ? DBNull.Value : lsaaio_facpar.OBS_FRAC;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);                    
                }  
            }
            catch (Exception ex)
            {               
                throw new Exception(ex.Message.ToString() + " NET_INSERT_SAAIO_FACPAR_EXPERTTI_NICO_2");
            }           
            return id;
        }

        public int Modificar(SaaioFacpar lsaaio_facpar)
        {
            int id;
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_UPDATE_SAAIO_FACPAR_EXPERTTI_NICO_2", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                string SQL = "NET_UPDATE_SAAIO_FACPAR ";

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = lsaaio_facpar.NUM_REFE;
                SQL = SQL + "'" + lsaaio_facpar.NUM_REFE + "',";

                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = lsaaio_facpar.CONS_FACT;
                SQL = SQL + "'" + lsaaio_facpar.CONS_FACT + "',";

                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = lsaaio_facpar.CONS_PART;
                SQL = SQL + "'" + lsaaio_facpar.CONS_PART + "',";
                                    
                cmd.Parameters.Add("@NUM_PART", SqlDbType.VarChar, 50).Value = lsaaio_facpar.NUM_PART;
                SQL = SQL + "'" + lsaaio_facpar.NUM_PART + "',";

                cmd.Parameters.Add("@PAI_ORIG", SqlDbType.VarChar, 3).Value = lsaaio_facpar.PAI_ORIG;
                SQL = SQL + "'" + lsaaio_facpar.PAI_ORIG + "',";

                cmd.Parameters.Add("@PAI_VEND", SqlDbType.VarChar, 3).Value = lsaaio_facpar.PAI_VEND;
                SQL = SQL + "'" + lsaaio_facpar.PAI_VEND + "',";

                cmd.Parameters.Add("@FRACCION", SqlDbType.VarChar, 8).Value = lsaaio_facpar.FRACCION;
                SQL = SQL + "'" + lsaaio_facpar.FRACCION + "',";

                cmd.Parameters.Add("@CAS_TLCS", SqlDbType.VarChar, 2).Value = lsaaio_facpar.CAS_TLCS;
                SQL = SQL + "'" + lsaaio_facpar.CAS_TLCS + "',";

                cmd.Parameters.Add("@COM_TLC", SqlDbType.VarChar, 35).Value = lsaaio_facpar.COM_TLC;
                SQL = SQL + "'" + lsaaio_facpar.COM_TLC + "',";

                cmd.Parameters.Add("@ADVAL", SqlDbType.Float, 18).Value = lsaaio_facpar.ADVAL;
                SQL = SQL + lsaaio_facpar.ADVAL + ",";
 
                cmd.Parameters.Add("@PORC_IVA", SqlDbType.Float, 18).Value = lsaaio_facpar.PORC_IVA;
                SQL = SQL + lsaaio_facpar.PORC_IVA + ",";

                cmd.Parameters.Add("@MON_FACT", SqlDbType.Float, 18).Value = lsaaio_facpar.MON_FACT;
                SQL = SQL + lsaaio_facpar.MON_FACT + ",";

                cmd.Parameters.Add("@TIP_MONE", SqlDbType.VarChar, 3).Value = lsaaio_facpar.TIP_MONE;
                SQL = SQL + "'" + lsaaio_facpar.TIP_MONE + "',";

                cmd.Parameters.Add("@CVE_VALO", SqlDbType.VarChar, 2).Value = lsaaio_facpar.CVE_VALO;
                SQL = SQL + "'" + lsaaio_facpar.CVE_VALO + "',";

                cmd.Parameters.Add("@CVE_VINC", SqlDbType.Char, 1).Value = lsaaio_facpar.CVE_VINC;
                SQL = SQL + "'" + lsaaio_facpar.CVE_VINC + "',";

                cmd.Parameters.Add("@CAN_FACT", SqlDbType.Float, 18).Value = lsaaio_facpar.CAN_FACT;
                SQL = SQL + lsaaio_facpar.CAN_FACT + ",";

                cmd.Parameters.Add("@UNI_FACT", SqlDbType.VarChar, 2).Value = lsaaio_facpar.UNI_FACT;
                SQL = SQL + "'" + lsaaio_facpar.UNI_FACT + "',";

                cmd.Parameters.Add("@CAN_TARI", SqlDbType.Float, 18).Value = lsaaio_facpar.CAN_TARI;
                SQL = SQL + lsaaio_facpar.CAN_TARI + ",";

                cmd.Parameters.Add("@UNI_TARI", SqlDbType.VarChar, 2).Value = lsaaio_facpar.UNI_TARI;
                SQL = SQL + "'" + lsaaio_facpar.UNI_TARI + "',";

                cmd.Parameters.Add("@VAL_AGRE", SqlDbType.Float, 18).Value = lsaaio_facpar.VAL_AGRE;
                SQL = SQL + lsaaio_facpar.VAL_AGRE + ",";
                 
                cmd.Parameters.Add("@VAL_UNIT", SqlDbType.Float, 18).Value = lsaaio_facpar.VAL_UNIT;
                SQL = SQL + lsaaio_facpar.VAL_UNIT + ",";

                cmd.Parameters.Add("@NUM_PEDIDO", SqlDbType.VarChar, 15).Value = lsaaio_facpar.NUM_PEDIDO;
                SQL = SQL + "'" + lsaaio_facpar.NUM_PEDIDO + "',";

                cmd.Parameters.Add("@CANT_COVE", SqlDbType.Float, 18).Value = lsaaio_facpar.CANT_COVE;
                SQL = SQL + lsaaio_facpar.CANT_COVE + ",";
 
                cmd.Parameters.Add("@UNI_COVE", SqlDbType.VarChar, 3).Value = lsaaio_facpar.UNI_COVE;
                SQL = SQL + "'" + lsaaio_facpar.UNI_COVE + "',";

                cmd.Parameters.Add("@DESC_COVE", SqlDbType.VarChar, 250).Value = lsaaio_facpar.DES_MERC;
                SQL = SQL + "'" + lsaaio_facpar.DES_MERC + "',";

                cmd.Parameters.Add("@SUB_PART", SqlDbType.VarChar, 2).Value = lsaaio_facpar.SUB_PART;
                cmd.Parameters.Add("@Validada", SqlDbType.Bit).Value = lsaaio_facpar.Validada ? false : lsaaio_facpar.Validada;
                cmd.Parameters.Add("@PES_UNIT", SqlDbType.Float, 18).Value = lsaaio_facpar.PES_UNIT == 0.0 ? DBNull.Value : lsaaio_facpar.PES_UNIT;
                cmd.Parameters.Add("@MAR_MERC", SqlDbType.VarChar, 80).Value = string.IsNullOrEmpty(lsaaio_facpar.MAR_MERC) ? DBNull.Value : lsaaio_facpar.MAR_MERC;
                cmd.Parameters.Add("@SUB_MODE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_facpar.SUB_MODE) ? DBNull.Value : lsaaio_facpar.SUB_MODE;
                cmd.Parameters.Add("@NUM_SERI", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(lsaaio_facpar.NUM_SERI) ? DBNull.Value : lsaaio_facpar.NUM_SERI;
                cmd.Parameters.Add("@OBS_FRAC", SqlDbType.VarChar, 8000).Value = string.IsNullOrEmpty(lsaaio_facpar.OBS_FRAC) ? DBNull.Value : lsaaio_facpar.OBS_FRAC;

                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();

                cmd.ExecuteNonQuery();
                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);              
                     
            }
            catch (Exception ex)
            {              
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_SAAIO_FACPAR_EXPERTTI_NEW");
            }            
            return id;
        }

        public List<SaaioFacpar> Cargar(string NUM_REFE, int CONS_FACT)
        {
            List<SaaioFacpar> lstSAAIO_FACPAR = new();                  
            try
            {

                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_SAAIO_FACPAR_EXPERTTI", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                
                con.Open();

                using SqlDataReader dr = cmd.ExecuteReader();
                    
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaaioFacpar objSAAIO_FACPAR = new();

                        objSAAIO_FACPAR.NUM_REFE = dr["NUM_REFE"].ToString();
                        objSAAIO_FACPAR.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                        objSAAIO_FACPAR.CONS_PART = Convert.ToInt32(dr["CONS_PART"]);
                        objSAAIO_FACPAR.NUM_PART = dr["NUM_PART"].ToString();
                        objSAAIO_FACPAR.PAI_ORIG = dr["PAI_ORIG"].ToString();
                        objSAAIO_FACPAR.PAI_VEND = dr["PAI_VEND"].ToString();
                        objSAAIO_FACPAR.FRACCION = dr["FRACCION"].ToString();
                        objSAAIO_FACPAR.CAS_TLCS = dr["CAS_TLCS"].ToString();
                        objSAAIO_FACPAR.COM_TLC = dr["COM_TLC"].ToString();
                        objSAAIO_FACPAR.ADVAL = Convert.ToDouble(dr["ADVAL"]);
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
                        objSAAIO_FACPAR.Validada = Convert.ToBoolean(dr["Validada"]);

                        lstSAAIO_FACPAR.Add(objSAAIO_FACPAR);
                    }
                }
                else
                    lstSAAIO_FACPAR = null;                    
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return lstSAAIO_FACPAR;
        }

        public List<SaaioFacpar> CargarTodas(string NUM_REFE)
        {
            List<SaaioFacpar> lstSAAIO_FACPAR = new();  
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_SAAIO_FACPAR_ALL_EXPERTTI", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SaaioFacpar objSAAIO_FACPAR = new();
                        
                        objSAAIO_FACPAR.NUM_REFE = dr["NUM_REFE"].ToString();
                        objSAAIO_FACPAR.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                        objSAAIO_FACPAR.CONS_PART = Convert.ToInt32(dr["CONS_PART"]);
                        objSAAIO_FACPAR.NUM_PART = dr["NUM_PART"].ToString();
                        objSAAIO_FACPAR.PAI_ORIG = dr["PAI_ORIG"].ToString();
                        objSAAIO_FACPAR.PAI_VEND = dr["PAI_VEND"].ToString();
                        objSAAIO_FACPAR.FRACCION = dr["FRACCION"].ToString();
                        objSAAIO_FACPAR.CAS_TLCS = dr["CAS_TLCS"].ToString();
                        objSAAIO_FACPAR.COM_TLC = dr["COM_TLC"].ToString();
                        objSAAIO_FACPAR.ADVAL = Convert.ToDouble(dr["ADVAL"]);
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
                        objSAAIO_FACPAR.Validada = Convert.ToBoolean(dr["Validada"]);
                    
                        lstSAAIO_FACPAR.Add(objSAAIO_FACPAR);
                    }
                }
                else
                    lstSAAIO_FACPAR = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstSAAIO_FACPAR;
        }

        public bool Borrar(string NUM_REFE, int CONS_FACT, int CONS_PART)
        {
            bool Borrar;

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_DELETE_SAAIO_FACPAR_EXPERTTI", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = CONS_PART;

                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();

                cmd.ExecuteNonQuery();

                Borrar = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value); 
            }
            catch (Exception ex)
            {               
                throw new Exception(ex.Message.ToString());
            }           
            return Borrar;
        }

        public decimal ImporteMaximoDeFactura(string NUM_REFE, int CONS_FACT, int CONS_PART)
        {            
            decimal MyImporte = 0;

            try
            {
                using SqlConnection con = new(SConexion);
                using (SqlCommand cmd = new("NET_SUMA_IMPORTE_DE_PARTIDAS_DE_FACTURA_EXPERTTI", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                    cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                    cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = CONS_PART;

                    cmd.Parameters.Add("@MyImporte", SqlDbType.Decimal, 18).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    MyImporte = Convert.ToDecimal(cmd.Parameters["@MyImporte"].Value);
                }    
            }
            catch (Exception ex)
            {             
                throw new Exception(ex.Message.ToString());
            }           
            return MyImporte;
        }

        public int Modificar(string NUM_REFE, string CONS_FACT, string TIP_MONE)
        {
            int id;

            try
            {
                using SqlConnection con = new(SConexion);
                using (SqlCommand cmd = new("NET_UPDATE_MONEDA_EN_SAAIO_FACPAR_EXPERTTI", con))
                {
                    con.Open();                   
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                    cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                    cmd.Parameters.Add("@TIP_MONE", SqlDbType.VarChar, 3).Value = TIP_MONE;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {              
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_MONEDA_EN_SAAIO_FACPAR_EXPERTTI");
            }            
            return id;
        }

        public DataTable CargarPartidas(string NUM_REFE, int CONS_FACT)
        {
            DataTable dtb = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                {
                    con.Open();
                    SqlDataAdapter dap = new();


                    dap.SelectCommand = new();
                    dap.SelectCommand.Connection = con;
                    dap.SelectCommand.CommandText = "NET_LOAD_SAAIO_FACPAR_EXPERTTI_NEW";

                    dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                    dap.SelectCommand.Parameters.Add("@CONS_FACT", SqlDbType.Int).Value = CONS_FACT;

                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;

                    dap.Fill(dtb);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dtb;
        }


    }
}
