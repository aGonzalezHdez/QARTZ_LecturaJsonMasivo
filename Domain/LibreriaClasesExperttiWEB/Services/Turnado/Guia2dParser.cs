using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesTurnadoImpo;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Services.Turnado
{
    public class Guia2dParser
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public Guia2dParser(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public Guia2dData getData(string Guia2d)
        {
            var obj = new Guia2dData();
            string[] strArr;
            strArr = Guia2d.Split("|");


            obj.Guia = strArr[5].Substring(0, 12);

            if (strArr[9].Contains("/"))
            {
                obj.Numero = Convert.ToInt32(strArr[9].Split("/")[0]);
                obj.Bultos = Convert.ToInt32(strArr[9].Split("/")[1]);

                if (strArr[10].Contains("LB"))
                {
                    // convertimos LB a KG
                    obj.Peso = decimal.Parse(strArr[10].Replace("LB", ""));
                    obj.Peso = obj.Peso / (decimal)2.205d;
                    obj.Peso = Math.Round(obj.Peso, 3, MidpointRounding.AwayFromZero);
                }
                else
                {
                    obj.Peso = decimal.Parse(strArr[10].Replace("KG", ""));
                }
            }
            else if (strArr[10].Contains("/"))
            {
                obj.Numero = Convert.ToInt32(strArr[10].Split("/")[0]);
                obj.Bultos = Convert.ToInt32(strArr[10].Split("/")[1]);

                if (strArr[11].Contains("LB"))
                {
                    // convertimos LB a KG
                    obj.Peso = decimal.Parse(strArr[11].Replace("LB", ""));
                    obj.Peso = obj.Peso / (decimal)2.205d;
                    obj.Peso = Math.Round(obj.Peso, 3, MidpointRounding.AwayFromZero);
                }
                else
                {
                    obj.Peso = decimal.Parse(strArr[11].Replace("KG", ""));
                }
            }

            if (obj.Bultos == 0)
            {
                obj.Bultos = 1;
            }


            // Si el numero es mayor a 1 significa que es una guia baby.
            if (obj.Numero > 1)
            {
                try
                {
                    var result = Enumerable.Range(0, strArr.Count()).Where(i => strArr[i].Contains("28Z"));
                    obj.GuiaMaster = strArr[result.ElementAtOrDefault(0)].Substring(3, 12);
                }
                catch (Exception ex)
                {

                }

            }
            int itemindex = Array.IndexOf(strArr, "MX");
            try
            {
                obj.Valor = decimal.Parse(strArr[itemindex + 1]);
                obj.ClaveDeMoneda = strArr[itemindex + 2];
                obj.Descripcion = strArr[itemindex + 3];
            }
            catch (Exception ex)
            {
                obj.Valor = 1;
                obj.ClaveDeMoneda = strArr[itemindex + 1];
                obj.Descripcion = strArr[itemindex + 2];
            }

            var CtarcDePais = consultarDivisa(obj.ClaveDeMoneda);
            if (CtarcDePais is null)
            {
                obj.ExisteMoneda = false;
            }
            else
            {
                obj.ExisteMoneda = true;
            }


            return obj;

        }

        public CtarcDePais consultarDivisa(string Moneda)
        {
            var CtarcDePais = new CtarcDePais();
            SqlDataReader dataReader;
            string query = string.Empty;
            query += @"SELECT * FROM CASA.DBO.CTARC_DEPAIS WHERE mon_pai = @Moneda
                    AND GETDATE() BETWEEN (fec_dof+1) AND IIF(vig_hast IS NULL,GETDATE(),vig_hast)";

            using (var con = new SqlConnection(SConexion))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandType = CommandType.Text;
                    com.CommandText = query;
                    com.Parameters.AddWithValue("@Moneda", Moneda);

                    try
                    {
                        con.Open();
                        dataReader = com.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            dataReader.Read();

                            CtarcDePais.MON_PAI = dataReader["MON_PAI"].ToString();
                            CtarcDePais.EQU_DLLS = Convert.ToDouble(dataReader["EQU_DLLS"]);
                            CtarcDePais.CVE_PAI = dataReader["EQU_DLLS"].ToString();
                        }

                        else
                        {
                            CtarcDePais = default;
                        }

                        dataReader.Close();
                        con.Close();
                        con.Dispose();
                        com.Parameters.Clear();
                    }

                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message.ToString());
                    }

                    return CtarcDePais;
                }
            }
        }
    }
}
