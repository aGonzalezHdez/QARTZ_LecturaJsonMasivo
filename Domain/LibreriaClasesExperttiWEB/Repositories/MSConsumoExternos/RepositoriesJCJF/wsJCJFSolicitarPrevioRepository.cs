using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;
using System.Collections.Immutable;
using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesJCJF
{
    public class wsJCJFSolicitarPrevioRepository
    {
        public string pMaster { get; set; }
        public string pHouse { get; set; }

        public string pTipoPrevio { get; set; }
        public bool Enviada = false;

        public IConfiguration _configuration;

        public wsJCJFSolicitarPrevioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> fSolicitarPrevioAsync(string GuiaHouse)
        {
            string Respuesta = string.Empty;
            BitacoraJCJF objBit = new BitacoraJCJF();
            BitacoraJCJFRepository objBitD = new BitacoraJCJFRepository(_configuration);
            try
            {
                List<CustomsAlerts> lstCA = new List<CustomsAlerts>();
                CargaManifiestosRepository objCA = new CargaManifiestosRepository(_configuration);
                lstCA = objCA.BuscarJCJF(GuiaHouse);

                if (lstCA.Count == 0)
                    throw new ArgumentException("La guía no esta manifestada, o no se selecciono para solicitar a JCJF");


                foreach (CustomsAlerts itemCA in lstCA)
                {
                 
                        wsJCJFSolicitarPrevioRepository objSolicitaPrevio = new wsJCJFSolicitarPrevioRepository(_configuration);
                        objSolicitaPrevio.pHouse = GuiaHouse;
                        objSolicitaPrevio.pMaster = itemCA.GuiaMaster.Trim();

                        if (itemCA.IdCategoria == 5)
                        {
                            objSolicitaPrevio.pTipoPrevio = "Global";
                        }
                        else
                        {
                            objSolicitaPrevio.pTipoPrevio = "Formal";
                        }

                        string vJson = Newtonsoft.Json.JsonConvert.SerializeObject(objSolicitaPrevio);

                        ApiJCJFRepository Api = new ApiJCJFRepository(_configuration);
                        await Api.WSpostJCJF("PRJCJFSolicitaPrevio", vJson);

                        objBit.GuiaHouse = GuiaHouse.Trim();
                        objBit.Respuesta = Api.Respuesta;
                        objBit.Mensaje = Api.Msg.Trim();
                        objBit.Tipo = 1;
                        objBit.TipoPrevio = objSolicitaPrevio.pTipoPrevio;
                        objBitD.Insertar(objBit);

                        if (objBit.Respuesta == false)
                            objBit.Mensaje = Api.Msg.Trim() + " " + "Guía House:" + itemCA.GuiaHouse + " Guía Master: " + itemCA.GuiaMaster;

                        Respuesta += " " + objBit.Mensaje + "\r\n";
                        
                    Enviada = true;
                }

            }

            catch (Exception ex)
            {
                objBit.GuiaHouse = GuiaHouse.Trim();
                objBit.Respuesta = false;
                objBit.Mensaje = ex.Message.Trim();
                objBit.TipoPrevio = "";
                objBitD.Insertar(objBit);
                Respuesta = objBit.Mensaje;
                Enviada = false;
                throw new ArgumentException(ex.Message.Trim());
            }

            return Respuesta;
        }


        public List <RespuestaMultipiezas> fVerificarMultipiezas(List<string> lstGuiaHouse  )
        {
            List<RespuestaMultipiezas> multipiezas = new();
            try
            {
                foreach (string GuiaHouse in lstGuiaHouse)
                {
                    List<CustomsAlerts> lstCA = new List<CustomsAlerts>();
                    CargaManifiestosRepository objCA = new CargaManifiestosRepository(_configuration);
                    CustomsAlertsRepository objCAD = new CustomsAlertsRepository(_configuration);

                    lstCA = objCA.Buscar(GuiaHouse);

                    if (lstCA.Count > 1) 
                    {
                        foreach (CustomsAlerts alert in lstCA)
                        {
                            RespuestaMultipiezas obj = new();
                            obj.IdCustomAlert = alert.IdCustomAlert;
                            obj.GuiaHouse = alert.GuiaHouse;
                            obj.GuiaMaster = alert.GuiaMaster;
                            obj.PrevioJCJF = alert.PrevioJCJF;

                            multipiezas.Add(obj);
                        }
                                 
                        
                    }
                    else
                    {
                        if (lstCA.Count > 0)
                        {
                             foreach (CustomsAlerts alert in lstCA)
                            {
                                objCAD.modificarPreviosJCJF(alert.IdCustomAlert, true);
                            }
                        }
                      
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
           
            return multipiezas;
        }
    }


}
