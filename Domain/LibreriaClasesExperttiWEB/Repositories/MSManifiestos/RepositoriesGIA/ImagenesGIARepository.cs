using ClosedXML.Excel;
using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF
{

    public class ImagenesGIARepository : IImagenesGIARepository
    {

        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public ImagenesGIARepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public bool GetExcelfile()
        {
            var dtbImagenes = GetCMFData();

            string base64String = string.Empty;
            using (XLWorkbook wb = new XLWorkbook())
            {
                var sheet = wb.AddWorksheet(dtbImagenes, "Summary");
                //  sheet.Columns(1, 5).Style.Font.FontColor = XLColor.Black;

                UbicaciondeArchivos objUbi = new();
                UbicaciondeArchivosRepository objUbiD = new(_configuration);
                objUbi = objUbiD.Buscar(261);

                string NombreArch = objUbi.Ubicacion.Trim() + "Input template AWB or Inv " + DateTime.Now.ToString("ddMMyyyy") + "_1.xlsx";
                wb.SaveAs(NombreArch);
                EnviarCorreo(NombreArch);
                File.Delete(NombreArch);
            }


            return true;
        }


        private DataTable GetCMFData()
        {
            DataTable data = new DataTable();
            data.TableName = "Imagenes GIA";
            //data.Columns.Add("IdCMF", typeof(int));
            data.Columns.Add("COUNTRY  CODE", typeof(string));
            data.Columns.Add("SHIPMENT ID", typeof(string));
            data.Columns.Add("MAWB (Only for BTO)", typeof(string));
            data.Columns.Add("BROKER NAME (AA)", typeof(string));
            data.Columns.Add("REGISTRY", typeof(string));
            data.Columns.Add("Invoice Req", typeof(string));
            var imagenesGiadata = Cargar();

            if (imagenesGiadata.Count > 0)
            {
                imagenesGiadata.ForEach(x =>
                {
                    //data.Rows.Add(x.IdCMF, x.CountryCode, x.ShipmentId, x.MAWB, x.Broker, x.Registry, x.Invoice);
                    data.Rows.Add(x.CountryCode, x.ShipmentId, x.MAWB, x.Broker, x.Registry, x.Invoice);
                });
            }

            return data;

        }




        public List<ImagenesGIA> Cargar()
        {
            List<ImagenesGIA> lst = new();

            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_LOAD_IMAGENESGIA", cn))
                {
                    cn.Open();

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ImagenesGIA obj = new();
                            obj.IdCMF = Convert.ToInt32(dr["IdCMF"]);
                            obj.CountryCode = dr["COUNTRYCODE"].ToString();
                            obj.ShipmentId = dr["SHIPMENTID"].ToString();
                            obj.MAWB = dr["MAWB"].ToString();
                            obj.Broker = dr["Broker"].ToString();
                            obj.Registry = dr["REGISTRY"].ToString();
                            obj.Invoice = dr["Invoice"].ToString();

                            lst.Add(obj);
                        }


                    }
                    else
                    {
                        lst.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lst;
        }

        public bool EnviarCorreo(string Archivo)
        {
            bool Envio;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_SEND_IMAGENESGIA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@MiNombreDeFichero", SqlDbType.VarChar, 800);
                @param.Value = Archivo;



                cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                Envio = true;
            }
            catch (Exception ex)
            {
                Envio = false;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_SEND_IMAGENESGIA");
            }

            cn.Close();
            cn.Dispose();

            return Envio;
        }
    }
}
