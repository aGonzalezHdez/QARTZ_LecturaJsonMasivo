using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogoDeAgentesAduanalesRepository : ICatalogoDeAgentesAduanalesRepository
    {
        public string SConexion { get; set; }
        string ICatalogoDeAgentesAduanalesRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogoDeAgentesAduanalesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<CatalogoDeAgentesAduanales> Buscar(string Patente)
        {
            List<CatalogoDeAgentesAduanales> agentesAduanales = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CatalogodeAgentesAduanales", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Patente", SqlDbType.VarChar).Value = Patente;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        agentesAduanales = SqlDataReaderToList.DataReaderMapToList<CatalogoDeAgentesAduanales>(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return agentesAduanales;
        }
      
        public CatalogoDeAgentesAduanales BuscarRFC(string RFC)
        {
            var objCatalogodeAgentesAduanales = new CatalogoDeAgentesAduanales();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CatalogodeAgentesAduanales_RFC";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @Patente varchar(4)
            @param = cmd.Parameters.Add("@RFC", SqlDbType.VarChar, 13);
            @param.Value = RFC;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCatalogodeAgentesAduanales.IdAgenteAduanal = Convert.ToInt32(dr["IdAgenteAduanal"]);
                    objCatalogodeAgentesAduanales.Patente = Convert.ToInt32(dr["Patente"]);
                    objCatalogodeAgentesAduanales.Nombre = dr["Nombre"].ToString();
                    objCatalogodeAgentesAduanales.Rfc = dr["Rfc"].ToString();
                    objCatalogodeAgentesAduanales.Activo = Convert.ToBoolean(dr["Activo"]);
                    objCatalogodeAgentesAduanales.Prefijo = dr["Prefijo"].ToString();
                    objCatalogodeAgentesAduanales.Nombres = dr["Nombres"].ToString();
                    objCatalogodeAgentesAduanales.ApellidoPaterno = dr["ApellidoPaterno"].ToString();
                    objCatalogodeAgentesAduanales.ApellidoMaterno = dr["ApellidoMaterno"].ToString();
                    objCatalogodeAgentesAduanales.CURP = dr["CURP"].ToString();
                    objCatalogodeAgentesAduanales.ClavedeRepresentante = dr["ClavedeRepresentante"].ToString();
                    objCatalogodeAgentesAduanales.EmpresaFactura = dr["EmpresaFactura"].ToString();
                    objCatalogodeAgentesAduanales.PasswordValidacion = dr["PasswordValidacion"].ToString();
                    objCatalogodeAgentesAduanales.PasswordPago = dr["PasswordPago"].ToString();
                    objCatalogodeAgentesAduanales.PatentesLocales = Convert.ToBoolean(dr["PatentesLocales"]);
                }


                else
                {
                    objCatalogodeAgentesAduanales = default;
                }
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCatalogodeAgentesAduanales;
        }

        public List<DropDownListDatos> Cargar(int IdOficina, string Aduana)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_PATENTES_LOCALES", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDOFICINA", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@ADUANA", SqlDbType.VarChar, 3).Value = Aduana;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return comboList;
        }      
        

    }
}
