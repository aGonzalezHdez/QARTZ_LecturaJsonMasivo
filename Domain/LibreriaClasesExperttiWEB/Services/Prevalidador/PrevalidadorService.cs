/******************************************************************************************************
  Fecha de Modificación: 2025-09-16
  Usuario Modifica: Edward - Cubits
  Funcionalidad: Se agrega parametro CargaManual para identificar si archivo Juliano se carga desde pc usuario.
******************************************************************************************************/
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPrevalidador;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using Microsoft.VisualBasic;
using NPOI.SS.Formula.Functions;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using static LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador.PrevalidadorResponse;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador;

namespace LibreriaClasesAPIExpertti.Services.Prevalidador
{
    public class PrevalidadorService
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;
        private string MyPedimentos = string.Empty;
        int iSinErrores = default;
        private CatalogoDeUsuarios GObjUsuario;
        public PrevalidadorService(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public PrevalidadorResponse prevalidar(int IdReferencia, int IdUsuario, int IDDatosDeEmpresa, string Aduana, string Patente)
        {
            PrevalidadorResponse response = new PrevalidadorResponse();
            List<PrevalidadorError> errores = new List<PrevalidadorError>();

            var objRefenciaData = new ReferenciasRepository(_configuration);
            var objReferencia = new Referencias();
            string MiPedimento = "";

            objReferencia = objRefenciaData.Buscar(IdReferencia, IDDatosDeEmpresa);
            if (objReferencia == null)
            {
                throw new Exception("La Referencia no existe");
            }


            var SAAIOPEDIMEDATA = new SaaioPedimeRepository(_configuration);
            var SAAIOPEDIME = new SaaioPedime();

            SAAIOPEDIME = SAAIOPEDIMEDATA.Buscar(objReferencia.NumeroDeReferencia);
            if (SAAIOPEDIME == null)
            {
                throw new Exception("No existe el Pedimento");
            }
            if (SAAIOPEDIME.NUM_PEDI == null)
            {
                MiPedimento = "";
            }
            else
            {
                MiPedimento = SAAIOPEDIME.NUM_PEDI;
            }

            var ObjBitacoraDePreval = new BitacoraDePrevalRepository(_configuration);

            ObjBitacoraDePreval.Delete(SAAIOPEDIME.NUM_REFE.Trim());

            var ObjCatalogodePreval = new CatalogoDePreval();
            var ObjCatalogodePrevalData = new CatalogodePrevalRepository(_configuration);

            ObjCatalogodePrevalData.EjecutarPrevalidadores(MiPedimento, SAAIOPEDIME.NUM_REFE.Trim(), IdUsuario);

            var objloaddgvErrores = new BitacoraDePrevalRepository(_configuration);
            var dtbErrores = new DataTable();
            dtbErrores = objloaddgvErrores.LlenarPorReferenciaErroresGrid(objReferencia.NumeroDeReferencia);


            var results = dtbErrores.AsEnumerable().Where(row => row.Field<int>("TIPODEERROR") == 1).FirstOrDefault();

            if (results is not null)
            {
                response.SinErrores = 1;
                response.mensaje = "Pre-Validación finalizada con errores";
            }
            else
            {
                response.SinErrores = 0;
                response.mensaje = "Pre-Validación finalizada sin errores";
            }

            foreach (DataRow row in dtbErrores.Rows)
            {
                PrevalidadorError prevalidadorError = new PrevalidadorError();
                prevalidadorError.IdError = row["idError"].ToString();
                prevalidadorError.MensajeError = row["MENSAJEERROR"].ToString();
                prevalidadorError.TipoDeError = row["TIPODEERROR"].ToString();
                prevalidadorError.Sugerencia = row["SUGERENCIA"].ToString();
                prevalidadorError.Referencia = row["REFERENCIA"].ToString();
                prevalidadorError.Consecutivo = row["CONSECUTIVO"].ToString();
                prevalidadorError.Justificado = row["JUSTIFICADO"].ToString();
                prevalidadorError.PartidaCove = row["PARTIDACOVE"].ToString();
                prevalidadorError.PartidaPedi = row["PARTIDAPEDI"].ToString();
                prevalidadorError.Pedimento = row["PEDIMENTO"].ToString();
                errores.Add(prevalidadorError);
            }

            response.errores = errores;
            return response;
        }
        private async Task<Dictionary<string, object>> prevalidador010(string Origen, string Aduana, int MyIDOficina, CatalogoDeAgentesAduanales objAA,bool CargaManual =false)
        {
            //string result = string.Empty;
            Dictionary<string, object> result = new Dictionary<string, object>();
            var objAAADAM = new ValidarAAADAM(_configuration);
            string vPath = Path.GetDirectoryName(Origen.Trim()) + @"\";
            string vArchivo = Strings.Mid(Path.GetFileNameWithoutExtension(Origen.Trim()), 2, Path.GetFileNameWithoutExtension(Origen.Trim()).Length);
            string vJuliano = Path.GetExtension(Origen.Trim());
            string vAduana = Strings.Mid(Aduana, 1, 2);

            int IdValidado = 0;

            IdValidado =  objAAADAM.EnviarValidacion(vPath, vArchivo, vJuliano, vAduana, objAA.PasswordValidacion, MyIDOficina, false, CargaManual);

            if (IdValidado != 0)
            {
                result["message"] = "¡Archivo enviado con éxito!";
            }
            else
            {
                result["message"] = "¡Error al enviar el archivo!";
            }

            result["IdValidado"] = IdValidado;

            return result;
        }
        private async Task<Dictionary<string, object>> prevalidador058(int idDatosDeEmpresa, string NombreJuliano, string Aduana, string Patente, int MyIDOficina, string Prevalidador,bool CargaManual = false)
        {
            //string result = string.Empty;
            Dictionary<string, object> result = new Dictionary<string, object>();
            var objHelp = new Helper();
            var objFtp = new CatalogoDeFtps();
            var objFtpData = new CatalogoDeFtpsRepository(_configuration);
            var idFtp = objFtpData.BuscarIdFtp(idDatosDeEmpresa, MyIDOficina, Prevalidador);
            objFtp = objFtpData.Buscar(idFtp);

            //switch (MyIDOficina)
            //{
            //    case 2:
            //        {
            //            objFtp = objFtpData.Buscar(16);
            //            break;
            //        }
            //    case 3:
            //        {
            //            switch (Aduana.Trim())
            //            {
            //                case "470":
            //                    {
            //                        objFtp = objFtpData.Buscar(16);
            //                        break;
            //                    }
            //                case "850":
            //                    {
            //                        objFtp = objFtpData.Buscar(33);
            //                        break;
            //                    }
            //            }

            //            break;
            //        }
            //    case 24:
            //        {
            //            objFtp = objFtpData.Buscar(33);
            //            break;
            //        }
            //    case 4:
            //        {
            //            objFtp = objFtpData.Buscar(17);
            //            break;
            //        }
            //    case 19:
            //        {
            //            objFtp = objFtpData.Buscar(30);
            //            break;
            //        }
            //}

            if (objFtp == null)
            {
                throw new ArgumentException("No existe una cuenta de ftp disponible ");
            }
            string gRutaFTP = objFtp.FTP;

            var ObjUsuariosItce = new CatalogoDeUsuariosITCE();
            var ObjUsuariosItceDATA = new CatalogoDeUsuariosITCERepository(_configuration);

            ObjUsuariosItce = ObjUsuariosItceDATA.Buscar(GObjUsuario.IdUsuario, Aduana, Patente);

            if (!(ObjUsuariosItce == null))
            {

                string UsuarioFtp = ObjUsuariosItce.Usuario;
                string PasswordFtp = ObjUsuariosItce.Psw;


                string vArchivo = Strings.Mid(Path.GetFileNameWithoutExtension(NombreJuliano.Trim()), 2, Path.GetFileNameWithoutExtension(NombreJuliano.Trim()).Length);
                string vJuliano = Path.GetExtension(NombreJuliano.Trim());
                string vAduana = Strings.Mid(Aduana, 1, 2);
                string MiArchivo = Path.GetFileName(NombreJuliano.Trim());

                if (objHelp.UnLoadFtp2(gRutaFTP + Strings.LCase(MiArchivo), UsuarioFtp, PasswordFtp, NombreJuliano.Trim()))
                {
                    var objValidacion = new ControldeValidacion();
                    objValidacion.Archivo = vArchivo;
                    objValidacion.Juliano = vJuliano;
                    objValidacion.Recibido = false;
                    objValidacion.Validacion = true;
                    objValidacion.Aduana = vAduana;
                    objValidacion.Prevalidador = Prevalidador;
                    objValidacion.CargaManual = CargaManual;

                    int IdValidado = 0;
                    var objValidacionD = new ControldeValidacionRepository(_configuration);
                    IdValidado = objValidacionD.Insertar(objValidacion, MyIDOficina, false);
                    result["message"] = "¡Archivo enviado con exito! ";
                    result["IdValidado"] = IdValidado;
                }
            }
            else
            {
                throw new ArgumentException("Usted no está autorizado para validar o pagar pedimentos con ITCE");
            }
            return result;
        }
        private async Task<Dictionary<string, object>> prevalidador011(int idDatosDeEmpresa, string Origen, string Aduana, string Patente, int MyIDOficina, string Prevalidador, bool CargaManual = false)
        {
            //string result = string.Empty;
            Dictionary<string, object> result = new Dictionary<string, object>();
            var objHelp = new Helper();
            var objFtp = new CatalogoDeFtps();
            var objFtpData = new CatalogoDeFtpsRepository(_configuration);
            var idFtp = objFtpData.BuscarIdFtp(idDatosDeEmpresa, MyIDOficina, Prevalidador);
            objFtp = objFtpData.Buscar(idFtp);

            //switch (MyIDOficina)
            //{
            //    case 2:
            //        {
            //            objFtp = objFtpData.Buscar(16);
            //            break;
            //        }
            //    case 3:
            //        {
            //            switch (Aduana.Trim())
            //            {
            //                case "470":
            //                    {
            //                        objFtp = objFtpData.Buscar(16);
            //                        break;
            //                    }
            //                case "850":
            //                    {
            //                        objFtp = objFtpData.Buscar(33);
            //                        break;
            //                    }
            //            }

            //            break;
            //        }
            //    case 24:
            //        {
            //            objFtp = objFtpData.Buscar(33);
            //            break;
            //        }
            //    case 4:
            //        {
            //            objFtp = objFtpData.Buscar(17);
            //            break;
            //        }
            //    case 19:
            //        {
            //            objFtp = objFtpData.Buscar(30);
            //            break;
            //        }
            //}

            if (objFtp == null)
            {
                throw new ArgumentException("No existe una cuenta de ftp disponible ");
            }
            string gRutaFTP = objFtp.FTP;

            var ObjUsuariosItce = new CatalogoDeUsuariosITCE();
            var ObjUsuariosItceDATA = new CatalogoDeUsuariosITCERepository(_configuration);

            ObjUsuariosItce = ObjUsuariosItceDATA.Buscar(GObjUsuario.IdUsuario, Aduana, Patente);

            if (!(ObjUsuariosItce == null))
            {

                string UsuarioFtp = ObjUsuariosItce.Usuario;
                string PasswordFtp = ObjUsuariosItce.Psw;


                string vArchivo = Strings.Mid(Path.GetFileNameWithoutExtension(Origen.Trim()), 2, Path.GetFileNameWithoutExtension(Origen.Trim()).Length);
                string vJuliano = Path.GetExtension(Origen.Trim());
                string vAduana = Strings.Mid(Aduana, 1, 2);
                string MiArchivo = Path.GetFileName(Origen.Trim());

                if (objHelp.UnLoadFtp2(gRutaFTP + Strings.LCase(MiArchivo), UsuarioFtp, PasswordFtp, Origen.Trim()))
                {
                    var objValidacion = new ControldeValidacion();
                    objValidacion.Archivo = vArchivo;
                    objValidacion.Juliano = vJuliano;
                    objValidacion.Recibido = false;
                    objValidacion.Validacion = true;
                    objValidacion.Aduana = vAduana;
                    objValidacion.Prevalidador = Prevalidador;
                    objValidacion.CargaManual = CargaManual;

                    int IdValidado = 0;
                    var objValidacionD = new ControldeValidacionRepository(_configuration);
                    IdValidado = objValidacionD.Insertar(objValidacion, MyIDOficina, false);
                    result["message"] = "¡Archivo enviado con exito! ";
                    result["IdValidado"] = IdValidado;
                }
            }
            else
            {
                throw new ArgumentException("Usted no está autorizado para validar o pagar pedimentos con ITCE");
            }
            return result;
        }
        private Dictionary<string, object> prevalidador039(int idDatosDeEmpresa, string Origen, string Aduana, string Patente, int MyIDOficina, string Prevalidador, bool CargaManual = false)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            //string result = string.Empty;
            var objHelp = new Helper();
            var objFtp = new CatalogoDeFtps();
            var objFtpData = new CatalogoDeFtpsRepository(_configuration);
            var idFtp = objFtpData.BuscarIdFtp(idDatosDeEmpresa, MyIDOficina, Prevalidador);
            objFtp = objFtpData.Buscar(idFtp);

            if (idDatosDeEmpresa == 2 && MyIDOficina == 22)
            {
                string MiRegex500;
                MiRegex500 = MiRegex(3);
                ParseaJuliano(Origen, MiRegex500);
            }

            //switch (GObjUsuario.Oficina.IDDatosDeEmpresa)
            //{
            //    case 1: // Grupo Ei en comercio
            //        {
            //            break;
            //        }

            //    case 2: // Grupo Ei multiservicios
            //        {

            //            switch (MyIDOficina)
            //            {
            //                case 19: // Toluca
            //                    {
            //                        objFtp = objFtpData.Buscar(20);
            //                        break;
            //                    }
            //                case 21: // Queretaro 
            //                    {
            //                        objFtp = objFtpData.Buscar(21);
            //                        break;
            //                    }
            //                case 22: // Monterrey 
            //                    {
            //                        objFtp = objFtpData.Buscar(22);
            //                        string MiRegex500;
            //                        MiRegex500 = MiRegex(3);

            //                        ParseaJuliano(Origen, MiRegex500);
            //                        break;
            //                    }
            //            }

            //            break;
            //        }

            //}


            if (objFtp == null)
            {
                throw new ArgumentException("No existe una cuenta de ftp disponible ");
            }
            string gRutaFTP = objFtp.FTP;



            string UsuarioFtp = objFtp.UsuarioFTP;
            string PasswordFtp = objFtp.PasswordFTP;


            string vArchivo = Strings.Mid(Path.GetFileNameWithoutExtension(Origen.Trim()), 2, Strings.Len(Path.GetFileNameWithoutExtension(Origen.Trim())));
            string vJuliano = Path.GetExtension(Origen.Trim());
            string vAduana = Strings.Mid(Aduana, 1, 2);
            string MiArchivo = Path.GetFileName(Origen.Trim());

            if (objHelp.UnLoadFtp2(gRutaFTP + Strings.LCase(MiArchivo), UsuarioFtp, PasswordFtp, Strings.LCase(Origen.Trim())))
            {
                var objValidacion = new ControldeValidacion();
                objValidacion.Archivo = vArchivo;
                objValidacion.Juliano = vJuliano;
                objValidacion.Recibido = false;
                objValidacion.Validacion = true;
                objValidacion.Aduana = vAduana;
                objValidacion.Prevalidador = Prevalidador;
                objValidacion.CargaManual = CargaManual;

                int IdValidado = 0;
                var objValidacionD = new ControldeValidacionRepository(_configuration);
                IdValidado = objValidacionD.Insertar(objValidacion, MyIDOficina, false);
                result["message"] = "¡Archivo enviado con exito! ";
                result["IdValidado"] = IdValidado;
        }
            return result;
        }
        private Dictionary<string, object> prevalidador040(int idDatosDeEmpresa, string Origen, string Aduana, string Patente, int MyIDOficina, string Prevalidador, bool CargaManual = false)
        {
            //string result = string.Empty;
            Dictionary<string, object> result = new Dictionary<string, object>();
            var objHelp = new Helper();
            var objFtp = new CatalogoDeFtps();
            var objFtpData = new CatalogoDeFtpsRepository(_configuration);
            var idFtp = objFtpData.BuscarIdFtp(idDatosDeEmpresa, MyIDOficina, Prevalidador);
            objFtp = objFtpData.Buscar(idFtp);

            if (idDatosDeEmpresa == 2 && MyIDOficina == 22)
            {
                string MiRegex500;
                MiRegex500 = MiRegex(3);
                ParseaJuliano(Origen, MiRegex500);
            }

            //switch (GObjUsuario.Oficina.IDDatosDeEmpresa)
            //{
            //    case 1: // Grupo Ei en comercio
            //        {
            //            break;
            //        }

            //    case 2: // Grupo Ei multiservicios
            //        {

            //            switch (MyIDOficina)
            //            {
            //                case 19: // Toluca
            //                    {
            //                        objFtp = objFtpData.Buscar(26);
            //                        break;
            //                    }
            //                case 21: // Queretaro 
            //                    {
            //                        objFtp = objFtpData.Buscar(27);
            //                        break;
            //                    }
            //                case 22: // Monterrey 
            //                    {
            //                        objFtp = objFtpData.Buscar(28);
            //                        string MiRegex500;
            //                        MiRegex500 = MiRegex(3);

            //                        ParseaJuliano(Origen, MiRegex500);
            //                        break;
            //                    }
            //            }

            //            break;
            //        }

            //}

            if (objFtp == null)
            {
                throw new ArgumentException("No existe una cuenta de ftp disponible ");
            }
            string gRutaFTP = objFtp.FTP;



            string UsuarioFtp = objFtp.UsuarioFTP;
            string PasswordFtp = objFtp.PasswordFTP;


            string vArchivo = Strings.Mid(Path.GetFileNameWithoutExtension(Origen.Trim()), 2, Strings.Len(Path.GetFileNameWithoutExtension(Origen.Trim())));
            string vJuliano = Path.GetExtension(Origen.Trim());
            string vAduana = Strings.Mid(Aduana, 1, 2);
            string MiArchivo = Path.GetFileName(Origen.Trim());

            if (objHelp.UnLoadFtp2(gRutaFTP + Strings.LCase(MiArchivo), UsuarioFtp, PasswordFtp, Strings.LCase(Origen.Trim())))
            {
                var objValidacion = new ControldeValidacion();
                objValidacion.Archivo = vArchivo;
                objValidacion.Juliano = vJuliano;
                objValidacion.Recibido = false;
                objValidacion.Validacion = true;
                objValidacion.Aduana = vAduana;
                objValidacion.Prevalidador = Prevalidador;
                objValidacion.CargaManual = CargaManual;

                int IdValidado = 0;
                var objValidacionD = new ControldeValidacionRepository(_configuration);
                IdValidado = objValidacionD.Insertar(objValidacion, MyIDOficina, false);
                result["message"] = "¡Archivo enviado con exito! ";
                result["IdValidado"] = IdValidado;
            }
            return result;
        }
        public async Task<Dictionary<string, object>> enviarValidacion(string Patente, string Origen, string Aduana, int IdDatosDeEmpresa, int MyIDOficina, int IdUsuario, string NombreJuliano,bool CargaManual=false)
        {
            //string result = string.Empty;
            Dictionary<string,object> result = new Dictionary<string,object>();

            var usuariosRepository = new CatalogoDeUsuariosRepository(_configuration);
            var objAA = new CatalogoDeAgentesAduanales();
            var objAAD = new CatalogoDeAgentesAduanalesRepository(_configuration);
            string Prevalidador;
            GObjUsuario = usuariosRepository.BuscarPorId(IdUsuario);
            if (GObjUsuario == null)
            {
                throw new ArgumentException("El Usuario no está configurado en Expertti");
            }
            objAA = objAAD.Buscar(Patente).First();

            if (objAA == null)
            {
                throw new ArgumentException("El Agente Aduanal no esta configurado en Expertti");
            }

            if (Origen.Length == 0)
            {
                throw new ArgumentException("Primero deberá buscar un archivo de validación");
            }
            Prevalidador = ExtraEPreValidador(Origen);
            switch (Prevalidador)
            {
                case "010":
                    {
                        result = await prevalidador010(Origen, Aduana, MyIDOficina, objAA, CargaManual);
                        break;
                    }
                case "058":
                    {
                        result = await prevalidador058(IdDatosDeEmpresa, Origen, Aduana, Patente, MyIDOficina, Prevalidador, CargaManual);
                        break;
                    }
                case "011":
                    {
                        result = await prevalidador011(IdDatosDeEmpresa, Origen, Aduana, Patente, MyIDOficina, Prevalidador, CargaManual);
                        break;
                    }
                case "039":
                    {
                        result = prevalidador039(IdDatosDeEmpresa, Origen, Aduana, Patente, MyIDOficina, Prevalidador, CargaManual);
                        break;
                    }
                case "040":
                    {
                        result = prevalidador040(IdDatosDeEmpresa, Origen, Aduana, Patente, MyIDOficina, Prevalidador, CargaManual);
                        break;
                    }
            }
            return result;

        }

