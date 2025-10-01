namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text.RegularExpressions;
    using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
    using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPrevalidador;
    using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
    using Microsoft.Extensions.Configuration;
    using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador;
    using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
    using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
    using LibreriaClasesAPIExpertti.Utilities.Helper;
    using System.Data;

    public class ValidarAAADAM
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public ValidarAAADAM(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        /// <summary>
        /// Enviar Archivo a validar por AAADAM
        /// </summary>
        /// <param name="lPath">Ubicacion del Archivo</param>
        /// <param name="lArchivo">Nombre del Archivo</param>
        /// <param name="lDiaJuliano">Dia Juliano</param>
        /// <param name="lAduana">Aduana 2 digitos</param>
        /// <param name="lPassword">Password del Agente Aduanal</param>
        /// <param name="MyConnectionString">Cadena de Conexion</param>
        /// <returns>Id de la tabla de Control de Validacion</returns>
        /// <remarks></remarks>
        /******************************************************************************************************
  Fecha de Modificación: 2025-09-16
  Usuario Modifica: Edward - Cubits
  Funcionalidad: Se agrega parametro CargaManual para identificar si archivo Juliano se carga desde pc usuario.
******************************************************************************************************/
        public int EnviarValidacion(string lPath, string lArchivo, string lDiaJuliano, string lAduana, string lPassword, int MyIdoficina, bool Tipo, bool CargaManual = false)
        {
            int IdValidado = 0;
            try
            {
                var wsAAA = new wsAAADAM.EnvioArchivosClient();
                string Contenido = string.Empty;
                byte[] ArchivoByte = File.ReadAllBytes(lPath + "m" + lArchivo + lDiaJuliano);

                Contenido = Convert.ToBase64String(ArchivoByte);
                wsAAA.enviarAsync(lAduana, "m" + lArchivo + lDiaJuliano, Contenido, lPassword);

                var objValidacion = new ControldeValidacion();
                objValidacion.Archivo = lArchivo;
                objValidacion.Juliano = lDiaJuliano;
                objValidacion.Recibido = false;
                objValidacion.Validacion = true;
                objValidacion.Aduana = lAduana;
                objValidacion.Prevalidador = "010";
                objValidacion.CargaManual = CargaManual;
                var objValidacionD = new ControldeValidacionRepository(_configuration);
                IdValidado = objValidacionD.Insertar(objValidacion, MyIdoficina, false);
            }

            catch (Exception ex)
            {
                throw new ArgumentException("Enviar a Validar(AAADAM): " + ex.Message);
            }
            return IdValidado;
        }
        public async Task<string> RecibirValidacion(string lPath, string lArchivo, string lDiaJuliano, string Aduana, string Password, int IdValidado, int IdUsuario)
        {
            string Respuesta = string.Empty;

            try
            {
                wsAAADAM.EnvioArchivosClient wsAAA = new wsAAADAM.EnvioArchivosClient();
                string[] vRespuesta = await wsAAA.recibirAsync(Aduana, "m" + lArchivo + lDiaJuliano, Password);

                if (vRespuesta != null && vRespuesta.Length > 0 && !string.IsNullOrEmpty(vRespuesta[0]))
                {
                    Respuesta = vRespuesta[0];
                }

                if (vRespuesta != null && vRespuesta.Length > 1 && !string.IsNullOrEmpty(vRespuesta[1]))
                {
                    byte[] ArchivoErr = Convert.FromBase64String(vRespuesta[1]);

                    if (ArchivoErr.Length > 0)
                    {
                        File.WriteAllBytes(Path.Combine(lPath, "m" + lArchivo + ".err"), ArchivoErr);

                        ControldeValidacionRepository objValidacionD = new ControldeValidacionRepository(_configuration);
                        objValidacionD.Modificar(IdValidado, true);
                    }
                }

                if (vRespuesta != null && vRespuesta.Length > 2 && !string.IsNullOrEmpty(vRespuesta[2]))
                {
                    byte[] ArchivoK = Convert.FromBase64String(vRespuesta[2]);

                    if (ArchivoK.Length > 0)
                    {
                        File.WriteAllBytes(Path.Combine(lPath, "k" + lArchivo + lDiaJuliano), ArchivoK);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Recibir a Validar(AAADAM): " + ex.Message);
            }

            return Respuesta;
        }

        public async Task<string> RecibirPago(string lPath, string lArchivo, string lDiaJuliano, string Aduana, string Password, int IdValidado, int IdUsuario)
        {
            string Respuesta = string.Empty;
            try
            {
                var wsAAA = new wsAAADAM.EnvioArchivosClient();
                string[] vRespuesta = await wsAAA.recibirAsync(Aduana, "e" + lArchivo + lDiaJuliano, Password);

                if (vRespuesta[0] != null)
                {
                    Respuesta = vRespuesta[0];
                }

                if (vRespuesta[1] != null)
                {
                    byte[] ArchivoA = Convert.FromBase64String(vRespuesta[1]);

                    if (ArchivoA.Length > 0)
                    {
                        string ArchivoFisico = Path.Combine(lPath, "A" + lArchivo + lDiaJuliano);
                        File.WriteAllBytes(ArchivoFisico, ArchivoA);

                        var objValidacionD = new ControldeValidacionRepository(_configuration);
                        objValidacionD.Modificar(IdValidado, true);

                        // AsignarFirmasDePago(ArchivoFisico, lArchivo.Substring(0, 4), IdUsuario, MyConnectionString);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Recibir a Pagar(AAADAM): " + ex.Message);
            }

            return Respuesta;
        }


        public void AsignarFirmasElectronicas(string fileName, string patente, int idUsuario, string aduana, bool activaPECA)
        {
            var objHelp = new Publicas();
            var objHelper = new Helper();

            if (activaPECA)
            {
                try
                {
                    foreach (Match m in Regex.Matches(objHelp.ReadFileText(fileName), "^.*$", RegexOptions.Multiline))
                    {
                        try
                        {
                            string[] arreglo = Regex.Split(m.ToString(), "^");

                            switch (arreglo[1].Substring(0, 1))
                            {
                                case "F":
                                    string vPedimento = arreglo[1].Substring(1, 7);
                                    var objPedime = new SaaioPedime();
                                    var objPedimeD = new SaaioPedimeRepository(_configuration);
                                    objPedime = objPedimeD.Buscar(vPedimento, patente, aduana);

                                    if (objPedime != null)
                                    {
                                        objPedimeD.ModificarFirmaElectronica(
                                            objPedime.NUM_REFE,
                                            arreglo[1].Substring(8, 8),
                                            idUsuario
                                        );
                                    }
                                    break;

                                case "E":
                                    string vPedimentoE = arreglo[1].Substring(1, 7);
                                    string vRegistro = arreglo[1].Substring(8, 3);
                                    string v = arreglo[1].Substring(11, 4);
                                    string vTipo = arreglo[1].Substring(15, 1);
                                    string vCampo = arreglo[1].Substring(16, 2);
                                    string vNumero = arreglo[1].Substring(18, 2);
                                    string vClaveimpeError = vTipo + vNumero + vRegistro + vCampo;
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
            else
            {
                string miRegex = "^F\\n{0}(?<PEDIMENTO>.{7}).{0}(?<FIRMA>.{0,8}).{9}(?<LINEA>.{0,20}).{0}(?<IMPUESTOS>.{0,12})\\n";
                var re = new Regex(miRegex, RegexOptions.Multiline | RegexOptions.Singleline);
                MatchCollection mc;
                string contenidoDelFicheroOriginal;
                string nombreJuliano = Path.GetFileName(fileName);

                contenidoDelFicheroOriginal = objHelp.MyReadFile(fileName);
                mc = re.Matches(contenidoDelFicheroOriginal);

                for (int i = 0; i < mc.Count; i++)
                {
                    var objPedime = new SaaioPedime();
                    var objPedimeD = new SaaioPedimeRepository(_configuration);
                    string firma = mc[i].Groups["FIRMA"].ToString();
                    string linea = mc[i].Groups["LINEA"].ToString();
                    double impuestos = 0;

                    if (string.IsNullOrEmpty(mc[i].Groups["IMPUESTOS"].ToString()))
                    {
                        impuestos = 0;
                    }
                    else
                    {
                        impuestos = Convert.ToDouble(mc[i].Groups["IMPUESTOS"].ToString());
                    }

                    objPedime = objPedimeD.Buscar(mc[i].Groups["PEDIMENTO"].ToString(), patente, aduana);

                    if (objPedime != null)
                    {
                        objPedimeD.ModificarFirmaElectronicaPECE(
                            objPedime.NUM_REFE,
                            firma,
                            linea,
                            nombreJuliano,
                            ref impuestos,
                            idUsuario
                        );
                    }
                }
            }
        }


        public void AsignarFirmasDePagoNew(string fileName, string patente, int idUsuario, string aduana, bool activaPeca)
        {
            var objHelp = new Helper();

            if (activaPeca)
            {
                try
                {
                    var fileContent = ReadFileText(fileName);
                    var regex = new Regex("^.*$", RegexOptions.Multiline);

                    foreach (Match m in regex.Matches(fileContent))
                    {
                        var arreglo = Regex.Split(m.ToString(), "^");

                        switch (arreglo[1].Substring(0, 2))
                        {
                            case "30":
                                try
                                {
                                    var vPedimento = arreglo[1].Substring(8, 7);
                                    var objPedimeD = new SaaioPedimeRepository(_configuration);
                                    var objPedime = objPedimeD.Buscar(vPedimento, patente, aduana);

                                    if (objPedime != null)
                                    {
                                        objPedime.NUM_CAJA = arreglo[1].Substring(28, 2);
                                        objPedime.DIA_PAGO = Convert.ToDateTime($"{arreglo[1].Substring(50, 2)}/{arreglo[1].Substring(52, 2)}/{arreglo[1].Substring(54, 4)} {arreglo[1].Substring(58, 8)}");
                                        objPedime.FIR_PAGO = arreglo[1].Substring(40, 10);
                                        objPedime.NUM_OPER = arreglo[1].Substring(30, 10);
                                        objPedime.HOR_PAGO = $"{arreglo[1].Substring(58, 2)}{arreglo[1].Substring(60, 2)}{arreglo[1].Substring(62, 2)}";

                                        objPedimeD.ModificarPagoElectronico(objPedime, idUsuario);
                                    }
                                }
                                catch
                                {
                                    // Continue to the next match if an error occurs
                                    continue;
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
            else
            {
                try
                {
                    var fileContent = ReadFileText(fileName);
                    var regex = new Regex("^.*$", RegexOptions.Multiline);

                    foreach (Match m in regex.Matches(fileContent))
                    {
                        var arreglo = Regex.Split(m.ToString(), "^");
                        var nombreJuliano = Path.GetFileName(fileName);

                        for (int i = 0; i <= 1; i++)
                        {
                            if (arreglo[i].Length > 80)
                            {
                                var objPedimeD = new SaaioPedimeRepository(_configuration);
                                var diaPago = string.Empty;
                                var horPago = string.Empty;
                                var firPago = string.Empty;
                                var numOper = string.Empty;
                                var impuestos = string.Empty;
                                var transicion = string.Empty;
                                var referencia = string.Empty;
                                var fechaCorta = string.Empty;
                                var fechaLarga = string.Empty;

                                var pedimento = arreglo[i].Substring(36, 7);
                                var patenteLocal = arreglo[i].Substring(28, 8);
                                var aduanaLocal = arreglo[i].Substring(25, 3);

                                var objPedime = objPedimeD.Buscar(pedimento, patenteLocal, aduanaLocal);

                                if (objPedime != null)
                                {
                                    referencia = objPedime.NUM_REFE;
                                    fechaCorta = arreglo[i].Substring(75, 8);
                                    fechaLarga = $"{fechaCorta.Substring(0, 2)}/{fechaCorta.Substring(2, 2)}/{fechaCorta.Substring(4, 4)} {arreglo[i].Substring(83, 8)}";
                                    diaPago = fechaLarga;
                                    firPago = $"{fechaCorta.Substring(6, 2)}{arreglo[i].Substring(83, 8)}";
                                    numOper = arreglo[i].Substring(91, 14);
                                    horPago = arreglo[i].Substring(83, 8);
                                    impuestos = arreglo[i].Substring(61, 14);
                                    transicion = arreglo[i].Substring(105, 20);

                                    objPedimeD.ModificarPagoElectronicoPECE(referencia, Convert.ToDateTime(diaPago), firPago, numOper, horPago, impuestos, transicion, nombreJuliano, idUsuario);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
        }

        public string ReadFileText(string path)
        {
            using (var sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }


    }
}
