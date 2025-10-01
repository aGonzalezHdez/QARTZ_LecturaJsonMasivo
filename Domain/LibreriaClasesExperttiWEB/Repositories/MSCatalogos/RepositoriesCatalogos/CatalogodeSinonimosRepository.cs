using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogodeSinonimosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodeSinonimosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public DataTable Cargar(int IdCliente)
        {
            DataTable dt = new();
            try
            {
                using (con = new SqlConnection(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODESINONIMOS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;

                    using SqlDataAdapter sda = new(cmd);
                    sda.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dt;
        }

        public DataTable CargarClientes(int IdSinonimos)
        {
            DataTable dt = new();
            try
            {
                using (con = new SqlConnection(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODESINONIMOSPORNOMBRE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDSinonimos", SqlDbType.Int).Value = IdSinonimos;

                    using SqlDataAdapter sda = new(cmd);
                    sda.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dt;
        }

        public int Insertar(CatalogodeSinonimos objCatalogodesinonimos)
        {
            int id;

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CATALOGODESINONIMOS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Sinonimo", SqlDbType.VarChar, 50).Value = objCatalogodesinonimos.Sinonimo;
                    cmd.Parameters.Add("@IDCategoria", SqlDbType.Int, 4).Value = objCatalogodesinonimos.IDCategoria;
                    cmd.Parameters.Add("@NombreDelCliente", SqlDbType.VarChar, 120).Value = objCatalogodesinonimos.NombreDelCliente;
                    cmd.Parameters.Add("@RFC", SqlDbType.VarChar, 13).Value = objCatalogodesinonimos.RFC;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCatalogodesinonimos.IdCliente;
                    cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4).Value = objCatalogodesinonimos.IDUsuario;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
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
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

        public int Modificar(CatalogodeSinonimos objCatalogodesinonimos)
        {
            int id;
            try
            {

                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_CATALOGODESINONIMOS", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDSinonimos", SqlDbType.Int, 4).Value = objCatalogodesinonimos.IDSinonimos;
                    cmd.Parameters.Add("@Sinonimo", SqlDbType.VarChar, 50).Value = objCatalogodesinonimos.Sinonimo;
                    cmd.Parameters.Add("@IDCategoria", SqlDbType.Int, 4).Value = objCatalogodesinonimos.IDCategoria;
                    cmd.Parameters.Add("@NombreDelCliente", SqlDbType.VarChar, 120).Value = objCatalogodesinonimos.NombreDelCliente;
                    cmd.Parameters.Add("@RFC", SqlDbType.Char, 13).Value = objCatalogodesinonimos.RFC;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCatalogodesinonimos.IdCliente;
                    cmd.Parameters.Add("@IDUsuario", SqlDbType.Int, 4).Value = objCatalogodesinonimos.IDUsuario;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    using (cmd.ExecuteReader())
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
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

        public bool Eliminar(int IDSinonimos)
        {
            //int id = 0;
            bool result = false;

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_DELETE_CATALOGODESINONIMOS", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDSinonimos", SqlDbType.Int, 4).Value = IDSinonimos;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (cmd.ExecuteReader())
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
                throw new Exception(ex.Message.ToString());
            }
            return result;
        }
    }
}
