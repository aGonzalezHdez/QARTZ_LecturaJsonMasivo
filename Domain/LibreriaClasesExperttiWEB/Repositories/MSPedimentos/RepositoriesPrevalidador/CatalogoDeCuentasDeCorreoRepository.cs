using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador
{
    public class CatalogoDeCuentasDeCorreoRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public CatalogoDeCuentasDeCorreoRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogoDeCuentasDeCorreo BuscarPorId(int MiIDMail)
        {
            var objCATALOGODECUENTASDECORREO = new CatalogoDeCuentasDeCorreo();
            using (var cn = new SqlConnection(SConexion))
            using (var cmd = new SqlCommand())
            {
                try
                {
                    cn.Open();
                    cmd.CommandText = "NET_SEARCH_CATALOGODECUENTASDECORREO_POR_IDMAIL";
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDMail", SqlDbType.Int).Value = MiIDMail;

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCATALOGODECUENTASDECORREO.IDMail = Convert.ToInt32(dr["IDMail"]);
                            objCATALOGODECUENTASDECORREO.Nombre = dr["Nombre"].ToString();
                            objCATALOGODECUENTASDECORREO.gEMail = dr["gEMail"].ToString();
                            objCATALOGODECUENTASDECORREO.gPassMail = dr["gPassMail"].ToString();
                            objCATALOGODECUENTASDECORREO.gHost = dr["gHost"].ToString();
                            objCATALOGODECUENTASDECORREO.gPuertoMail = Convert.ToInt32(dr["gPuertoMail"]);
                            objCATALOGODECUENTASDECORREO.EnableSsl = Convert.ToBoolean(dr["EnableSsl"]);
                            objCATALOGODECUENTASDECORREO.CuentaActiva = Convert.ToBoolean(dr["CuentaActiva"]);
                            objCATALOGODECUENTASDECORREO.Relay = Convert.ToBoolean(dr["Relay"]);
                        }
                        else
                        {
                            return null; // No record found
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCATALOGODECUENTASDECORREO;
        }


    }
}
