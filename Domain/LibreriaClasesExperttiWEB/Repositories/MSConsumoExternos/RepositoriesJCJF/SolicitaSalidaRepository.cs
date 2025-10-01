using DocumentFormat.OpenXml.Drawing.Diagrams;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesJCJF;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.API_MSGeneraParaExternos.JCJF;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF
{
    public class SolicitaSalidaRepository : ISolicitaSalidaRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public SolicitaSalidaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public SolicitaSalidaParaJCJF fSolicitarSalida(GuiaHouseRequest guiaHouse)
        {
            BitacoraJCJF bitacoraJCJF = new BitacoraJCJF();
            BitacoraJCJFRepository bitacoraJCJFRepository = new(_configuration);
            SolicitaSalidaParaJCJF objSolicitaSalida = new SolicitaSalidaParaJCJF();

            try
            {
                WECEstatus wECEstatus = new WECEstatus();
                WECEstatusRepository wECEstatusRepository = new WECEstatusRepository(_configuration);
                wECEstatus = wECEstatusRepository.Buscar(guiaHouse);

                if (wECEstatus != null)
                {
                    objSolicitaSalida.EstatusGuia = wECEstatus.Tipo;
                }


                SaaioGuias saaioGuias = new SaaioGuias();
                SaaioGuiasRepository saaioGuiasRepository = new(_configuration);
                saaioGuias = saaioGuiasRepository.BuscarGuiaUltima(guiaHouse.GuiaHouse, "H");

                if (saaioGuias == null)
                {
                    objSolicitaSalida.pSalHouse = guiaHouse.GuiaHouse;
                    objSolicitaSalida.pSalMaster = guiaHouse.GuiaMaster;

                }
                else
                {
                    SaaioPedime saaioPedime = new SaaioPedime();
                    SaaioPedimeRepository saaioPedimeRepository = new(_configuration);
                    saaioPedime = saaioPedimeRepository.Buscar(saaioGuias.NUM_REFE);

                    if (string.IsNullOrEmpty(saaioPedime.FIR_ELEC))
                    {
                        throw new Exception("La guía aún no ha sido pagada");
                    }

                    DataTable dtb = new DataTable();

                    dtb = CargaReporteJCJRPorGuia(1, saaioPedime.NUM_REFE, guiaHouse);

                    foreach (DataRow item in dtb.Rows)
                    {
                        try
                        {
                            string anio = saaioPedime.FEC_ENTR.ToString().Substring(8, 2);
                            string aduana = saaioPedime.ADU_DESP.Substring(0, 2);
                            objSolicitaSalida.pSalpedimento = anio + aduana + saaioPedime.PAT_AGEN.ToString() + saaioPedime.NUM_PEDI.ToString();
                            objSolicitaSalida.pSalpedimentocve = saaioPedime.CVE_PEDI.ToString();
                            objSolicitaSalida.pSaloperacion = saaioPedime.IMP_EXPO == "1" ? "IMP" : "EXP";
                            objSolicitaSalida.pSalidaspatente = saaioPedime.PAT_AGEN.ToString();
                            objSolicitaSalida.pSalMaster = item["Master"].ToString().Replace("-", "");

                            int LongituddeguiaI = item["House"].ToString().Length - 10;


                            int LongituddeguiaF = item["House"].ToString().Length;

                            objSolicitaSalida.pSalHouse = item["House"].ToString().Substring(LongituddeguiaI, LongituddeguiaF);

                            objSolicitaSalida.pSalPeso = Convert.ToDouble(item["Peso"]);
                            objSolicitaSalida.pSalbultos = Convert.ToInt32(item["Piezas"]);
                            objSolicitaSalida.pTransValorPedimento = (item["Valor Comercial"]).ToString();
                            objSolicitaSalida.pSalpfecha = saaioPedime.FEC_PAGO;
                            objSolicitaSalida.TipodePedimento = (item["TipodePedimento"]).ToString();
                            objSolicitaSalida.RFC = item["RFC"].ToString();
                            objSolicitaSalida.pSalpfechaModulacion = Convert.ToDateTime(item["FechayHoraDeModulacion"]);
                            objSolicitaSalida.pSalRemitente = item["Remitente"].ToString();
                            objSolicitaSalida.pSalRemitenteDir = item["RemitenteDir"].ToString();
                            objSolicitaSalida.pSalConsignatario = item["Consignatario"].ToString();
                            objSolicitaSalida.pSalConsignatarioDir = item["ConsignatarioDir"].ToString();
                            objSolicitaSalida.pSalDestino = item["Destino"].ToString();
                            objSolicitaSalida.pSalDescripcion = item["Descripcion"].ToString();



                            bitacoraJCJF.GuiaHouse = objSolicitaSalida.pSalHouse;
                            bitacoraJCJF.Respuesta = true;
                            bitacoraJCJF.Mensaje = "JCJF.- Solicita guia en servio EI";
                            bitacoraJCJF.Tipo = 3;
                            bitacoraJCJF.TipoPrevio = "";
                            bitacoraJCJF.FechaModulacion = Convert.ToDateTime(item["FechayHoraDeModulacion"]);

                            bitacoraJCJFRepository.Insertar(bitacoraJCJF);

                        }
                        catch (Exception ex)
                        {
                            bitacoraJCJF.GuiaHouse = guiaHouse.GuiaHouse;
                            bitacoraJCJF.Respuesta = false;
                            bitacoraJCJF.Mensaje = "JCJF.- Solicita guia en servio EI " + ex.Message;
                            bitacoraJCJF.Tipo = 3;
                            bitacoraJCJF.TipoPrevio = "";
                            bitacoraJCJF.FechaModulacion = Convert.ToDateTime(item["FechayHoraDeModulacion"]);

                            bitacoraJCJFRepository.Insertar(bitacoraJCJF);
                            throw new Exception(ex.Message);
                        }
                    }



                }

                if (saaioGuias == null && wECEstatus == null)
                {
                    throw new Exception("La guía house no existe");
                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return objSolicitaSalida;
        }


        public DataTable CargaReporteJCJRPorPedimento(int idDatosDeEmpresa, string numerodeReferencia)
        {
            DataTable dtb = new DataTable();
            SqlParameter param;

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LAYOUT_SALIDAS_JC_JR_POR_REFERENCIA";

                    // @IDE INT 
                    param = dap.SelectCommand.Parameters.Add("@IDE", SqlDbType.Int, 4);
                    param.Value = idDatosDeEmpresa;

                    // , @NUM_REFE VARCHAR(14)
                    param = dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                    param.Value = numerodeReferencia;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();

                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();

                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_LAYOUT_SALIDAS_JC_JR_POR_REFERENCIA");
                }
            }
            return dtb;
        }

        /// <summary>
        /// Buscar los valores que solicita JCJF
        /// </summary>
        /// <param name="idDatosDeEmpresa"></param>
        /// <param name="numerodeReferencia"></param>
        /// <param name="guiaHouse"></param>
        /// <param name="guiaMaster"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <remarks>Vanessa Báez|19/08/2025|STI-0000173</remarks>
        public DataTable CargaReporteJCJRPorGuia(int idDatosDeEmpresa, string numerodeReferencia, GuiaHouseRequest guiaHouse)
        {
            DataTable dtb = new DataTable();
            SqlParameter param;

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter();

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LAYOUT_SALIDAS_JC_JR_POR_REFERENCIA_GUIA";

                    // @IDE INT 
                    param = dap.SelectCommand.Parameters.Add("@IDE", SqlDbType.Int, 4);
                    param.Value = idDatosDeEmpresa;

                    // , @NUM_REFE VARCHAR(14)
                    param = dap.SelectCommand.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                    param.Value = numerodeReferencia;

                    param = dap.SelectCommand.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
                    param.Value = guiaHouse.GuiaHouse;

                    param = dap.SelectCommand.Parameters.Add("@GuiaMaster", SqlDbType.VarChar, 25);
                    param.Value = guiaHouse.GuiaMaster;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.Fill(dtb);
                    cn.Close();

                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();

                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_LAYOUT_SALIDAS_JC_JR_POR_REFERENCIA");
                }
            }
            return dtb;
        }

    }
}
