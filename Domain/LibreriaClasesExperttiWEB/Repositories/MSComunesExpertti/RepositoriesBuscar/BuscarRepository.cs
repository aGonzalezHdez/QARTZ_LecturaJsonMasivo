using LibreriaClasesAPIExpertti.Entities.EntitiesBuscar;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesBuscar
{
    public class BuscarRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con = null!;

        public BuscarRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI");
        }

        public List<BuscarIndice> CargarCampos(int idIndice)
        {
            List<BuscarIndice> Indices = new();
            try
            {
                using (con = new SqlConnection(SConexion))
                {
                    con.Open();
                    var cmd = new SqlCommand("[Buscar].[NET_SEARCH_INDICES_id]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add("@idIndice", SqlDbType.Int, 4).Value = idIndice;
                    SqlDataReader reader = cmd.ExecuteReader();

                    Indices = SqlDataReaderToList.DataReaderMapToList<BuscarIndice>(reader);
                    con.Close();
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                con.Dispose();
                throw new Exception(ex.Message);
            }
            return Indices;
        }

        public DataTable CargarBusquedaDT(int idIndice, int IdCampo, string ValorABuscar, int IDDatosDeEmpresa, string Condicion)
        {
            DataTable Indices = new();
            try
            {
                using (con = new SqlConnection(SConexion))
                {
                    con.Open();
                    var cmd = new SqlCommand("[Buscar].[NET_SEARCH_INDICES_DINAMICOS_id]", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add("@idIndice", SqlDbType.Int, 4).Value = idIndice;
                    cmd.Parameters.Add("@IdCampo", SqlDbType.Int, 4).Value = IdCampo;
                    cmd.Parameters.Add("@ValorABuscar", SqlDbType.VarChar).Value = ValorABuscar;
                    cmd.Parameters.Add("@Condicion", SqlDbType.VarChar).Value = Condicion;
                    cmd.Parameters.Add("@IDDatosDeEmpresa ", SqlDbType.Int, 4).Value = IDDatosDeEmpresa;
                    var da = new SqlDataAdapter(cmd);
                    da.Fill(Indices);



                    con.Close();
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                con.Dispose();
                throw new Exception(ex.Message);
            }
            return Indices;
        }
    }
}
