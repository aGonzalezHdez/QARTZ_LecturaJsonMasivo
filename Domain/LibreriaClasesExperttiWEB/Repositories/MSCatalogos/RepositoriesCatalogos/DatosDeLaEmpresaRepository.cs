using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class DatosDeLaEmpresaRepository : IDatosDeLaEmpresaRepository
    {
        public string SConexion { get; set; }
        string IDatosDeLaEmpresaRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public DatosDeLaEmpresaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public DatosDeLaEmpresa Buscar(int id)
        {
            DatosDeLaEmpresa objDATOSDELAEMPRESA = new();
            try
            {

                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_DATOSDELAEMPRESA", con))
                {
                    con.Open();   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = id;
                    
                    SqlDataReader  dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        objDATOSDELAEMPRESA.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                        objDATOSDELAEMPRESA.Nombre = dr["Nombre"].ToString();
                        objDATOSDELAEMPRESA.RFC = dr["RFC"].ToString();
                        objDATOSDELAEMPRESA.CURP = dr["CURP"].ToString();
                        objDATOSDELAEMPRESA.Calle = dr["Calle"].ToString();
                        objDATOSDELAEMPRESA.NumeroExt = dr["NumeroExt"].ToString();
                        objDATOSDELAEMPRESA.NumeroInt = dr["NumeroInt"].ToString();
                        objDATOSDELAEMPRESA.Colonia = dr["Colonia"].ToString();
                        objDATOSDELAEMPRESA.CiudadOPoblacion = dr["CiudadOPoblacion"].ToString();
                        objDATOSDELAEMPRESA.Estado = dr["Estado"].ToString();
                        objDATOSDELAEMPRESA.CodigoPostal = dr["CodigoPostal"].ToString();
                        objDATOSDELAEMPRESA.Telefono = dr["Telefono"].ToString();
                        objDATOSDELAEMPRESA.EntreLaCalleDe = dr["EntreLaCalleDe"].ToString();
                        objDATOSDELAEMPRESA.RegimenFiscal = dr["RegimenFiscal"].ToString();
                         
                    }
                    else
                        objDATOSDELAEMPRESA = null!;
                }
              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objDATOSDELAEMPRESA;
        }


        public List<DropDownListDatos> Cargar()
        {
            List<DropDownListDatos> comboList = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_LOAD_DATOSDELAEMPRESA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(dr);
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();

        }

    }
}
