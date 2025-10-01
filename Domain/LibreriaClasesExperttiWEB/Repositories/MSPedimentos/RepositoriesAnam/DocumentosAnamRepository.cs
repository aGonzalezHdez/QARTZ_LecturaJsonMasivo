using LibreriaClasesAPIExpertti.Entities.EntitiesAnam;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesEquialencia;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesAnam.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica;
using LibreriaClasesAPIExpertti.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesAnam
{
    public class ArchivosActualizarMasivo
    {
        public string Master { get; set; }
        public string Patente { get; set; }
        public int IdUsuario { get; set; }
        public int IdReferencia { get; set; }
    }
    public class DocumentosAnamRepository : IDocumentosAnamRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        private readonly WebApisRepository _webApisRepository;
        private readonly CatalogodeSellosDigitalesRepository _catalogodeSellosDigitalesRepository;
        private readonly UbicaciondeArchivosRepository _ubicaciondeArchivosRepository;
        private TokenService _tokenService;


        public DocumentosAnamRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
            _webApisRepository = new WebApisRepository(configuration);
            _catalogodeSellosDigitalesRepository = new CatalogodeSellosDigitalesRepository(configuration);
            _ubicaciondeArchivosRepository = new UbicaciondeArchivosRepository(configuration);
            _tokenService = new TokenService();
        }
        public async Task GuardarRequestResponse(RequestAnam request, string message, int idReferencia, int idDocumento, int? folioNumero, string folio, string errorLoc, string errorMsg, string errorType, int Consecutivo)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SConexion))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("NET_REQUEST_RESPONSE_ANAM", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros del request
                        cmd.Parameters.AddWithValue("@Aduana", request.Aduana);
                        cmd.Parameters.AddWithValue("@Patente", request.Patente);
                        cmd.Parameters.AddWithValue("@idReferencia ", idReferencia);
                        cmd.Parameters.AddWithValue("@idDocumento ", idDocumento);
                        cmd.Parameters.AddWithValue("@Email", request.Email);
                        cmd.Parameters.AddWithValue("@TipoDocumento", request.Documentos.First().TipoDocumento);
                        cmd.Parameters.AddWithValue("@UsuarioConsulta", request.Documentos.First().UsuarioConsulta);
                        cmd.Parameters.AddWithValue("@MedioTransporte", (object)request.Documentos.First().MedioTransporte ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ClavePedimento", (object)request.Documentos.First().ClavePedimento ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Consecutivo ", Consecutivo);
                        // Parámetros del response (éxito o error)
                        cmd.Parameters.AddWithValue("@Message", (object)message ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FolioNumero", (object)folioNumero ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Folio", (object)folio ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ErrorLoc", (object)errorLoc ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ErrorMsg", (object)errorMsg ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ErrorType", (object)errorType ?? DBNull.Value);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de error en la función (se puede registrar en logs si es necesario)
                Console.WriteLine("Error al guardar en la base de datos: " + ex.Message);
            }
        }

        public List<ArchivosActualizarMasivo> ObtenerArchivos()
        {
            var list = new List<ArchivosActualizarMasivo>();

            using (var cn = new SqlConnection(SConexion))
            using (var cmd = new SqlCommand("NET_PENDIENTES_ARCHIVO_ANAM", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new ArchivosActualizarMasivo
                            {
                                Master = dr["FolioMaster"] != DBNull.Value ? dr["FolioMaster"].ToString() : string.Empty,
                                IdReferencia = dr["idReferencia"] != DBNull.Value ? Convert.ToInt32(dr["idReferencia"]) : 0,
                                IdUsuario = dr["IdUsuario"] != DBNull.Value ? Convert.ToInt32(dr["IdUsuario"]) : 0,
                                Patente = dr["Patente"] != DBNull.Value ? dr["Patente"].ToString() : string.Empty,
                            };
                            list.Add(obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return list;
        }
        public List<ArchivosActualizarMasivo> ObtenerMasterSinFolios()
        {
            var list = new List<ArchivosActualizarMasivo>();

            using (var cn = new SqlConnection(SConexion))
            using (var cmd = new SqlCommand("NET_PENDIENTES_FOLIO_ANAM", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new ArchivosActualizarMasivo
                            {
                                Master = dr["FolioMaster"] != DBNull.Value ? dr["FolioMaster"].ToString() : string.Empty,
                                IdReferencia = dr["idReferencia"] != DBNull.Value ? Convert.ToInt32(dr["idReferencia"]) : 0,
                                IdUsuario = dr["IdUsuario"] != DBNull.Value ? Convert.ToInt32(dr["IdUsuario"]) : 0,
                                Patente = dr["Patente"] != DBNull.Value ? dr["Patente"].ToString() : string.Empty,
                            };
                            list.Add(obj);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return list;
        }

        public async Task ActualizarRequestResponse(string folioMaster, int? folioNumero, string folio, int IdDocumento)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SConexion))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("NET_UPDATE_REQUEST_RESPONSE_ANAM", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros del request

                        cmd.Parameters.AddWithValue("@FolioNumero", (object)folioNumero ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Folio", (object)folio ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FolioMaster", (object)folioMaster ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IdDocumento", (object)IdDocumento ?? DBNull.Value);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de error en la función (se puede registrar en logs si es necesario)
                Console.WriteLine("Error al guardar en la base de datos: " + ex.Message);
            }
        }
        public async Task<Boolean> EliminarFolio(string Folio)
        {
            Boolean resultado = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(SConexion))
                {
                    await conn.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("NET_DELETE_FOLIO_ANAM", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros del request
                        cmd.Parameters.AddWithValue("@Folio", Folio);
                        await cmd.ExecuteNonQueryAsync();
                        resultado = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de error en la función (se puede registrar en logs si es necesario)
                Console.WriteLine("Error al guardar en la base de datos: " + ex.Message);
            }
            return resultado;
        }
        public async Task<ResultadoOperacion> ProcesarStatusBatchAsync(string master, string patente)
        {
            try
            {
                WebApis objWebApi = _webApisRepository.Buscar(51);
                CatalogodeSellosDigitales catalogodeSellosDigitales = _catalogodeSellosDigitalesRepository.BuscarporPatente(patente);
                var urlDigitalizacion = $"{objWebApi.URL}/repadi/master/{master}";

                string token = await _tokenService.GetToken(
                    objWebApi.URL,
                    catalogodeSellosDigitales.UsuarioANAM,
                    catalogodeSellosDigitales.passwordAnam
                );

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(urlDigitalizacion);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ResultadoOperacion.Fallido($"Error API externa: {errorContent}", (int)response.StatusCode);
                }

                var result = await response.Content.ReadAsStringAsync();
                var datos = JsonConvert.DeserializeObject<ResponseRepadiMaster>(result);
                
                await ProcesarDocumentos(datos.documentos, master);

                return ResultadoOperacion.Exitoso("Procesado Correctamente");
            }
            catch (Exception error)
            {
                return ResultadoOperacion.Fallido(error.Message, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task ProcesarDocumentos(List<ResponseRepadiMasterDocumento> documentos, string master)
        {
            int folioNo = 0;
            foreach (var item in documentos)
            {
                int? folioNumero = ++folioNo;
                string folio = item.folio;

                if (string.IsNullOrEmpty(folio)) continue;

                var ubicacion = _ubicaciondeArchivosRepository.Buscar(180);
                await ActualizarRequestResponse(
                    master,
                    folioNumero.Value,
                    folio,
                    0 // idS3 (valor hardcodeado del ejemplo original)
                );
            }
        }

        public async Task<ResultadoOperacion> ProcesarRenombrarBatchAsync(string master, string patente, int idReferencia, int idUsuario)
        {
            try
            {
                WebApis objWebApi = _webApisRepository.Buscar(51);
                CatalogodeSellosDigitales catalogodeSellosDigitales = _catalogodeSellosDigitalesRepository.BuscarporPatente(patente);
                var urlDigitalizacion = $"{objWebApi.URL}/repadi/master/{master}";

                string token = await _tokenService.GetToken(
                    objWebApi.URL,
                    catalogodeSellosDigitales.UsuarioANAM,
                    catalogodeSellosDigitales.passwordAnam
                );

                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.GetAsync(urlDigitalizacion);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return ResultadoOperacion.Fallido($"Error API externa: {errorContent}", (int)response.StatusCode);
                }

                var result = await response.Content.ReadAsStringAsync();
                var datos = JsonConvert.DeserializeObject<ResponseRepadiMaster>(result);

                await ProcesarRenombrarDocumentos(datos.documentos, master,idReferencia,idUsuario);

                return ResultadoOperacion.Exitoso("Procesado Correctamente");
            }
            catch (Exception error)
            {
                return ResultadoOperacion.Fallido(error.Message, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task ProcesarRenombrarDocumentos(List<ResponseRepadiMasterDocumento> documentos, string master, int idReferencia, int idUsuario)
        {
            int folioNo = 0;
            foreach (var item in documentos)
            {
                int? folioNumero = ++folioNo;
                string folio = item.folio;

                string NombreArchivo = master + "-" + (folioNo + 1).ToString();
                UbicaciondeArchivos ubicaciondeArchivos = new UbicaciondeArchivos();
                UbicaciondeArchivosRepository ubicaciondeArchivosRepository = new UbicaciondeArchivosRepository(_configuration);
                ubicaciondeArchivos = ubicaciondeArchivosRepository.Buscar(180);

                if (string.IsNullOrEmpty(folio)) continue;

                string archivo = ubicaciondeArchivos.Ubicacion + master + "-" + NombreArchivo + ".pdf";

                var ubicacion = _ubicaciondeArchivosRepository.Buscar(180);

                Byte[] bytes = Convert.FromBase64String(item.archivo);
                System.IO.File.WriteAllBytes(archivo, bytes);
                CentralizarDocsS3 centralizarDocS3 = new CentralizarDocsS3(_configuration);
                Referencias referencias = new Referencias();
                ReferenciasRepository referenciasRepository = new ReferenciasRepository(_configuration);
                referencias = referenciasRepository.Buscar(idReferencia);
                var GObjUsuario = new CatalogoDeUsuarios();
                var GObjUsuarioRepository = new CatalogoDeUsuariosRepository(_configuration);
                GObjUsuario = GObjUsuarioRepository.BuscarPorId(idUsuario);
                if (GObjUsuario == null)
                {
                    throw new ArgumentException("No se encontró el usuario");
                }
                int idS3 = await centralizarDocS3.AgregarDocumentos(archivo, referencias, 1152, NombreArchivo, 0, GObjUsuario, false, "");

                await ActualizarRequestResponse(
                    master,
                    folioNumero.Value,
                    folio,
                    idS3
                );
            }
        }
    }
}

public class ResultadoOperacion
{
    public bool Exito { get; set; }
    public string Mensaje { get; set; }
    public int CodigoEstado { get; set; }

    public static ResultadoOperacion Exitoso(string mensaje) => new()
    {
        Exito = true,
        Mensaje = mensaje,
        CodigoEstado = StatusCodes.Status200OK
    };

    public static ResultadoOperacion Fallido(string mensaje, int codigoEstado) => new()
    {
        Exito = false,
        Mensaje = mensaje,
        CodigoEstado = codigoEstado
    };
}