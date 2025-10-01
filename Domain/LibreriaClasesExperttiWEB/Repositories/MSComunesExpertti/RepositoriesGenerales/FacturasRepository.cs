using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales
{
    public class FacturasRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public FacturasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        //public bool GenerarArchivoFacFedexPorPedimentos(DateTime MyFechaDePago, string MyPatente, int MyLote, string MiAduana,
        //                                        string MyUbicacionDeArchivos, string Pedimentos)
        //{
        //    bool result = true;

        //    using (SqlConnection cn = new SqlConnection(SConexion))
        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        try
        //        {
        //            cn.Open();

        //            cmd.CommandText = "NET_GENERA_ARCHIVOS_XML_FEDEX_PEDIMENTOS";
        //            cmd.Connection = cn;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            // @FechaInicial DATETIME
        //            cmd.Parameters.Add(new SqlParameter("@FechaInicial", SqlDbType.DateTime) { Value = MyFechaDePago });

        //            // @Patente CHAR(4)
        //            cmd.Parameters.Add(new SqlParameter("@Patente", SqlDbType.VarChar, 4) { Value = MyPatente });

        //            // @Lote INT
        //            cmd.Parameters.Add(new SqlParameter("@Lote", SqlDbType.Int) { Value = MyLote });

        //            // @Aduana VARCHAR(3)
        //            cmd.Parameters.Add(new SqlParameter("@Aduana", SqlDbType.VarChar, 3) { Value = MiAduana });

        //            // @ArrayPedimentos VARCHAR(MAX)
        //            cmd.Parameters.Add(new SqlParameter("@ArrayPedimentos", SqlDbType.VarChar, 8000) { Value = Pedimentos });

        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                if (dr.HasRows)
        //                {
        //                    while (dr.Read())
        //                    {
        //                        try
        //                        {
        //                            string FileName = dr["NumeroDePedimento"].ToString().Trim() + ".xml";
        //                            string FilePath = Path.Combine(MyUbicacionDeArchivos, FileName);

        //                            // Asumiendo que GenerarXML es una clase con un método Generar que genera el archivo XML
        //                            GenerarXML objXml = new GenerarXML();
        //                            objXml.Generar(dr["Referencia"].ToString(), FilePath);

        //                            if (File.Exists(FilePath))
        //                            {
        //                                GeneraLineaDeComandosEnvioDesftp("sftpFedexImp.bat", FilePath);
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            // Manejo de excepciones específico para el bloque de lectura de datos y generación de XML
        //                            // Puedes añadir un manejo de errores aquí si es necesario
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            result = false;
        //            throw new Exception(ex.Message);
        //        }
        //    }

        //    return result;
        //}


        public bool GeneraLineaDeComandosEnvioDesftp(string MyBat, string MyFile)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();

            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();

                // Asigno el Stored Procedure
                cmd.CommandText = "EXECUTE NET_ENVIO_SFTP '" + MyBat + "','" + MyFile + "'";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;

                // Ejecuto el sp y obtengo el DataSet
                cmd.ExecuteNonQuery();
            }
            // Cierro el DataReader
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message.ToString());
            }

            cmd.Parameters.Clear();
            cn.Close();
            cn.Dispose();

            return true;

        }

    }
}
