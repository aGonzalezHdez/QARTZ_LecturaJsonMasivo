using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class ClientesyOficinasRepository : IClientesyOficinasRepository
    {     

        public string SConexion { get; set; }
        string IClientesyOficinasRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public ClientesyOficinasRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        /// <summary>
        ///    
        /// </summary>
        /// <param name="Carga combo de oficinas por operación, que se seleccionaron para el cliente"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<OficinaporOperacion> CargarficinasporCliente(int IdCliente)
        {
            List<OficinaporOperacion> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_CASAEI_LOAD_CLIENTESYOFICINAS_OPERACIONPORCLIENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;
                    using SqlDataReader reader = cmd.ExecuteReader();
                    comboList = SqlDataReaderToList.DataReaderMapToList<OficinaporOperacion>(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();
        }

        public string Insertar(ClientesyOficinas objClientesyOficinas)
        {
            string Error = string.Empty;

            int id;
            using (SqlConnection con = new(SConexion))
            using (var cmd = new SqlCommand("NET_INSERT_CLIENTESYOFICINASPOROPERACION", con))
            {
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.Int, 4).Value = objClientesyOficinas.IdCliente;
                cmd.Parameters.Add("@ID_OFICINA", SqlDbType.Int, 4).Value = objClientesyOficinas.IdOficina;
                cmd.Parameters.Add("@STATUS", SqlDbType.Int, 4).Value = objClientesyOficinas.StatusClienteOficina;
                cmd.Parameters.Add("@OPERACION", SqlDbType.Int, 4).Value = objClientesyOficinas.Operacion;
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                try
                {
                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                            Error = "OK";
                        }
                        else
                        {
                            id = 0;
                            Error = "No se guardar los datos de la oficina del cliente.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message.ToString());}
                    Error = ex.Message.ToString();
                }
            }
            return Error;
        }

        public string EliminarAllOficinas(int IdCliente)
        {
            string Error = string.Empty;


            using (SqlConnection con = new(SConexion))
            using (var cmd = new SqlCommand("NET_CASAEI_DELETE_CLIENTESYOFICINAS_ALL", con))
            {
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = IdCliente;
                cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                try
                {
                    using (cmd.ExecuteReader())
                    {
                        //if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        //{
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) == 1)
                        {
                            Error = "OK";
                        }
                        else
                        {
                            Error = "Hubo un error al eliminar los registros de la oficinas del cliente.";
                        }
                        //}                        
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message.ToString() + "NET_INSERT_CLIENTESYOFICINAS");
                    Error = ex.Message.ToString();
                }
            }
            return Error;
        }
    }
}
