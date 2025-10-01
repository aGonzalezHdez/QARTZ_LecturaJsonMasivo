using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CuentasDHLRepository : ICuentasDHLRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CuentasDHLRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<List<CuentasDHL>> Cargar(int IdCliente)
        {
            List<CuentasDHL> listCuentasDHL = new List<CuentasDHL>();
            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_LOAD_CatalogodeCuentasDHL", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = IdCliente;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        listCuentasDHL = SqlDataReaderToList.DataReaderMapToList<CuentasDHL>(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return listCuentasDHL.ToList();
        }


        public async Task<int> Insertar(CuentasDHL objCatalogoDeCuentasDHL)
        {
            int id;

            try
            {
                using (con = new SqlConnection(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_CatalogodeCuentasDHL", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCatalogoDeCuentasDHL.IdCliente;
                    cmd.Parameters.Add("@NumerodeCuenta", SqlDbType.VarChar, 25).Value = objCatalogoDeCuentasDHL.NumerodeCuenta;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (await cmd.ExecuteReaderAsync())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CatalogodeCuentasDHL");
            }

            return id;
        }
        public async Task<bool> Eliminar(int idCuenta)
        {
            bool result = false;
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_CASAEI_DELETE_CATALOGODECUENTASDHL", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCUENTA", SqlDbType.Int, 4).Value = idCuenta;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (await cmd.ExecuteReaderAsync())
                    {
                        //if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        //{
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                        {
                            result = true;
                        }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        public string ValidaSiExisteCuentaDHL(CuentasDHL objCatalogoDeCuentasDHL)
        {
            CuentasDHLRepository objGuardaCuentasDHLD = new(_configuration);
            List<CuentasDHL> CuentasDHLDb = objGuardaCuentasDHLD.BuscarSiExisteCuentaDHL(objCatalogoDeCuentasDHL.IdCliente, objCatalogoDeCuentasDHL.NumerodeCuenta);

            string Error = string.Empty;

            if (CuentasDHLDb.Count != 0)
            {
                Error = $"Este núumero de cuenta {objCatalogoDeCuentasDHL.NumerodeCuenta} ya fue Ingresado con anterioridad";
            }
            return Error;
        }
        public List<CuentasDHL> BuscarSiExisteCuentaDHL(int IDCliente, string NumerodeCuenta)
        {
            List<CuentasDHL> CuentasDHL = new();
            try
            {
                using (con = new SqlConnection(SConexion))
                {
                    con.Open();
                    var cmd = new SqlCommand("NET_SEARCH_CATALOGODECUENTASDHLSIEXISTE", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = IDCliente;
                    cmd.Parameters.Add("@NumerodeCuenta", SqlDbType.VarChar).Value = NumerodeCuenta;
                    SqlDataReader reader = cmd.ExecuteReader();

                    CuentasDHL = SqlDataReaderToList.DataReaderMapToList<CuentasDHL>(reader);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return CuentasDHL.ToList();
        }

    }
}

