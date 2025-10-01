using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogodeRegionesRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public CatalogodeRegionesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogodeRegiones Buscar(int idRegion, int idOficina)
        {
            CatalogodeRegiones objCatalogoDeRegiones = new CatalogodeRegiones();
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_SEARCH_CATALOGODEREGIONES_NEW", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdRegion", SqlDbType.Int).Value = idRegion;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int).Value = idOficina;

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                objCatalogoDeRegiones.IdRegion = Convert.ToInt32(dr["IdRegion"]);
                                objCatalogoDeRegiones.Region = dr["Region"].ToString();
                                objCatalogoDeRegiones.Activa = Convert.ToBoolean(dr["Activa"]);
                                objCatalogoDeRegiones.Color = dr["Color"].ToString();
                                objCatalogoDeRegiones.SalidasExigePedimento = Convert.ToBoolean(dr["SalidasExigePedimento"]);
                                objCatalogoDeRegiones.SalidasExcluye = Convert.ToBoolean(dr["SalidasExcluye"]);
                                objCatalogoDeRegiones.ActivarDia = Convert.ToBoolean(dr["ActivarDia"]);
                                objCatalogoDeRegiones.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                            }
                            else
                            {
                                objCatalogoDeRegiones = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return objCatalogoDeRegiones;
        }

    }
}
