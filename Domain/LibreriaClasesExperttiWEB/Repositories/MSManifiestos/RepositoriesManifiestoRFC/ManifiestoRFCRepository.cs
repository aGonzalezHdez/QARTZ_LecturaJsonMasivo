using Chilkat;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesManifiestoRFC;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using LibreriaClasesAPIExpertti.Entities.EntitiesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesManifiestoRFC
{
    public class ManifiestoRFCRepository : IManifiestoRFCRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;


        int totalGuias = 0;
        int GuiasGuardadas = 0;
        public ManifiestoRFCRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public ManifiestosErrores SubirporDirectorioRFC()
        {
            ManifiestosErrores obj = new ManifiestosErrores();
            List<string> lstErrores = new();
            List<string> lstErrorestodos = new();
            try
            {
                DescargaFTPRepository objDescarga = new(_configuration);
                objDescarga.descargarFTP(42);
                objDescarga.descargarFTP(43);
                UbicaciondeArchivosRepository objUbicacionDeArchivosRepository = new(_configuration);
                UbicaciondeArchivos objRuta = objUbicacionDeArchivosRepository.Buscar(251);


                if (Directory.Exists(objRuta.Ubicacion) == false)
                {
                    throw new Exception("El directorio no esta disponible");
                }

                string DirectorioNuevo = objRuta.Ubicacion.Trim() + DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
                if (Directory.Exists(DirectorioNuevo) == false)
                {
                    Directory.CreateDirectory(DirectorioNuevo);
                }

                int ArchivosProcesados = 0;
                foreach (string Archivo in Directory.GetFiles(objRuta.Ubicacion.Trim()))
                {
                    try
                    {
                        if (Archivo.Length > 0)
                        {
                            if (Path.GetExtension(Archivo) == ".json")
                            {
                                string jsonString = System.IO.File.ReadAllText(Archivo.Trim());
                                ManifiestoRFC objRespuesta = Newtonsoft.Json.JsonConvert.DeserializeObject<ManifiestoRFC>(jsonString);

                                lstErrores = Guardar(objRespuesta);

                                foreach (string str in lstErrores)
                                {
                                    lstErrorestodos.Add(str);
                                }


                                ArchivosProcesados += 1;
                                if (System.IO.File.Exists(Archivo))
                                {
                                    System.IO.File.Move(Archivo, DirectorioNuevo + "/" + Path.GetFileName(Archivo));
                                }
                            }

                        }

                    }
                    catch (Exception err)
                    {

                        string directorioerror = DirectorioNuevo + "/error/";
                        if (Directory.Exists(directorioerror) == false)
                        { Directory.CreateDirectory(directorioerror); }

                        System.IO.File.Move(Archivo, directorioerror + Path.GetFileName(Archivo));
                        lstErrores.Add(err.Message.Trim());
                    }


                }
                obj.txtTotalError = lstErrorestodos.Count();
                obj.lstErrores = lstErrorestodos;
                obj.txtFilas = totalGuias;
                obj.txtProcesadas = GuiasGuardadas;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }



            return obj;
        }

        public List<string> Guardar(ManifiestoRFC obj)
        {
            List<string> errores = new();
            try
            {
                CustomerMasterFileRepository objd = new(_configuration);
                foreach (ManifiestoAWBs item in obj.awbs)
                {
                    objd.modificarRFC(item.house, item.cneeRfc, item.cneeTel, item.cneeEmail);
                }
            }
            catch (Exception ex)
            {

                errores.Add(ex.Message.Trim());
            }
            return errores;

        }

    

        bool IManifiestoRFCRepository.descargarFTP()
        {
            throw new NotImplementedException();
        }
    }
}
