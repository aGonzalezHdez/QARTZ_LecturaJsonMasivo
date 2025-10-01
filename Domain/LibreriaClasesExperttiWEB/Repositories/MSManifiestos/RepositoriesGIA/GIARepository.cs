using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF;
using Microsoft.Extensions.Configuration;


namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesGIA
{

    public class GIARepository : IGIARepository
    {
        public IConfiguration _configuration;
        public GIARepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool GIADocumentos()
        {
            try
            {
                DescargaFTPRepository objDescarga = new(_configuration);
                objDescarga.descargarFTP(39);

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        public ManifiestosErrores SubirporDirectorio()
        {
            ManifiestosErrores obj = new ManifiestosErrores();
            List<string> lstErrores = new();
            List<string> lstErrorestodos = new();
            try
            {
                GIADocumentos();

                UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
                UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(261);


                if (Directory.Exists(objRuta.Ubicacion) == false)
                {
                    throw new Exception("El directorio no esta disponible");
                }


                int ArchivosProcesados = 0;
                foreach (string Archivo in Directory.GetFiles(objRuta.Ubicacion.Trim()))
                {
                    try
                    {
                        if (Archivo.Length > 0)
                        {
                            if (Path.GetExtension(Archivo) == ".tiff")
                            {
                                string Arch = Path.GetFileName(Archivo);
                                string GuiaHouse = Arch.Substring(0, 10);

                                SubirDocumentos(GuiaHouse, Archivo);

                                File.Delete(Archivo);
                                ArchivosProcesados += 1;
                            }

                        }

                    }
                    catch (Exception err)
                    {

                        lstErrores.Add(err.Message.Trim());
                    }


                }
                obj.txtTotalError = lstErrorestodos.Count();
                obj.lstErrores = lstErrorestodos;
                //obj.txtFilas = ;
                obj.txtProcesadas = ArchivosProcesados;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }



            return obj;
        }

        public ManifiestosErrores ActualizarExiste()
        {
            ManifiestosErrores obj = new ManifiestosErrores();
            List<string> lstErrores = new();
            List<string> lstErrorestodos = new();
            try
            {
                ImagenesGIARepository objImg = new(_configuration);

                List<ImagenesGIA> lstImagenes = new();
                lstImagenes = objImg.Cargar();


                int ArchivosProcesados = 0;
                foreach (ImagenesGIA item in lstImagenes)
                {
                    try
                    {
                        UbicaciondeArchivos objUbi = new();
                        UbicaciondeArchivosRepository objUbiD = new(_configuration);
                        objUbi = objUbiD.Buscar(4);

                        CustomerMasterFile objCMF = new();
                        CustomerMasterFileRepository objCMFD = new(_configuration);
                        objCMF = objCMFD.Buscar(item.ShipmentId);

                        string Ruta = string.Empty;
                        Ruta = objUbi.Ubicacion.Trim() + objCMF.FechaAlta.Month.ToString() + objCMF.FechaAlta.Year.ToString()
                               + "\\" + objCMF.FechaAlta.Day.ToString() + "\\";


                        string TipoDocuGuia = item.ShipmentId + "_C2_01.TIF";
                        string TipoDocuFac = item.ShipmentId + "_M9_01.TIF";

                        string ArchivoGuia = Ruta + TipoDocuGuia;
                        if (File.Exists(ArchivoGuia))
                        {
                            objCMF.ExisteGuia = true;
                            objCMFD.modificarFacturayGuia(objCMF);
                        }

                        string ArchivoFac = Ruta + TipoDocuGuia;
                        if (File.Exists(ArchivoFac))
                        {
                            objCMF.ExisteFactura = true;
                            objCMFD.modificarFacturayGuia(objCMF);
                        }

                        try
                        {
                            objCMFD.Asignar(objCMF);
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    }
                    catch (Exception err)
                    {

                        lstErrores.Add(err.Message.Trim());
                    }


                }
                obj.txtTotalError = lstErrorestodos.Count();
                obj.lstErrores = lstErrorestodos;
                //obj.txtFilas = ;
                obj.txtProcesadas = ArchivosProcesados;


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }



            return obj;
        }


        public bool SubirDocumentos(string GuiaHouse, string Archivo)
        {
            try
            {
                UbicaciondeArchivos objUbi = new();
                UbicaciondeArchivosRepository objUbiD = new(_configuration);
                objUbi = objUbiD.Buscar(4);

                CustomerMasterFile objCMF = new();
                CustomerMasterFileRepository objCMFD = new(_configuration);
                objCMF = objCMFD.Buscar(GuiaHouse);

                if (objCMF != null)
                {
                    string Ruta = string.Empty;
                    Ruta = objUbi.Ubicacion.Trim() + objCMF.FechaAlta.Month.ToString() + objCMF.FechaAlta.Year.ToString()
                           + "\\" + objCMF.FechaAlta.Day.ToString() + "\\";

                    string Arch = Path.GetFileName(Archivo);
                    string Serie = Arch.Substring(11, 3);

                    string TipoDocu = string.Empty;

                    if (Serie == "EWB")
                    {
                        TipoDocu = GuiaHouse + "_C2_01.TIF";

                        string ArchivoGuia = Ruta + TipoDocu;
                        if (File.Exists(ArchivoGuia) == false)
                        {
                            File.Move(Archivo, ArchivoGuia);
                            objCMF.ExisteGuia = true;
                            objCMFD.modificarFacturayGuia(objCMF);
                        }
                        else
                        {
                            objCMF.ExisteGuia = true;
                            objCMFD.modificarFacturayGuia(objCMF);
                        }
                    }

                    if (Serie == "INV")
                    {
                        TipoDocu = GuiaHouse + "_M9_01.TIF";

                        string ArchivoGuia = Ruta + TipoDocu;
                        if (File.Exists(ArchivoGuia) == false)
                        {
                            File.Move(Archivo, ArchivoGuia);
                            objCMF.ExisteFactura = true;
                            objCMFD.modificarFacturayGuia(objCMF);
                        }
                        else
                        {
                            objCMF.ExisteFactura = true;
                            objCMFD.modificarFacturayGuia(objCMF);
                        }

                    }

                    try
                    {
                        objCMFD.Asignar(objCMF);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }






            }
            catch (Exception)
            {

                throw;
            }




            return true;
        }

    }
}
