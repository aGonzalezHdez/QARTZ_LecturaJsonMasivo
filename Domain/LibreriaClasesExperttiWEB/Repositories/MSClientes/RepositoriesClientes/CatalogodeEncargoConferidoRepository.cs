using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class CatalogodeEncargoConferidoRepository : ICatalogodeEncargoConferidoRepository
    {
     
        public SqlConnection con;

        public string SConexion { get; set; }
        string ICatalogodeEncargoConferidoRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogodeEncargoConferidoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CatalogodeEncargoConferido Buscar(int IDEncargoConferido)
        {
            CatalogodeEncargoConferido objEncargoConferido = new();
            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_CASAEI_SEARCH_CATALOGODEENCARGOCONFERIDO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDEncargoConferido", SqlDbType.VarChar).Value = IDEncargoConferido;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();

                        objEncargoConferido.IDEncargoConferido = Convert.ToInt32(dr["IDEncargoConferido"]);
                        objEncargoConferido.IDPatente = Convert.ToInt32(dr["IDPatente"]);
                        objEncargoConferido.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objEncargoConferido.RFC = string.Format("{0}", dr["RFC"]);
                        objEncargoConferido.Nombre = string.Format("{0}", dr["Nombre"]);
                        objEncargoConferido.Estatus = Convert.ToInt32(dr["Estatus"]);
                        objEncargoConferido.FechaAceptacion = Convert.ToDateTime(dr["FechaAceptacion"]);
                        objEncargoConferido.VigenciaInicio = Convert.ToDateTime(dr["VigenciaInicio"]);
                        objEncargoConferido.VigenciaFinal = Convert.ToDateTime(dr["VigenciaFinal"]);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objEncargoConferido;
        }

        public List<CatalogodeEncargoConferidoIdCliente> Cargar(int IDCliente)
        {
            List<CatalogodeEncargoConferidoIdCliente> list = new();
            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_CASAEI_LOAD_CATALOGODEENCARGOCONFERIDO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = IDCliente;

                    using SqlDataReader reader = cmd.ExecuteReader();
                    list = SqlDataReaderToList.DataReaderMapToList<CatalogodeEncargoConferidoIdCliente>(reader);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return list;
        }

        public int Insertar(CatalogodeEncargoConferido objCatalogodeEncargoConferido)
        {
            int id = 0;
            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_CATALOGODEENCARGOCONFERIDO", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDPatente", SqlDbType.Int, 4).Value = objCatalogodeEncargoConferido.IDPatente;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objCatalogodeEncargoConferido.IDCliente;
                    cmd.Parameters.Add("@Estatus", SqlDbType.Int, 4).Value = objCatalogodeEncargoConferido.Estatus;
                    cmd.Parameters.Add("@FechaAceptacion", SqlDbType.DateTime, 4).Value = objCatalogodeEncargoConferido.FechaAceptacion;
                    cmd.Parameters.Add("@VigenciaInicio", SqlDbType.DateTime, 4).Value = objCatalogodeEncargoConferido.VigenciaInicio;
                    cmd.Parameters.Add("@VigenciaFinal", SqlDbType.DateTime, 4).Value = objCatalogodeEncargoConferido.VigenciaFinal;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int).Direction = ParameterDirection.Output;


                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
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

        public int Modificar(CatalogodeEncargoConferido objCatalogodeEncargoConferido)
        {

            int id = 0;
            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_UPDATE_CATALOGODEENCARGOCONFERIDO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDEncargoConferido", SqlDbType.Int, 4).Value = objCatalogodeEncargoConferido.IDEncargoConferido;
                    cmd.Parameters.Add("@IDPatente", SqlDbType.Int, 4).Value = objCatalogodeEncargoConferido.IDPatente;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objCatalogodeEncargoConferido.IDCliente;
                    cmd.Parameters.Add("@Estatus", SqlDbType.Int, 4).Value = objCatalogodeEncargoConferido.Estatus;
                    cmd.Parameters.Add("@FechaAceptacion", SqlDbType.DateTime, 4).Value = objCatalogodeEncargoConferido.FechaAceptacion;
                    cmd.Parameters.Add("@VigenciaInicio", SqlDbType.DateTime, 4).Value = objCatalogodeEncargoConferido.VigenciaInicio;
                    cmd.Parameters.Add("@VigenciaFinal", SqlDbType.DateTime, 4).Value = objCatalogodeEncargoConferido.VigenciaFinal;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
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

        public bool Eliminar(int IDEncargoConferido)
        {
            bool id = false;
            {
                try
                {
                    using (con = new(SConexion))
                    using (SqlCommand cmd = new("NET_DELETE_CATALOGODEENCARGOCONFERIDO", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IDEncargoConferido", SqlDbType.Int, 4).Value = IDEncargoConferido;
                        cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                        using (cmd.ExecuteReader())
                        {
                            if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                            {
                                id = Convert.ToBoolean(cmd.Parameters["@newid_registro"].Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            return id;
        }

        public bool ExisteEncargo(int IDCliente, int IDPatente)
        {
            bool Existe = false;
            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CATALOGODEENCARGOCONFERIDOXIDCLIENTE", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.VarChar).Value = IDCliente;
                    cmd.Parameters.Add("@IDPatente", SqlDbType.VarChar).Value = IDPatente;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();

                        Existe = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Existe;
        }

        public List<DropDownListDatos> TipodeDocumento(int Patente)
        {
            List<DropDownListDatos> comboList = new();

            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_CASAEI_LOAD_TIPOSDEDOCUMENTOS_ENCARGOCONFERIDO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Patente", SqlDbType.VarChar).Value = Patente;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    comboList = SqlDataReaderToDropDownList.DropDownList<DropDownListDatos>(dr);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList.ToList();
        }
    }
}
