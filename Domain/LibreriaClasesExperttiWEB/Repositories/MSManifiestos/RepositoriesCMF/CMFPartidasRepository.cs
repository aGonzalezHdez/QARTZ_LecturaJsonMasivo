using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF
{
    public class CMFPartidasRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;


        int totalGuias = 0;
        int GuiasGuardadas = 0;
        public CMFPartidasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(cmfLineItems objCMFPartidas, int idcmf)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CASAEI_CMFPartidas";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@idCMF", SqlDbType.Int);
                @param.Value = idcmf;

                @param = cmd.Parameters.Add("@goodsItemNo", SqlDbType.Int);
                @param.Value = objCMFPartidas.goodsItemNo == null ? 0 : objCMFPartidas.goodsItemNo; 

                @param = cmd.Parameters.Add("@descOfGoods", SqlDbType.VarChar, 250);
                @param.Value = objCMFPartidas.descOfGoods == null ? "" : objCMFPartidas.descOfGoods;

                @param = cmd.Parameters.Add("@tariffQnty", SqlDbType.Money);
                @param.Value = objCMFPartidas.tariffQnty == null ? 0 : objCMFPartidas.tariffQnty;

                @param = cmd.Parameters.Add("@measureUnitQualifier", SqlDbType.VarChar, 3);
                @param.Value = objCMFPartidas.measureUnitQualifier == null ? "" : objCMFPartidas.measureUnitQualifier;

                @param = cmd.Parameters.Add("@netWeight", SqlDbType.Money);
                @param.Value = objCMFPartidas.netWeight==null ? 0 : objCMFPartidas.netWeight; 
                

                @param = cmd.Parameters.Add("@ctryMfctrerOrgn", SqlDbType.VarChar, 3);
                @param.Value = objCMFPartidas.ctryMfctrerOrgn == null ? "" : objCMFPartidas.ctryMfctrerOrgn;

                @param = cmd.Parameters.Add("@ctryOrgnCd", SqlDbType.VarChar, 3);
                @param.Value = objCMFPartidas.ctryOrgnCd == null ? "" : objCMFPartidas.ctryOrgnCd;

                @param = cmd.Parameters.Add("@invLineVal", SqlDbType.Money);
                @param.Value = objCMFPartidas.invLineVal == null ? 0 : objCMFPartidas.invLineVal;

                @param = cmd.Parameters.Add("@invCrncyCd", SqlDbType.VarChar, 3);
                @param.Value = objCMFPartidas.invCrncyCd == null ? "" : objCMFPartidas.invCrncyCd;

                @param = cmd.Parameters.Add("@tariffCdNo", SqlDbType.VarChar, 10);
                @param.Value = objCMFPartidas.tariffCdNo == null ? "" : objCMFPartidas.tariffCdNo;

                @param = cmd.Parameters.Add("@unitPrice", SqlDbType.Money);
                @param.Value = objCMFPartidas.unitPrice == null ? 0 : objCMFPartidas.unitPrice;
                

                @param = cmd.Parameters.Add("@invNo", SqlDbType.VarChar, 200);
                @param.Value = objCMFPartidas.invNo ==null ? "": objCMFPartidas.invNo;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

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
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public List<CMFPartidas> Cargar(int idCMF)
        {
            List<CMFPartidas> lstPartidas = new();

            try
            {
                using (SqlConnection con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CMFPartidas", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@idCMF", SqlDbType.Int, 4).Value = idCMF;

                    using SqlDataReader dr = cmd.ExecuteReader();
                      
                    if (dr.HasRows)
                    {

                        while (dr.Read())
                        {
                           
                            CMFPartidas objCMFPartidas = new();
                            objCMFPartidas.idPartidasCMF = Convert.ToInt32(dr["idPartidasCMF"]);
                            objCMFPartidas.idCMF = Convert.ToInt32(dr["idCMF"]);
                            objCMFPartidas.goodsItemNo = Convert.ToInt32(dr["goodsItemNo"]);
                            objCMFPartidas.descOfGoods = (dr["descOfGoods"]).ToString();
                            objCMFPartidas.tariffQnty = Convert.ToDouble(dr["tariffQnty"]);
                            objCMFPartidas.measureUnitQualifier = (dr["measureUnitQualifier"]).ToString();
                            objCMFPartidas.netWeight = Convert.ToDouble(dr["netWeight"]);
                            objCMFPartidas.ctryMfctrerOrgn = (dr["ctryMfctrerOrgn"]).ToString();
                            objCMFPartidas.ctryOrgnCd = (dr["ctryOrgnCd"]).ToString();
                            objCMFPartidas.invLineVal = Convert.ToDouble(dr["invLineVal"]);
                            objCMFPartidas.invCrncyCd = (dr["invCrncyCd"]).ToString();
                            objCMFPartidas.tariffCdNo = (dr["tariffCdNo"]).ToString();
                            objCMFPartidas.unitPrice = Convert.ToDouble(dr["unitPrice"]);
                            objCMFPartidas.idCategoriaSR = Convert.ToInt32(dr["idCategoriaSR"]);
                            objCMFPartidas.invNo = (dr["invNo"]).ToString();

                            lstPartidas.Add(objCMFPartidas);
                        }
                       
                    }
                    else
                    {
                        lstPartidas.Clear();
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }


            return lstPartidas;
        }

    }
}
