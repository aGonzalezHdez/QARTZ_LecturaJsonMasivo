using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesBuscar;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesDocumentos.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesDocumentos
{
    public class DocumentosRepository : IDocumentosRepository
    {


        public string SConexion { get; set; }
        string IDocumentosRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        //private object objCentralizar;

        public DocumentosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public Errores SubirDocumento()
        {
            Errores objErrores = new();
            List<string> lstErrores = new();

            int TotalError = 0;
            int TotalProcesadas = 0;


            // Buscar información del usuario
            CatalogoDeUsuariosRepository GObjUsuarioRepository = new(_configuration);
            CatalogoDeUsuarios GObjUsuario = GObjUsuarioRepository.BuscarPorId(6301);
            if (GObjUsuario == null || GObjUsuario.IdUsuario == 0)
            {
                throw new ArgumentException("No se encontró el usuario");
            }

            // Verificar ubicación de archivos
            UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
            UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(250);
            if (objRuta == null || string.IsNullOrWhiteSpace(objRuta.Ubicacion) || !Directory.Exists(objRuta.Ubicacion))
            {
                throw new Exception("El directorio no está disponible o no existe");
            }

            //Obtener lista de archivos
            string[] archivos = Directory.GetFiles(objRuta.Ubicacion.Trim(), "*.pdf");
            //string[] archivos = Directory.GetFiles(@"D:\atgomez\Downloads\MVyHCSIEMENS_1", "*.pdf");

            int TotalArchivos = archivos.Length;

            if (TotalArchivos > 0)
            {
                foreach (string Archivo in archivos)
                {
                    string? NombreArchivo = null;
                    string Referencia = string.Empty;
                    string Tipo = string.Empty;
                    string Aduana = string.Empty;
                    string Patente = string.Empty;
                    string Pedimento = string.Empty;
                    string Valor = string.Empty;

                    int IdReferencia = 0;
                    int idTipoDocumento = 0;
                    ReferenciasRepository objRefeD = new(_configuration);

                    try
                    {
                        // Obtener el nombre y cadena base del archivo
                        NombreArchivo = Path.GetFileName(Archivo);
                        string Cadena = Path.GetFileNameWithoutExtension(NombreArchivo);

                        // Extraer el valor y tipo del nombre del archivo usando Regex                      
                        Regex regex = new(@"([A-Za-z0-9]+(?=\s(MV|HC)$))|(\d{3})\s(\d{4})\s(\d{7})(?=\s(MV|HC)$)|(\d{4})\s(\d{3})\s(\d{7})(?=\s(MV|HC)$)");
                        Match match = regex.Match(Cadena);

                        if (match.Success)
                        {
                            if (match.Groups[1].Value != "" && match.Groups[2].Value != "")
                            {
                                Referencia = match.Groups[1].Value;
                                Tipo = match.Groups[2].Value;
                                Valor = Referencia;
                            }

                            if (match.Groups[3].Value != "" && match.Groups[4].Value != "" && match.Groups[5].Value != "" && match.Groups[6].Value != "")
                            {
                                Aduana = match.Groups[3].Value;
                                Patente = match.Groups[4].Value;
                                Pedimento = match.Groups[5].Value;
                                Tipo = match.Groups[6].Value;
                                Valor = Pedimento;
                            }

                            if (match.Groups[7].Value != "" && match.Groups[8].Value != "" && match.Groups[9].Value != "" && match.Groups[10].Value != "")
                            {                              
                                Aduana = match.Groups[8].Value;
                                Patente = match.Groups[7].Value;
                                Pedimento = match.Groups[9].Value;
                                Tipo = match.Groups[10].Value;
                                Valor = Pedimento;
                            }
                        }

                        // Cargar datos según la longitud de "Valor"
                        BuscarRepository? listBuscarRepository = new(_configuration);
                        DataTable dt = (Valor.Length == 7) ? listBuscarRepository.CargarBusquedaDT(10, 5, Pedimento, GObjUsuario.IDDatosDeEmpresa, "")
                                     : (Valor.Length == 13) ? listBuscarRepository.CargarBusquedaDT(10, 4, Referencia, GObjUsuario.IDDatosDeEmpresa, "")
                                     : throw new ArgumentException($"Formato de nombre inválido en el documento: {NombreArchivo}");

                        // Procesar filas en el DataTable
                        if (dt?.Rows.Count == 0) throw new Exception($"No se encontraron Referencias relacionadas: {NombreArchivo}");

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string ReferenciaArchivo = dt.Rows[i]["Referencia"]?.ToString() ?? string.Empty;
                            string PedimentoArchivo = dt.Rows[i]["Pedimento"]?.ToString() ?? string.Empty;
                            string PatenteArchivo = dt.Rows[i]["Patente"]?.ToString() ?? string.Empty;
                            string AduanaArchivo = dt.Rows[i]["Aduana"]?.ToString() ?? string.Empty;

                            // Validaciones separadas para mayor claridad
                            bool esReferenciaValida = (ReferenciaArchivo == Referencia);
                            bool esPedimentoValido = (PedimentoArchivo == Pedimento && PatenteArchivo == Patente && AduanaArchivo == Aduana);

                            // Si cumple alguna de las validaciones, entra en el if
                            if (esReferenciaValida || esPedimentoValido)
                            {
                                IdReferencia = (int)dt.Rows[i]["idReferencia"];

                                // Buscar referencia
                                Referencias objRefe = objRefeD.Buscar(IdReferencia) ?? throw new ArgumentException($"Referencia inválida en el documento: {NombreArchivo}");

                                // Asignar el tipo de documento según el tipo y la empresa
                                idTipoDocumento = Tipo switch
                                {
                                    "MV" when objRefe.IDDatosDeEmpresa == 1 => 1140,
                                    "MV" when objRefe.IDDatosDeEmpresa == 2 => 1143,
                                    "HC" when objRefe.IDDatosDeEmpresa == 1 => 1207,
                                    "HC" when objRefe.IDDatosDeEmpresa == 2 => 1206,
                                    _ => throw new ArgumentException($"Formato de nombre inválido en el documento: {NombreArchivo}")
                                };

                                // Agregar el documento
                                CentralizarDocsS3 objCentralizar = new(_configuration);
                                int IdDocumento = objCentralizar.AgregarDocumentosSynchronous(Archivo, objRefe, idTipoDocumento, NombreArchivo, 0, GObjUsuario, false, "");

                                if (IdDocumento == 0)
                                    throw new Exception($"No se subio el archivo: {NombreArchivo} de la referencia {objRefe.NumeroDeReferencia}");

                                TotalProcesadas++;
                                File.Delete(Archivo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores
                        lstErrores.Add(ex.Message);
                        TotalError++;
                        continue; // Continuar con el siguiente archivo
                    }
                }
            }
            else
            {
                lstErrores.Add("En la carpeta no hay documentos de MV y HC a cargar.");
            }

            // Asignar resultados al objeto de errores
            objErrores.ArchivosProcesados = TotalProcesadas;
            objErrores.ArchivosErrores = TotalError;
            objErrores.TotalArchivos = TotalArchivos;
            objErrores.ListaErrores = lstErrores;

            return objErrores;
        }
    }
}
