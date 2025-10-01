using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using Chilkat;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System.Text.RegularExpressions;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCatalogosCasa;
using NPOI.SS.Formula.Functions;
using Match = System.Text.RegularExpressions.Match;
using API_MSPedimentos.DTO;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RespositoriesVentanillaUnica;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesDHL;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesTurnado;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPrevalidador;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador
{
    public class ControldeValidacionRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        private readonly UbicaciondeArchivosRepository _ubicacionDeArchivosRepository;


        public ControldeValidacionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
            _ubicacionDeArchivosRepository = new UbicaciondeArchivosRepository(_configuration);
        }

        private string getPatente(ControldeValidacion lcontroldevalidacion)
        {
            return lcontroldevalidacion.Archivo.Substring(0, 4);
        }

        private string getAduana(ControldeValidacion lcontroldevalidacion)
        {
            return lcontroldevalidacion.Aduana + "0";
        }
        /******************************************************************************************************
          Fecha de Modificación: 2025-09-16
          Usuario Modifica: Edward - Cubits
          Funcionalidad: Se agrega parametro CargaManual para identificar si archivo Juliano se carga desde pc usuario.
        ******************************************************************************************************/
        public int Insertar(ControldeValidacion lcontroldevalidacion, int MyIdOficina, bool pGlobal)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;


            cn.ConnectionString = sConexion;
            cn.Open();
            cmd.CommandText = "NET_INSERT_CONTROLDEVALIDACION_NEW_CON_IDOFICINA_GLOBAL";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // ,@Archivo  varchar
            @param = cmd.Parameters.Add("@Archivo", SqlDbType.VarChar, 7);
            @param.Value = lcontroldevalidacion.Archivo;

            // ,@Juliano  varchar
            @param = cmd.Parameters.Add("@Juliano", SqlDbType.VarChar, 4);
            @param.Value = lcontroldevalidacion.Juliano;

            // ,@Recibido  bit
            @param = cmd.Parameters.Add("@Recibido", SqlDbType.Bit, 4);
            @param.Value = lcontroldevalidacion.Recibido;

            @param = cmd.Parameters.Add("@Validacion", SqlDbType.Bit, 4);
            @param.Value = lcontroldevalidacion.Validacion;

            @param = cmd.Parameters.Add("@Aduana", SqlDbType.VarChar, 3);
            @param.Value = lcontroldevalidacion.Aduana;

            @param = cmd.Parameters.Add("@Prevalidador", SqlDbType.VarChar, 3);
            @param.Value = lcontroldevalidacion.Prevalidador;

            @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
            @param.Value = MyIdOficina;

            // @pGlobal
            @param = cmd.Parameters.Add("@pGlobal", SqlDbType.Bit);
            @param.Value = pGlobal;

            //Carga manual de juliano
            @param = cmd.Parameters.Add("@CargaManual", SqlDbType.Bit, 4);
            @param.Value = lcontroldevalidacion.CargaManual;

            // ,@newid_registro INT OUTPUT
            @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            @param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                id = (int)cmd.Parameters["@newid_registro"].Value;

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CONTROLDEVALIDACION");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }
        public List<ControldeValidacion> Cargar()
        {
            var list = new List<ControldeValidacion>();

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_LOAD_CONTROLDEVALIDACION", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new ControldeValidacion
                            {
                                IdValidado = Convert.ToInt32(dr["IdValidado"]),
                                Archivo = dr["Archivo"].ToString(),
                                Juliano = dr["Juliano"].ToString(),
                                Recibido = Convert.ToBoolean(dr["Recibido"]),
                                FechaEnvio = Convert.ToDateTime(dr["FechaEnvio"]),
                                Aduana = dr["Aduana"].ToString(),
                                Validacion = Convert.ToBoolean(dr["Validacion"])
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
        public int Modificar(int idValidado, bool recibido)
        {
            int id;

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_UPDATE_CONTROLDEVALIDACION", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdValidado", idValidado);
                    cmd.Parameters.AddWithValue("@Recibido", recibido);

                    SqlParameter outputIdParam = new SqlParameter("@newid_registro", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(outputIdParam);

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        id = Convert.ToInt32(outputIdParam.Value);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"{ex.Message} - NET_UPDATE_CONTROLDEVALIDACION", ex);
                    }
                }
            }

            return id;
        }
        public List<ControldeValidacion> Cargar(bool validacion, string miPatente, string miAduana, string miPrevalidador, int idOficina, bool pGlobal)
        {
            List<ControldeValidacion> lstControlDeValidacion = new List<ControldeValidacion>();

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_LOAD_CONTROLDEVALIDACION_NEW4", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Validacion", validacion);
                    cmd.Parameters.AddWithValue("@Patente", miPatente);
                    cmd.Parameters.AddWithValue("@Aduana", miAduana);
                    cmd.Parameters.AddWithValue("@Prevalidador", miPrevalidador);
                    cmd.Parameters.AddWithValue("@IDOficina", idOficina);
                    cmd.Parameters.AddWithValue("@pGlobal", pGlobal);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ControldeValidacion controlDeValidacion = new ControldeValidacion
                                {
                                    IdValidado = Convert.ToInt32(dr["IdValidado"]),
                                    Archivo = dr["Archivo"].ToString(),
                                    Juliano = dr["Juliano"].ToString(),
                                    Recibido = Convert.ToBoolean(dr["Recibido"]),
                                    FechaEnvio = (DateTime)(dr.IsDBNull(dr.GetOrdinal("FechaEnvio")) ? (DateTime?)null : Convert.ToDateTime(dr["FechaEnvio"])),
                                    Recepcion = dr["Recepcion"].ToString(),
                                    Aduana = dr["Aduana"].ToString(),
                                    Validacion = Convert.ToBoolean(dr["Validacion"])
                                };

                                lstControlDeValidacion.Add(controlDeValidacion);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"{ex.Message} - NET_LOAD_CONTROLDEVALIDACION_NEW4", ex);
                    }
                }
            }

            return lstControlDeValidacion;
        }

        public ControldeValidacion Obtener(int IdValidado)
        {
            ControldeValidacion controlDeValidacion = new ControldeValidacion();

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_OBTENER_CONTROLDEVALIDACION", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdValidado", IdValidado);

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                controlDeValidacion = new ControldeValidacion
                                {
                                    IdValidado = Convert.ToInt32(dr["IdValidado"]),
                                    IDOficina = Convert.ToInt32(dr["IDOficina"]),
                                    Archivo = dr["Archivo"].ToString(),
                                    Juliano = dr["Juliano"].ToString(),
                                    Recibido = Convert.ToBoolean(dr["Recibido"]),
                                    Prevalidador = dr["Prevalidador"].ToString(),
                                    FechaEnvio = (DateTime)(dr.IsDBNull(dr.GetOrdinal("FechaEnvio")) ? (DateTime?)null : Convert.ToDateTime(dr["FechaEnvio"])),
                                    Aduana = dr["Aduana"].ToString(),
                                    Validacion = Convert.ToBoolean(dr["Validacion"])
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"{ex.Message} - NET_OBTENER_CONTROLDEVALIDACION", ex);
                    }
                }
            }

            return controlDeValidacion;
        }


        public string ObtenerRegex(int idRegex, string connectionString)
        {
            string regex = "";

            try
            {
                CatalogoDeRegexRepository regexData = new CatalogoDeRegexRepository(_configuration);
                CatalogoDeRegex expresion = regexData.Buscar(idRegex);

                if (expresion != null)
                {
                    regex = expresion.Regex;
                }
            }
            catch (Exception ex)
            {
                // Manejo del error si es necesario
            }

            return regex;
        }



        public ValidacionResponse VerValidacion(int idValidado)
        {
            ValidacionResponse result = new ValidacionResponse();
            List<ArchivoErr> resultArchivoErr = new List<ArchivoErr>();
            String resultArchivoK = String.Empty;
            String resultArchivoJuliano = String.Empty;

            var controlDeValidacion = Obtener(idValidado);
            var ubicacionDeValidados = ObtenerUbicacionDeValidados(controlDeValidacion);
            var Archivo = controlDeValidacion.Archivo;
            var Juliano = controlDeValidacion.Juliano;

            string ArcErr = Path.Combine(ubicacionDeValidados, "m" + Archivo + ".err");
            string ArcJul = Path.Combine(ubicacionDeValidados, "m" + Archivo + Juliano);
            String ArchivoK = Path.Combine(ubicacionDeValidados, "k" + Archivo + Juliano);

            if (File.Exists(ArcErr))
            {
                resultArchivoErr = GetArchivoErr(ArcErr, Archivo.Substring(0, 4), ArcJul);
            }
            if (File.Exists(ArcJul))
            {
                resultArchivoJuliano = GetArchivoJuliano(ArcJul);
            }
            if (File.Exists(ArchivoK))
            {
                resultArchivoK = GetArchivoK(ArchivoK);
            }
            result.archivoErrs = resultArchivoErr;
            result.archivoK = resultArchivoK;
            result.archivoJuliano = resultArchivoJuliano;
            result.archivoJulianoRuta = ArcJul;
            return result;
        }

        public PagoResponse VerPago(int idValidado)
        {
            var controlDeValidacion = Obtener(idValidado);
            var ubicacionDeValidados = ObtenerUbicacionDeValidados(controlDeValidacion);
            var Archivo = controlDeValidacion.Archivo;
            var Juliano = controlDeValidacion.Juliano;
            string ArcA = ubicacionDeValidados + "A" + Archivo + Juliano;
            string ArcE = ubicacionDeValidados + "E" + Archivo + Juliano;
            var response = new PagoResponse();

            if (File.Exists(ArcA))
            {
                response.archivoAPECE = GetArchivoAPECE(ArcA, Archivo.Substring(0, 4));

                if (File.Exists(ArcE))
                {
                    response.archivoArcE = GetArchivoArcE(ArcE);
                }
            }
            return response;

        }

        private string ObtenerUbicacionDeValidados(ControldeValidacion controlDeValidacion)
        {
            var patente = controlDeValidacion.Archivo.Substring(0, 4);
            var aduana = controlDeValidacion.Aduana + "0";
            return _ubicacionDeArchivosRepository.DeterminarUbicacionDeArchivos(patente, aduana, controlDeValidacion.Prevalidador);
        }

        public List<ArchivoErr> GetArchivoErr(string FileName, string Patente, string MiJuliano)
        {
            List<ArchivoErr> lst = new List<ArchivoErr>();
            Publicas publicas = new Publicas();
            foreach (Match m in Regex.Matches(publicas.ReadFileText(FileName), "^.*$", RegexOptions.Multiline))
            {
                // Carga los campos de la línea leída separados por "|"
                string[] Arreglo = Regex.Split(m.ToString(), "^");
                if (Arreglo[1] != string.Empty)
                {

                    switch (Arreglo[1].Substring(0, 1))
                    {
                        case "F":
                            var objArchivoErrFirma = new ArchivoErr
                            {
                                Situacion = "FIRMA",
                                Pedimento = Arreglo[1].Substring(1, 7),
                                FirmaElectronica = Arreglo[1].Substring(8, 8),
                                ImpeError = "",
                                SolucionExpertti = "",
                                ClaveimpeError = ""
                            };
                            lst.Add(objArchivoErrFirma);
                            break;

                        case "E":
                            string vPedimentoE = Arreglo[1].Substring(1, 7);
                            string vRegistroE = Arreglo[1].Substring(8, 3);
                            string vE = Arreglo[1].Substring(11, 4);
                            string vTipoE = Arreglo[1].Substring(15, 1);
                            string vCampoE = Arreglo[1].Substring(16, 2);
                            string vNumeroE = Arreglo[1].Substring(18, 2);
                            string vClaveimpeErrorE = vTipoE + vNumeroE + vRegistroE + vCampoE;

                            var objArchivoErrError = new ArchivoErr
                            {
                                Situacion = "ERROR",
                                Pedimento = vPedimentoE,
                                FirmaElectronica = "",
                                ClaveimpeError = vClaveimpeErrorE,
                                ImpeError = GetImpeError(vClaveimpeErrorE),
                                SolucionExpertti = GetSolucionExpertti(vClaveimpeErrorE)
                            };

                            lst.Add(objArchivoErrError);
                            lst.Add(objArchivoErrError);
                            break;

                        case "A":
                            string vPedimentoA = Arreglo[1].Substring(1, 7);
                            string vRegistroA = Arreglo[1].Substring(8, 3);
                            string vA = Arreglo[1].Substring(11, 4);
                            string vTipoA = Arreglo[1].Substring(15, 1);
                            string vCampoA = Arreglo[1].Substring(16, 2);
                            string vNumeroA = Arreglo[1].Substring(18, 2);
                            string vClaveimpeErrorA = vTipoA + vNumeroA + vRegistroA + vCampoA;

                            var objArchivoErrAdvertencia = new ArchivoErr
                            {
                                Situacion = "ADVERTENCIA",
                                Pedimento = vPedimentoA,
                                FirmaElectronica = "",
                                ClaveimpeError = vClaveimpeErrorA,
                                ImpeError = GetImpeError(vClaveimpeErrorA),
                                SolucionExpertti = GetSolucionExpertti(vClaveimpeErrorA)
                            };

                            lst.Add(objArchivoErrAdvertencia);
                            break;
                    }

                }
            }
            return lst;
        }

        private string GetImpeError(string vClaveimpeError)
        {
            var objCatGraD = new Ctarc_CatGraRepository(_configuration);
            var objCatGra = objCatGraD.Buscar("ERR2", vClaveimpeError);
            return objCatGra != null ? objCatGra.DESCRIP.Trim() : "";
        }

        private string GetSolucionExpertti(string vClaveimpeError)
        {
            var objImpeExperttiD = new CatalogodeImpeerrorRepository(_configuration);
            var objImpeExpertti = objImpeExperttiD.Buscar(vClaveimpeError.Trim());
            return objImpeExpertti != null ? objImpeExpertti.Descripcion.Trim() : "";
        }
        public string GetArchivo(string path)
        {
            Publicas publicas = new Publicas();
            return publicas.ReadFileText(path);
        }

        public String GetArchivoK(String PathArchivoK)
        {
            return GetArchivo(PathArchivoK);
        }
        public String GetArchivoJuliano(String PathArchivoJuliano)
        {
            return GetArchivo(PathArchivoJuliano);
        }

        public String GetArchivoArcE(String PathArchivoArcE)
        {
            return GetArchivo(PathArchivoArcE);
        }
        public List<ArchivoAPECE> GetArchivoAPECE(string fileName, string patente)
        {

            var objHelp = new Publicas();
            var lst = new List<ArchivoAPECE>();
            string miRegex = "^.*$";

            foreach (Match m in Regex.Matches(objHelp.ReadFileText(fileName), miRegex, RegexOptions.Multiline))
            {
                string[] arreglo = Regex.Split(m.ToString(), "^");

                for (int i = 0; i <= 1; i++)
                {
                    if (arreglo[i].Length != 0)
                    {
                        var objArchivoA = new ArchivoAPECE();

                        if (arreglo[i].Length > 80)
                        {
                            objArchivoA.Banco = arreglo[i].Substring(0, 5);
                            objArchivoA.Linea = arreglo[i].Substring(5, 20);
                            objArchivoA.Aduana = arreglo[i].Substring(25, 3);
                            objArchivoA.Patente = arreglo[i].Substring(28, 8);
                            objArchivoA.Pedimento = arreglo[i].Substring(36, 7);
                            objArchivoA.RFC = arreglo[i].Substring(43, 13);
                            objArchivoA.ID = arreglo[i].Substring(56, 5);
                            objArchivoA.Impuestos = arreglo[i].Substring(61, 14);
                            objArchivoA.FechaDePago = arreglo[i].Substring(75, 8);
                            objArchivoA.HoraDePago = arreglo[i].Substring(83, 8);
                            objArchivoA.Operacion = arreglo[i].Substring(91, 14);
                            objArchivoA.Transaccion = arreglo[i].Substring(105, 20);

                            lst.Add(objArchivoA);
                        }
                        else if (arreglo[i].Length > 1)
                        {
                            objArchivoA.Banco = arreglo[i].Substring(0, 5);
                            objArchivoA.Linea = arreglo[i].Substring(5, 20);
                            objArchivoA.Aduana = arreglo[i].Substring(25, 3);
                            objArchivoA.Patente = arreglo[i].Substring(28, 8);
                            objArchivoA.Pedimento = arreglo[i].Substring(36, 7);
                            objArchivoA.RFC = arreglo[i].Substring(43, 13);
                            objArchivoA.ID = arreglo[i].Substring(56, 5);
                            objArchivoA.Impuestos = arreglo[i].Substring(61, 14);
                            objArchivoA.FechaDePago = "PEDIMENTO";
                            objArchivoA.HoraDePago = "SIN";
                            objArchivoA.Operacion = "PAGO";
                            objArchivoA.Transaccion = "REFERENCIADO";

                            lst.Add(objArchivoA);
                        }
                    }
                }
            }
            return lst;
        }
        public string BuscaUbicacionDeArchivos(int myIdUbicacion)
        {
            string miRegreso = string.Empty;

            try
            {
                UbicaciondeArchivosRepository uadata = new UbicaciondeArchivosRepository(_configuration);
                UbicaciondeArchivos ua = uadata.Buscar(myIdUbicacion);

                if (ua != null)
                {
                    miRegreso = ua.Ubicacion;
                }
                else
                {
                    miRegreso = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return miRegreso;
        }
        public string MiRegex(int myIdRegex)
        {
            string miRegreso = string.Empty;

            try
            {
                CatalogoDeRegexRepository regexData = new CatalogoDeRegexRepository(_configuration);
                CatalogoDeRegex expresion = regexData.Buscar(myIdRegex);

                if (expresion != null)
                {
                    miRegreso = expresion.Regex;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return miRegreso;
        }
        public async Task<Boolean> RecuperarValidacion(int idValidado, int IdUsuario)
        {
            Boolean response = false;
            String resultArchivoK = String.Empty;
            String resultArchivoJuliano = String.Empty;
            CatalogoDeUsuarios GObjUsuario = new CatalogoDeUsuarios();
            CatalogoDeUsuariosRepository GObjUsuarioRepository = new CatalogoDeUsuariosRepository(_configuration);
            var controlDeValidacion = Obtener(idValidado);
            var ubicacionDeValidados = ObtenerUbicacionDeValidados(controlDeValidacion);
            var Archivo = controlDeValidacion.Archivo;
            var Juliano = controlDeValidacion.Juliano;
            GObjUsuario = GObjUsuarioRepository.BuscarPorId(IdUsuario);
            if (GObjUsuario == null)
            {
                throw new Exception("El usuario con ID: " + IdUsuario.ToString() + " no se encuentra registrado");
            }

            string ArcErr = Path.Combine(ubicacionDeValidados, "m" + Archivo + ".err");
            string ArcJul = Path.Combine(ubicacionDeValidados, "m" + Archivo + Juliano);
            String ArchivoK = Path.Combine(ubicacionDeValidados, "k" + Archivo + Juliano);

                if (File.Exists(ubicacionDeValidados + "m" + Archivo + ".err"))
                {
                    // DividirJuliano(Archivo.Substring(0, 4), objUbicacion.Ubicacion.Trim() + "m" + Archivo + vJuliano);
                    var objAAADAM = new ValidarAAADAM(_configuration);
                    var objDividirJuliano = new DividirJulianosRepository(_configuration);

                    objAAADAM.AsignarFirmasElectronicas(
                        ubicacionDeValidados + "m" + Archivo + ".err",
                        Archivo.Substring(0, 4),
                        GObjUsuario.IdUsuario,
                        controlDeValidacion.Aduana.Substring(0, 2),
                        false
                    );

                    string rutaDestino = BuscaUbicacionDeArchivos(93);

                    string miRegex500 = MiRegex(3);
                    string miRegexErr = MiRegex(2003);

                    // Publicas.DividirUnArchivoJuliano(UbicacionDeValidados + "m" + Archivo + vJuliano, miRegex500, miRegexErr, rutaDestino, this.chkPeca.Checked);

                    response = await objDividirJuliano.ParseaJulianoAsync(ubicacionDeValidados + "m" + Archivo + Juliano, rutaDestino, GObjUsuario);

            }
            else
            {
                throw new FileNotFoundException($"No se encontró el archivo: {ubicacionDeValidados}m{Archivo}.err");

            }
            return response;
        }
        public string AtrapaNumeros(string fileName, string patente, int idUsuario, string aduana, bool activaPeca)
        {
            Publicas publicas = new Publicas();
            string atrapaNumeros = string.Empty;
            if (activaPeca)
            {
                try
                {
                    string fileContent = publicas.ReadFileText(fileName);
                    var matches = Regex.Matches(fileContent, @"^.*$", RegexOptions.Multiline);

                    foreach (Match m in matches)
                    {
                        string[] arreglo = Regex.Split(m.ToString(), @"^");

                        if (arreglo.Length > 1 && arreglo[1].Length >= 9)
                        {
                            if (arreglo[1].Substring(0, 2) == "30")
                            {
                                try
                                {
                                    string vPedimento = arreglo[1].Substring(8, 7);
                                    atrapaNumeros += vPedimento + "|";
                                }
                                catch (Exception)
                                {
                                    continue;
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
            else
            {
                try
                {
                    string fileContent = publicas.ReadFileText(fileName);
                    string miRegex = @"^.*$";
                    var matches = Regex.Matches(fileContent, miRegex, RegexOptions.Multiline);
                    string nombreJuliano = Path.GetFileNameWithoutExtension(fileName);

                    foreach (Match m in matches)
                    {
                        string[] arreglo = Regex.Split(m.ToString(), @"^");

                        for (int i = 0; i <= 1; i++)
                        {
                            if (arreglo.Length > i && arreglo[i].Length > 80)
                            {
                                string vPedimento = arreglo[i].Substring(36, 7);
                                atrapaNumeros += vPedimento + "|";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }
            return atrapaNumeros;
        }

        public string AtrapaNumerosDePedimento(string fileName, string patente, int idUsuario, string aduana, bool activaPeca)
        {
            Publicas publicas = new Publicas();
            string atrapaNumerosDePedimento = string.Empty;

            if (activaPeca)
            {
                try
                {
                    string fileContent = publicas.ReadFileText(fileName);
                    var matches = Regex.Matches(fileContent, @"^.*$", RegexOptions.Multiline);

                    foreach (Match m in matches)
                    {
                        string[] arreglo = Regex.Split(m.ToString(), @"^");

                        if (arreglo.Length > 1 && arreglo[1].Length >= 9)
                        {
                            if (arreglo[1].Substring(0, 2) == "30")
                            {
                                try
                                {
                                    string vPedimento = aduana.Substring(0, 2) + patente + arreglo[1].Substring(8, 7);
                                    atrapaNumerosDePedimento += vPedimento + "|";
                                }
                                catch (Exception)
                                {
                                    continue;
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
            else
            {
                try
                {
                    string fileContent = publicas.ReadFileText(fileName);
                    string miRegex = @"^.*$";
                    var matches = Regex.Matches(fileContent, miRegex, RegexOptions.Multiline);
                    string nombreJuliano = Path.GetFileNameWithoutExtension(fileName);

                    foreach (Match m in matches)
                    {
                        string[] arreglo = Regex.Split(m.ToString(), @"^");

                        for (int i = 0; i <= 1; i++)
                        {
                            if (arreglo.Length > i && arreglo[i].Length > 80)
                            {
                                string vPedimento = aduana.Substring(0, 2) + patente + arreglo[i].Substring(36, 7);
                                atrapaNumerosDePedimento += vPedimento + "|";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }
            }

            return atrapaNumerosDePedimento;
        }
        public bool NET_LoadFacturaDHLConModulo(string myPedimentos, int myLote, int myPatente, ControldeValidacion controldeValidacion)
        {
            var facturasDHLDATA = new FacturasDHLRepository(_configuration);

            try
            {
                if (facturasDHLDATA.Insertar(myPedimentos, myPatente.ToString(), myLote, getAduana(controldeValidacion)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public bool EnviaPedPdfSfpFedex(DateTime fechaDePago, int miLote, string pedimentos, ControldeValidacion controldeValidacion)
        //{
        //    var facturas = new FacturasRepository(_configuration);

        //    try
        //    {
        //        if (facturas.EnviaPedPdfSfpFedexPorPedimento(fechaDePago, getPatente(controldeValidacion), miLote, getAduana(controldeValidacion), pedimentos))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        //public bool GeneraArchivosFacFedex(DateTime fechaDePago, int miLote, string pedimentos, ControldeValidacion controldeValidacion)
        //{
        //    try
        //    {
        //        var facturas = new FacturasRepository(_configuration);
        //        var facturasDHLD = new FacturasDHLData();
        //        string ubicacionDeArchivosFac = "";
        //        var ubicacionDeArchivosData = new UbicaciondeArchivosRepository(_configuration);
        //        UbicaciondeArchivos ubicacionFac = ubicacionDeArchivosData.Buscar(99);

        //        if (ubicacionFac == null)
        //        {
        //            ubicacionDeArchivosFac = "";
        //            return false;
        //        }
        //        else
        //        {
        //            ubicacionDeArchivosFac = ubicacionFac.Ubicacion;
        //        }

        //        if (facturas.GenerarArchivoFacFedexPorPedimentos(fechaDePago, getPatente(controldeValidacion), miLote, getAduana(controldeValidacion), ubicacionDeArchivosFac, pedimentos))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        public async Task<Boolean> RecuperarPago(int idValidado, int IdUsuario)
        {
            Boolean response = false;

            {
                try
                {
                    CatalogoDeUsuarios GObjUsuario = new CatalogoDeUsuarios();
                    CatalogoDeUsuariosRepository GObjUsuarioRepository = new CatalogoDeUsuariosRepository(_configuration);
                    GObjUsuario = GObjUsuarioRepository.BuscarPorId(IdUsuario);
                    var controlDeValidacion = Obtener(idValidado);
                    var ubicacionDeValidados = ObtenerUbicacionDeValidados(controlDeValidacion);
                    var Archivo = controlDeValidacion.Archivo;
                    var Juliano = controlDeValidacion.Juliano;
                    string archivoPath = Path.Combine(ubicacionDeValidados, "A" + Archivo + Juliano);
                    string miFechaDePago = DateTime.Now.ToString("dd/MM/yyyy");
                    string rutaDestino = BuscaUbicacionDeArchivos(93);
                    string rutaRecibidos = Path.Combine(ubicacionDeValidados, "E" + Archivo + Juliano);
                    string rutaEnviados = Path.Combine(ubicacionDeValidados, "A" + Archivo + Juliano);
                    if (File.Exists(archivoPath))
                    {
                        var objAAADAM = new ValidarAAADAM(_configuration);
                        var objDividirJuliano = new DividirJulianosRepository(_configuration);


                        string miRegexA;
                        string miRegexE;


                        var objExpedienteDigital = new ExpedienteDigitalRepository(_configuration);
                        var objCampaD = new CampaRepository(_configuration);
                        var objClientesInterD = new CatalogoDeClientesInterRepository(_configuration);

                        objAAADAM.AsignarFirmasDePagoNew(archivoPath, Archivo.Substring(0, 4), GObjUsuario.IdUsuario, controlDeValidacion.Aduana.Substring(0, 2), false);

                        if (!Directory.Exists(rutaDestino))
                        {
                            Directory.CreateDirectory(rutaDestino);
                        }

                        miRegexA = MiRegex(2004);
                        miRegexE = MiRegex(2002);

                        await objDividirJuliano.LeerArchivosDePagoCentralizadoA(rutaEnviados, rutaDestino, false, GObjUsuario);

                        var MisPedimentos = string.Empty;
                        MisPedimentos = AtrapaNumerosDePedimento(archivoPath, Archivo.Substring(0, 4), GObjUsuario.IdUsuario, controlDeValidacion.Aduana.Substring(0, 2), false);

                        objExpedienteDigital.CopiarJulianosYenviaPorFtp(MisPedimentos);

                        UbicaciondeArchivos objUbicacion = new UbicaciondeArchivos();
                        UbicaciondeArchivosRepository objUbicacionD = new UbicaciondeArchivosRepository(_configuration);
                        objUbicacion = objUbicacionD.Buscar(121);

                        if (objUbicacion == null)
                        {
                            throw new ArgumentException("No existe ubicacion de archivos Id. 121, MisDocumentos");
                        }

                        var pMisDocumentos = Path.Combine(objUbicacion.Ubicacion, GObjUsuario.Usuario.Trim(), "ExperttiTmp");

                        if (!Directory.Exists(pMisDocumentos))
                        {
                            Directory.CreateDirectory(pMisDocumentos);
                        }

                        //pendiente:
                        //objCampaD.EnviaDespuesDelPago(ref MisPedimentos, GObjUsuario.IdUsuario, pMisDocumentos);                        
                        //fin pendiente

                        objClientesInterD.EnviaDespuesDelPago(ref MisPedimentos, GObjUsuario.IdUsuario, pMisDocumentos);

                        if (GObjUsuario.IdOficina == 22)
                        {
                            string nosPedimentos = AtrapaNumeros(ubicacionDeValidados + "A" + Archivo + Juliano, Archivo.Substring(0, 4), GObjUsuario.IdUsuario, controlDeValidacion.Aduana.Substring(0, 2), false);

                            if (!string.IsNullOrEmpty(nosPedimentos))
                            {
                                try
                                {
                                    if (NET_LoadFacturaDHLConModulo(nosPedimentos, 666, int.Parse(getPatente(controlDeValidacion)), controlDeValidacion))
                                    {
                                        //pendiente:
                                        //GeneraArchivosFacFedex(Convert.ToDateTime(miFechaDePago), 666, nosPedimentos, controlDeValidacion);
                                        //EnviaPedPdfSfpFedex(Convert.ToDateTime(miFechaDePago), 666, nosPedimentos, controlDeValidacion);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }

                        switch (getAduana(controlDeValidacion))
                        {
                            case "850":
                                if (GObjUsuario.IDDatosDeEmpresa == 1)
                                {
                                    //pendiente: 
                                    var soiaData = new SoiaDataRepository(_configuration);
                                    string rutaAIFA = BuscaUbicacionDeArchivos(223);

                                    soiaData.EnviaPedimentosAJCJFDesdePago(rutaAIFA, MisPedimentos, GObjUsuario.IDDatosDeEmpresa);
                                    soiaData.EnviaPedimentosAWsJCJFDesdePago(MisPedimentos, GObjUsuario.IDDatosDeEmpresa);
                                }
                                break;
                        }

                        switch (controlDeValidacion.IDOficina)
                        {
                            case 2:
                            case 24:
                                var objActivar = new ActivarFuncionalidades();
                                var objActivarD = new ActivarFuncionalidadesRepository(_configuration);
                                objActivar = objActivarD.Buscar(2);
                                if (objActivar.Activo)
                                {
                                    var objGenerar = new APIGenerarConAgtAdu(_configuration);
                                    //pendiente: objGenerar.GenerarConagtadusporArchivodePago(MisPedimentos);
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            return response;
        }
        public List<ArchivoA> SetArchivoA(string fileName, string patente)
        {
            try
            {
                var lst = new List<ArchivoA>();
                var objhelp = new Publicas();
                foreach (Match m in Regex.Matches(objhelp.ReadFileText(fileName), "^.*$", RegexOptions.Multiline))
                {
                    var arreglo = Regex.Split(m.ToString(), "^");
                    if (arreglo.Length > 1 && arreglo.Skip(1).Any(s => string.IsNullOrEmpty(s)))
                    {
                        return lst;
                    }

                    switch (arreglo[1].Substring(0, 2))
                    {
                        case "30":
                            try
                            {
                                var vPedimento = arreglo[1].Substring(8, 7).ToString();
                                var vAaduana = arreglo[1].Substring(2, 2).ToString();

                                var objPedimeD = new SaaioPedimeRepository(_configuration);
                                var objPedime = objPedimeD.Buscar(vPedimento, patente, vAaduana);
                                if (objPedime != null)
                                {
                                    var objArcA = new ArchivoA
                                    {
                                        Caja = arreglo[1].Substring(28, 2).ToString(),
                                        DiaPago = DateTime.Parse($"{arreglo[1].Substring(50, 2)}/{arreglo[1].Substring(52, 2)}/{arreglo[1].Substring(54, 4)} {arreglo[1].Substring(58, 8)}")
                                    };

                                    var objCatGraD = new Ctarc_CatGraRepository(_configuration);
                                    var objCatGra = objCatGraD.Buscar("BAN", objPedime.CVE_BANC);
                                    if (objCatGra != null)
                                    {
                                        objArcA.Banco = objCatGra.DESCRIP;
                                    }

                                    objArcA.Operacion = arreglo[1].Substring(30, 10).ToString();
                                    objArcA.Firma = arreglo[1].Substring(40, 10).ToString();
                                    objArcA.Referencia = objPedime.NUM_REFE.Trim();
                                    objArcA.Pedimento = objPedime.NUM_PEDI.Trim();

                                    lst.Add(objArcA);
                                }
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            break;
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public async Task<List<ArchivoA>> RecibirPago(int IdValidado, int IdUsuario)
        {
            List<ArchivoA> result = new List<ArchivoA>();
            CatalogoDeUsuarios GObjUsuario = new CatalogoDeUsuarios();
            CatalogoDeUsuariosRepository GObjUsuarioRepository = new CatalogoDeUsuariosRepository(_configuration);
            GObjUsuario = GObjUsuarioRepository.BuscarPorId(IdUsuario);
            var controlDeValidacion = Obtener(IdValidado);
            var ubicacionDeValidados = ObtenerUbicacionDeValidados(controlDeValidacion);
            var Archivo = controlDeValidacion.Archivo;
            var Juliano = controlDeValidacion.Juliano;
            string ArchivoJuliano = Path.Combine(ubicacionDeValidados, "A" + Archivo + Juliano);
            string ArchivoJulianoValidar = Path.Combine(ubicacionDeValidados, "m" + Archivo + Juliano);
            string RutaLocal = Path.GetDirectoryName(ArchivoJuliano) + Path.DirectorySeparatorChar;
            string MiArchivo = Path.GetFileName(ArchivoJuliano);
            string MiArchivoSinExt = Path.GetFileNameWithoutExtension(ArchivoJuliano);
            string FilePath = Path.Combine(RutaLocal, MiArchivo);
            string MiFechaDePago = DateTime.Now.ToString("dd/MM/yyyy");
            string RutaDestino = string.Empty;
            string RutaRecibidos = string.Empty;
            string RutaEnviados = string.Empty;

            switch (controlDeValidacion.Prevalidador)
            {
                case "010":
                    var objAA = new CatalogoDeAgentesAduanales();
                    var objAAD = new CatalogoDeAgentesAduanalesRepository(_configuration);
                    objAA = objAAD.Buscar(getPatente(controlDeValidacion)).FirstOrDefault();
                    var objAAADAM = new ValidarAAADAM(_configuration);
                    string Respuesta = string.Empty;


                    Respuesta = await objAAADAM.RecibirPago(ubicacionDeValidados, Archivo, Juliano, getAduana(controlDeValidacion), objAA.PasswordPago, 0, GObjUsuario.IdUsuario);

                    switch (getPatente(controlDeValidacion))
                    {
                        case "3547":
                            RutaDestino = BuscaUbicacionDeArchivos(41);
                            RutaRecibidos = BuscaUbicacionDeArchivos(2);
                            RutaEnviados = BuscaUbicacionDeArchivos(1);
                            break;
                        case "1764":
                            RutaDestino = BuscaUbicacionDeArchivos(42);
                            RutaRecibidos = BuscaUbicacionDeArchivos(2);
                            RutaEnviados = BuscaUbicacionDeArchivos(1);
                            break;
                        case "1693":
                            switch (controlDeValidacion.IDOficina)
                            {
                                case 4:
                                    RutaDestino = BuscaUbicacionDeArchivos(51);
                                    RutaRecibidos = BuscaUbicacionDeArchivos(49);
                                    RutaEnviados = BuscaUbicacionDeArchivos(49);
                                    break;
                                case 14:
                                    RutaDestino = BuscaUbicacionDeArchivos(66);
                                    RutaRecibidos = BuscaUbicacionDeArchivos(50);
                                    RutaEnviados = BuscaUbicacionDeArchivos(50);
                                    break;
                                case 9:
                                    RutaDestino = BuscaUbicacionDeArchivos(51);
                                    RutaRecibidos = BuscaUbicacionDeArchivos(49);
                                    RutaEnviados = BuscaUbicacionDeArchivos(49);
                                    break;
                            }
                            break;
                    }

                    RutaDestino = BuscaUbicacionDeArchivos(93);
                    RutaDestino = Path.Combine(RutaDestino, getPatente(controlDeValidacion), MiFechaDePago.Substring(3, 2) + MiFechaDePago.Substring(6, 4) + MiFechaDePago.Substring(0, 2));
                    RutaRecibidos = Path.Combine(RutaRecibidos, "E" + Archivo + Juliano);
                    RutaEnviados = Path.Combine(RutaEnviados, "A" + Archivo + Juliano);
                    if (!Directory.Exists(RutaDestino))
                    {
                        Directory.CreateDirectory(RutaDestino);
                    }
                    break;
                case "058":
                case "011":
                    var objFtp = new CatalogoDeFtps();
                    var objFtpData = new CatalogoDeFtpsRepository(_configuration);

                    var ObjUsuariosItce = new CatalogoDeUsuariosITCE();
                    var ObjUsuariosItceDATA = new CatalogoDeUsuariosITCERepository(_configuration);

                    switch (controlDeValidacion.IDOficina)
                    {
                        case 2:
                            objFtp = objFtpData.Buscar(16);
                            break;
                        case 3:
                            switch (getAduana(controlDeValidacion))
                            {
                                case "470":
                                    objFtp = objFtpData.Buscar(16);
                                    break;
                                case "850":
                                    objFtp = objFtpData.Buscar(33);
                                    break;
                            }
                            break;
                        case 24:
                            objFtp = objFtpData.Buscar(33);
                            break;
                        case 4:
                            objFtp = objFtpData.Buscar(17);
                            break;
                        case 19: // Toluca
                            objFtp = objFtpData.Buscar(30);
                            break;
                        case 21: // Queretaro
                            objFtp = objFtpData.Buscar(21);
                            break;
                        case 22: // Monterrey
                            objFtp = objFtpData.Buscar(22);
                            break;
                    }

                    if (objFtp == null)
                    {
                        throw new ArgumentException("No existe una cuenta de ftp disponible ");
                    }

                    ObjUsuariosItce = ObjUsuariosItceDATA.Buscar(GObjUsuario.IdUsuario, getAduana(controlDeValidacion), getPatente(controlDeValidacion));
                    if (ObjUsuariosItce != null)
                    {
                        var UsuarioFtp = ObjUsuariosItce.Usuario;
                        var PasswordFtp = ObjUsuariosItce.Psw;
                        var objHelp = new Helper();

                        var RutaRemota = objFtp.FTP;

                        //if (!File.Exists(FilePath))
                        objHelp.DownLoadFTP(RutaRemota + MiArchivo.ToLower(), FilePath, UsuarioFtp, PasswordFtp);

                        if (File.Exists(FilePath))
                        {
                            var objValidacionD = new ControldeValidacionRepository(_configuration);
                            objValidacionD.Modificar(IdValidado, true);

                            var ArcA = ubicacionDeValidados + "A" + Archivo + Juliano;
                            if (File.Exists(ArcA))
                            {
                                result = SetArchivoA(ArcA, Archivo.Substring(0, 4));
                            }
                        }

                        if (File.Exists(FilePath))
                        {
                            var objValidacionD = new ControldeValidacionRepository(_configuration);
                            objValidacionD.Modificar(IdValidado, true);

                            var ArcA = ubicacionDeValidados + "A" + Archivo + Juliano;
                            if (File.Exists(ArcA))
                            {
                                result = SetArchivoA(ArcA, Archivo.Substring(0, 4));
                            }

                            switch (Archivo.Substring(0, 4))
                            {
                                case "3547":
                                    RutaDestino = BuscaUbicacionDeArchivos(41);
                                    RutaRecibidos = BuscaUbicacionDeArchivos(2);
                                    RutaEnviados = BuscaUbicacionDeArchivos(1);
                                    break;
                                case "1764":
                                    RutaDestino = BuscaUbicacionDeArchivos(42);
                                    RutaRecibidos = BuscaUbicacionDeArchivos(2);
                                    RutaEnviados = BuscaUbicacionDeArchivos(1);
                                    break;
                                case "1693":
                                    switch (controlDeValidacion.IDOficina)
                                    {
                                        case 4:
                                            RutaDestino = BuscaUbicacionDeArchivos(51);
                                            RutaRecibidos = BuscaUbicacionDeArchivos(49);
                                            RutaEnviados = BuscaUbicacionDeArchivos(49);
                                            break;
                                        case 14:
                                            RutaDestino = BuscaUbicacionDeArchivos(66);
                                            RutaRecibidos = BuscaUbicacionDeArchivos(50);
                                            RutaEnviados = BuscaUbicacionDeArchivos(50);
                                            break;
                                    }
                                    break;
                            }

                            RutaDestino = BuscaUbicacionDeArchivos(93);
                            RutaDestino = Path.Combine(RutaDestino, getPatente(controlDeValidacion), $"{MiFechaDePago.Substring(3, 2)}{MiFechaDePago.Substring(6, 4)}", MiFechaDePago.Substring(0, 2));
                            RutaRecibidos = Path.Combine(RutaRecibidos, "E" + Archivo + Juliano);
                            RutaEnviados = Path.Combine(RutaEnviados, "A" + Archivo + Juliano);

                            if (!Directory.Exists(RutaDestino))
                            {
                                Directory.CreateDirectory(RutaDestino);
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Usted no está autorizado para validar o pagar pedimentos con ITCE");
                    }
                    break;

            }


            //si es global y empresa 1 mando
            int iEliminacion = 0;
            iEliminacion = ValidaTipoMovimiento(ArchivoJulianoValidar);
            List<string> MyPedimentos;
            if (iEliminacion == 2) {
                MyPedimentos = ExtraENumerosDePedimento(ArchivoJulianoValidar, 2006, getAduana(controlDeValidacion), getPatente(controlDeValidacion), MiArchivo);
            }
            else
            {
                MyPedimentos = ExtraENumerosDePedimento(ArchivoJulianoValidar, 3, getAduana(controlDeValidacion), getPatente(controlDeValidacion), MiArchivo);
            }

            SaaioPedimeRepository saaioPedimeRepository = new SaaioPedimeRepository(_configuration);
            ReferenciasRepository referenciasRepository = new ReferenciasRepository(_configuration);
            
            APIGenerarConAgtAdu objAPIGenerarConAgtAdu = new(_configuration);
            List<string> lstRespuestas = new();
            foreach (string Pedimento in MyPedimentos)
            {
                SaaioPedime saaioPedime = saaioPedimeRepository.Buscar(Pedimento, getPatente(controlDeValidacion), getAduana(controlDeValidacion));
                Referencias referencia =referenciasRepository.Buscar(saaioPedime.NUM_REFE);
                lstRespuestas = objAPIGenerarConAgtAdu.EnvioporPedimento(referencia.IDReferencia);
            }
            
            //lstRespuestas = objAPIGenerarConAgtAdu.EnvioporPedimento(idReferencia);
            return result;
        }
        public string RegexClass(int MyIDRegex)
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
        
        public List<string> ExtraENumerosDePedimento(string Juliano, int IdRegex, string Aduana, string Patente, string NombreJuliano)
        {
            List<string> result = new List<string>();   
            var re = new Regex(RegexClass(IdRegex), RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;            
            var publicas = new Publicas(); 
            ContenidoDelFicheroOriginal = publicas.MyReadFile(Juliano);
            mc = re.Matches(ContenidoDelFicheroOriginal);

            int cuantos = mc.Count - 1;
            for (int i = 0; i <= cuantos; i++)
            {
                var Pedimento = mc[i].Groups["PEDIMENTO"].Value;
                result.Add(Pedimento.ToString());
            }

            return result;
        }

        public int ValidaTipoMovimiento(string Juliano)
        {
            int ValidaTipoMovimientoRet = default;
            var re = new Regex(RegexClass(2006), RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc;
            string ContenidoDelFicheroOriginal;
            int Elimina = 0;
            int Cuantos;
            var publicas = new Publicas();
            ContenidoDelFicheroOriginal = publicas.MyReadFile(Juliano);
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
        public List<ArchivoErr> SetArchivoErr(string FileName, string Patente, string MiJuliano)
        {
            List<ArchivoErr> lst = new List<ArchivoErr>();
            Publicas publicas = new Publicas();
            foreach (Match m in Regex.Matches(publicas.ReadFileText(FileName), "^.*$", RegexOptions.Multiline))
            {
                // Carga a un arreglo de "strings" los campos de la línea leída separados por "^"
                string[] Arreglo = Regex.Split(m.ToString(), "^");

                switch (!string.IsNullOrEmpty(Arreglo[1]) ? Arreglo[1].Substring(0, 1) : "default")
                {
                    case "F":
                        ArchivoErr objArchivoErrF = new ArchivoErr
                        {
                            Situacion = "FIRMA",
                            Pedimento = Arreglo[1].Substring(1, 7),
                            FirmaElectronica = Arreglo[1].Substring(8, 8),
                            ImpeError = "",
                            SolucionExpertti = "",
                            ClaveimpeError = ""
                        };
                        lst.Add(objArchivoErrF);
                        break;

                    case "E":
                        string vPedimentoE = Arreglo[1].Substring(1, 7);
                        string vRegistroE = Arreglo[1].Substring(8, 3);
                        string vE = Arreglo[1].Substring(11, 4);
                        string vTipoE = Arreglo[1].Substring(15, 1);
                        string vCampoE = Arreglo[1].Substring(16, 2);
                        string vNumeroE = Arreglo[1].Substring(18, 2);
                        string vClaveimpeErrorE = vTipoE + vNumeroE + vRegistroE + vCampoE;

                        ArchivoErr objArchivoErrE = new ArchivoErr
                        {
                            Situacion = "ERROR",
                            Pedimento = vPedimentoE,
                            FirmaElectronica = "",
                            ClaveimpeError = vClaveimpeErrorE
                        };

                        publicas.BuscarRegistroDeError(MiJuliano, vE, vTipoE, vCampoE, vNumeroE, "^500\\|.*?^801\\|.*?$");

                        Ctarc_CatGra objCatGraE = new Ctarc_CatGra();
                        Ctarc_CatGraRepository objCatGraDE = new Ctarc_CatGraRepository(_configuration);
                        objCatGraE = objCatGraDE.Buscar("ERR2", vClaveimpeErrorE);

                        objArchivoErrE.ImpeError = objCatGraE != null ? objCatGraE.DESCRIP.Trim() : "";

                        CatalogodeImpeerror objImpeExperttiE = new CatalogodeImpeerror();
                        CatalogodeImpeerrorRepository objImpeExperttiDE = new CatalogodeImpeerrorRepository(_configuration);
                        objImpeExperttiE = objImpeExperttiDE.Buscar(vClaveimpeErrorE.Trim());

                        objArchivoErrE.SolucionExpertti = objImpeExperttiE != null ? objImpeExperttiE.Descripcion.Trim() : "";

                        lst.Add(objArchivoErrE);
                        break;

                    case "A":
                        string vPedimentoA = Arreglo[1].Substring(1, 7);
                        string vRegistroA = Arreglo[1].Substring(8, 3);
                        string vA = Arreglo[1].Substring(11, 4);
                        string vTipoA = Arreglo[1].Substring(15, 1);
                        string vCampoA = Arreglo[1].Substring(16, 2);
                        string vNumeroA = Arreglo[1].Substring(18, 2);
                        string vClaveimpeErrorA = vTipoA + vNumeroA + vRegistroA + vCampoA;

                        ArchivoErr objArchivoErrA = new ArchivoErr
                        {
                            Situacion = "ADVERTENCIA",
                            Pedimento = vPedimentoA,
                            FirmaElectronica = "",
                            ClaveimpeError = vClaveimpeErrorA
                        };

                        Ctarc_CatGra objCatGraA = new Ctarc_CatGra();
                        Ctarc_CatGraRepository objCatGraDA = new Ctarc_CatGraRepository(_configuration);
                        objCatGraA = objCatGraDA.Buscar("ERR2", vClaveimpeErrorA);

                        objArchivoErrA.ImpeError = objCatGraA != null ? objCatGraA.DESCRIP.Trim() : "";

                        CatalogodeImpeerror objImpeExperttiA = new CatalogodeImpeerror();
                        CatalogodeImpeerrorRepository objImpeExperttiDA = new CatalogodeImpeerrorRepository(_configuration);
                        objImpeExperttiA = objImpeExperttiDA.Buscar(vClaveimpeErrorA.Trim());

                        objArchivoErrA.SolucionExpertti = objImpeExperttiA != null ? objImpeExperttiA.Descripcion.Trim() : "";

                        lst.Add(objArchivoErrA);
                        break;
                }
            }
            return lst;

        }

        public async Task<ValidacionResponse> RecibirValidacion(int IdValidado, int IdUsuario)
        {
            ValidacionResponse result = new ValidacionResponse();

            // Obtener información del usuario
            CatalogoDeUsuariosRepository usuarioRepo = new CatalogoDeUsuariosRepository(_configuration);
            CatalogoDeUsuarios usuario = usuarioRepo.BuscarPorId(IdUsuario);

            // Obtener control de validación
            var controlDeValidacion = Obtener(IdValidado);
            var ubicacionDeValidados = ObtenerUbicacionDeValidados(controlDeValidacion);
            var archivo = controlDeValidacion.Archivo;
            var juliano = controlDeValidacion.Juliano;

            // Definir rutas de archivo
            string archivoJuliano = Path.Combine(ubicacionDeValidados, "M" + archivo + juliano);
            string rutaLocal = Path.GetDirectoryName(archivoJuliano) + "\\";
            string miArchivo = Path.GetFileName(archivoJuliano).ToLower().Replace("m", "k");
            string miArchivoSinExt = Path.GetFileNameWithoutExtension(archivoJuliano);
            string filePath = Path.Combine(rutaLocal, miArchivo);

            // Validación de Agente Aduanal
            CatalogoDeAgentesAduanalesRepository agenteRepo = new CatalogoDeAgentesAduanalesRepository(_configuration);
            CatalogoDeAgentesAduanales agente = agenteRepo.Buscar(getPatente(controlDeValidacion)).FirstOrDefault();
            if (agente == null)
            {
                throw new Exception("No se encuentra agente aduanal");
            }

            // Lógica de Prevalidador
            switch (controlDeValidacion.Prevalidador)
            {
                case "010":
                    var objAAADAM = new ValidarAAADAM(_configuration);
                    await objAAADAM.RecibirValidacion(
                        ubicacionDeValidados, archivo, juliano, getAduana(controlDeValidacion),
                        agente.PasswordValidacion, 0, usuario.IdUsuario);
                    break;

                case "058":
                case "011":
                    result = await ProcesarFtpValidacion(controlDeValidacion, usuario, IdValidado, ubicacionDeValidados, archivo, juliano, miArchivoSinExt, filePath, rutaLocal);
                    break;

                case "039":
                case "040":
                    if (string.IsNullOrEmpty(ubicacionDeValidados))
                    {
                        throw new ArgumentException($"No existe ruta definida para la patente {archivo.Substring(0, 4)}");
                    }
                    result = await ProcesarFtpValidacion(controlDeValidacion, usuario, IdValidado, ubicacionDeValidados, archivo, juliano, miArchivoSinExt, filePath, rutaLocal);
                    break;
            }

            return result;
        }

        // Método para procesar la validación mediante FTP
        private async Task<ValidacionResponse> ProcesarFtpValidacion(dynamic controlDeValidacion, CatalogoDeUsuarios usuario, int IdValidado,
            string ubicacionDeValidados, string archivo, string juliano, string miArchivoSinExt, string filePath, string rutaLocal)
        {
            ValidacionResponse result = new ValidacionResponse();
            var ftpConfig = ObtenerConfiguracionFtp(controlDeValidacion);
            if (ftpConfig == null)
            {
                throw new ArgumentException("No existe una cuenta de FTP disponible");
            }

            // Descargar archivo desde FTP
            var archivoErr = miArchivoSinExt + ".err";
            var archivoDescargado = await DescargarArchivoFtp(ftpConfig, Path.GetFileName(filePath), rutaLocal, archivoErr);

            // Si se descargó correctamente
            if (File.Exists(archivoDescargado))
            {
                var validacionRepo = new ControldeValidacionRepository(_configuration);
                validacionRepo.Modificar(IdValidado, true);

                // Procesar archivos err y k
                var arcErr = Path.Combine(ubicacionDeValidados, "m" + archivo + ".err");
                var arcJul = Path.Combine(ubicacionDeValidados, "m" + archivo + juliano);
                if (File.Exists(arcJul))
                {
                    result.archivoJuliano = GetArchivoJuliano(arcJul);
                }

                if (File.Exists(arcErr))
                {
                    result.archivoErrs= SetArchivoErr(arcErr, archivo.Substring(0, 4), arcJul);
                }

                var archivoK = Path.Combine(ubicacionDeValidados, "k" + archivo + juliano);
                if (File.Exists(archivoK))
                {
                    result.archivoK = GetArchivoK(archivoK);
                }
            }
            else
            {
                throw new Exception("Archivo no encontrado tras la descarga");
            }
            return result;
        }

        // Método para descargar archivos del FTP
        private async Task<string> DescargarArchivoFtp(CatalogoDeFtps ftpConfig, string archivo, string rutaLocal, string archivoErr)
        {
            Helper helper = new Helper();
            string filePath = Path.Combine(rutaLocal, archivo.ToLower());

            // Intentar descargar el archivo principal
            await helper.DownLoadFTPAsync(ftpConfig.FTP + archivo.ToUpper(), filePath, ftpConfig.UsuarioFTP, ftpConfig.PasswordFTP);

            // Verificar si el archivo err ya existe localmente
            string errFilePath = Path.Combine(rutaLocal, archivoErr);
            if (!File.Exists(errFilePath))
            {
                await helper.DownLoadFTPAsync(ftpConfig.FTP + archivoErr.ToLower(), errFilePath, ftpConfig.UsuarioFTP, ftpConfig.PasswordFTP);
            }

            return errFilePath;
        }

        // Método auxiliar para obtener la configuración FTP
        private CatalogoDeFtps ObtenerConfiguracionFtp(dynamic controlDeValidacion)
        {
            CatalogoDeFtpsRepository ftpRepo = new CatalogoDeFtpsRepository(_configuration);
            CatalogoDeFtps ftpConfig = null;

            switch (controlDeValidacion.IDOficina)
            {
                case 2:
                    ftpConfig = ftpRepo.Buscar(16);
                    break;
                case 3:
                    if (getAduana(controlDeValidacion) == "470") ftpConfig = ftpRepo.Buscar(16);
                    if (getAduana(controlDeValidacion) == "850") ftpConfig = ftpRepo.Buscar(33);
                    break;
                case 24:
                    ftpConfig = ftpRepo.Buscar(33);
                    break;
                case 4:
                    ftpConfig = ftpRepo.Buscar(17);
                    break;
                case 19:
                    ftpConfig = ftpRepo.Buscar(30);
                    break;
                case 21:
                    ftpConfig = ftpRepo.Buscar(21);
                    break;
                case 22:
                    ftpConfig = ftpRepo.Buscar(22);
                    break;
                default:
                    ftpConfig = null;
                    break;
            }

            return ftpConfig;
        }



    }
}
