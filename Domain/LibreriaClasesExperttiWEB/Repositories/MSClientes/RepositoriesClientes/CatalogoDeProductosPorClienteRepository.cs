using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesS3;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;


namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogoDeProductosPorClienteRepository : ICatalogoDeProductosPorClienteRepository
    {
        public string SConexion { get; set; }
        string ICatalogoDeProductosPorClienteRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogoDeProductosPorClienteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
 
        public CatalogoDeProductosPorCliente Buscar(int MyIdCliente, string MyCodigoDeProducto)
        {
            CatalogoDeProductosPorCliente objCatalogoDeProductos = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODEPRODUCTOSPORCLIENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCliente ", SqlDbType.Int, 4).Value = MyIdCliente;
                    cmd.Parameters.Add("@CodigoDelProducto ", SqlDbType.VarChar, 50).Value = MyCodigoDeProducto;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();
                        objCatalogoDeProductos.IDProductoPorCliente = Convert.ToInt32(dr["IDProductoPorCliente"]);
                        objCatalogoDeProductos.Fraccion = string.Format("{0}", dr["Fraccion"]);
                        objCatalogoDeProductos.NICO = string.Format("{0}", dr["Nico"]);
                        objCatalogoDeProductos.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objCatalogoDeProductos.CodigoDelProducto = string.Format("{0}", dr["CodigoDelProducto"]);
                        objCatalogoDeProductos.DescripcionDelProducto = string.Format("{0}", dr["DescripcionDelProducto"]);
                        objCatalogoDeProductos.DescripcionEnIngles = string.Format("{0}", dr["DescripcionEnIngles"]);
                        objCatalogoDeProductos.ObservacionesDelProducto = string.Format("{0}", dr["ObservacionesDelProducto"]);
                        objCatalogoDeProductos.UnidadDeMedidaDelProducto = string.Format("{0}", dr["UnidadDeMedidaDelProducto"]);
                        objCatalogoDeProductos.PaisDeOrigen = string.Format("{0}", dr["PaisDeOrigen"]);
                        objCatalogoDeProductos.Activo = Convert.ToInt32(dr["Activo"]);
                        objCatalogoDeProductos.FechaDeAltaDeProducto = Convert.ToDateTime(dr["FechaDeAltaDeProducto"]);
                        objCatalogoDeProductos.IdUsuarioAlta = Convert.ToInt32(dr["IdUsuarioAlta"]);
                        objCatalogoDeProductos.FraccionEspecifica = string.Format("{0}", dr["FraccionEspecifica"]);
                        objCatalogoDeProductos.DescripcionEspecifica = string.Format("{0}", dr["DescripcionEspecifica"]);
                        objCatalogoDeProductos.ClaveProductoSAT = string.Format("{0}", dr["ClaveProductoSAT"]);
                        objCatalogoDeProductos.DescripcionSAT = string.Format("{0}", dr["DescripcionSAT"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objCatalogoDeProductos;
        }

        public int Insertar(CatalogoDeProductosPorCliente objCatalogoDeProductosPorCliente)
        {

            int IdProducto = 0;
            ValidaComponenetes(objCatalogoDeProductosPorCliente);
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CATALOGODEPRODUCTOSPORCLIENTE_NEW_ESPECIFICO_NICO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Fraccion", SqlDbType.VarChar, 8).Value = objCatalogoDeProductosPorCliente.Fraccion;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objCatalogoDeProductosPorCliente.IDCliente;
                    cmd.Parameters.Add("@CodigoDelProducto", SqlDbType.VarChar, 50).Value = objCatalogoDeProductosPorCliente.CodigoDelProducto;
                    cmd.Parameters.Add("@DescripcionDelProducto", SqlDbType.VarChar, 300).Value = objCatalogoDeProductosPorCliente.DescripcionDelProducto;
                    cmd.Parameters.Add("@DescripcionEnIngles", SqlDbType.VarChar, 300).Value = objCatalogoDeProductosPorCliente.DescripcionEnIngles;
                    cmd.Parameters.Add("@ObservacionesDelProducto", SqlDbType.VarChar, 800).Value = objCatalogoDeProductosPorCliente.ObservacionesDelProducto;
                    cmd.Parameters.Add("@UnidadDeMedidaDelProducto", SqlDbType.VarChar, 3).Value = objCatalogoDeProductosPorCliente.UnidadDeMedidaDelProducto;
                    cmd.Parameters.Add("@PaisDeOrigen", SqlDbType.VarChar, 3).Value = objCatalogoDeProductosPorCliente.PaisDeOrigen;
                    cmd.Parameters.Add("@Activo", SqlDbType.Int, 4).Value = objCatalogoDeProductosPorCliente.Activo;
                    cmd.Parameters.Add("@Proveedor", SqlDbType.VarChar, 250).Value = objCatalogoDeProductosPorCliente.Proveedor;
                    cmd.Parameters.Add("@Tipo", SqlDbType.Int).Value = objCatalogoDeProductosPorCliente.Tipo;
                    cmd.Parameters.Add("@FraccionEspecifica", SqlDbType.VarChar, 8).Value = objCatalogoDeProductosPorCliente.FraccionEspecifica;
                    cmd.Parameters.Add("@DescripcionEspecifica", SqlDbType.VarChar, 300).Value = objCatalogoDeProductosPorCliente.DescripcionEspecifica;
                    cmd.Parameters.Add("@Nico", SqlDbType.VarChar, 2).Value = objCatalogoDeProductosPorCliente.NICO;
                    cmd.Parameters.Add("@IdUsuarioAlta", SqlDbType.Int, 4).Value = objCatalogoDeProductosPorCliente.IdUsuarioAlta;
                    cmd.Parameters.Add("@ClaveProductoSAT", SqlDbType.VarChar, 15).Value = objCatalogoDeProductosPorCliente.ClaveProductoSAT;
                    cmd.Parameters.Add("@DescripcionSAT", SqlDbType.VarChar, 250).Value = objCatalogoDeProductosPorCliente.DescripcionSAT;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToDouble(cmd.Parameters["@newid_registro"].Value) != 0)
                        {
                            IdProducto = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return IdProducto;
        }

        public int Modificar(CatalogoDeProductosPorCliente objCatalogoDeProductosPorCliente)
        {
            int IdProducto = 0;

            ValidaComponenetes(objCatalogoDeProductosPorCliente);

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_CATALOGODEPRODUCTOSPORCLIENTE_ESPECIFICO_NICO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDProductoPorCliente", SqlDbType.Int, 4).Value = objCatalogoDeProductosPorCliente.IDProductoPorCliente;
                    cmd.Parameters.Add("@Fraccion", SqlDbType.VarChar, 8).Value = objCatalogoDeProductosPorCliente.Fraccion;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objCatalogoDeProductosPorCliente.IDCliente;
                    cmd.Parameters.Add("@CodigoDelProducto", SqlDbType.VarChar, 50).Value = objCatalogoDeProductosPorCliente.CodigoDelProducto;
                    cmd.Parameters.Add("@DescripcionDelProducto", SqlDbType.VarChar, 300).Value = objCatalogoDeProductosPorCliente.DescripcionDelProducto;
                    cmd.Parameters.Add("@DescripcionEnIngles", SqlDbType.VarChar, 300).Value = objCatalogoDeProductosPorCliente.DescripcionEnIngles;
                    cmd.Parameters.Add("@ObservacionesDelProducto", SqlDbType.VarChar, 800).Value = objCatalogoDeProductosPorCliente.ObservacionesDelProducto;
                    cmd.Parameters.Add("@UnidadDeMedidaDelProducto", SqlDbType.VarChar, 3).Value = objCatalogoDeProductosPorCliente.UnidadDeMedidaDelProducto;
                    cmd.Parameters.Add("@PaisDeOrigen", SqlDbType.VarChar, 3).Value = objCatalogoDeProductosPorCliente.PaisDeOrigen;
                    cmd.Parameters.Add("@Activo", SqlDbType.Int, 4).Value = objCatalogoDeProductosPorCliente.Activo;
                    cmd.Parameters.Add("@FraccionEspecifica", SqlDbType.VarChar, 8).Value = objCatalogoDeProductosPorCliente.FraccionEspecifica;
                    cmd.Parameters.Add("@DescripcionEspecifica", SqlDbType.VarChar, 300).Value = objCatalogoDeProductosPorCliente.DescripcionEspecifica;
                    cmd.Parameters.Add("@Nico", SqlDbType.VarChar, 2).Value = objCatalogoDeProductosPorCliente.NICO;
                    cmd.Parameters.Add("@IdUsuarioAlta", SqlDbType.Int, 4).Value = objCatalogoDeProductosPorCliente.IdUsuarioAlta;
                    cmd.Parameters.Add("@ClaveProductoSAT", SqlDbType.VarChar, 15).Value = objCatalogoDeProductosPorCliente.ClaveProductoSAT;
                    cmd.Parameters.Add("@DescripcionSAT", SqlDbType.VarChar, 250).Value = objCatalogoDeProductosPorCliente.DescripcionSAT;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            //IdProducto = System.Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                            IdProducto = objCatalogoDeProductosPorCliente.IDProductoPorCliente;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return IdProducto;
        }


        public async Task<string> CargarArchivoDeExcel(CatalogoDeProductosSubirArchivo objCatalogoDeProductosMasivo)
        {
            string respuesta = string.Empty;
            DataTable dtCatalogoDeProductosPorCliente = CrearDataTableTetra();
            ISheet hojaExcel;
            IWorkbook miExcel;
            string nombreArchivo = PrepararArchivo(objCatalogoDeProductosMasivo);

            try
            {
                if (Path.GetExtension(nombreArchivo).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {

                    miExcel = new XSSFWorkbook(nombreArchivo);
                    miExcel.Close();
                    File.Delete(nombreArchivo);
                    hojaExcel = miExcel.GetSheetAt(0);
                    int cantidadFilas = hojaExcel.PhysicalNumberOfRows;

                    // Procesar las filas del Excel
                    for (int i = 1; i < cantidadFilas; i++) // Comienza en 1 para omitir encabezados
                    {
                        IRow fila = hojaExcel.GetRow(i);
                        if (fila != null)
                        {
                            dtCatalogoDeProductosPorCliente.Rows.Add(LeerFilaTetra(fila));
                        }
                    }

                    // Eliminamos el archivo temporal
                    File.Delete(nombreArchivo);

                    // Guardamos los datos en la base de datos
                    respuesta = await GuardarDatosEnBaseDeDatosTetra(dtCatalogoDeProductosPorCliente);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar el archivo: {ex.Message}", ex);
            }

            return respuesta;
        }
        private static DataTable CrearDataTableTetra()
        {
            DataTable dt = new();
            dt.Columns.Add("CLAVE MATERIAL", typeof(string));
            dt.Columns.Add("UNIDAD TP", typeof(string));
            dt.Columns.Add("HTS 2020", typeof(string));
            dt.Columns.Add("NICO", typeof(string));
            dt.Columns.Add("DESCRIPCION 2020", typeof(string));
            dt.Columns.Add("DESCRIPCION COMERCIAL", typeof(string));
            dt.Columns.Add("QUE ES", typeof(string));
            dt.Columns.Add("TASA 2020", typeof(string));
            dt.Columns.Add("TASA ADV CON TLC", typeof(string));
            dt.Columns.Add("NORMAS OFICIALES", typeof(string));
            dt.Columns.Add("IDENTIFICADOR TLCUEM", typeof(string));
            dt.Columns.Add("UNIDAD TARIFA", typeof(string));
            dt.Columns.Add("UNIDAD COMERCIAL", typeof(string));
            dt.Columns.Add("PO ANEXO 22", typeof(string));
            dt.Columns.Add("NOMBRE PAIS", typeof(string));
            dt.Columns.Add("PO ISO", typeof(string));
            dt.Columns.Add("PESO NETO", typeof(string));
            dt.Columns.Add("UNIDAD PESO NETO", typeof(string));
            dt.Columns.Add("PESO BRUTO", typeof(string));
            dt.Columns.Add("UNIDAD PESO BRUTO", typeof(string));
            dt.Columns.Add("FECHA ACTUALIZACIO", typeof(DateTime));
            return dt;
        }

        private static object[] LeerFilaTetra(IRow fila)
        {
            return new object[]
            {
                fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).DateCellValue.ToString("yyyy-MM-dd 00:00:00.000")
            };
        }   

        private async Task<string> GuardarDatosEnBaseDeDatosTetra(DataTable dtCatalogoDeProductosPorCliente)
        {
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_CATALOGODEPRODUCTOSTETRAPAK_TableType", con))
                {                
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tabla", SqlDbType.Structured)).Value = dtCatalogoDeProductosPorCliente;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    await con.OpenAsync();                   
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    int newIdRegistro = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                    return newIdRegistro switch
                    {
                        1 => "Ok",
                        -1 => "Ocurrió un error al insertar el catálogo de productos, inténtalo de nuevo.",
                        _ => "Ocurrió un error desconocido."
                    };

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar datos en la base de datos: {ex.Message}", ex);
            }
        }


        public async Task<string> CargarArchivoDeExcelVariosClientes(CatalogoDeProductosSubirArchivo objCatalogoDeProductosMasivo)
        {
            string respuesta = string.Empty;
            DataTable dtCatalogoDeProductosPorCliente = CrearDataTable();
            ISheet hojaExcel;
            IWorkbook miExcel;
            string nombreArchivo = PrepararArchivo(objCatalogoDeProductosMasivo);

            try
            {
                if (Path.GetExtension(nombreArchivo).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {                

                    miExcel = new XSSFWorkbook(nombreArchivo);
                    miExcel.Close();
                    File.Delete(nombreArchivo);
                    hojaExcel = miExcel.GetSheetAt(0); 
                    int cantidadFilas = hojaExcel.PhysicalNumberOfRows;

                    // Procesar las filas del Excel
                    for (int i = 1; i < cantidadFilas; i++) // Comienza en 1 para omitir encabezados
                    {
                        IRow fila = hojaExcel.GetRow(i);
                        if (fila != null)
                        {
                            dtCatalogoDeProductosPorCliente.Rows.Add(LeerFila(fila));
                        }
                    }                    

                    // Eliminamos el archivo temporal
                    File.Delete(nombreArchivo);

                    // Guardamos los datos en la base de datos
                    respuesta = await GuardarDatosEnBaseDeDatos(dtCatalogoDeProductosPorCliente, objCatalogoDeProductosMasivo.IDCliente);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar el archivo: {ex.Message}", ex);
            }

            return respuesta;
        }
        private static DataTable CrearDataTable()
        {
            DataTable dt = new();
            dt.Columns.Add("CodigoDelProducto", typeof(string));
            dt.Columns.Add("Fraccion", typeof(string));
            dt.Columns.Add("Nico", typeof(string));
            dt.Columns.Add("UnidadDeMedidaDelProducto", typeof(string));
            dt.Columns.Add("DescripcionDelProducto", typeof(string));
            dt.Columns.Add("DescripcionEnIngles", typeof(string));
            dt.Columns.Add("PaisDeOrigen", typeof(string));
            dt.Columns.Add("ObservacionesDelProducto", typeof(string));
            dt.Columns.Add("FechaDeAltaDeProducto", typeof(DateTime));
            dt.Columns.Add("Peso", typeof(string));
            return dt;
        }

        private static object[] LeerFila(IRow fila)
        {
            return new object[]
            {
                fila.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                fila.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).DateCellValue.ToString("yyyy-MM-dd 00:00:00.000"),
                fila.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString()
            };
        }

        private string PrepararArchivo(CatalogoDeProductosSubirArchivo objCatalogoDeProductosMasivo)
        {
            UbicaciondeArchivosRepository ubicacionRepo = new(_configuration);
            UbicaciondeArchivos objRuta = ubicacionRepo.Buscar(171);

            // Crear directorio si no existe
            if (!Directory.Exists(objRuta.Ubicacion))
            {
                Directory.CreateDirectory(objRuta.Ubicacion);
            }

            string nombreArchivo = Path.Combine(objRuta.Ubicacion, objCatalogoDeProductosMasivo.NombreArchivo);

            // Guardar el archivo temporalmente
            byte[] bytes = Convert.FromBase64String(objCatalogoDeProductosMasivo.ArchivoBase64);
            File.WriteAllBytes(nombreArchivo, bytes);

            return nombreArchivo;
        }

        private async Task<string> GuardarDatosEnBaseDeDatos(DataTable dtCatalogoDeProductosPorCliente, int idCliente)
        {
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CATALOGODEPRODUCTOSPORCLIENTE_TableType", con))
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Tabla", SqlDbType.Structured) { Value = dtCatalogoDeProductosPorCliente });
                    cmd.Parameters.Add(new SqlParameter("@IDCliente", SqlDbType.Int) { Value = idCliente });
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int).Direction = ParameterDirection.Output;

                    await con.OpenAsync();                    
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    int newIdRegistro = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                    return newIdRegistro switch
                    {
                        1 => "Ok",
                        -1 => "Ocurrió un error al insertar el catálogo de productos, inténtalo de nuevo.",
                        _ => "Ocurrió un error desconocido."
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar datos en la base de datos: {ex.Message}", ex);
            }
        }
        public async Task<string> CopiaArchivoAlDestinoS3(string MyRutadeOrigen, int IdCliente, string CodigoDelProducto)
        {
            string RutaS3;
            try
            {
                AWSS3Repository objS3BucketsRepository = new(_configuration);
                S3Buckets objS3Buckets = await objS3BucketsRepository.Buscar(3);
                if (objS3Buckets == null)
                {
                    throw new ArgumentException("No existe Bucket Configurado, favor de avisar al equipo de desarrollo");
                }

                int Consecutivo = 0;
                CatalogodeDocumentosDeProductosRepository CatalogodeDocumentosDeProductosRepository = new(_configuration);
                Consecutivo = CatalogodeDocumentosDeProductosRepository.BuscarConsecutivo(IdCliente, CodigoDelProducto);

                string Extension = string.Empty;
                Extension = Path.GetExtension(MyRutadeOrigen);
                RutaS3 = IdCliente + "/" + "Productos" + "/" + CodigoDelProducto.Trim() + "/" + "Imagen_" + Consecutivo.ToString("000") + Extension.Trim();

                string subioS3 = string.Empty;
                BucketsS3Repository objS3 = new(_configuration);
                subioS3 = await objS3.SubirObjetoAsync(MyRutadeOrigen, objS3Buckets.Bucket, RutaS3.Trim());
                //subioS3 = "OK";
                if (subioS3.ToUpper() != "OK")
                {
                    RutaS3 = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return RutaS3;
        }

        public List<HistoricodeFracciones> CargarHistorico(string Clave, int NoPagina)
        {

            List<HistoricodeFracciones> list = new();

            {
                try
                {
                    using (SqlConnection con = new(SConexion))
                    using (SqlCommand cmd = new("NET_LOAD_FRACCIONESUTILIZADASPORCLIENTE_PAGINADO", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CVE_IMPO", SqlDbType.VarChar, 6).Value = Clave;
                        cmd.Parameters.Add("@PageNumber", SqlDbType.Int, 4).Value = NoPagina;

                        using SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                HistoricodeFracciones objHistoricodeFracciones = new()
                                {
                                    Patente = dr["Patente"].ToString(),
                                    Pedimento = dr["Pedimento"].ToString(),
                                    Operacion = dr["Operacion"].ToString(),
                                    Guia = dr["Guia"].ToString(),
                                    Partida = Convert.ToDouble(dr["Partida"]),
                                    Fraccion = dr["Fraccion"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Origen = dr["Origen"].ToString(),
                                    Pago = Convert.ToDateTime(dr["Pago"]),
                                    RowNumber = Convert.ToInt32(dr["RowNumber"]),
                                    CurrentPage = Convert.ToInt32(dr["CurrentPage"]),
                                    TotalPages = Convert.ToInt32(dr["TotalPages"])
                                };
                                list.Add(objHistoricodeFracciones);
                            }
                        }
                        else
                            list = null/* TODO Change to default(_) if this is not a reference type */;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            return list;
        }

        public List<CatalogoDeProductosPorClientePaginado> Cargar(int IdCliente, int PageNumber)
        {
            List<CatalogoDeProductosPorClientePaginado> list = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODEPRODUCTOSPORCLIENTEIDCLIENTE_PAGINADO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int).Value = IdCliente;
                    cmd.Parameters.Add("@PageNumber", SqlDbType.Int, 4).Value = PageNumber;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CatalogoDeProductosPorClientePaginado objCatalogoDeProductosPorClientePaginado = new()
                            {
                                IDProductoPorCliente = Convert.ToInt32(dr["IDProductoPorCliente"]),
                                Fraccion = dr["Fraccion"].ToString(),
                                Nico = dr["Nico"].ToString(),
                                CodigoDelProducto = dr["CodigoDelProducto"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                DescripcionEnIngles = dr["DescripcionEnIngles"].ToString(),
                                Observaciones = dr["Observaciones"].ToString(),
                                UsuarioAlta = dr["UsuarioAlta"].ToString(),
                                RowNumber = Convert.ToInt32(dr["RowNumber"]),
                                CurrentPage = Convert.ToInt32(dr["CurrentPage"]),
                                TotalPages = Convert.ToInt32(dr["TotalPages"])
                            };
                            list.Add(objCatalogoDeProductosPorClientePaginado);
                        }
                    }
                    else
                        list = null/* TODO Change to default(_) if this is not a reference type */;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return list;
        }

        public List<DropDownListDatos> TipodeDocumento()
        {
            List<DropDownListDatos> comboList = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_LOAD_TIPOSDEDOCUMENTOSPRODUCTOS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(dr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();
        }

        public async Task<string> SubirImagen(SubirImagen objSubirImagen)
        {
            string Respuesta = string.Empty;

            UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
            string NombreArchivo = objSubirImagen.NombreArchivo;

            UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(180);
            if (Directory.Exists(objRuta.Ubicacion) == false)
            {
                Directory.CreateDirectory(objRuta.Ubicacion);
            }

            try
            {
                string rutaArchivo = objRuta.Ubicacion + NombreArchivo;
                byte[] bytes = Convert.FromBase64String(objSubirImagen.ArchivoBase64);
                File.WriteAllBytes(rutaArchivo, bytes);

                if (Path.GetExtension(rutaArchivo) == ".jpg" || Path.GetExtension(rutaArchivo) == ".pdf" || Path.GetExtension(rutaArchivo) == ".doc" || Path.GetExtension(rutaArchivo) == ".docx")
                {
                    CatalogoDeProductosPorClienteRepository CatalogoDeProductosPorClienteRepository = new(_configuration);
                    string RutaS3 = await CatalogoDeProductosPorClienteRepository.CopiaArchivoAlDestinoS3(rutaArchivo, objSubirImagen.IDCliente, objSubirImagen.CodigoDelProducto);

                    if (RutaS3 == "")
                    {
                        return Respuesta = "No fue posible guardar el documento en S3";
                    }

                    CatalogodeDocumentosDeProductos objCatalogodeDocumentosDeProductos = new();
                    CatalogodeDocumentosDeProductosRepository CatalogodeDocumentosDeProductosRepository = new(_configuration);
                    objCatalogodeDocumentosDeProductos.IdCliente = objSubirImagen.IDCliente;
                    objCatalogodeDocumentosDeProductos.CodigoDeProducto = objSubirImagen.CodigoDelProducto;
                    objCatalogodeDocumentosDeProductos.IdTipoDocumento = objSubirImagen.IdTipoDocumento;
                    objCatalogodeDocumentosDeProductos.Archivo = NombreArchivo;
                    objCatalogodeDocumentosDeProductos.S3 = true;
                    objCatalogodeDocumentosDeProductos.RutaS3 = RutaS3;

                    int IdImagen = CatalogodeDocumentosDeProductosRepository.Insertar(objCatalogodeDocumentosDeProductos);

                    if (IdImagen == 0)
                    {
                        return Respuesta = "No se logro subir la imagen";
                    }

                    Respuesta = "Ok";
                }
                else
                {
                    return Respuesta = "Documentos permitidos con extensión .jpg, .pdf, .doc o .docx";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Respuesta;
        }

        public void ValidaComponenetes(CatalogoDeProductosPorCliente objCatalogoDeProductosPorCliente)
        {
            if (objCatalogoDeProductosPorCliente.IDCliente == 0)
                throw new Exception("Ingrese el Cliente");

            if (string.IsNullOrEmpty(objCatalogoDeProductosPorCliente.CodigoDelProducto))
                throw new Exception("Ingrese el Codigo de Producto");

            if (string.IsNullOrEmpty(objCatalogoDeProductosPorCliente.Fraccion))
                throw new Exception("Ingrese la Fracción");

            if (string.IsNullOrEmpty(objCatalogoDeProductosPorCliente.UnidadDeMedidaDelProducto))
                throw new Exception("Ingrese la Clave UMT");

            //if (this.txtDescripcionUMC.Text.Trim == "")
            //    throw new Exception("Ingrese la Clave UMT");

            if (string.IsNullOrEmpty(objCatalogoDeProductosPorCliente.DescripcionDelProducto))
                throw new Exception("Ingrese la Descripcion para el Pedimento");

            if (string.IsNullOrEmpty(objCatalogoDeProductosPorCliente.PaisDeOrigen))
                throw new Exception("Ingrese la Clave de Pais");


        }
        //Se Implementa el método para registrar en la bitacora quien sube el archivo 04/09/2025 OLE
        public async Task<string> RegistraBitacoraMasivos(int IdCliente, string NombreArchivo, int IdUsuario)
        {
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_BITACORAALTAPRODUCTOSPORCLIENTEMASIVOS", con))
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdCliente", SqlDbType.Int)).Value = IdCliente;
                    cmd.Parameters.Add(new SqlParameter("@NombreArchivo", SqlDbType.VarChar,100)).Value = NombreArchivo;
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int)).Value = IdUsuario;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    await con.OpenAsync();
                    SqlDataReader dr = await cmd.ExecuteReaderAsync();

                    int newIdRegistro = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                    return newIdRegistro switch
                    {
                        1 => "Ok",
                        -1 => "Ocurrió un error al insertar en la bitácora.",
                        _ => "Ocurrió un error desconocido."
                    };

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar datos en la base de datos: {ex.Message}", ex);
            }
        }
    }
}
