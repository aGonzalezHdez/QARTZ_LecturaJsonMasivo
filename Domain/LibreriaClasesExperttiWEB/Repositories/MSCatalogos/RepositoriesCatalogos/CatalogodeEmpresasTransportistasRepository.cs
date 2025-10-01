using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogodeEmpresasTransportistasRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public CatalogodeEmpresasTransportistasRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CatalogodeEmpresasTransportistas Buscar(int IdEmpTransportista)
        {
            CatalogodeEmpresasTransportistas objCatalogodeEmpresasTransportistas = new CatalogodeEmpresasTransportistas();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_SEARCH_CASAEI_CATALOGODEEMPRESASTRANSPORTISTAS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                param = cmd.Parameters.Add("@IdEmpTransportista", SqlDbType.Int, 4);
                param.Value = IdEmpTransportista;
                //Insertar Parametro de busqueda

                cn.ConnectionString = SConexion;
                cn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objCatalogodeEmpresasTransportistas.IdEmpTransportista = Convert.ToInt32(dr["IdEmpTransportista"]);
                    objCatalogodeEmpresasTransportistas.Nombre = dr["Nombre"].ToString();
                    objCatalogodeEmpresasTransportistas.CAAT = dr["CAAT"].ToString();
                    objCatalogodeEmpresasTransportistas.Activa = Convert.ToBoolean(dr["Activa"]);
                }
                else
                {
                    objCatalogodeEmpresasTransportistas = null;
                }
                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return objCatalogodeEmpresasTransportistas;
        }
    }
}
