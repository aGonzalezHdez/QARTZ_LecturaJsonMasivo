using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesJCJF;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Services.Prevalidador
{
    public class SolicitaSalidaService
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;


        public SolicitaSalidaService(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public void fSolicitarSalida(int idReferencia, int IdDatosdeEmpresa, WebApis objApi, Referencias objRefe)
        {
            var objBit = new BitacoraJCJF();
            var objBitD = new BitacoraJCJFRepository(_configuration);

            try
            {
                var objPedime = new SaaioPedime();
                var objSaaioPedimeD = new SaaioPedimeRepository(_configuration);
                objPedime = objSaaioPedimeD.Buscar(objRefe.NumeroDeReferencia);

                if (objPedime == null)
                {
                    throw new ArgumentException("No existe el pedimento");
                }

                System.Data.DataTable dtb;
                var iExp = new FuncionExcelSafranRepository(_configuration);

                dtb = iExp.CargaReporteJCJRPorPedimento(IdDatosdeEmpresa, objRefe.NumeroDeReferencia);

                foreach (DataRow item in dtb.Rows)
                {
                    try
                    {
                        var objSolicitaSalida = new SolicitaSalida();
                        objSolicitaSalida.pSalpedimento =
                            objPedime.FEC_ENTR.ToString().Substring(8, 2) +
                            objPedime.ADU_DESP.Substring(0, 2) +
                            objPedime.PAT_AGEN.Trim() +
                            objPedime.NUM_PEDI.Trim();

                        objSolicitaSalida.pSalpedimentocve = objPedime.CVE_PEDI.Trim();
                        objSolicitaSalida.pSaloperacion = objPedime.IMP_EXPO == "1" ? "IMP" : "EXP";
                        objSolicitaSalida.pSalidaspatente = objPedime.PAT_AGEN.Trim();

                        objSolicitaSalida.pSalMaster = item["Master"].ToString().Trim().Replace("-", "");

                        int LongituddeguiaI = item["House"].ToString().Trim().Length - 10;
                        if (LongituddeguiaI == 0)
                        {
                            LongituddeguiaI = 1;
                        }

                        int LongituddeguiaF = item["House"].ToString().Trim().Length;

                        objSolicitaSalida.pSalHouse = item["House"].ToString().Trim().Substring(LongituddeguiaI, LongituddeguiaF);
                        objSolicitaSalida.pSalPeso = Convert.ToDouble(item["Peso"]);
                        objSolicitaSalida.pSalbultos = Convert.ToInt32(item["Piezas"]);
                        objSolicitaSalida.pTransValorPedimento = (item["Valor Comercial"]).ToString();
                        objSolicitaSalida.pSalpfecha = objPedime.FEC_PAGO;
                        objSolicitaSalida.TipodePedimento = item["TipodePedimento"].ToString();
                        objSolicitaSalida.RFC = item["RFC"].ToString();
                        objSolicitaSalida.pSalpfechaModulacion = Convert.ToDateTime(item["FechayHoraDeModulacion"]);
                        objSolicitaSalida.pSalRemitente = item["Remitente"].ToString();
                        objSolicitaSalida.pSalRemitenteDir = item["RemitenteDir"].ToString();
                        objSolicitaSalida.pSalConsignatario = item["Consignatario"].ToString();
                        objSolicitaSalida.pSalConsignatarioDir = item["ConsignatarioDir"].ToString();
                        objSolicitaSalida.pSalDestino = item["Destino"].ToString();
                        objSolicitaSalida.pSalDescripcion = item["Descripcion"].ToString();

                        string vJson = Newtonsoft.Json.JsonConvert.SerializeObject(objSolicitaSalida);

                        var Api = new ApiJCJFRepository(_configuration);
                        Api.WSpostJCJF("PRJCJFSolicitaSalida", vJson);

                        objBit.GuiaHouse = item["House"].ToString().Trim();
                        objBit.Respuesta = Api.Respuesta;
                        objBit.Mensaje = Api.Msg.Trim();
                        objBit.Tipo = 2;
                        objBit.TipoPrevio = item["TipodePedimento"].ToString();
                        objBit.RFC = item["RFC"].ToString();
                        objBit.FechaModulacion = objSolicitaSalida.pSalpfechaModulacion;
                        objBitD.Insertar(objBit);
                    }
                    catch (Exception ex)
                    {
                        objBit.GuiaHouse = item["House"].ToString().Trim();
                        objBit.Respuesta = false;
                        objBit.Mensaje = ex.Message.Trim();
                        objBit.Tipo = 2;
                        objBit.TipoPrevio = "0";
                        objBit.FechaModulacion = DateTime.Now;
                        objBitD.Insertar(objBit);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message.Trim());
            }
        }

    }
}
