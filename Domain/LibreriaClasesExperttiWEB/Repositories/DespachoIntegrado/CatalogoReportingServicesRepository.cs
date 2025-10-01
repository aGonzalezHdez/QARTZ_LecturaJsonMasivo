using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class CatalogoReportingServicesRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public CatalogoReportingServicesRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int GetIdReportingService(string NombreDelReporte)
        {
            int IdReportingRegresar;

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("dbo.getIdReportingService", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NOMBRE_REPORTE", NombreDelReporte);
                    cmd.Parameters.Add("@IdReporting", SqlDbType.VarChar).Direction = ParameterDirection.ReturnValue;

                    cmd.ExecuteScalar();

                    IdReportingRegresar = (int)cmd.Parameters["@IdReporting"].Value;
                }
            }

            return IdReportingRegresar;
        }

        public CatalogoDeReportingService Buscar(int IdReporting)
        {
            CatalogoDeReportingService objCatalogoDeReportingService = new CatalogoDeReportingService();

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("NET_SEARCH_CatalogoDeReportingService", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdReporting", SqlDbType.Int, 4).Value = IdReporting;

                    try
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();

                                objCatalogoDeReportingService.IdReporting = Convert.ToInt32(dr["IdReporting"]);
                                objCatalogoDeReportingService.NombreReporting = dr["NombreReporting"].ToString();
                                objCatalogoDeReportingService.Usuario = dr["Usuario"].ToString();
                                objCatalogoDeReportingService.Password = dr["Password"].ToString();
                                objCatalogoDeReportingService.Url = dr["Url"].ToString();
                                objCatalogoDeReportingService.Path = dr["Path"].ToString();
                                objCatalogoDeReportingService.PathPdf = dr["PathPdf"].ToString();
                                objCatalogoDeReportingService.GenerarPdf = Convert.ToBoolean(dr["GenerarPdf"]);
                                objCatalogoDeReportingService.NuevoUrl = dr["NuevoUrl"].ToString();
                                objCatalogoDeReportingService.NuevoPath = dr["NuevoPath"].ToString();
                            }
                            else
                            {
                                objCatalogoDeReportingService = null;
                            }

                            dr.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return objCatalogoDeReportingService;
        }
    }


}
