using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado.PredodaDHL;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Negocio
{
    public class UtilDespacho
    {
        public IConfiguration _configuration;

        RelacionBitacoraRepository relacionBitacoraRepository;
        SaaioPedimeRepository saaioPedimeRepository;
        dat_DODARepository dat_DODARepository;
        PreDODADetalleDataRepository preDODADetalleDataRepository;
        RelaciondeEnvioRepository relaciondeEnvioRepository;
        DetalledeEnvioRepository detalledeEnvioRepository;
        SalidasConsolRepository salidasConsolRepository;
        SaaioFacturRepository saaioFacturRepository;
        public UtilDespacho(IConfiguration configuration)
        {
            _configuration = configuration;
            relacionBitacoraRepository = new RelacionBitacoraRepository(_configuration);
            saaioPedimeRepository = new SaaioPedimeRepository(_configuration);
            dat_DODARepository = new dat_DODARepository(_configuration);
            preDODADetalleDataRepository = new PreDODADetalleDataRepository(_configuration);
            relaciondeEnvioRepository = new RelaciondeEnvioRepository(_configuration);
            detalledeEnvioRepository = new DetalledeEnvioRepository(_configuration);
            salidasConsolRepository = new SalidasConsolRepository(_configuration);
            saaioFacturRepository = new SaaioFacturRepository(_configuration);
        }
        public ResponseValidarPredoda ValidarPredoda(int idRelacionBitacora,int IdRelaciondeEnvio,int IdSalidasConsol,int IdOficina)
        {
            ResponseValidarPredoda response = new ResponseValidarPredoda();
            List<string> errores = new List<string>();
            SalidasConsol salidasConsol;
            List<PredodaDetalle> detallerelacionbitacora = new List<PredodaDetalle>();
            List<PredodaDetalle> detallerelacionenvio = new List<PredodaDetalle>();
            List<PredodaDetalle> detallerelacionsalidasconsol = new List<PredodaDetalle>();
            List<PredodaDetalle> resultPredodaDetalle = new List<PredodaDetalle>();
            if (idRelacionBitacora != 0)
            {

                var detalleBitacora = relacionBitacoraRepository.CargarDetalleDoda(idRelacionBitacora);
                foreach (var item in detalleBitacora)
                {
                    try
                    {
                        var objPedime = saaioPedimeRepository.Buscar(item.Referencia);
                        if (objPedime == null)
                        {
                            throw new Exception("No existe pedimento en CASA " + item.Referencia);
                        }
                        if (item.Remesa == 0 && (objPedime.FIR_PAGO == null || objPedime.FIR_PAGO == ""))
                        {
                            throw new Exception("No esta pagada la referencia con número " + item.Referencia);
                        }
                        detallerelacionbitacora.Add(item);
                    }
                    catch (Exception e)
                    {
                        errores.Add(e.Message);
                    }
                }
            }

            if (IdRelaciondeEnvio != 0)
            {

                detallerelacionenvio = relaciondeEnvioRepository.CargarDetalleDoda(IdRelaciondeEnvio);
            }
            if (IdSalidasConsol != 0)
            {
                salidasConsol = salidasConsolRepository.Buscar(IdSalidasConsol);

                detallerelacionsalidasconsol = salidasConsolRepository.CargarDoda(salidasConsol.IdCorte, salidasConsol.IDRegion, IdOficina);

                if (detallerelacionsalidasconsol.Count() > 0)
                {
                    if (!detalledeEnvioRepository.VerificarPago(salidasConsol.IdCorte, salidasConsol.IDRegion, Convert.ToInt32(salidasConsol.IdOficina))) ;
                    {
                        errores.Add("Es necesario que este todo pagado, corte:" + salidasConsol.NoCorte);
                    }
                }

            }

            var pedimentos = detallerelacionbitacora.Concat(detallerelacionenvio).Concat(detallerelacionsalidasconsol)
                    .Select(x => new { x.IdReferencia, x.Referencia, x.Remesa, x.NumeroDeCOVE, x.Pedimento })
                    .Distinct().ToList();
            foreach (var item in pedimentos)
            {
                try
                {
                    int idReferencia = item.IdReferencia;
                    int remesa = item.Remesa;
                    string COVE = item.NumeroDeCOVE;

                    var objPedime = saaioPedimeRepository.Buscar(item.Referencia);
                    if (objPedime == null)
                    {
                        throw new Exception("No existe pedimento en CASA");
                    }
                    var objenDoda = dat_DODARepository.ExisteenDODA(item.IdReferencia, item.Remesa);

                    if (objenDoda != null)
                    {
                        throw new Exception("El pedimento " + item.Pedimento + " de la idreferencia " + item.IdReferencia + " ya existe en No. Ticket " + objenDoda._N_Ticket + " No. de Integración " + objenDoda._N_Integracion);
                    }

                    if (item.Remesa != 0)
                    {
                        int cantidad = saaioFacturRepository.NET_SABER_CUANTOS_COVE_TIENE_LAREMESA(item.Referencia, item.Remesa);
                        if (cantidad > 1)
                        {
                            throw new Exception("Existen " + cantidad.ToString() + "  COVES para la remesa " + item.Remesa.ToString() + " de la referencia " + item.Referencia.ToString());
                        }
                    }
                    if (COVE != "")
                    {
                        var objcove = dat_DODARepository.ExisteenDODACOVE(idReferencia, COVE);
                        if (objcove != null)
                        {
                            throw new Exception("El Cove " + COVE + " del pedimento: " + item.Pedimento + " ya existe en No. Ticket " + objcove._N_Ticket + " No. de Integración " + objcove._N_Integracion + " ¿Se trata de una subdivisión?");
                        }
                    }

                    resultPredodaDetalle.Add( new PredodaDetalle
                    {
                        idPreDoda = 0,
                        IdReferencia = idReferencia,
                        Remesa = remesa,
                        NumeroDeCOVE = COVE
                    });
                    
                }
                catch (Exception ex1)
                {
                    errores.Add(ex1.Message);

                }
            }
            response.detalles = resultPredodaDetalle;
            response.errores = errores;
            return response;
        }
    }
}
