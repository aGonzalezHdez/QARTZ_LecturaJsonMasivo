using LibreriaClasesAPIExpertti.Entities.EntitiesXMLGrupoEI;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using System.Data.SqlClient;
using System.Xml.Serialization;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesXMLGrupoEI.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesXMLGrupoEI
{
    public class XMLEiRepository : IXMLEiRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public XMLEiRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        private XMLEiCliente ConvertirObjetoCliente(CatalogoDeClientesFormales lCliente, string NoCuenta)
        {
            XMLEiCliente objCliente = new();
            objCliente.Nombre = lCliente.Nombre.Trim();
            objCliente.RFC = lCliente.RFC.Trim();
            objCliente.Calle = lCliente.DireccionesDeClientes.Direccion.Trim();
            objCliente.Colonia = lCliente.DireccionesDeClientes.Colonia.Trim();
            objCliente.NoExterior = lCliente.DireccionesDeClientes.NumeroExt.Trim();
            objCliente.NoInterior = lCliente.DireccionesDeClientes.NumeroInt.Trim();
            objCliente.CodigoPostal = lCliente.DireccionesDeClientes.CodigoPostal.Trim();
            objCliente.EntidadFederativa = lCliente.DireccionesDeClientes.Entidad.Trim(); ;
            objCliente.NoCuenta = NoCuenta;

            return objCliente;
        }

        private XMLEiCliente ConvertirObjetoClienteBroker(CustomsAlerts lCMF)
        {
            XMLEiCliente objCliente = new XMLEiCliente();
            objCliente.Nombre = lCMF.Cliente.Trim();
            objCliente.RFC = "";
            objCliente.Calle = "";
            objCliente.Colonia = "";
            objCliente.NoExterior = "";
            objCliente.NoInterior = "";
            objCliente.CodigoPostal = "";
            objCliente.EntidadFederativa = "";


            return objCliente;
        }

        private XMLEiRiel ConvertirObjetoRiel(CatalogodeRielesWEC lRiel)
        {
            XMLEiRiel objRiel = new();

            objRiel.Riel = lRiel.NoRiel;
            objRiel.Descripcion = lRiel.Descripcion.Trim();

            return objRiel;
        }



        public XMLEi ConstruyeXMLWEC_CustomAlert(int IdCustomAlert)
        {
            XMLEi objXML = new();
            CustomsAlerts objCA = new();
            CargaManifiestosRepository objCAD = new(_configuration);
            try
            {
                objCA = objCAD.Buscar(IdCustomAlert);

                if (objCA.IdRielWEC == 0)
                {
                    objXML = null/* TODO Change to default(_) if this is not a reference type */;
                    return objXML;
                    //return;
                }

                if (objCA == null)
                {
                    CatalogoDeClientesFormales objCliente = new();
                    CatalogoDeClientesFormalesRepository objClienteD = new(_configuration);
                    objCliente = objClienteD.Buscar(objCA.IdCliente);

                    XMLEiCliente objxmlCliente = new();
                    if (objCA.IdCategoria != 1)
                    {
                        if (objCA.IdCliente != 17169)
                            objxmlCliente = ConvertirObjetoCliente(objCliente, null/* TODO Change to default(_) if this is not a reference type */);
                        else
                            objxmlCliente = ConvertirObjetoClienteBroker(objCA);
                    }
                    else
                        objxmlCliente = ConvertirObjetoClienteBroker(objCA);


                    CatalogodeRielesWEC objRiel = new();
                    CatalogodeRielesWECRepository objRielD = new(_configuration);
                    objRiel = objRielD.Buscar(objCA.IdRielWEC);

                    XMLEiRiel objXMLRiel = new();
                    objXMLRiel = ConvertirObjetoRiel(objRiel);

                    objXML.Cliente = objxmlCliente;
                    objXML.GuiaHouse = objCA.GuiaHouse.Trim();
                    objXML.Riel = objXMLRiel;
                    objXML.IATAOrigen = objCA.OrigenIata;
                    objXML.IATADestino = objCA.DestinoIata;
                    objXML.Bultos = objCA.Piezas;
                    objXML.ValorDolares = objCA.ValorEnDolares;

                    if (Directory.Exists(@"C:\xmlCMFWEC\") == false)
                        Directory.CreateDirectory(@"C:\xmlCMFWEC\");
                    // Serialize object to a text file.
                    StreamWriter objStreamWriter = new(@"C:\xmlCMFWEC\" + objCA.GuiaHouse.Trim() + ".xml");
                    XmlSerializer x = new(objXML.GetType());
                    x.Serialize(objStreamWriter, objXML);
                    objStreamWriter.Close();

                    CatalogoDeFtps objFtp = new CatalogoDeFtps();
                    CatalogoDeFtpsRepository objFtpd = new(_configuration);
                    objFtp = objFtpd.Buscar(9);
                    if (objFtp == null)
                    {
                        Helper objHelp = new Helper();
                        objHelp.UnloadFtp(objFtp.FTP + objCA.GuiaHouse.Trim() + ".xml", @"C:\xmlCMFWEC\" + objCA.GuiaHouse.Trim() + ".xml", objFtp.UsuarioFTP, objFtp.PasswordFTP);
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return objXML;
        }

    }
}
