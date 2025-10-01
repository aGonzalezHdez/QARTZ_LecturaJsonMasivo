using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.Conagtadu;
using LibreriaClasesAPIExpertti.Entities.EntitiesGlobal;
using LibreriaClasesAPIExpertti.Entities.EntitiesJsonDHL;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSGlobal;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL
{
    /// <summary>
    /// Genera Json ConAgtAdus DHL 
    /// </summary>
    public class APIGenerarConAgtAdu : IAPIGenerarConAgtAdu
    {

        public IConfiguration _configuration;
        public string gtw = string.Empty;
        public int idReferencia;
        public APIGenerarConAgtAdu(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string JsonConagtaduGuia(string GuiaHouse, int idReferencia)
        {
            string vJson = string.Empty;
            int IDDatosDeEmpresa = 1;
            try
            {
                ConsolAnexos objCA = new();
                ConsolAnexosRepository objCAD = new(_configuration);
                objCA = objCAD.BuscarGuiayIdReferencia(GuiaHouse, idReferencia, IDDatosDeEmpresa);

                if (objCA == null)
                    throw new ArgumentException("No existe la guía solicitado en consol Anexos");

                Referencias objRefe = new();
                ReferenciasRepository objRefeD = new(_configuration);
                objRefe = objRefeD.Buscar(objCA.IdReferencia, 1);
                if (objRefe == null)
                {
                    throw new ArgumentException("No existe id de la referencia");
                }
                idReferencia = objRefe.IDReferencia;

                CatalogoDeOficinas oficinas = new();
                CatalogoDeOficinasRepository oficinasD = new(_configuration);
                oficinas = oficinasD.Buscar(objRefe.IdOficina);
                if (oficinas == null)
                {
                    throw new ArgumentException("No existe id de la referencia");
                }
                gtw = oficinas.GTWFac;

                List<jsonConagtadu> lst = new List<jsonConagtadu>();
                jsonConagtadu obj = new jsonConagtadu();

                ConsolBloques objBloque = new ConsolBloques();
                ConsolBloquesRepository objBloqueD = new(_configuration);
                objBloque = objBloqueD.Buscar(objCA.IdBloque);
                if (objBloque == null)
                    throw new ArgumentException("El bloque no existe");

                CatalogodeMaster objMaster = new CatalogodeMaster();
                CatalogodeMasterRepository objMasterD = new(_configuration);
                objMaster = objMasterD.Buscar(objBloque.IDMasterConsol);
                if (objMaster == null)
                    throw new ArgumentException("La guía master no fue registrada en expertti");

                SaaioPedime objPedime = new SaaioPedime();
                SaaioPedimeRepository objPedimeD = new(_configuration);
                objPedime = objPedimeD.Buscar(objRefe.NumeroDeReferencia.Trim());
                if (objPedime == null)
                    throw new ArgumentException("Aún no se ha cerrado el pedimento");

                NuevoConagtadu objNuevoConagtadu = new();
                objNuevoConagtadu = objCAD.BuscarProrateo(objRefe.IDReferencia, objCA.GuiaHouse.Trim());

                if (objNuevoConagtadu == null)
                    throw new ArgumentException("No existe prorrateo");


                obj.brokerNum = Convert.ToInt32(objPedime.PAT_AGEN);
                obj.pedimentNum = Convert.ToInt32(objPedime.NUM_PEDI.Trim());
                obj.guia = objCA.GuiaHouse.Trim();
                obj.contenido = objCA.Descripcion.Trim();
                obj.valor = objNuevoConagtadu.Valor;
                obj.userID = "MAVA";
                obj.dateCapture = objCA.FechaDeOperacion;
                obj.impuestosYDerechos = objNuevoConagtadu.ImpuestosYDerechos;
                obj.prevalidacion = objNuevoConagtadu.ProrateroPRV;
                obj.iva = objNuevoConagtadu.IVA;

                lst.Add(obj);


                vJson = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Hubo un error para generar el Json:" + ex.Message.Trim());
            }

            return vJson;
        }


        public string JsonConagtaduPedimento(List<ConsolAnexos> lstConsolAnexos)
        {
            string vJson = string.Empty;
            int IDDatosDeEmpresa = 1;
            try
            {
                ConsolAnexosRepository objCAD = new(_configuration);



                List<jsonConagtadu> lst = new List<jsonConagtadu>();

                foreach (ConsolAnexos objCA in lstConsolAnexos)
                {
                    try
                    {
                        Referencias objRefe = new();
                        ReferenciasRepository objRefeD = new(_configuration);
                        objRefe = objRefeD.Buscar(objCA.IdReferencia, 1);
                        if (objRefe == null)
                        {
                            throw new ArgumentException("No existe id de la referencia");
                        }


                        CatalogoDeOficinas oficinas = new();
                        CatalogoDeOficinasRepository oficinasD = new(_configuration);
                        oficinas = oficinasD.Buscar(objRefe.IdOficina);
                        if (oficinas == null)
                        {
                            throw new ArgumentException("No existe id de la referencia");
                        }
                        gtw = oficinas.GTWFac;


                        jsonConagtadu obj = new jsonConagtadu();

                        ConsolBloques objBloque = new ConsolBloques();
                        ConsolBloquesRepository objBloqueD = new(_configuration);
                        objBloque = objBloqueD.Buscar(objCA.IdBloque);
                        if (objBloque == null)
                            throw new ArgumentException("El bloque no existe");

                        CatalogodeMaster objMaster = new CatalogodeMaster();
                        CatalogodeMasterRepository objMasterD = new(_configuration);
                        objMaster = objMasterD.Buscar(objBloque.IDMasterConsol);
                        if (objMaster == null)
                            throw new ArgumentException("La guía master no fue registrada en expertti");

                        SaaioPedime objPedime = new SaaioPedime();
                        SaaioPedimeRepository objPedimeD = new(_configuration);
                        objPedime = objPedimeD.Buscar(objRefe.NumeroDeReferencia.Trim());
                        if (objPedime == null)
                            throw new ArgumentException("Aún no se ha cerrado el pedimento");

                        NuevoConagtadu objNuevoConagtadu = new();
                        objNuevoConagtadu = objCAD.BuscarProrateo(objRefe.IDReferencia, objCA.GuiaHouse.Trim());

                        if (objNuevoConagtadu == null)
                            throw new ArgumentException("No existe prorrateo");


                        obj.brokerNum = Convert.ToInt32(objPedime.PAT_AGEN);
                        obj.pedimentNum = Convert.ToInt32(objPedime.NUM_PEDI.Trim());
                        obj.guia = objCA.GuiaHouse.Trim();
                        obj.contenido = objCA.Descripcion.Trim();
                        obj.valor = objNuevoConagtadu.Valor;
                        obj.userID = "MAVA";
                        obj.dateCapture = objCA.FechaDeOperacion;
                        obj.impuestosYDerechos = objNuevoConagtadu.ImpuestosYDerechos;
                        obj.prevalidacion = objNuevoConagtadu.ProrateroPRV;
                        obj.iva = objNuevoConagtadu.IVA;

                        lst.Add(obj);
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }





                vJson = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Hubo un error para generar el Json:" + ex.Message.Trim());
            }

            return vJson;
        }

        public List<string> EnvioporGuias()
        {
            List<string> lstRespuestas = new();

            List<GuiasPendientes> lstGuiasPendientes = new();

            ApiDHL objApi = new(_configuration);

            string token = objApi.getToken();

            ConsolAnexosRepository objca = new(_configuration);

            lstGuiasPendientes = objca.ConagtadusPendientes();
            foreach (GuiasPendientes guia in lstGuiasPendientes)
            {
                try
                {
                    string jsonGuia = JsonConagtaduGuia(guia.GuiaHouse, guia.idReferencia);
                    JsonRepuestaDHL objResp = new();

                    objResp = objApi.postDHL(jsonGuia, token, gtw, "PedimentoConsolidado/");
                    objApi.Bitacora(objResp.respuesta, objResp.status, guia.idReferencia, guia.GuiaHouse);
                    lstRespuestas.Add(objResp.respuesta.Trim());
                }
                catch (Exception ex)
                {

                    objApi.Bitacora(ex.Message.Trim(), true, idReferencia, guia.GuiaHouse);
                    lstRespuestas.Add(ex.Message.Trim());
                }


            }

            return lstRespuestas;
        }

        public JsonRepuestaDHL EnvioporGuia(string guia, int idReferencia)
        {
            ApiDHL objApi = new(_configuration);
            JsonRepuestaDHL objrep = new();
            string token = objApi.getToken();


            objrep.TokenDHL = token;
            if (objApi.GeneroToken == true)
            {
                string jsonGuia = JsonConagtaduGuia(guia, idReferencia);
                objrep = objApi.postDHL(jsonGuia, token, gtw, "PedimentoConsolidado/");
                objApi.Bitacora(objrep.respuesta, objrep.status, idReferencia, guia);

            }

            return objrep;
        }

        public List<string> EnvioporGuias(List<GuiasPendientes> guias)
        {
            ApiDHL objApi = new(_configuration);
            JsonRepuestaDHL objrep = new();
            string token = objApi.getToken();
            List<string> Respuestas = new();


            objrep.TokenDHL = token;
            if (objApi.GeneroToken == true)
            {
                foreach (GuiasPendientes guia in guias)
                {
                    try
                    {
                        string jsonGuia = JsonConagtaduGuia(guia.GuiaHouse, guia.idReferencia);
                        objrep = objApi.postDHL(jsonGuia, token, gtw, "PedimentoConsolidado/");
                        objApi.Bitacora(objrep.respuesta, objrep.status, guia.idReferencia, guia.GuiaHouse);
                        Respuestas.Add(objrep.respuesta.Trim());
                    }
                    catch (Exception)
                    {
                        objApi.Bitacora(objrep.respuesta, objrep.status, guia.idReferencia, guia.GuiaHouse);
                        Respuestas.Add(objrep.respuesta.Trim());
                    }

                }

            }

            return Respuestas;
        }

        public List<string> EnvioporPedimento(int idBloque)
        {


            List<string> lstRespuestas = new();


            ApiDHL objApi = new(_configuration);

            string token = objApi.getToken();

            ConsolAnexosRepository objCAD = new(_configuration);

            List<ConsolAnexos> lstConsolAnexos = new();
            lstConsolAnexos = objCAD.Cargarlst(idBloque);


            string jsonGuia = JsonConagtaduPedimento(lstConsolAnexos);

            JsonRepuestaDHL objResp = new();

            objResp = objApi.postDHL(jsonGuia, token, gtw, "PedimentoConsolidado/");

            foreach (ConsolAnexos item in lstConsolAnexos)
            {
                objApi.Bitacora(objResp.respuesta, objResp.status, item.IdReferencia, item.GuiaHouse);

            }

            lstRespuestas.Add(jsonGuia);
            lstRespuestas.Add(objResp.respuesta);
            return lstRespuestas;
        }

        public List<string> EnvioporGuiasHilos(int Hilo)
        {
            List<string> lstRespuestas = new();

            List<GuiasPendientes> lstGuiasPendientes = new();

            ApiDHL objApi = new(_configuration);

            string token = objApi.getToken();

            ConsolAnexosRepository objca = new(_configuration);

            lstGuiasPendientes = objca.ConagtadusPendientesHilos(Hilo);
            foreach (GuiasPendientes guia in lstGuiasPendientes)
            {
                try
                {
                    string jsonGuia = JsonConagtaduGuia(guia.GuiaHouse, guia.idReferencia);
                    JsonRepuestaDHL objResp = new();

                    objResp = objApi.postDHL(jsonGuia, token, gtw, "PedimentoConsolidado/");
                    objApi.Bitacora(objResp.respuesta, objResp.status, guia.idReferencia, guia.GuiaHouse);
                    lstRespuestas.Add(objResp.respuesta.Trim());
                }
                catch (Exception ex)
                {

                    objApi.Bitacora(ex.Message.Trim(), true, idReferencia, guia.GuiaHouse);
                    lstRespuestas.Add(ex.Message.Trim());
                }


            }

            return lstRespuestas;
        }

        public List<string> EnvioporGuiasporDODA(int idDODA)
        {
            List<string> lstRespuestas = new();
            try
            {
                List<JsonDetalleporDODA> lstEnvios = new List<JsonDetalleporDODA>();


                ApiDHL objApi = new(_configuration);
                ApiDHLControl objControl = new(_configuration);

                string token = objApi.getToken();

                ConsolAnexosRepository objca = new(_configuration);


                lstEnvios = objControl.DetalleporDODAConAgtAdu(idDODA);
                foreach (JsonDetalleporDODA guia in lstEnvios)
                {
                   try
                    {
                        string jsonGuia = JsonConagtaduGuia(guia.GuiaHouse, guia.IdReferencia);
                        JsonRepuestaDHL objResp = new();

                        objResp = objApi.postDHL(jsonGuia, token, gtw, "PedimentoConsolidado/");
                        objApi.Bitacora(objResp.respuesta, objResp.status, guia.IdReferencia, guia.GuiaHouse);
                        lstRespuestas.Add(objResp.respuesta.Trim());
                    }
                    catch (Exception ex)
                    {

                        objApi.Bitacora(ex.Message.Trim(), true, idReferencia, guia.GuiaHouse);
                        lstRespuestas.Add(ex.Message.Trim());
                    }


                }
                
                }
                catch (Exception ex)
                {

                        lstRespuestas.Add(ex.Message.Trim()) ;
                }
   
           

            return lstRespuestas;
        }

    }


    public class GuiasPendientes
    {
        public string GuiaHouse { get; set; }
        public int idReferencia { get; set; }


    }
}
