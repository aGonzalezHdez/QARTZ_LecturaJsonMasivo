using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogodeDestinatariosRepository : ICatalogodeDestinatariosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public CatalogodeDestinatariosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbMSG")!;
        }

        public int Insertar(CatalogodeDestinatarios objCatalogodeDestinatarios)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                Validar(objCatalogodeDestinatarios);

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CATALOGODEDESTINATARIOS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 120);
                @param.Value = objCatalogodeDestinatarios.Nombre;

                @param = cmd.Parameters.Add("@RFC", SqlDbType.Char, 13);
                @param.Value = objCatalogodeDestinatarios.RFC;

                @param = cmd.Parameters.Add("@CURP", SqlDbType.VarChar, 18);
                @param.Value = objCatalogodeDestinatarios.CURP;

                @param = cmd.Parameters.Add("@NSS", SqlDbType.VarChar, 50);
                @param.Value = objCatalogodeDestinatarios.NSS;

                @param = cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 80);
                @param.Value = objCatalogodeDestinatarios.Telefono;

                @param = cmd.Parameters.Add("@CorreoElectronico", SqlDbType.VarChar, 80);
                @param.Value = objCatalogodeDestinatarios.CorreoElectronico;

                @param = cmd.Parameters.Add("@Contacto", SqlDbType.VarChar, 80);
                @param.Value = objCatalogodeDestinatarios.Contacto;


                @param = cmd.Parameters.Add("@IDCapturo", SqlDbType.SmallInt);
                @param.Value = objCatalogodeDestinatarios.IDCapturo;


                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CATALOGODEDESTINATARIOS");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public int Modificar(CatalogodeDestinatarios objCatalogodeDestinatarios)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                if (objCatalogodeDestinatarios.IdDestinatario == 0)
                {
                    throw new Exception("para modificar es necesario seleccionar un destinatario");
                }
                Validar(objCatalogodeDestinatarios);

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_UPDATE_CATALOGODEDESTINATARIOS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdDestinatario", SqlDbType.SmallInt);
                @param.Value = objCatalogodeDestinatarios.IdDestinatario;


                @param = cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 120);
                @param.Value = objCatalogodeDestinatarios.Nombre;

                @param = cmd.Parameters.Add("@RFC", SqlDbType.Char, 13);
                @param.Value = objCatalogodeDestinatarios.RFC;

                @param = cmd.Parameters.Add("@CURP", SqlDbType.VarChar, 18);
                @param.Value = objCatalogodeDestinatarios.CURP;

                @param = cmd.Parameters.Add("@NSS", SqlDbType.VarChar, 50);
                @param.Value = objCatalogodeDestinatarios.NSS;

                @param = cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 80);
                @param.Value = objCatalogodeDestinatarios.Telefono;

                @param = cmd.Parameters.Add("@CorreoElectronico", SqlDbType.VarChar, 80);
                @param.Value = objCatalogodeDestinatarios.CorreoElectronico;

                @param = cmd.Parameters.Add("@Contacto", SqlDbType.VarChar, 80);
                @param.Value = objCatalogodeDestinatarios.Contacto;

                @param = cmd.Parameters.Add("@Activo", SqlDbType.Bit);
                @param.Value = objCatalogodeDestinatarios.Activo;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;
                }
                else
                {
                    id = 0;
                }

                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                id = 0;
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CATALOGODEDESTINATARIOS");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public CatalogodeDestinatarios Buscar(int IdDestinatario)
        {
            CatalogodeDestinatarios objCatalogodeDestinatarios = new();

            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODEDESTINATARIOS", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDestinatario", SqlDbType.Int, 4).Value = IdDestinatario;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objCatalogodeDestinatarios.IdDestinatario = Convert.ToInt32(dr["IdDestinatario"]);
                        objCatalogodeDestinatarios.Nombre = dr["Nombre"].ToString();
                        objCatalogodeDestinatarios.RFC = dr["RFC"].ToString();
                        objCatalogodeDestinatarios.CURP = dr["CURP"].ToString();
                        objCatalogodeDestinatarios.NSS = dr["NSS"].ToString();
                        objCatalogodeDestinatarios.Telefono = dr["Telefono"].ToString();
                        objCatalogodeDestinatarios.CorreoElectronico = dr["CorreoElectronico"].ToString();
                        objCatalogodeDestinatarios.Contacto = dr["Contacto"].ToString();
                        objCatalogodeDestinatarios.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                        objCatalogodeDestinatarios.IDCapturo = Convert.ToInt32(dr["IDCapturo"]);
                        objCatalogodeDestinatarios.Activo = Convert.ToBoolean(dr["Activo"]);

                    }
                    else
                    {
                        objCatalogodeDestinatarios = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCatalogodeDestinatarios;
        }


        public bool Validar(CatalogodeDestinatarios objCatalogodeDestinatarios)
        {
            bool valido = false;
            try
            {
                if (objCatalogodeDestinatarios.Nombre == "")
                {
                    throw new Exception("El nombre es obligatorio, favor de ingresarlo");
                }

                if (objCatalogodeDestinatarios.RFC == "")
                {
                    throw new Exception("El RFC es obligatorio, favor de ingresarlo");
                }

                if (objCatalogodeDestinatarios.IDCapturo == 0)
                {
                    throw new Exception("El usuario de captura es obligatorio");
                }

                if (objCatalogodeDestinatarios.CorreoElectronico == "")
                {
                    throw new Exception("El correo electronico es obligatorio, favor de ingresarlo");
                }

                if (objCatalogodeDestinatarios.Telefono == "")
                {
                    throw new Exception("El telefono es obligatorio, favor de ingresarlo");
                }
                valido = true;
            }
            catch (Exception ex)
            {
                valido = false;
                throw new Exception(ex.Message.ToString());
            }

            return valido;
        }
    }
}
