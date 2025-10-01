namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos
{
    using Microsoft.Extensions.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    public class ListadoDeGuiasEnviadasNLURepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public ListadoDeGuiasEnviadasNLURepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = configuration.GetConnectionString("dbCASAEI");
        }
        public async Task<int> Insertar(string GuiaHouse, string GuiaMaster)
        {
            int id = 0;
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_INSERT_CASAEI_LISTADODEGUIASENVIADASNLU", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@GuiaMaster", SqlDbType.VarChar, 15).Value = GuiaMaster;
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 10).Value = GuiaHouse;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (await cmd.ExecuteReaderAsync())
                    {
                        id = (int)cmd.Parameters["@newid_registro"].Value;
                        cmd.Parameters.Clear();
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_LISTADODEGUIASENVIADASNLU");
            }

            return id;
        }
    }
}
