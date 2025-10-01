using Chilkat;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos
{
    public class DescargaFTPRepository
    {
        public IConfiguration _configuration;
        public DescargaFTPRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool descargarFTP(int idFtp)
        {
            bool Respuesta = false;
            try
            {


                SFtp objftp = new SFtp();

                CatalogoDeFtps objCatftp = new CatalogoDeFtps();
                CatalogoDeFtpsRepository objCatftpRep = new CatalogoDeFtpsRepository(_configuration);
                objCatftp = objCatftpRep.Buscar(idFtp);

                BitacoraJobs objbit2 = new BitacoraJobs();

                objbit2.Descripcion = "Mensaje Informativo";
                objbit2.Job = "DescargaCMF_RFC";
                objbit2.ErrorT = "Entra a Decarga de FTP RFC";

                BitacoraJobsData bitacoraJobs2 = new BitacoraJobsData(_configuration);

                bitacoraJobs2.Insertar(objbit2);


                if (objftp.UnlockComponent("EHLECT.CB1112023_3H2GfVbS4R2j"))
                {
                    bool success = objftp.Connect(objCatftp.FTP.Trim(), objCatftp.Puerto);

                    objbit2.Descripcion = "Mensaje Informativo";
                    objbit2.Job = "DescargaCMF_RFC";
                    objbit2.ErrorT = "Conecta a FTP es: " + success.ToString();

                    BitacoraJobsData bitacoraJobs3 = new BitacoraJobsData(_configuration);

                    bitacoraJobs2.Insertar(objbit2);

                    if (success)
                    {
                        success = objftp.AuthenticatePw(objCatftp.UsuarioFTP.Trim(), objCatftp.PasswordFTP.Trim());
                    }

                    if (success == true)
                    {
                        success = objftp.InitializeSftp();
                    }

                    if (success == true)
                    {
                        try
                        {
                            int _mode = 1;
                            bool _recursive = true;

                            success = objftp.SyncTreeDownload(objCatftp.FTPRecibidos.Trim(), objCatftp.PathLocal.Trim(), _mode, _recursive);

                            foreach (string Archivo in Directory.GetFiles(objCatftp.PathLocal.Trim()))
                            {
                                string ArchivoRemoto = objCatftp.FTPRecibidos.Trim() + "/" + Path.GetFileName(Archivo).Trim();
                                success = objftp.RemoveFile(ArchivoRemoto);

                            }

                            objbit2.Descripcion = "Mensaje Informativo";
                            objbit2.Job = "DescargaCMF_RFC";
                            objbit2.ErrorT = "No descarga del FTP" + success.ToString();

                            BitacoraJobsData bitacoraJobs4 = new BitacoraJobsData(_configuration);

                            bitacoraJobs2.Insertar(objbit2);


                            if (success != true)
                            {
                                Debug.WriteLine(objftp.LastErrorText);
                                string _error = objftp.LastErrorText;
                                BitacoraJobs objbit = new BitacoraJobs();

                                objbit.Descripcion = "error de proceso";
                                objbit.Job = "DescargaCMF_RFC";
                                objbit.ErrorT = _error;

                                BitacoraJobsData bitacoraJobs = new BitacoraJobsData(_configuration);

                                bitacoraJobs.Insertar(objbit);

                            }
                        }
                        catch (Exception ex)
                        {
                            BitacoraJobs objbit = new BitacoraJobs();

                            objbit.Descripcion = "error de proceso";
                            objbit.Job = "DescargaCMF_RFC";
                            objbit.ErrorT = ex.Message;

                            BitacoraJobsData bitacoraJobs = new BitacoraJobsData(_configuration);

                            bitacoraJobs.Insertar(objbit);
                        }
                    }

                }


                Respuesta = true;
                //return true;
            }
            catch (Exception ex)
            {
                BitacoraJobs objbit = new BitacoraJobs();

                objbit.Descripcion = "error de proceso";
                objbit.Job = "DescargaCMF";
                objbit.ErrorT = ex.Message;

                BitacoraJobsData bitacoraJobs = new BitacoraJobsData(_configuration);

                bitacoraJobs.Insertar(objbit);
            }
            return Respuesta;
        }

    }
}
