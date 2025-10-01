using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;


namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioFacParRepository : ISaaioFacParRepository
    {

        public string SConexion { get; set; }

        string ISaaioFacParRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IConfiguration _configuration;

        public SaaioFacParRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = configuration.GetConnectionString("dbCASAEI")!;
        }
        public SaaioFacpar Buscar(string NUM_REFE, int CONS_FACT, int CONS_PART)
        {
            SaaioFacpar objSAAIO_FACPAR = new (); 
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_SAAIO_FACPAR", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = CONS_PART;

                con.Open();

                using SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    return new SaaioFacpar
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
                        DESC_COVE = dr["DESC_COVE"].ToString(),
                        SUB_PART = dr["SUB_PART"].ToString(),
                        Ultimo = dr["Ultimo"].ToString(),
                        Primero = dr["Primero"].ToString(),
                        Siguiente = dr["Siguiente"].ToString(),
                        Anterior = dr["Anterior"].ToString()
                    };
                }           
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objSAAIO_FACPAR;
        }

        public int Insertar(SaaioFacpar lsaaio_facpar)
        {
            int id;
          
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_INSERT_SAAIO_FACPAR_NEW_2", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = lsaaio_facpar.NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = lsaaio_facpar.CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = lsaaio_facpar.CONS_PART;
                cmd.Parameters.Add("@NUM_PART", SqlDbType.VarChar, 50).Value = lsaaio_facpar.NUM_PART;
                cmd.Parameters.Add("@PAI_ORIG", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(lsaaio_facpar.PAI_ORIG)?(object)DBNull.Value: lsaaio_facpar.PAI_ORIG;
                cmd.Parameters.Add("@PAI_VEND", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(lsaaio_facpar.PAI_VEND)? (object)DBNull.Value: lsaaio_facpar.PAI_VEND;
                cmd.Parameters.Add("@FRACCION", SqlDbType.VarChar, 8).Value = string.IsNullOrEmpty(lsaaio_facpar.FRACCION)? (object)DBNull.Value: lsaaio_facpar.FRACCION;
                cmd.Parameters.Add("@CAS_TLCS", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_facpar.CAS_TLCS)? (object)DBNull.Value: lsaaio_facpar.CAS_TLCS;
                cmd.Parameters.Add("@COM_TLC", SqlDbType.VarChar, 35).Value = string.IsNullOrEmpty(lsaaio_facpar.COM_TLC)? (object)DBNull.Value : lsaaio_facpar.COM_TLC;
                cmd.Parameters.Add("@ADVAL", SqlDbType.Float, 18).Value = lsaaio_facpar.ADVAL== null ? (object)DBNull.Value: lsaaio_facpar.ADVAL;
                cmd.Parameters.Add("@PORC_IVA", SqlDbType.Float, 18).Value = lsaaio_facpar.PORC_IVA == null ? DBNull.Value: lsaaio_facpar.PORC_IVA;
                cmd.Parameters.Add("@MON_FACT", SqlDbType.Float, 18).Value = lsaaio_facpar.MON_FACT == null? DBNull.Value: lsaaio_facpar.MON_FACT;
                cmd.Parameters.Add("@TIP_MONE", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(lsaaio_facpar.TIP_MONE)? DBNull.Value: lsaaio_facpar.TIP_MONE;
                cmd.Parameters.Add("@CVE_VALO", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_facpar.CVE_VALO)? DBNull.Value: lsaaio_facpar.CVE_VALO;
                cmd.Parameters.Add("@CVE_VINC", SqlDbType.Char, 1).Value = string.IsNullOrEmpty(lsaaio_facpar.CVE_VINC)? DBNull.Value: lsaaio_facpar.CVE_VINC;
                cmd.Parameters.Add("@CAN_FACT", SqlDbType.Float).Value = lsaaio_facpar.CAN_FACT == null ? (object)DBNull.Value : lsaaio_facpar.CAN_FACT;                    
                cmd.Parameters.Add("@UNI_FACT", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_facpar.UNI_FACT)? DBNull.Value: lsaaio_facpar.UNI_FACT;
                cmd.Parameters.Add("@CAN_TARI", SqlDbType.Float, 18).Value = lsaaio_facpar.CAN_TARI== null? DBNull.Value: lsaaio_facpar.CAN_TARI;
                cmd.Parameters.Add("@UNI_TARI", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(lsaaio_facpar.UNI_TARI)? DBNull.Value: lsaaio_facpar.UNI_TARI;
                cmd.Parameters.Add("@VAL_AGRE", SqlDbType.Float, 18).Value = lsaaio_facpar.VAL_AGRE == null? DBNull.Value: lsaaio_facpar.VAL_AGRE;
                cmd.Parameters.Add("@VAL_UNIT", SqlDbType.Float, 18).Value = lsaaio_facpar.VAL_UNIT == null? DBNull.Value: lsaaio_facpar.VAL_UNIT;
                cmd.Parameters.Add("@NUM_PEDIDO", SqlDbType.VarChar, 15).Value = lsaaio_facpar.NUM_PEDIDO == null ? DBNull.Value: lsaaio_facpar.NUM_PEDIDO;
                cmd.Parameters.Add("@CANT_COVE", SqlDbType.Float, 18).Value = lsaaio_facpar.CANT_COVE == null? DBNull.Value: lsaaio_facpar.CANT_COVE;
                cmd.Parameters.Add("@UNI_COVE", SqlDbType.VarChar, 3).Value = lsaaio_facpar.UNI_COVE == null ? DBNull.Value: lsaaio_facpar.UNI_COVE;
                cmd.Parameters.Add("@DESC_COVE", SqlDbType.VarChar, 250).Value = lsaaio_facpar.DESC_COVE == null ? DBNull.Value: lsaaio_facpar.DESC_COVE;
                cmd.Parameters.Add("@SUB_PART", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(lsaaio_facpar.SUB_PART)? DBNull.Value: lsaaio_facpar.SUB_PART;
                cmd.Parameters.Add("@MAR_MERC", SqlDbType.VarChar, 80).Value = string.IsNullOrEmpty(lsaaio_facpar.MAR_MERC)? DBNull.Value: lsaaio_facpar.MAR_MERC;
                cmd.Parameters.Add("@SUB_MODE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_facpar.SUB_MODE)? DBNull.Value: lsaaio_facpar.SUB_MODE;
                cmd.Parameters.Add("@NUM_SERI", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(lsaaio_facpar.NUM_SERI)? DBNull.Value: lsaaio_facpar.NUM_SERI;
                cmd.Parameters.Add("@OBS_FRAC", SqlDbType.VarChar, 8000).Value = string.IsNullOrEmpty(lsaaio_facpar.OBS_FRAC)? DBNull.Value: lsaaio_facpar.OBS_FRAC;
                cmd.Parameters.Add("@codigoSAT", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(lsaaio_facpar.codigoSAT)? DBNull.Value: lsaaio_facpar.codigoSAT;
                cmd.Parameters.Add("@DescripcionSAT", SqlDbType.VarChar, 250).Value = string.IsNullOrEmpty(lsaaio_facpar.DescripcionSAT) ? DBNull.Value: lsaaio_facpar.DescripcionSAT;

                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                con.Open();

                cmd.ExecuteNonQuery();

                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                          
            }
            catch (Exception ex)
            {               
                throw new Exception(ex.Message.ToString() + " NET_INSERT_SAAIO_FACPAR_NEW_2");
            }          
            return id;
        }

        public int Modificar(SaaioFacpar lsaaio_facpar)
        {
            int id = 0;
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_UPDATE_SAAIO_FACPAR_NEW_2", con)
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
                 
                 cmd.Parameters.Add("@SUB_PART", SqlDbType.VarChar, 3).Value = lsaaio_facpar.SUB_PART;
                 SQL = SQL + "'" + lsaaio_facpar.SUB_PART + "',";
                 
                 cmd.Parameters.Add("@MAR_MERC", SqlDbType.VarChar, 80).Value = string.IsNullOrEmpty(lsaaio_facpar.MAR_MERC)? DBNull.Value: lsaaio_facpar.MAR_MERC;
                 cmd.Parameters.Add("@SUB_MODE", SqlDbType.VarChar, 50).Value = string.IsNullOrEmpty(lsaaio_facpar.SUB_MODE)? DBNull.Value: lsaaio_facpar.SUB_MODE;
                 cmd.Parameters.Add("@NUM_SERI", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(lsaaio_facpar.NUM_SERI)? DBNull.Value: lsaaio_facpar.NUM_SERI;
                 cmd.Parameters.Add("@OBS_FRAC", SqlDbType.VarChar, 8000).Value = string.IsNullOrEmpty(lsaaio_facpar.OBS_FRAC)? DBNull.Value: lsaaio_facpar.OBS_FRAC;
                 cmd.Parameters.Add("@codigoSAT", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(lsaaio_facpar.codigoSAT)? DBNull.Value: lsaaio_facpar.codigoSAT;
                 cmd.Parameters.Add("@DescripcionSAT", SqlDbType.VarChar, 250).Value = string.IsNullOrEmpty(lsaaio_facpar.DescripcionSAT)? DBNull.Value: lsaaio_facpar.DescripcionSAT;
                 cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                 
                 con.Open();
                 
                 cmd.ExecuteNonQuery();
                 
                 id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                 cmd.Parameters.Clear();
                 
            }
            catch (Exception ex)
            {   
                throw new Exception(ex.Message.ToString() + " NET_UPDATE_SAAIO_FACPAR_NEW");
            }            
            return id;
        }

        public List<SaaioFacpar>? Cargar(string NUM_REFE, int CONS_FACT)
        {
            List<SaaioFacpar>? lstSAAIO_FACPAR = new ();
           
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_SAAIO_FACPAR", con)
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
                        
                        lstSAAIO_FACPAR.Add(objSAAIO_FACPAR);
                    }
                }
                else
                    lstSAAIO_FACPAR.Clear();                  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return lstSAAIO_FACPAR;
        }

        public bool Borrar(string NUM_REFE, int CONS_FACT, int CONS_PART)
        {
            bool id;

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_DELETE_SAAIO_FACPAR", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                cmd.Parameters.Add("@CONS_FACT", SqlDbType.Int, 4).Value = CONS_FACT;
                cmd.Parameters.Add("@CONS_PART", SqlDbType.Int, 4).Value = CONS_PART;

                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                
                con.Open();
                
                cmd.ExecuteNonQuery();

                id = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);    
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }     
            return id;
        }
    }
}
