using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesProyectos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Proyectos
{
    public class ModulosSistemaRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public ModulosSistemaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<ModulosDelSistema> Cargar()
        {
            List<ModulosDelSistema> modulos = new List<ModulosDelSistema>();

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_LOAD_MODULOSDELSISTEMA", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        cn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ModulosDelSistema obj = new ModulosDelSistema
                                {
                                    IdModulo = Convert.ToInt32(dr["IdModulo"]),
                                    Descripcion = dr["Descripcion"].ToString()
                                };

                                modulos.Add(obj);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return modulos;
        }

    }
}
