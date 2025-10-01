using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos
{
    public class CatalogoGruposdeCorreoRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public CatalogoGruposdeCorreoRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<CatalogoDeGruposdeCorreo> Cargar(int idGrupo)
        {
            List<CatalogoDeGruposdeCorreo> lstCATALOGODECUENTASDECORREO = new List<CatalogoDeGruposdeCorreo>();

            using (var cn = new SqlConnection(SConexion))
            {
                using (var cmd = new SqlCommand("NET_LOAD_RELACIONESDEGRUPOSDECORREOPORIDGRUPO", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDGRUPO", SqlDbType.Int).Value = idGrupo;

                    try
                    {
                        cn.Open();
                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var objCATALOGODECUENTASDECORREO = new CatalogoDeGruposdeCorreo
                                    {
                                        IDMail = Convert.ToInt32(dr["IDMail"]),
                                        Nombre = dr["Nombre"].ToString(),
                                        Email = dr["Email"].ToString(),
                                        IDRelacion = Convert.ToInt32(dr["IDRelacion"])
                                    };

                                    lstCATALOGODECUENTASDECORREO.Add(objCATALOGODECUENTASDECORREO);
                                }
                            }
                            else
                            {
                                return null; // Or return an empty list as per your requirement
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return lstCATALOGODECUENTASDECORREO;
        }


    }
}
