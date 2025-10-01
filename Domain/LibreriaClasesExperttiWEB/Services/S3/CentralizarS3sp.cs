using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
using LibreriaClasesAPIExpertti.Entities.EntitiesGei;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using System.Data.SqlClient;
using wsCentralizar;
using CatalogoDeUsuarios = LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos.CatalogoDeUsuarios;
using Microsoft.Extensions.Configuration;
using Amazon.S3;
namespace LibreriaClasesAPIExpertti.Services.S3
{
    public class CentralizarS3sp
    {
        public string SConexion { get; set; }
        public string SConexionGP { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CentralizarS3sp(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            SConexionGP = _configuration.GetConnectionString("dbCASAEIGP")!;
        }
        public async Task<int> AgregarDocumentos(string Archivo, int IdReferencia, int idTipoDocumento, string NombreArchivo, int Id, CatalogoDeUsuarios GObjUsuario, bool Preguntar)
        {
            bool Subio = false;
            int IdDocumento = 0;
            try
            {
                var objRefe = new Referencias();
                var objRefeD = new ReferenciasRepository(_configuration);
                objRefe = objRefeD.Buscar(IdReferencia);
                if (objRefe == null)
                {
                    throw new ArgumentException("Existe un problema con el número de referencia");
                }

                var objTipDoc = new CatalogodeTiposDeDocumentos();
                var objTipDocD = new CatalogodeTiposDeDocumentosRepository(_configuration);
                objTipDoc = objTipDocD.Buscar(idTipoDocumento);
                if (objTipDoc == null)
                {
                    throw new ArgumentException("Hubo un error al obtener el tipo de documento");
                }

                var objDocD = new DocumentosRepository(_configuration);
                var objPaths = new PathsDocumentos();
                objPaths = objDocD.BuscarPath(objRefe.NumeroDeReferencia.Trim(), objTipDoc.Identificador);
                if (objPaths == null)
                {
                    throw new ArgumentException("No existe ruta disponible para almacenar el archivo.");
                }


                string Extension;
                Extension = Path.GetExtension(Archivo);


                IdDocumento = await SubirDocumentosporGuia(idTipoDocumento, Archivo, objRefe.IDReferencia, Id, NombreArchivo, GObjUsuario, "grupoei.documentos");
                // End If

                try
                {
                    if (GObjUsuario.Oficina.UtilizaGP)
                    {
                        if (!string.IsNullOrEmpty(SConexionGP))
                        {
                            var objArchivosGp = new GEI_Archivos();
                            var objArchivosGpD = new GEI_ArchivosRepository(_configuration);
                            objArchivosGp.Id_Tipo = 1;

                            string RutaGp = objArchivosGpD.getUbicacionGP(objRefe.NumeroDeReferencia.Trim());
                            if (Directory.Exists(RutaGp) == false)
                            {
                                Directory.CreateDirectory(RutaGp);
                            }
                            var objDocGP = new GEI_Documentos();
                            var objDocGPD = new GEI_DocumentosRepository(_configuration);
                            objDocGP = objDocGPD.Buscar(objTipDoc.IdDocumentoGP);
                            if (objDocGP == null)
                            {
                                throw new ArgumentException("No existe relacion del documento con el GP");
                            }

                            string UbicacionArch = RutaGp + objDocGP.Nombre.Trim() + Extension;

                            objArchivosGp.Path = UbicacionArch.Trim();
                            objArchivosGp.OC = objDocGP.Nombre.Trim();
                            objArchivosGp.Referencia = objRefe.NumeroDeReferencia.Trim();
                            objArchivosGp.Id_Usuario = GObjUsuario.Email.Trim();
                            objArchivosGp.Ctl_Alta = DateTime.Now;

                            try
                            {
                                if (File.Exists(UbicacionArch.Trim()) == false)
                                {
                                    File.Move(Archivo, UbicacionArch.Trim());
                                    objArchivosGpD.Insertar(objArchivosGp);
                                }
                            }

                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }

                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return IdDocumento;
        }
        public async Task<int> SubirDocumentosporGuia(int idTipoDocumento, string Documento, int IdReferencia, int id, string NombreArchivo, CatalogoDeUsuarios GObjUsuario, string S3Buckets)
        {
            int idDocumento = 0;
            try
            {
                var objTipDoc = new CatalogodeTiposDeDocumentos();
                var objTipDocD = new CatalogodeTiposDeDocumentosRepository(_configuration);
                objTipDoc = objTipDocD.BuscarS3(idTipoDocumento, IdReferencia, id);
                if (objTipDoc == null)
                {
                    throw new ArgumentException("hubo un problema en el tipo de documento");
                }

                if (!string.IsNullOrEmpty(NombreArchivo))
                {
                    objTipDoc.NombreExpediente = NombreArchivo.Trim();
                }


                var objRefe = new Referencias();
                var objRefeD = new ReferenciasRepository(_configuration);
                objRefe = objRefeD.Buscar(IdReferencia);
                if (objRefe == null)
                {
                    throw new ArgumentException("Existe un problema con el número de referencia");
                }

                string nombre = objTipDoc.NombreExpediente.Trim() + Path.GetExtension(Documento);

                string TipoExpediente = string.Empty;
                if (objTipDoc.EXPEDIENTEDIGITAL == true)
                {
                    TipoExpediente = "ExpedienteDigital";
                }
                else
                {
                    TipoExpediente = "ExpedienteAA";
                }

                string Ruta = Convert.ToInt32(objRefe.FechaApertura.ToString("yyyy")).ToString("0000") + "/" + Convert.ToInt32(objRefe.FechaApertura.ToString("MM")).ToString("00") + "/" + Convert.ToInt32(objRefe.FechaApertura.ToString("dd")).ToString("00") + "/" + objRefe.NumeroDeReferencia.Trim() + "/" + TipoExpediente.Trim() + "/" + nombre.Trim();

                var objDocumentosporguia = new DocumentosporGuia();
                var objDocumentosporguiaD = new DocumentosporGuiaRepository(_configuration);

                objDocumentosporguia.idTipoDocumento = idTipoDocumento;
                objDocumentosporguia.idReferencia = objRefe.IDReferencia;
                objDocumentosporguia.RutaFecha = "";
                objDocumentosporguia.Extension = Path.GetExtension(Documento);
                objDocumentosporguia.IdUsuario = GObjUsuario.IdUsuario;
                objDocumentosporguia.RutaS3 = Ruta;
                objDocumentosporguia.S3 = true;
                objDocumentosporguia.Consecutivo = objTipDoc.Consecutivo;

                int CUANTOS = objDocumentosporguiaD.BuscarRuta(Ruta);
                if (CUANTOS > 0)
                {
                    CUANTOS += 1;

                    Ruta = Convert.ToInt32(objRefe.FechaApertura.ToString("yyyy")).ToString("0000") + "/" + Convert.ToInt32(objRefe.FechaApertura.ToString("MM")).ToString("00") + "/" + Convert.ToInt32(objRefe.FechaApertura.ToString("dd")).ToString("00") + "/" + objRefe.NumeroDeReferencia.Trim() + "/" + TipoExpediente.Trim() + "/" + Path.GetFileNameWithoutExtension(nombre.Trim()) + "_" + CUANTOS.ToString() + Path.GetExtension(nombre.Trim());

                    objDocumentosporguia.RutaS3 = Ruta;
                }

                string subio = string.Empty;
                var objS3 = new BucketsS3Repository(_configuration);
                subio = await objS3.SubirObjetoAsync(Documento, S3Buckets, Ruta.Trim());

                if (subio.ToUpper() == "OK")
                {

                    idDocumento = objDocumentosporguiaD.InsertarConsecutivo(objDocumentosporguia);
                }
                else
                {
                    throw new ArgumentException("Ocurrio un error al subir el documento a S3 " + subio.ToUpper());
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message.Trim());

            }

            return idDocumento;
        }
        public async Task<int> SubirDocumentosporGuiaIntegracion(int idTipoDocumento, string Documento, int IdReferencia, int id, string NombreArchivo, CatalogoDeUsuarios GObjUsuario, string S3Buckets)
        {
            int idDocumento = 0;
            try
            {
                var objTipDoc = new CatalogodeTiposDeDocumentos();
                var objTipDocD = new CatalogodeTiposDeDocumentosRepository(_configuration);
                objTipDoc = objTipDocD.BuscarS3(idTipoDocumento, IdReferencia, id);
                if (objTipDoc == null)
                {
                    throw new ArgumentException("hubo un problema en el tipo de documento");
                }

                if (!string.IsNullOrEmpty(NombreArchivo))
                {
                    objTipDoc.NombreExpediente = NombreArchivo.Trim();
                }


                var objRefe = new Referencias();
                var objRefeD = new ReferenciasRepository(_configuration);
                objRefe = objRefeD.Buscar(IdReferencia);
                if (objRefe == null)
                {
                    throw new ArgumentException("Existe un problema con el número de referencia");
                }

                string nombre = objTipDoc.NombreExpediente.Trim() + Path.GetExtension(Documento);

                string TipoExpediente = string.Empty;
                if (objTipDoc.EXPEDIENTEDIGITAL == true)
                {
                    TipoExpediente = "ExpedienteDigital";
                }
                else
                {
                    TipoExpediente = "ExpedienteAA";
                }

                string Ruta = Convert.ToInt32(objRefe.FechaApertura.ToString("yyyy")).ToString("0000") + 
                                "/" + Convert.ToInt32(objRefe.FechaApertura.ToString("MM")).ToString("00") + 
                                "/" + Convert.ToInt32(objRefe.FechaApertura.ToString("dd")).ToString("00") + 
                                "/" + objRefe.NumeroDeReferencia.Trim() + 
                                "/" + TipoExpediente.Trim() + 
                                "/" + nombre.Trim();

                var objDocumentosporguia = new DocumentosporGuia();
                var objDocumentosporguiaD = new DocumentosporGuiaRepository(_configuration);

                objDocumentosporguia.idTipoDocumento = idTipoDocumento;
                objDocumentosporguia.idReferencia = objRefe.IDReferencia;
                objDocumentosporguia.RutaFecha = "";
                objDocumentosporguia.Extension = Path.GetExtension(Documento);
                objDocumentosporguia.IdUsuario = GObjUsuario.IdUsuario;
                objDocumentosporguia.RutaS3 = Ruta;
                objDocumentosporguia.S3 = true;
                objDocumentosporguia.Consecutivo = objTipDoc.Consecutivo;

                string subio = string.Empty;
                var objS3 = new BucketsS3Repository(_configuration);
                subio = await objS3.SubirObjetoAsync(Documento, S3Buckets, Ruta.Trim());

                if (subio.ToUpper() == "OK")
                {

                    idDocumento = objDocumentosporguiaD.InsertarConsecutivo(objDocumentosporguia);
                }
                else
                {
                    throw new ArgumentException("Ocurrio un error al subir el documento a S3 " + subio.ToUpper());
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message.Trim());

            }

            return idDocumento;
        }
    }
}
