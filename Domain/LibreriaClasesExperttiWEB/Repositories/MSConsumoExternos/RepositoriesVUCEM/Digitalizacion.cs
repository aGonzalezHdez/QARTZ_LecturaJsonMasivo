using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using System.Data.SqlClient;
using System.ServiceModel.Channels;
using System.Security.Cryptography;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;

using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM
{
    public class Digitalizacion
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public Digitalizacion(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        private string fPath(string NumerodeReferencia, int IDOficina)
        {
            var objRutas = new UbicaciondeArchivosRepository(_configuration);
            var Rutas = new UbicaciondeArchivos();
            Rutas = objRutas.Buscar(30);
            return Rutas.Ubicacion;
        }

        public async Task<DigitalizadosRecibir> Digitalizar(int IdDigitalizadosVucem, CatalogoDeUsuarios GObjUsuario, string pMisDocumentos)
        {

            var objDigitalizadosVucem = new DigitalizadosVucem();
            var objDigitalizadosVucemD = new DigitalizadosVucemRepository(_configuration);
            objDigitalizadosVucem = objDigitalizadosVucemD.Buscar(IdDigitalizadosVucem);




            if (objDigitalizadosVucem == null)
            {
                throw new ArgumentException("No fue posible encontrar el registro del documento :DigitalizadosVucem");
            }

            var objDocumentosVucem = new CatalogodeDocumentosVuce();
            var objDocumentosVucemD = new CatalogodeDocumentosVuceRepository(_configuration);
            objDocumentosVucem = objDocumentosVucemD.BuscarId(objDigitalizadosVucem.IDDocumento);


            var eDocuments = new DigitalizadosRecibir();

            var objRefe = new Referencias();
            var objRefeD = new ReferenciasRepository(_configuration);
            objRefe = objRefeD.Buscar(objDigitalizadosVucem.IDReferencia, GObjUsuario.IDDatosDeEmpresa);
            if (objRefe == null)
            {
                throw new Exception("No existe la referencia");
            }

            string Archivo = string.Empty;
            Archivo = fPath(objRefe.NumeroDeReferencia, GObjUsuario.IdOficina) + objRefe.NumeroDeReferencia.Trim() + "_" + objDocumentosVucem.NoOficial.ToString() + "_" + objDigitalizadosVucem.Consecutivo.ToString() + objDigitalizadosVucem.Extension;

            if (File.Exists(Archivo) == false)
            {
                throw new ArgumentException("El Archivo " + Archivo.Trim() + " fue removido de su equipo");
            }

            var objCliente = new Clientes();
            var objClienteD = new ClientesRepository(_configuration);
            objCliente = objClienteD.Buscar(objRefe.IDCliente);
            if (!(objCliente == null))
            {
                if (objCliente.Prospecto)
                {
                    throw new Exception("No puede enviar a cove un clientes prospecto");
                }
            }


            var objGenerales = new ComponentesGenerales();
            var objDatos = new Datos(_configuration);

            objGenerales = objDatos.DatosGenerales(objRefe.NumeroDeReferencia, GObjUsuario.IDDatosDeEmpresa);

            var documento = new WSDigitalizar.Documento();

            string hash = string.Empty;
            hash = CalcularHash(Archivo);

            string vCadenaOriginal = string.Empty;

            vCadenaOriginal = "|" + objGenerales.Sello.UsuarioWebService;
            vCadenaOriginal += "|" + objGenerales.Sello.Email;
            vCadenaOriginal += "|" + objDocumentosVucem.NoOficial;
            vCadenaOriginal += "|" + Path.GetFileNameWithoutExtension(Archivo);

            foreach (string elemento in objGenerales.RfcConsulta)
                vCadenaOriginal += "|" + elemento.Trim();



            vCadenaOriginal += "|" + hash + "|";


            byte[] vArchivo;
            vArchivo = File.ReadAllBytes(Archivo);

            var objFirma = new wsVentanillaUnica.FirmaElectronica();
            objFirma = objDatos.getFirmaElectronica(objGenerales.Sello, vCadenaOriginal, pMisDocumentos);

            documento.archivo = vArchivo;
            documento.idTipoDocumento = objDocumentosVucem.NoOficial;
            documento.nombreDocumento = Path.GetFileNameWithoutExtension(Archivo);
            documento.rfcConsulta = objGenerales.Sello.RFCConsulta;

            var FirmaElect = new WSDigitalizar.FirmaElectronica();
            FirmaElect.cadenaOriginal = objFirma.cadenaOriginal;
            FirmaElect.certificado = objFirma.certificado;
            FirmaElect.firma = objFirma.firma;

            string FirmaBase64 = Convert.ToBase64String(objFirma.firma);


            var PeticionBase = new WSDigitalizar.PeticionBase();
            PeticionBase.firmaElectronica = FirmaElect;

            var Registro = new WSDigitalizar.RegistroDigitalizarDocumentoRequest();
            Registro.correoElectronico = objGenerales.Sello.Email;
            Registro.documento = documento;
            Registro.peticionBase = PeticionBase;

            var ClienteDigi = new WSDigitalizar.ReceptorClient(null,null);

            //var ClienteDigi = new WSDigitalizar.ReceptorClient();
            ClienteDigi.ClientCredentials.UserName.UserName = objGenerales.Sello.UsuarioWebService;
            ClienteDigi.ClientCredentials.UserName.Password = objGenerales.Sello.PasswordWebService;

            BindingElementCollection elements = ClienteDigi.Endpoint.Binding.CreateBindingElements();
            TransportSecurityBindingElement transportSecurity;
            transportSecurity = elements.Find<TransportSecurityBindingElement>();
            transportSecurity.IncludeTimestamp = false;

            ClienteDigi.Endpoint.Binding = new CustomBinding(elements);
            ClienteDigi.Endpoint.EndpointBehaviors.Add(new InspectorMensajesCliente());

            string ErrorXml = "No se alcanzo Web Service";
            var lstErrores = new List<string>();
            try
            {



                var Acuse = new WSDigitalizar.RegistroDigitalizarDocumentoResponse1();
                Acuse = await ClienteDigi.RegistroDigitalizarDocumentoAsync(Registro);
                //Acuse = ClienteDigi.RegistroDigitalizarDocumento(Registro);
                if (Acuse.registroDigitalizarDocumentoServiceResponse.respuestaBase.tieneError)
                {
                    ErrorXml = "Error en el XML";
                    //foreach (string Err in Acuse.respuestaBase.error)
                    string Err = Acuse.registroDigitalizarDocumentoServiceResponse.respuestaBase.error.ToString();
                        lstErrores.Add(Err);

                    eDocuments.Err = true;
                    eDocuments.Errores = lstErrores;
                }
                else
                {
                    objDigitalizadosVucem.NoOperacion = int.Parse(Acuse.registroDigitalizarDocumentoServiceResponse.acuse.numeroOperacion.ToString());
                    objDigitalizadosVucem.EnviadoSAT = true;
                    objDigitalizadosVucem.ErrorArchivo = Acuse.registroDigitalizarDocumentoServiceResponse.respuestaBase.tieneError;
                    objDigitalizadosVucem.FechaEnvio = true;
                    objDigitalizadosVucem.RFCSello = objGenerales.Sello.UsuarioWebService;
                    objDigitalizadosVucem.HashDoc = hash;
                    objDigitalizadosVucem.FirmaBase64 = FirmaBase64;
                    objDigitalizadosVucemD.Modificar(objDigitalizadosVucem);

                    ErrorXml = Acuse.registroDigitalizarDocumentoServiceResponse.acuse.mensaje.Trim();


                    eDocuments = await RecuperarEdocuments(objDigitalizadosVucem.IdDigitalizadosVucem, GObjUsuario, pMisDocumentos);


                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Conexion con Ventanilla Unica: " + ex.Message);
            }


            return eDocuments;
        }


        private string CalcularHash(string ruta_archivo)
        {
            byte[] result;
            var sha = new SHA1CryptoServiceProvider();
            result = sha.ComputeHash(File.ReadAllBytes(ruta_archivo));
            string strHash = BitConverter.ToString(result).Replace("-", "").ToLower();

            return strHash;
        }

        public async Task<DigitalizadosRecibir> RecuperarEdocuments(int IdDigitalizadosVucem, CatalogoDeUsuarios GObjUsuario, string pMisDocumentos)
        {
            var eDocumets = new DigitalizadosRecibir();
            var objGenerales = new ComponentesGenerales();
            var objDatos = new Datos(_configuration);

            var objDigitalizadosVucem = new DigitalizadosVucem();
            var objDigitalizadosVucemD = new DigitalizadosVucemRepository(_configuration);
            objDigitalizadosVucem = objDigitalizadosVucemD.Buscar(IdDigitalizadosVucem);

            var objDocumentosVucem = new CatalogodeDocumentosVuce();
            var objDocumentosVucemD = new CatalogodeDocumentosVuceRepository(_configuration);
            objDocumentosVucem = objDocumentosVucemD.BuscarId(objDigitalizadosVucem.IDDocumento);

            var objRefe = new Referencias();
            var objRefeD = new ReferenciasRepository(_configuration);
            objRefe = objRefeD.Buscar(objDigitalizadosVucem.IDReferencia, GObjUsuario.IDDatosDeEmpresa);
            if (objRefe == null)
            {
                throw new Exception("No existe la referencia");
            }


            objGenerales = objDatos.DatosGenerales(objRefe.NumeroDeReferencia, GObjUsuario.IDDatosDeEmpresa);

            var ClienteDigi = new WSDigitalizar.ReceptorClient(null,null);
            //var ClienteDigi = new WSDigitalizar.ReceptorClient();
            ClienteDigi.ClientCredentials.UserName.UserName = objGenerales.Sello.UsuarioWebService;
            ClienteDigi.ClientCredentials.UserName.Password = objGenerales.Sello.PasswordWebService;

            BindingElementCollection elements = ClienteDigi.Endpoint.Binding.CreateBindingElements();
            TransportSecurityBindingElement transportSecurity;
            transportSecurity = elements.Find<TransportSecurityBindingElement>();
            transportSecurity.IncludeTimestamp = false;

            ClienteDigi.Endpoint.Binding = new CustomBinding(elements);
            ClienteDigi.Endpoint.EndpointBehaviors.Add(new InspectorMensajesCliente());


            string CadenaOriginal;

            CadenaOriginal = "|" + objGenerales.Sello.UsuarioWebService;
            CadenaOriginal += "|" + objDigitalizadosVucem.NoOperacion.ToString() + "|";

            var objFirma = new wsVentanillaUnica.FirmaElectronica();
            objFirma = objDatos.getFirmaElectronica(objGenerales.Sello, CadenaOriginal, pMisDocumentos);


            var documento = new WSDigitalizar.Documento();

            var FirmaElect = new WSDigitalizar.FirmaElectronica();
            FirmaElect.cadenaOriginal = objFirma.cadenaOriginal;
            FirmaElect.certificado = objFirma.certificado;
            FirmaElect.firma = objFirma.firma;



            var Peticion = new WSDigitalizar.PeticionBase();
            var registro = new WSDigitalizar.ConsultaDigitalizarDocumentoRequest();

            Peticion.firmaElectronica = FirmaElect;


            string FirmaBase64 = Convert.ToBase64String(objFirma.firma);
            registro.numeroOperacion = objDigitalizadosVucem.NoOperacion;
            registro.peticionBase = Peticion;

            var lstErrores = new List<string>();
            try
            {
                var objDigital = new DigitalizadosVucem();
                var objDigitalD = new DigitalizadosVucemRepository(_configuration);
                objDigital = objDigitalD.Buscar(IdDigitalizadosVucem);

                var resultado = new WSDigitalizar.ConsultaEDocumentDigitalizarDocumentoResponse();
                resultado = await ClienteDigi.ConsultaEDocumentDigitalizarDocumentoAsync(registro);


                if (resultado.consultaDigitalizarDocumentoServiceResponse.respuestaBase.tieneError)
                {
                    //foreach (string Err in resultado.respuestaBase.error)
                    string Err = resultado.consultaDigitalizarDocumentoServiceResponse.respuestaBase.error.ToString();
                    //var ClienteDigi = new WSDigitalizar.ReceptorClient();
                    lstErrores.Add(Err);
                    eDocumets.Err = true;
                    eDocumets.Errores = lstErrores;

                    objDigital.ErrorArchivo = true;
                    objDigitalD.Modificar(objDigital);
                }
                else
                {
                    objDigital.EnviadoSAT = true;
                    objDigital.ErrorArchivo = resultado.consultaDigitalizarDocumentoServiceResponse.respuestaBase.tieneError;
                    objDigital.FechaEnvio = true;
                    objDigital.eDocument = resultado.consultaDigitalizarDocumentoServiceResponse.eDocument;
                    objDigital.numeroDeTramite = resultado.consultaDigitalizarDocumentoServiceResponse.numeroDeTramite;
                    objDigital.FechaRecibido = true;


                    string ArchivoLocal = string.Empty;
                    ArchivoLocal = fPath(objRefe.NumeroDeReferencia, GObjUsuario.IdOficina) + objRefe.NumeroDeReferencia.Trim() + "_" + objDocumentosVucem.NoOficial.ToString() + "_" + objDigitalizadosVucem.Consecutivo.ToString() + objDigitalizadosVucem.Extension;

                    objDigitalD.Modificar(objDigital);

                    eDocumets.CadenaOriginal = resultado.consultaDigitalizarDocumentoServiceResponse.cadenaOriginal;
                    eDocumets.eDocument = resultado.consultaDigitalizarDocumentoServiceResponse.eDocument;
                    eDocumets.NumerodeTramite = resultado.consultaDigitalizarDocumentoServiceResponse.numeroDeTramite;
                    eDocumets.Err = false;
                    eDocumets.Errores = null;

                    try
                    {
                        int Result = 0;
                        var objDocumentosporguia = new DocumentosporGuia();
                        var objCentralizar = new CentralizarDocsS3(_configuration);
                        Result = await objCentralizar.AgregarDocumentos(ArchivoLocal, objRefe, 1152, "", IdDigitalizadosVucem, GObjUsuario, false, "");
                        objDigitalD.ModificarS3(IdDigitalizadosVucem, Result);

                        File.Delete(ArchivoLocal);
                        File.Delete(ArchivoLocal);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
                eDocumets = default;
            }

            return eDocumets;
        }
    }
}

