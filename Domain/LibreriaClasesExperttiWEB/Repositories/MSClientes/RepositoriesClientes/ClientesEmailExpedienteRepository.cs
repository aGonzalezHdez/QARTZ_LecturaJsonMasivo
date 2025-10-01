using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    /// <summary>
    ///  Repositorio para gestionar operaciones CRUD relacionadas con los correos electrónicos de clientes en el expediente.
    /// </summary>
    public class ClientesEmailExpedienteRepository : IClientesEmailExpedienteRepository
    {

        public string SConexion { get; set; }
        string IClientesEmailExpedienteRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public ClientesEmailExpedienteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        /// <summary>
        /// Inserta un nuevo registro de correo electrónico del cliente en la base de datos.
        /// </summary>
        /// <param name="objClientesEmailExpediente">Entidad con la información del cliente.</param>
        /// <returns>Id del nuevo registro insertado.</returns>
        public int Insertar(ClientesEmailExpediente objClientesEmailExpediente)
        {
            int Id = 0;
            try
            {       
                using SqlConnection con = new (SConexion);
                using SqlCommand cmd = new ("NET_INSERT_CASAEI_ClientesEmailExpediente", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de entrada
                cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = objClientesEmailExpediente.IdCliente;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = objClientesEmailExpediente.Email;
                cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int).Value = objClientesEmailExpediente.IDDatosDeEmpresa;
                cmd.Parameters.Add("@Operacion", SqlDbType.Int).Value = objClientesEmailExpediente.Operacion;
                cmd.Parameters.Add("@IdOficina", SqlDbType.Int).Value = objClientesEmailExpediente.IdOficina;

                // Parámetro de salida
                SqlParameter outputIdParam = new("@newid_registro", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                con.Open();
                cmd.ExecuteNonQuery(); // Ejecuta sin esperar resultados tipo tabla

                // Recupera el valor del parámetro de salida
                Id = (int)outputIdParam.Value;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Id;
        }

        public List<ClientesEmailExpediente>Buscar(int IdCliente)
        {
            List<ClientesEmailExpediente> listClientesEmailExpediente = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CASAEI_ClientesEmailExpediente", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Parámetros de entrada
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = IdCliente;

                    using SqlDataReader dr = cmd.ExecuteReader();

                    //Salida
                    while (dr.Read())
                    {
                        ClientesEmailExpediente objClientesEmailExpediente = new()
                        {
                            IdEmail = Convert.ToInt32(dr["IdEmail"]),
                            IdCliente = Convert.ToInt32(dr["IdCliente"]),
                            Email = dr["Email"].ToString(),
                            IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]),
                            IdOperacion = Convert.ToInt32(dr["IdOperacion"]),
                            IdOficina = Convert.ToInt32(dr["IdOficina"]),
                            Empresa = dr["Empresa"].ToString(),
                            Operacion = dr["Operacion"].ToString(),
                            Oficina = dr["Oficina"].ToString()
                        };

                        listClientesEmailExpediente.Add(objClientesEmailExpediente);

                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error general: {ex.Message}", ex);
            }
            return listClientesEmailExpediente;
        }

        public int Modificar(ClientesEmailExpediente objClientesEmailExpediente)
        {
            int Id = 0;

            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_SEARCH_ClientesEmailExpediente", con);          
                cmd.CommandType = CommandType.StoredProcedure;

                // Agregar el parámetro de entrada
                cmd.Parameters.Add("@IdEmail", SqlDbType.Int, 4).Value = objClientesEmailExpediente.IdEmail;
                cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 10).Value = objClientesEmailExpediente.IdCliente;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 50).Value = objClientesEmailExpediente.Email;
                cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = objClientesEmailExpediente.IDDatosDeEmpresa;
                cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objClientesEmailExpediente.Operacion;
                cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = objClientesEmailExpediente.IdOficina;

                // Agregar el parámetro de salida
                SqlParameter outputParam = new("@Resultado", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputParam);

                con.Open();
                cmd.ExecuteNonQuery();

                // Recupera el valor del parámetro de salida
                Id = (int)outputParam.Value;
                          
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Id;

        }


        public int Eliminar(int IdEmail)
        {
            int Id =0;
            try
            {
                using SqlConnection con = new(SConexion);
                using var cmd = new SqlCommand("NET_DELETE_CASAEI_ClientesEmailExpediente", con);           
                cmd.CommandType = CommandType.StoredProcedure;

                // Agregar el parámetro de entrada
                cmd.Parameters.Add("@IdEmail", SqlDbType.Int, 4).Value = IdEmail;

                // Agregar el parámetro de salida
                SqlParameter outputIdParam = new("@newid_registro", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputIdParam);

                con.Open();
                cmd.ExecuteNonQuery(); // Ejecuta sin esperar resultados tipo tabla

                // Recupera el valor del parámetro de salida
                Id = (int)outputIdParam.Value;

              
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }  
            return Id;
        }       

    }
}