        public string ParseaJuliano(string Juliano)
        {
            string ParseaJulianoRet = default;
            var re = new Regex(Regex(3), RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;
            string Pedimentos = "";
            int Cuantos;
            ContenidoDelFicheroOriginal = MyReadFile(Juliano);
            mc = re.Matches(ContenidoDelFicheroOriginal);

            Cuantos = mc.Count - 1;
            for (int i = 0, loopTo = mc.Count - 1; i <= loopTo; i++)
            {
                Pedimentos = Pedimentos + mc[i].Groups["PEDIMENTO"].ToString() + "|";
            }
            ParseaJulianoRet = Pedimentos;
            return ParseaJulianoRet;

        }
        public void ParseaJuliano(string FileName, string MiRegex500)
        {
            string Pedimento = "";
            string Patente = "";
            string Aduana = "";
            string TodoElRegistro = "";
            string NombreDeArchivo = "";
            string ExtencionDeArchivo = "";
            string Referencia = "";
            string Sello = "";
            var re = new Regex(MiRegex500, RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;

            NombreDeArchivo = Path.GetFileNameWithoutExtension(FileName);
            NombreDeArchivo = Strings.Mid(NombreDeArchivo, 6, 3);
            ExtencionDeArchivo = Path.GetExtension(FileName);
            ContenidoDelFicheroOriginal = MyReadFile(FileName);
            mc = re.Matches(ContenidoDelFicheroOriginal);


            for (int i = 0, loopTo = mc.Count - 1; i <= loopTo; i++)
            {
                TodoElRegistro = mc[i].ToString();
                Patente = mc[i].Groups["PATENTE"].ToString();
                Pedimento = mc[i].Groups["PEDIMENTO"].ToString();
                Aduana = mc[i].Groups["ADUANA"].ToString();

                var ObjSaaioPedimeDATA = new SaaioPedimeRepository(_configuration);
                var ObjSaaioPedime = new SaaioPedime();
                ObjSaaioPedime = ObjSaaioPedimeDATA.Buscar(Pedimento, Patente, Aduana);
                if (!(ObjSaaioPedime == null))
                {
                    Referencia = ObjSaaioPedime.NUM_REFE;
                    Sello = ExtraERegistro800(TodoElRegistro);
                    var ObjSaaioFea = new SaaioFea();
                    var ObjSaaioFeaD = new SaaioFeaRepository(_configuration);
                    ObjSaaioFea.NUM_REFE = Referencia;
                    ObjSaaioFea.NOM_ARCH = FileName;
                    ObjSaaioFea.CVE_CAPT = GObjUsuario.UsuarioCASA;
                    ObjSaaioFea.NUM_FEA = Strings.Mid(Sello, 1, 250);
                    ObjSaaioFea.NUM_FEA2 = Strings.Mid(Sello, 251, 94);
                    ObjSaaioFeaD.Insertar(ObjSaaioFea);


                }
            }
        }
        public string ExtraERegistro800(string Linea)
        {
            string ExtraERegistro800Ret = default;
            var re = new Regex(Regex(2011), RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;
            string Sello = "";
            int Cuantos;
            // ContenidoDelFicheroOriginal = Juliano
            mc = re.Matches(Linea);

            Cuantos = mc.Count - 1;
            for (int i = 0, loopTo = mc.Count - 1; i <= loopTo; i++)
            {
                Sello = mc[i].Groups["SELLO"].ToString();
            }

            ExtraERegistro800Ret = Sello;
            return ExtraERegistro800Ret;

        }

        public string ExtraEPreValidador(string Juliano)
        {
            string ExtraEPreValidadorRet = default;
            var re = new Regex(Regex(2005), RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;
            string Prevalidador = "'";
            int Cuantos;
            ContenidoDelFicheroOriginal = MyReadFile(Juliano);
            mc = re.Matches(ContenidoDelFicheroOriginal);

            Cuantos = mc.Count - 1;
            for (int i = 0, loopTo = mc.Count - 1; i <= loopTo; i++)

                Prevalidador = mc[i].Groups["PREVAL"].ToString();
            ExtraEPreValidadorRet = Prevalidador;
            return ExtraEPreValidadorRet;

        }
        public string MyReadFile(string ArchivoJuliano)
        {
            var response = string.Empty;
            try
            {
                var sr = new StreamReader(ArchivoJuliano);
                response = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }

        public string PADL(string Cadena, int longitud, string relleno, bool Numero)
        {
            string PADLRet = default;
            int LenCadena;
            int i;
            int pnPosicion;
            if (Numero)
            {
                pnPosicion = Strings.InStr(Cadena, ".");
                if (pnPosicion == 0)
                {
                    Cadena = Cadena + ".00";
                }
                else if (Strings.Len(Strings.Trim(Cadena)) - pnPosicion == 1)
                {
                    Cadena = Cadena + "0";
                }
            }
            LenCadena = Strings.Len(Cadena);
            if (LenCadena != longitud)
            {
                var loopTo = longitud - LenCadena;
                for (i = 2; i <= loopTo; i++)
                    relleno = relleno + Strings.Mid(relleno, 1, 1);
                relleno = relleno + Cadena;
                Cadena = relleno;
            }
            PADLRet = Cadena;
            return PADLRet;
        }

        public PrevalidadorResponse prevalidarArchivo(string pathArchivoJuliano, string Aduana, string Patente, int IdUsuario, string NombreJuliano)
        {
            PrevalidadorResponse prevalidadorResponse = new PrevalidadorResponse();
            List<PrevalidadorError> errores = new List<PrevalidadorError>();
            DateTime PrimerDiaAno;
            int JulianoInicial;
            string FechaDelHoy;
            string Extencion = "";
            var catalogoDeUsuariosRepository = new CatalogoDeUsuariosRepository(_configuration);
            GObjUsuario = catalogoDeUsuariosRepository.BuscarPorId(IdUsuario);
            DataTable dtbErrores = new DataTable();
            FechaDelHoy = DateTime.Now.ToString(); // FechaDelServidor
            PrimerDiaAno = DateTime.Parse("01/01/" + Thread.CurrentThread.CurrentCulture.Calendar.GetYear(DateTime.Now));
            JulianoInicial = (int)(DateAndTime.DateDiff(DateInterval.Day, PrimerDiaAno, DateTime.Parse(FechaDelHoy)) + 1L);
            Extencion = PADL(JulianoInicial.ToString(), 3, "0", false);

            int iEliminacion = 0;
            iEliminacion = ValidaTipoMovimiento(pathArchivoJuliano);
            if (iEliminacion == 2)
            {
                iSinErrores = 0;
                MyPedimentos = ExtraENumerosDePedimento(pathArchivoJuliano, 2006, Aduana, Patente, pathArchivoJuliano);
            }
            else
            {
                MyPedimentos = ExtraENumerosDePedimento(pathArchivoJuliano, 3, Aduana, Patente, NombreJuliano);

                var objloaddgvErrores = new BitacoraDePrevalRepository(_configuration);
                dtbErrores = objloaddgvErrores.LlenarPorJulianoErroresGrid(Convert.ToString(Thread.CurrentThread.CurrentCulture.Calendar.GetYear(DateTime.Now)) + "-" + Aduana + "-" + NombreJuliano);

                var results = dtbErrores.AsEnumerable().Where(row => row.Field<int>("TIPODEERROR") == 1).FirstOrDefault();

                if (results is not null)
                {
                    iSinErrores = 1;
                }
                else
                {
                    iSinErrores = 0;
                }
                if (iSinErrores != 1)
                {
                    string MiRegex500;
                    MiRegex500 = MiRegex(3);
                    if (ParseaJulianoFirma(pathArchivoJuliano.Trim(), MiRegex500) == 0)
                    {
                        FirmaJuliano(Aduana, Patente, pathArchivoJuliano);
                    }

                }
            }
            switch (iSinErrores)
            {
                case 0:
                    {
                        prevalidadorResponse.mensaje = "Pre-Validación del archivo finalizada sin errores y se puede enviar a validar";

                        break;
                    }

                case 1:
                    {
                        prevalidadorResponse.mensaje = "Pre-Validación del archivo finalizada con errores, por lo cual no puede enviarse a validar";
                        break;
                    }
                case 2:
                    {
                        prevalidadorResponse.mensaje = "Pre-Validación del archivo finalizada sin errores, pero solo se puede enviar a validar con autorización de un supervisor";
                        break;
                    }
                case 3:
                    {
                        prevalidadorResponse.mensaje = "No fue posible generar la Firma del archivo";
                        break;
                    }
            }
            foreach (DataRow row in dtbErrores.Rows)
            {
                PrevalidadorError prevalidadorError = new PrevalidadorError();
                prevalidadorError.IdError = row["idError"].ToString();
                prevalidadorError.MensajeError = row["MENSAJEERROR"].ToString();
                prevalidadorError.TipoDeError = row["TIPODEERROR"].ToString();
                prevalidadorError.Sugerencia = row["SUGERENCIA"].ToString();
                prevalidadorError.Referencia = row["REFERENCIA"].ToString();
                prevalidadorError.Consecutivo = row["CONSECUTIVO"].ToString();
                prevalidadorError.Justificado = row["JUSTIFICADO"].ToString();
                prevalidadorError.PartidaCove = row["PARTIDACOVE"].ToString();
                prevalidadorError.PartidaPedi = row["PARTIDAPEDI"].ToString();
                prevalidadorError.Pedimento = row["PEDIMENTO"].ToString();
                errores.Add(prevalidadorError);
            }

            prevalidadorResponse.errores = errores;
            return prevalidadorResponse;


        }
        public string MiRegex(int MyIDRegex)
        {
            var REGEXDDATA = new CatalogoDeRegexRepository(_configuration);
            var EXPRESION = new CatalogoDeRegex();
            string MiRegreso = "";
            try
            {
                EXPRESION = REGEXDDATA.Buscar(MyIDRegex);

                if (!(EXPRESION == null))
                {

                    MiRegreso = EXPRESION.Regex;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

            return MiRegreso;
        }

        public string ExtraENumerosDePedimento(string Juliano, int IdRegex, string Aduana, string Patente, string NombreJuliano)
        {
            var re = new Regex(Regex(IdRegex), RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;
            int Cuantos;
            ContenidoDelFicheroOriginal = MyReadFile(Juliano);
            mc = re.Matches(ContenidoDelFicheroOriginal);

            Cuantos = mc.Count - 1;
            for (int i = 0, loopTo = mc.Count - 1; i <= loopTo; i++)
            {
                BuscarInformacionDelPedimento(mc[i].Groups["PEDIMENTO"].ToString(), 1, Aduana, Patente, NombreJuliano);
            }
            return default;

        }
        public void BuscarInformacionDelPedimento(string Valor, int TocToc, string Aduana, string Patente, string NombreJuliano)
        {

            var SAAIOPEDIMEDATA = new SaaioPedimeRepository(_configuration);
            var SAAIOPEDIME = new SaaioPedime();
            string sOperacion;

            switch (TocToc) // Busco por Pedimento
            {
                case 1:
                    {
                        SAAIOPEDIME = SAAIOPEDIMEDATA.Buscar(Valor, Patente, Aduana);
                        break;
                    }
                case 2: // Busco por Referencia
                    {
                        SAAIOPEDIME = SAAIOPEDIMEDATA.Buscar(Valor);
                        break;
                    }
            }



            if (!(SAAIOPEDIME == null))
            {

                var ObjBitacoraDePreval = new BitacoraDePrevalRepository(_configuration);

                ObjBitacoraDePreval.Delete(SAAIOPEDIME.NUM_REFE.Trim());

                var ObjCatalogodePreval = new CatalogoDePreval();
                var ObjCatalogodePrevalData = new CatalogodePrevalRepository(_configuration);
                ObjCatalogodePrevalData.EjecutarPrevalidadores(Valor, SAAIOPEDIME.NUM_REFE.Trim(), GObjUsuario.IdUsuario, Convert.ToString(Thread.CurrentThread.CurrentCulture.Calendar.GetYear(DateTime.Now)) + "-" + Aduana + "-" + NombreJuliano);

            }
            else
            {
                throw new Exception("No existe el pedimento ó la referencia " + Valor);

            }
        }

        public string Regex(int MyIDRegex)
        {
            var REGEXDDATA = new CatalogoDeRegexRepository(_configuration);
            var EXPRESION = new CatalogoDeRegex();
            string MyRegexP = string.Empty;
            try
            {
                EXPRESION = REGEXDDATA.Buscar(MyIDRegex);

                if (!(EXPRESION == null))
                {

                    MyRegexP = EXPRESION.Regex;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }

            return MyRegexP;
        }

        public int ValidaTipoMovimiento(string Juliano)
        {
            int ValidaTipoMovimientoRet = default;
            var re = new Regex(Regex(2006), RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;
            int Elimina = 0;
            int Cuantos;
            ContenidoDelFicheroOriginal = MyReadFile(Juliano);
            mc = re.Matches(ContenidoDelFicheroOriginal);

            Cuantos = mc.Count - 1;
            for (int i = 0, loopTo = mc.Count - 1; i <= loopTo; i++)
            {
                Elimina = int.Parse(mc[i].Groups["OPERACION"].ToString());
                ValidaTipoMovimientoRet = Elimina;
                return ValidaTipoMovimientoRet;
            }

            return ValidaTipoMovimientoRet;
        }
        public void FirmaJuliano(string Aduana, string Patente, string Origen)
        {

            string vRutaO = string.Empty;
            string vRutaD = string.Empty;
            string vDirD = string.Empty;
            vRutaO = Origen.Trim();
            vDirD = Path.GetDirectoryName(Origen.Trim()) + @"\CarpetTmp\";
            vRutaD = vDirD + Path.GetFileName(Origen.Trim());

            if (Directory.Exists(vDirD) == false)
            {
                Directory.CreateDirectory(vDirD);
            }

            string LineadeComando = string.Empty;
            LineadeComando = @"C:\CASAWIN\CSAAIWIN-SQL\CCFIRMAS.EXE -firmar pat=" + Patente.Trim() + ",adu=" + Aduana.Trim() + ",archivo=" + vRutaO + ",destino=" + vDirD;
            Interaction.Shell(LineadeComando, AppWinStyle.Hide, true);

            if (File.Exists(vRutaD))
            {
                File.Delete(vRutaO);
                File.Copy(vRutaD, vRutaO);
                File.Delete(vRutaD);
                Directory.Delete(vDirD);
            }
            else
            {
                Directory.Delete(vDirD);
                iSinErrores = 3;
            }

        }
        public int ParseaJulianoFirma(string FileName, string MiRegex500)
        {
            string Sello = "";
            var re = new Regex(MiRegex500, RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;
            int iCuentaSellos = 0;


            ContenidoDelFicheroOriginal = MyReadFile(FileName);
            mc = re.Matches(ContenidoDelFicheroOriginal);

            int lineCount = File.ReadAllLines(FileName).Length;
            var data1 = File.ReadLines(FileName);
            string s1;
            s1 = data1.ToArray()[lineCount - 2];



            var re2 = new Regex(Regex(2011), RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc2;
            // 'ContenidoDelFicheroOriginal = lector
            int Cuantos;
            // 'ContenidoDelFicheroOriginal = Juliano
            mc2 = re2.Matches(s1);

            Cuantos = mc.Count - 1;
            for (int i = 0, loopTo = mc2.Count - 1; i <= loopTo; i++)
            {

                Sello = mc2[i].Groups["SELLO"].ToString();
                iCuentaSellos = 1;
            }

            return iCuentaSellos;
        }


    }
}
