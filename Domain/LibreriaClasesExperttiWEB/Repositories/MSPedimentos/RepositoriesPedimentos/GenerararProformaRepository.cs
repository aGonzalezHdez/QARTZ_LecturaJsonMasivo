using DocumentFormat.OpenXml.Office.Word;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;



namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos
{
    public class GenerararProformaRepository : IGenerararProformaRepository
    {
        private readonly IConfiguration _configuration;
        public string sConexion { get; set; }

        public GenerararProformaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }



        //public async Task<(string salida, string error, int codigoSalida, string rutaArchivoDestino)> GenerarProformaAsync(GeneraProformaRequest request)
        //{
        //    string referencia = request.Referencia;
        //    string archivoPdf = Path.Combine(request.Ruta, referencia + ".pdf");

        //    if (File.Exists(archivoPdf))
        //    {
        //        try
        //        {
        //            File.Delete(archivoPdf);
        //        }
        //        catch (Exception ex)
        //        {
        //            return ($"Error al eliminar archivo previo: {ex.Message}", string.Empty, -1, string.Empty);
        //        }
        //    }

        //    string rutaDestino = ObtenerRutaDestinoProforma(referencia);
        //    var resultadoTotal = new StringBuilder();
        //    var erroresTotal = new StringBuilder();
        //    int ultimoCodigoSalida = 0;

        //    var comandos = new[]
        //    {
        //        new ProcessStartInfo
        //        {
        //            FileName = @"C:\CASAWIN\CSAAIWIN-SQL\CMDCierreSaai.exe",
        //            Arguments = $"Referencia={referencia}",
        //            WorkingDirectory = @"C:\CASAWIN\CSAAIWIN-SQL",
        //            UseShellExecute = false,
        //            RedirectStandardOutput = true,
        //            RedirectStandardError = true,
        //            CreateNoWindow = true
        //        },
        //        new ProcessStartInfo
        //        {
        //            FileName = @"C:\CASAWIN\CSAAIWIN-SQL\ImprimePed.exe",
        //            Arguments = $"Referencia={referencia},Ruta={archivoPdf},Tipo=N,BloqueF=S,Piep=S,IMPRESORA=PDF24",
        //            WorkingDirectory = @"C:\CASAWIN\CSAAIWIN-SQL",
        //            UseShellExecute = false,
        //            RedirectStandardOutput = true,
        //            RedirectStandardError = true,
        //            CreateNoWindow = true
        //        }
        //    };

        //    foreach (var psi in comandos)
        //    {
        //        if (!File.Exists(psi.FileName))
        //        {
        //            erroresTotal.AppendLine($"❌ El archivo ejecutable no existe: {psi.FileName}");
        //            return (resultadoTotal.ToString(), erroresTotal.ToString(), -1, string.Empty);
        //        }

        //        try
        //        {
        //            using (var proceso = Process.Start(psi))
        //            {
        //                if (proceso == null)
        //                    throw new InvalidOperationException("No se pudo iniciar el proceso.");

        //                proceso.WaitForExit();
        //                ultimoCodigoSalida = proceso.ExitCode;

        //                resultadoTotal.AppendLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Ejecutable: {psi.FileName} finalizado con código {ultimoCodigoSalida}");

        //                if (ultimoCodigoSalida != 0)
        //                {
        //                    erroresTotal.AppendLine($"⚠️ {psi.FileName} terminó con código de salida {ultimoCodigoSalida}");
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            erroresTotal.AppendLine($"❌ Error ejecutando {psi.FileName}: {ex.Message}");
        //            return (resultadoTotal.ToString(), erroresTotal.ToString(), -1, string.Empty);
        //        }
        //    }

        //    try
        //    {
        //        await EsperarArchivoGeneradoAsync(archivoPdf);

        //        var directorioDestino = Path.GetDirectoryName(rutaDestino);
        //        if (!Directory.Exists(directorioDestino))
        //            Directory.CreateDirectory(directorioDestino!);

        //        await CopiarYEliminarArchivoAsync(archivoPdf, rutaDestino);
        //    }
        //    catch (Exception ex)
        //    {
        //        erroresTotal.AppendLine($"❌ Error en copia/eliminación: {ex.Message}");
        //        return (resultadoTotal.ToString(), erroresTotal.ToString(), -2, string.Empty);
        //    }

        //    return (resultadoTotal.ToString(), erroresTotal.ToString(), ultimoCodigoSalida, rutaDestino);
        //}

        public async Task<(string salida, string error, int codigoSalida, string rutaArchivoDestino)> GenerarProformaAsync(GeneraProformaRequest request)
        {
            string referencia = request.Referencia;
            string archivoPdf = Path.Combine(request.Ruta, referencia + ".pdf");

            if (File.Exists(archivoPdf))
            {
                try { File.Delete(archivoPdf); }
                catch (Exception ex) { return ($"Error al eliminar archivo previo: {ex.Message}", string.Empty, -1, string.Empty); }
            }

            string rutaDestino = ObtenerRutaDestinoProforma(referencia);
            var resultadoTotal = new StringBuilder();
            var erroresTotal = new StringBuilder();
            int ultimoCodigoSalida = 0;

            var comandos = new[]
            {
        new ProcessStartInfo
        {
            FileName = @"C:\CASAWIN\CSAAIWIN-SQL\CMDCierreSaai.exe",
            Arguments = $"Referencia={referencia}",
            WorkingDirectory = @"C:\CASAWIN\CSAAIWIN-SQL",
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = true
        },
        new ProcessStartInfo
        {
            FileName = @"C:\CASAWIN\CSAAIWIN-SQL\ImprimePed.exe",
            Arguments = $"Referencia={referencia},Ruta={archivoPdf},Tipo=N,BloqueF=S,Piep=S,IMPRESORA=PDF24",
            WorkingDirectory = @"C:\CASAWIN\CSAAIWIN-SQL",
            UseShellExecute = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            CreateNoWindow = true
        }
    };

            foreach (var psi in comandos)
            {
                if (!File.Exists(psi.FileName))
                {
                    erroresTotal.AppendLine($"❌ El archivo ejecutable no existe: {psi.FileName}");
                    return (resultadoTotal.ToString(), erroresTotal.ToString(), -1, string.Empty);
                }

                try
                {
                    using var proceso = new Process { StartInfo = psi, EnableRaisingEvents = true };

                    var salidaBuilder = new StringBuilder();
                    var errorBuilder = new StringBuilder();

                    //proceso.OutputDataReceived += (s, e) => { if (e.Data != null) salidaBuilder.AppendLine(e.Data); };
                    //proceso.ErrorDataReceived += (s, e) => { if (e.Data != null) errorBuilder.AppendLine(e.Data); };

                    proceso.Start();
                    //proceso.BeginOutputReadLine();
                    //proceso.BeginErrorReadLine();

                    var timeout = TimeSpan.FromMinutes(2);
                    var delayTask = Task.Delay(timeout);
                    var waitTask = Task.Run(() => proceso.WaitForExit());

                    var completedTask = await Task.WhenAny(waitTask, delayTask);

                    if (completedTask == delayTask)
                    {
                        // Timeout alcanzado, forzamos cierre
                        try
                        {
                            proceso.Kill(true);
                            erroresTotal.AppendLine($"⏱ Proceso {psi.FileName} forzado a cerrar tras {timeout.TotalMinutes} minutos.");
                            ultimoCodigoSalida = -99; // Código personalizado para indicar cierre forzado
                        }
                        catch (Exception killEx)
                        {
                            erroresTotal.AppendLine($"❌ Error al forzar cierre de {psi.FileName}: {killEx.Message}");
                            return (resultadoTotal.ToString(), erroresTotal.ToString(), -99, string.Empty);
                        }
                    }
                    else
                    {
                        ultimoCodigoSalida = proceso.ExitCode;
                        resultadoTotal.AppendLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Ejecutable: {psi.FileName} finalizado con código {ultimoCodigoSalida}");
                        resultadoTotal.AppendLine(salidaBuilder.ToString());

                        if (ultimoCodigoSalida != 0)
                        {
                            erroresTotal.AppendLine($"⚠️ {psi.FileName} terminó con código de salida {ultimoCodigoSalida}");
                            erroresTotal.AppendLine(errorBuilder.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    erroresTotal.AppendLine($"❌ Error ejecutando {psi.FileName}: {ex.Message}");
                    return (resultadoTotal.ToString(), erroresTotal.ToString(), -1, string.Empty);
                }
            }

            try
            {
                await EsperarArchivoGeneradoAsync(archivoPdf);

                var directorioDestino = Path.GetDirectoryName(rutaDestino);
                if (!Directory.Exists(directorioDestino))
                    Directory.CreateDirectory(directorioDestino!);

                await CopiarYEliminarArchivoAsync(archivoPdf, rutaDestino);
            }
            catch (Exception ex)
            {
                erroresTotal.AppendLine($"❌ Error en copia/eliminación: {ex.Message}");
                return (resultadoTotal.ToString(), erroresTotal.ToString(), -2, string.Empty);
            }

            return (resultadoTotal.ToString(), erroresTotal.ToString(), ultimoCodigoSalida, rutaDestino);
        }



        //public string TipodeImpresion(string Referencia)
        //{

        //}

        private async Task CopiarYEliminarArchivoAsync(string origen, string destino)
        {
            File.Copy(origen, destino, true);

            if (!File.Exists(destino))
                throw new ApplicationException("La copia del archivo falló.");

            File.Delete(origen);

            if (File.Exists(origen))
                throw new ApplicationException("No se pudo eliminar el archivo original.");
        }

        public ValidacionArchivoResult RealizarValidaciones(string rutaArchivo)
        {
            var validaciones = new List<string>();

            if (!File.Exists(rutaArchivo))
                throw new ApplicationException("El archivo no existe en la ruta esperada.");

            var info = new FileInfo(rutaArchivo);

            if (info.Length == 0)
                validaciones.Add("El archivo está vacío.");

            if (!info.Extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                validaciones.Add($"Extensión no válida: {info.Extension}");

            if (validaciones.Count == 0)
                validaciones.Add("Archivo válido");

            return new ValidacionArchivoResult
            {
                Archivo = info.Name,
                Tamaño = info.Length,
                Validaciones = validaciones
            };
        }

        private string ObtenerBatPorTipoImpresion(string NumerodeReferencia)
        {
            string NombreBat = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(sConexion))
                {
                    using (SqlCommand command = new SqlCommand("NET_DATOS_PROFORMA_S3_TIPOIMPRESION", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@NUMERODEREFERENCIA", SqlDbType.VarChar, 15).Value = NumerodeReferencia;
                        command.Parameters.Add("@NOMBREBAT", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output;


                        connection.Open();
                        command.ExecuteNonQuery();
                        NombreBat = (string)command.Parameters["@NOMBREBAT"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al bat para la impresión de Proforma.", ex);
            }
            return NombreBat;
        }

        private string ObtenerRutaDestinoProforma(string NumerodeReferencia)
        {
            string RutaDestino = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(sConexion))
                {
                    using (SqlCommand command = new SqlCommand("NET_DATOS_PROFORMA_S3_RUTADESTINO", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@NUMERODEREFERENCIA", SqlDbType.VarChar, 15).Value = NumerodeReferencia;
                        command.Parameters.Add("@RUTADESTINO", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;


                        connection.Open();
                        command.ExecuteNonQuery();
                        RutaDestino = (string)command.Parameters["@RUTADESTINO"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al Obtener Ruta Destino Proforma.", ex);
            }
            return RutaDestino;
        }

        private async Task EsperarArchivoGeneradoAsync(string rutaArchivo, int maxIntentos = 20, int intervaloMs = 500)
        {
            int intentos = 0;
            while (intentos < maxIntentos)
            {
                if (File.Exists(rutaArchivo))
                {
                    try
                    {
                        using (FileStream stream = File.Open(rutaArchivo, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            return; // archivo existe y está accesible
                        }
                    }
                    catch (IOException)
                    {
                        // archivo existe pero está bloqueado, esperamos
                    }
                }

                intentos++;
                await Task.Delay(intervaloMs);
            }

            throw new ApplicationException("El archivo PDF no se generó o no está disponible después del tiempo esperado.");
        }

        public async Task<string> EjecutarProcesoAsync(string referencia)
        {
            string rutaEjecutable = @"C:\Temporal\Proformas\Proformas.exe";
            string argumentos = $"\"{referencia}\"";

            try
            {
                using (var proceso = new Process())
                {
                    proceso.StartInfo.FileName = rutaEjecutable;
                    proceso.StartInfo.Arguments = argumentos;
                    proceso.StartInfo.UseShellExecute = false;
                    proceso.StartInfo.RedirectStandardOutput = true;
                    proceso.StartInfo.RedirectStandardError = true;
                    proceso.StartInfo.CreateNoWindow = true;

                    proceso.Start();

                    // Leer salida estándar (opcional)
                    string salida = await proceso.StandardOutput.ReadToEndAsync();
                    string errores = await proceso.StandardError.ReadToEndAsync();

                    await proceso.WaitForExitAsync();

                    if (proceso.ExitCode == 0)
                    {
                        return $"✅ Proceso terminado correctamente. Salida: {salida}";
                    }
                    else
                    {
                        return $"⚠️ Proceso terminó con errores. Código: {proceso.ExitCode}, Errores: {errores}";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"❌ Error al ejecutar el proceso: {ex.Message}";
            }
        }
    }
}
