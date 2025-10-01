using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using PdfSharp.Drawing;
using AForge.Imaging.Filters;
using PdfSharp;
using IronSoftware.Drawing;

namespace LibreriaClasesAPIExpertti.Services.Negocios
{
    public class ClConvertir300dpi
    {
        public string ConvertirPDF(string RutaPdf, string RutaSalida, string NombredeArchivo)
        {
            if (File.Exists(RutaPdf) == false)
            {
                throw new ArgumentException("No se pudo acceder al archivo");
            }


            string filename = RutaSalida + NombredeArchivo;
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            if (Directory.Exists(RutaSalida) == false)
            {
                Directory.CreateDirectory(RutaSalida);
            }


            try
            {

                var pdf = IronPdf.PdfDocument.FromFile(RutaPdf);
                pdf.RasterizeToImageFiles(RutaSalida + "*.jpg", ImageType.Jpeg, 300);
                AnyBitmap[] pageImages = pdf.ToBitmap();

                var Doc = new PdfSharp.Pdf.PdfDocument();

                int img = 1;

                foreach (Bitmap item in pageImages)
                {
                    // ConvertScaleGrayImage(item)

                    // Dim grayImage As New Bitmap(New Bitmap(item), 2480, 3248)
                    var grayImage = new Bitmap(new Bitmap(item), 1480, 2248);
                    var filter = new Grayscale(0.2125d, 0.7154d, 0.0721d);
                    Bitmap grayImage2 = filter.Apply(grayImage);
                    grayImage2.SetResolution(300.0f, 300.0f);
                    grayImage2.Save(RutaSalida + "Page-Gray" + img + ".jpg");

                    var pagina = new PdfSharp.Pdf.PdfPage();
                    pagina.Size = PdfSharp.PageSize.Letter;
                    pagina.Orientation = PageOrientation.Portrait;
                    Doc.Pages.Add(pagina);
                    XGraphics pgfx = XGraphics.FromPdfPage(pagina);
                    XImage imagen = XImage.FromFile(RutaSalida + "Page-Gray" + img + ".jpg");
                    pgfx.DrawImage(imagen, 5, 5, 600, 780);
                    // pgfx.DrawImage(imagen, 5, 5)

                    if (Directory.Exists(RutaSalida))
                    {
                        string RutaImg = string.Empty;
                        RutaImg = RutaSalida + img + ".jpg";
                        if (File.Exists(RutaImg))
                        {
                            File.Delete(RutaImg);
                        }
                    }

                    img += 1;

                    grayImage2.Dispose();
                    imagen.Dispose();
                }
                Doc.Save(filename);
                Doc.Close();
                for (int i = 1, loopTo = img - 1; i <= loopTo; i++)
                {
                    if (Directory.Exists(RutaSalida))
                    {
                        string RutaImgGris = string.Empty;
                        RutaImgGris = RutaSalida + "Page-Gray" + i + ".jpg";
                        if (File.Exists(RutaImgGris))
                        {
                            File.Delete(RutaImgGris);
                        }
                    }
                }
            }

            // Dim ImageFiles = Directory.EnumerateFiles(RutaSalida).Where(Function(f) f.EndsWith(".jpg") OrElse f.EndsWith(".jpeg"))
            // ImageToPdfConverter.ImageToPdf(ImageFiles).SaveAs(filename)

            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }

            try
            {
                var info = new FileInfo(filename);
                long length = info.Length;
                double kb = length / 1024d;
                double mb = kb / 1024d;

                int index = 0;
                while (Math.Round(mb, 2) > 5.0d)
                {
                    var infos = new FileInfo(ReConvertirPDF(RutaPdf, RutaSalida, NombredeArchivo));
                    length = infos.Length;
                    double kbs = length / 1024d;
                    double mbs = kbs / 1024d;
                    mb = mbs;
                }
            }
            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }

            return filename;
        }
        public string ReConvertirPDF(string RutaPdf, string RutaSalida, string NombredeArchivo)
        {
            if (File.Exists(RutaPdf) == false)
            {
                throw new ArgumentException("No se pudo acceder al archivo");
            }

            string filename = RutaSalida + NombredeArchivo;

            try
            {

                var pdf = IronPdf.PdfDocument.FromFile(RutaPdf);
                pdf.RasterizeToImageFiles(RutaSalida + "*.jpg", ImageType.Jpeg, 300);
                AnyBitmap[] pageImages = pdf.ToBitmap();

                var Doc = new PdfSharp.Pdf.PdfDocument();

                int img = 1;

                foreach (Bitmap item in pageImages)
                {
                    // ConvertScaleGrayImage(item)

                    // Dim grayImage As New Bitmap(New Bitmap(item), 2480, 3248)
                    var grayImage = new Bitmap(new Bitmap(item), 480, 1248);
                    var filter = new Grayscale(0.2125d, 0.7154d, 0.0721d);
                    Bitmap grayImage2 = filter.Apply(grayImage);
                    grayImage2.SetResolution(300.0f, 300.0f);
                    grayImage2.Save(RutaSalida + "Page-Gray" + img + ".jpg");

                    var pagina = new PdfSharp.Pdf.PdfPage();
                    pagina.Size = PdfSharp.PageSize.Letter;
                    pagina.Orientation = PageOrientation.Portrait;
                    Doc.Pages.Add(pagina);
                    XGraphics pgfx = XGraphics.FromPdfPage(pagina);
                    XImage imagen = XImage.FromFile(RutaSalida + "Page-Gray" + img + ".jpg");
                    pgfx.DrawImage(imagen, 5, 5, 600, 815);
                    // pgfx.DrawImage(imagen, 5, 5)

                    if (Directory.Exists(RutaSalida))
                    {
                        string RutaImg = string.Empty;
                        RutaImg = RutaSalida + img + ".jpg";
                        if (File.Exists(RutaImg))
                        {
                            File.Delete(RutaImg);
                        }
                    }

                    img += 1;

                    grayImage2.Dispose();
                    imagen.Dispose();
                }
                Doc.Save(filename);
                Doc.Close();
                for (int i = 1, loopTo = img - 1; i <= loopTo; i++)
                {
                    if (Directory.Exists(RutaSalida))
                    {
                        string RutaImgGris = string.Empty;
                        RutaImgGris = RutaSalida + "Page-Gray" + i + ".jpg";
                        if (File.Exists(RutaImgGris))
                        {
                            File.Delete(RutaImgGris);
                        }
                    }
                }
            }

            // Dim ImageFiles = Directory.EnumerateFiles(RutaSalida).Where(Function(f) f.EndsWith(".jpg") OrElse f.EndsWith(".jpeg"))
            // ImageToPdfConverter.ImageToPdf(ImageFiles).SaveAs(filename)

            catch (Exception Ex)
            {
                throw new ArgumentException(Ex.Message);
            }

            return filename;
        }

    }
}
