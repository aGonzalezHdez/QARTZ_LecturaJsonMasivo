using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{

    public class CatalogodeApoderadosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogodeApoderadosRepository(IConfiguration configuration)
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
                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODEAPODERADOSPORIDCLIENTE", con))
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


        public int Insertar(CatalogodeApoderados objCatalogodeApoderados)
        {
            bool Nuevo = true;
            ValidaObjeto(objCatalogodeApoderados, Nuevo);

            int id;

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CATALOGODEAPODERADOS_AGA", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = objCatalogodeApoderados.Nombre;
                    cmd.Parameters.Add("@Rfc", SqlDbType.NVarChar, 13).Value = objCatalogodeApoderados.Rfc;
                    cmd.Parameters.Add("@Curp", SqlDbType.NVarChar, 20).Value = objCatalogodeApoderados.Curp;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objCatalogodeApoderados.IDCliente;
                    cmd.Parameters.Add("@Activo", SqlDbType.Int, 4).Value = objCatalogodeApoderados.Activo;
                    cmd.Parameters.Add("@ADefault", SqlDbType.Int, 4).Value = objCatalogodeApoderados.ADefault;
                    cmd.Parameters.Add("@EscrituraPublica", SqlDbType.NVarChar, 50).Value = objCatalogodeApoderados.EscrituraPublica;
                    cmd.Parameters.Add("@FechaEscritura", SqlDbType.Date).Value = objCatalogodeApoderados.FechaEscritura;
                    cmd.Parameters.Add("@DomicilioFiscal", SqlDbType.NVarChar, 800).Value = objCatalogodeApoderados.DomicilioFiscal;
                    cmd.Parameters.Add("@CorreoElectronico", SqlDbType.NVarChar, 100).Value = objCatalogodeApoderados.CorreoElectronico;
                    cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value = objCatalogodeApoderados.Telefono;



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


        public int Modificar(CatalogodeApoderados objCatalogodeApoderados)
        {
            bool Nuevo = false;
            ValidaObjeto(objCatalogodeApoderados, Nuevo);
            int id;
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_UPDATE_CATALOGODEAPODERADOS_AGA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idApoderado", SqlDbType.Int, 4).Value = objCatalogodeApoderados.IdApoderado;
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50).Value = objCatalogodeApoderados.Nombre;
                    cmd.Parameters.Add("@Rfc", SqlDbType.NVarChar, 13).Value = objCatalogodeApoderados.Rfc;
                    cmd.Parameters.Add("@Curp", SqlDbType.NVarChar, 20).Value = objCatalogodeApoderados.Curp;
                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objCatalogodeApoderados.IDCliente;
                    cmd.Parameters.Add("@Activo", SqlDbType.Int, 4).Value = objCatalogodeApoderados.Activo;
                    cmd.Parameters.Add("@ADefault", SqlDbType.Int).Value = objCatalogodeApoderados.ADefault;
                    cmd.Parameters.Add("@EscrituraPublica", SqlDbType.NVarChar, 50).Value = objCatalogodeApoderados.EscrituraPublica;
                    cmd.Parameters.Add("@FechaEscritura", SqlDbType.Date).Value = objCatalogodeApoderados.FechaEscritura;
                    cmd.Parameters.Add("@DomicilioFiscal", SqlDbType.NVarChar, 800).Value = objCatalogodeApoderados.DomicilioFiscal;
                    cmd.Parameters.Add("@CorreoElectronico", SqlDbType.NVarChar, 100).Value = objCatalogodeApoderados.CorreoElectronico;
                    cmd.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value = objCatalogodeApoderados.Telefono;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CATALOGODEAPODERADOS");
            }
            return id;
        }

        public void NadieEsDefaultAsync(int IdCliente)
        {
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_APODERADOS_TOGGLE_DEFAULT", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDCLIENTE", SqlDbType.Int, 4).Value = IdCliente;

                    cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public void ValidaObjeto(CatalogodeApoderados objCatalogodeApoderados, bool Nuevo)
        {
            try
            {

                ValidarRFC objValidarRFC = new();
                objValidarRFC.ValidaRFC(objCatalogodeApoderados.Rfc);


                if (objCatalogodeApoderados.Curp == null)
                {
                    objCatalogodeApoderados.Curp = "";
                }
                else
                {
                    ValidarCurp objValidarCurp = new();
                    objValidarCurp.ValidateCurp(objCatalogodeApoderados.Curp);
                }

                if (objCatalogodeApoderados.Activo == false && objCatalogodeApoderados.ADefault == true)
                {
                    throw new Exception($"Estimado usuario, no puede se por default un usuario inactivo");
                }

                if (objCatalogodeApoderados.ADefault == true)
                {
                    NadieEsDefaultAsync(objCatalogodeApoderados.IDCliente);
                }
                else
                {
                    ValidaEstadoForma(objCatalogodeApoderados, Nuevo);
                }



                if (objCatalogodeApoderados.EscrituraPublica == null)
                {
                    objCatalogodeApoderados.EscrituraPublica = "";
                }

                if (objCatalogodeApoderados.DomicilioFiscal == null)
                {
                    objCatalogodeApoderados.DomicilioFiscal = "";
                }

                if (objCatalogodeApoderados.CorreoElectronico == null)
                {
                    objCatalogodeApoderados.CorreoElectronico = "";
                }
                else
                {
                    ValidarEmail objValidarEmail = new();
                    if (!objValidarEmail.IsValidEmailFormat(objCatalogodeApoderados.CorreoElectronico))
                    {
                        throw new Exception($"La dirección de Correo Electrónico proporcionada es inválida.");
                    }
                }

                if (objCatalogodeApoderados.Telefono == null)
                {
                    objCatalogodeApoderados.Telefono = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public void ValidaEstadoForma(CatalogodeApoderados objCatalogodeApoderados, bool Nuevo)
        {
            List<Default> listDefault = new();
            int cuantosDefault = 0;
            int cuantosNoDefault = 0;
            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODEAPODERADOS_VALIDAESTADOFORMA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int, 4).Value = objCatalogodeApoderados.IDCliente;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Default objDefault = new()
                            {
                                ADefault = Convert.ToBoolean(dr["ADefault"]),
                                CantidadRegistros = Convert.ToInt32(dr["CantidadRegistros"].ToString())

                            };
                            listDefault.Add(objDefault);
                        }
                    }
                }

                if (listDefault.Count != 0 || listDefault == null)
                {
                    foreach (Default item in listDefault)
                    {
                        if (item.ADefault == true)
                        {
                            cuantosDefault = item.CantidadRegistros;
                        }

                        if (item.ADefault == false)
                        {
                            cuantosNoDefault = item.CantidadRegistros;
                        }
                    }
                }

                if (Nuevo == true)
                {
                    if (cuantosDefault == 0 && objCatalogodeApoderados.ADefault == false)
                    {
                        throw new Exception($"Error: El cliente debe tener al menos un Apoderado marcado como Default.");
                    }
                }

                if (Nuevo == false)
                {
                    if ((cuantosDefault == 0 || cuantosDefault == 1) && objCatalogodeApoderados.ADefault == false)
                    {
                        throw new Exception($"Error: El cliente debe tener al menos un Apoderado marcado como Default.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public class Default
        {
            public bool ADefault { get; set; }
            public int CantidadRegistros { get; set; }
        }
    }
}
