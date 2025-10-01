using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using NPOI.SS.Formula.Functions;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using NPOI.HPSF;
//using iText;
//using static iTextSharp.text.pdf.AcroFields;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos
{
    public class JulianosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public JulianosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public RespuestaJuliano Generar(Julianos objJuliano)
        {

            RespuestaJuliano objrespuestaJuliano = new RespuestaJuliano();
            string ArchivoKey = string.Empty;
            string ArchivoCer = string.Empty;

            try
            {
                DataTable dtb = new DataTable();


                ListtoDataTableConverter x = new ListtoDataTableConverter();
                dtb = x.ToDataTable(objJuliano.Referencias);


                using (cn = new(sConexion))
                using (SqlCommand cmd = new("Juliano.NET_INSERT_Juliano", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Aduana", SqlDbType.VarChar, 3).Value = objJuliano.Aduana;
                    cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4).Value = objJuliano.Patente;
                    cmd.Parameters.Add("@Movimiento", SqlDbType.Int, 4).Value = objJuliano.Movimiento;
                    cmd.Parameters.Add("@Prevalidador", SqlDbType.VarChar, 3).Value = objJuliano.Prevalidador;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objJuliano.Operacion;
                    cmd.Parameters.Add("@idOficina", SqlDbType.Int, 4).Value = objJuliano.idOficina;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 4).Value = objJuliano.IdUsuario;
                    cmd.Parameters.Add("@Representante", SqlDbType.VarChar, 2).Value = objJuliano.Representante;
                    cmd.Parameters.Add("@Tabla", SqlDbType.Structured).Value = dtb;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Archivo", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ArchivoTMP", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@RFCAA", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.ExecuteReader();


                    if (Convert.ToInt32(cmd.Parameters["@NEWID_REGISTRO"].Value) != -1)
                    {
                        objrespuestaJuliano.idJuliano = Convert.ToInt32(cmd.Parameters["@NEWID_REGISTRO"].Value);
                        //@Archivo'
                        objrespuestaJuliano.Archivo = cmd.Parameters["@Archivo"].Value.ToString();
                        objrespuestaJuliano.ArchivoTmp = cmd.Parameters["@ArchivoTMP"].Value.ToString();
                        objrespuestaJuliano.RFCAA = cmd.Parameters["@RFCAA"].Value.ToString();
                    }

                    UbicaciondeArchivos objubicacion = new UbicaciondeArchivos();
                    UbicaciondeArchivosRepository objubicacionD = new UbicaciondeArchivosRepository(_configuration);
                    objubicacion = objubicacionD.Buscar(237);

                    if (objubicacion != null)
                    {
                        CatalogodeSellosDigitales objSellos = new CatalogodeSellosDigitales();
                        CatalogodeSellosDigitalesRepository objSellosD = new CatalogodeSellosDigitalesRepository(_configuration);
                        objSellos = objSellosD.Buscar(objrespuestaJuliano.RFCAA.Trim());

                        if (objSellos == null)
                        {
                            throw new Exception("No existen sellos configurados para la patente" + objJuliano.Patente + " y representante con clave" + objJuliano.Representante);
                        }

                        ArchivoKey = objubicacion.Ubicacion + objSellos.UsuarioWebService.Trim() + ".key";
                        ArchivoCer = objubicacion.Ubicacion + objSellos.UsuarioWebService.Trim() + ".cer";
                        string ArchivoBat = objrespuestaJuliano.ArchivoTmp + ".bat";

                        File.WriteAllBytes(ArchivoKey, objSellos.ArchivoKey);
                        File.WriteAllBytes(ArchivoCer, objSellos.ArchivoCer);


                        string PasswordAA = objSellos.PasswordSello.Trim();


                        UbicaciondeArchivos objubicacionExe = new UbicaciondeArchivos();
                        objubicacionExe = objubicacionD.Buscar(240);

                        string CadenaFirmar = string.Empty;
                        CadenaFirmar = objubicacionExe.Ubicacion.Trim() + " " + objrespuestaJuliano.ArchivoTmp + " 0 " + ArchivoCer.Trim() + " " + ArchivoKey.Trim() + " " + PasswordAA;

                        CrearBat(ArchivoBat, CadenaFirmar);

                        //pendiente el firmado
                        firmar(ArchivoBat);


                        if (File.Exists(ArchivoKey))
                        {
                            File.Delete(ArchivoKey);
                        }
                        if (File.Exists(ArchivoCer))
                        {
                            File.Delete(ArchivoCer);
                        }
                        if (File.Exists(ArchivoBat))
                        {
                            File.Delete(ArchivoBat);
                        }


                        if (File.Exists(objrespuestaJuliano.ArchivoTmp + ".OK"))
                        {



                            if (File.Exists(objrespuestaJuliano.Archivo))
                            {
                                File.Delete(objrespuestaJuliano.Archivo);
                                File.Move(objrespuestaJuliano.ArchivoTmp, objrespuestaJuliano.Archivo);
                                File.Delete(objrespuestaJuliano.ArchivoTmp + ".OK");
                            }
                            else
                            {
                                File.Move(objrespuestaJuliano.ArchivoTmp, objrespuestaJuliano.Archivo);
                                File.Delete(objrespuestaJuliano.ArchivoTmp + ".OK");
                            }
                            CatalogoDeUsuariosRepository catalogoDeUsuariosRepository = new CatalogoDeUsuariosRepository(_configuration);
                            CatalogoDeUsuarios catalogoDeUsuarios = catalogoDeUsuariosRepository.BuscarPorId(objJuliano.IdUsuario);

                            firmarEnCasa(objJuliano.Aduana, objJuliano.Patente, catalogoDeUsuarios.UsuarioCASA, objrespuestaJuliano.Archivo);
                        }
                        else
                        {
                            throw new Exception("No se logró firmar el archivo Juliano: " + Path.GetFileName(objrespuestaJuliano.ArchivoTmp));
                        }
                    }
                    else
                    {
                        throw new Exception("No existe ruta de puente");
                    }
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(ArchivoKey))
                {
                    File.Delete(ArchivoKey);
                }
                if (File.Exists(ArchivoCer))
                {
                    File.Delete(ArchivoCer);
                }
                if (File.Exists(objrespuestaJuliano.ArchivoTmp + ".OK"))
                {
                    if (File.Exists(objrespuestaJuliano.ArchivoTmp))
                    {
                        File.Delete(objrespuestaJuliano.ArchivoTmp);
                        File.Delete(objrespuestaJuliano.ArchivoTmp + ".OK");
                    }
                }
                throw new Exception(ex.Message.ToString());
            }
            return objrespuestaJuliano;

        }
        /******************************************************************************************************
          Fecha de Creación: 2025-09-10
          Usuario Crea: Edward - Cubits
          Funcionalidad: Obtener ruta donde se almacenan los julianos por Aduana, Patente y Prevalidador.
        ******************************************************************************************************/
        public string RutaJuliano(string patente,string aduana,string prevalidador)
        {
            try
            {
                string ruta = "";
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("Juliano.NET_Ruta_Juliano", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Aduana", SqlDbType.VarChar, 3).Value = aduana;
                    cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4).Value = patente;
                    cmd.Parameters.Add("@Prevalidador", SqlDbType.VarChar, 3).Value = prevalidador;

                    cmd.Parameters.Add("@Ubicacion", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.ExecuteReader();

                    ruta = cmd.Parameters["@Ubicacion"].Value.ToString();
                    
                }
                return ruta;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public RespuestaPago InsertarPagoConDetalle(Pago objPago)
        {
            RespuestaPago respuesta = new RespuestaPago();
            try
            {
                DataTable dtReferencias = new ListToDataTableConverter().ToDataTable(objPago.Referencias);

                using (SqlConnection cn = new SqlConnection(sConexion))
                using (SqlCommand cmd = new SqlCommand("juliano.InsertarPagoConDetalle", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Aduana", SqlDbType.VarChar, 3).Value = objPago.Aduana;
                    cmd.Parameters.Add("@Patente", SqlDbType.VarChar, 4).Value = objPago.Patente;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int).Value = objPago.Operacion;
                    cmd.Parameters.Add("@Cuenta", SqlDbType.VarChar, 2).Value = objPago.Cuenta ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Prevalidador", SqlDbType.VarChar, 4).Value = objPago.Prevalidador;
                    cmd.Parameters.Add("@idOficina", SqlDbType.Int).Value = objPago.idOficina;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = objPago.idUsuario;
                    cmd.Parameters.Add("@Referencias", SqlDbType.Structured).Value = dtReferencias;
                    cmd.Parameters["@Referencias"].TypeName = "juliano.TipoReferencia";

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            respuesta.idPago = Convert.ToInt32(dr["idPago"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar pago con detalle: " + ex.Message);
            }

            return respuesta;
        }

        public class ListToDataTableConverter
        {
            public DataTable ToDataTable(List<int> items)
            {
                DataTable table = new DataTable();
                table.Columns.Add("idReferencia", typeof(int));

                foreach (var item in items)
                {
                    table.Rows.Add(item);
                }

                return table;
            }
        }


        public bool firmar(string FileName)
        {
            bool firmado = false;
            using (cn = new(sConexion))
            using (SqlCommand cmd = new("NET_FIRMAR_JULIANO", cn))
            {
                cn.Open();

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@FileName", SqlDbType.VarChar, 500).Value = FileName;
                cmd.Parameters.Add("@result", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.ExecuteReader();
                var result = cmd.Parameters["@result"].Value.ToString();
                if (result != string.Empty)
                {
                    firmado = true;
                }
            }
            return firmado;
        }
        public string ObtenerNumeroReferencia(string numPedi, string aduDesp, string patAgen)
        {
            string resultado = null;

            using (SqlConnection conn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_OBTENER_NUM_REF_DESDE_PEDIMENTO", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del procedimiento almacenado
                    cmd.Parameters.AddWithValue("@NUM_PEDI", numPedi);
                    cmd.Parameters.AddWithValue("@ADU_DESP", aduDesp);
                    cmd.Parameters.AddWithValue("@PAT_AGEN", patAgen);

                    conn.Open();

                    // Ejecutar el procedimiento y obtener el resultado
                    object obj = cmd.ExecuteScalar();
                    if (obj != null)
                    {
                        resultado = obj.ToString();
                    }
                }
            }
            return resultado;
        }
        public bool firmarEnCasa(string Aduana, string Patente, string UsuarioCasa, string Juliano)
        {
            bool result = false;
            List<string> valoresExtraidos = new List<string>();

            foreach (var line in File.ReadLines(Juliano))
            {
                var partes = line.Split('|');
                if (partes.Length > 3 && partes[0] == "800")
                {
                    string NumeroDeReferencia = ObtenerNumeroReferencia(partes[1], Aduana, Patente);
                    string FirmaHash = partes[3];
                    int mitad = FirmaHash.Length / 2;

                    // Dividir la cadena en dos partes
                    string parte1 = FirmaHash.Substring(0, mitad);
                    string parte2 = FirmaHash.Substring(mitad);

                    result = InsertOrUpdateSaaioFea(NumeroDeReferencia, parte1, Juliano, UsuarioCasa, parte2);
                }
            }
            return result;
        }

        public bool InsertOrUpdateSaaioFea(string numRefe, string numFea, string nomArch, string cveCapt, string numFea2)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(sConexion))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("NET_INSERT_UPDATE_SAAIO_FEA", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agregar parámetros
                        cmd.Parameters.AddWithValue("@NUM_REFE", numRefe);
                        cmd.Parameters.AddWithValue("@NUM_FEA", numFea);
                        cmd.Parameters.AddWithValue("@NOM_ARCH", nomArch);
                        cmd.Parameters.AddWithValue("@CVE_CAPT", cveCapt);
                        cmd.Parameters.AddWithValue("@NUM_FEA2", numFea2);

                        // Ejecutar el procedimiento almacenado
                        int filasAfectadas = cmd.ExecuteNonQuery();

                        return filasAfectadas > 0; // Retorna true si se afectaron filas
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
        public bool CrearBat(string Archivo, string Cadena)
        {
            bool creado = false;
            try
            {
                string path = Archivo;
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(Cadena);

                    }
                }

                // Open the file to read from.
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
            return creado;
        }


        public bool ValidarReferencias(List<idsReferencias> Referencias, int IDDatosDeEmpresa)
        {
            bool RefeOK = true;

            foreach (idsReferencias idRefe in Referencias)
            {

                Referencias objRefe = new();
                ReferenciasRepository objRefeD = new(_configuration);
                if (idRefe.idReferencia != 0)
                {
                    objRefe = objRefeD.Buscar(idRefe.idReferencia, IDDatosDeEmpresa);
                }

                if (objRefe == null)
                {
                    RefeOK = false;


                }

            }
            return RefeOK;
        }





    }

    public class ListtoDataTableConverter
    {
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }


}
