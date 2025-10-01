using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesToken;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.RepositoriesUsuarios
{
    public class UsuarioRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public UsuarioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = configuration.GetConnectionString("dbCASAEI")!;
        }

        public  CatalogoDeUsuarios Buscar(string Username)
        {
            CatalogoDeUsuarios catalogoDeUsuarios = new();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_VALIDA_USUARIO", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 30).Value = Username;
                    using (SqlDataReader reader =  cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            catalogoDeUsuarios.IdUsuario = reader.GetInt32("IdUsuario");
                            catalogoDeUsuarios.Usuario = reader.GetString("Usuario");
                            catalogoDeUsuarios.Psw = reader.GetString("Psw");
                            catalogoDeUsuarios.Email = reader.GetString("Email");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return catalogoDeUsuarios;
        }

        public CatalogoDeUsuarios Buscar(int IdUsuario)
        {
            CatalogoDeUsuarios objCatalogoDeUsuarios = new CatalogoDeUsuarios();
            using (SqlConnection cn = new SqlConnection(sConexion))
            using (SqlCommand cmd = new SqlCommand("NET_VALIDA_USUARIO_XID", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id_Usuario", SqlDbType.Int).Value = IdUsuario;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCatalogoDeUsuarios.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objCatalogoDeUsuarios.Nombre = dr["Nombre"].ToString();
                            objCatalogoDeUsuarios.Usuario = dr["Usuario"].ToString();
                            objCatalogoDeUsuarios.Psw = dr["Psw"].ToString();
                            objCatalogoDeUsuarios.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                            objCatalogoDeUsuarios.Email = dr["Email"].ToString();
                            objCatalogoDeUsuarios.Autorizaegreso = Convert.ToBoolean(dr["Autorizaegreso"]);
                            objCatalogoDeUsuarios.MontodeAutorizacion = Convert.ToDouble(dr["MontodeAutorizacion"]);
                            objCatalogoDeUsuarios.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);
                            objCatalogoDeUsuarios.IdModulo = Convert.ToInt32(dr["IdModulo"]);
                            objCatalogoDeUsuarios.UsuarioActivo = Convert.ToBoolean(dr["UsuarioActivo"]);
                            objCatalogoDeUsuarios.CapturoUsuario = Convert.ToInt32(dr["CapturoUsuario"]);
                            objCatalogoDeUsuarios.FechaDeCaptura = Convert.ToDateTime(dr["FechaDeCaptura"]);
                            objCatalogoDeUsuarios.EsUnCliente = Convert.ToBoolean(dr["EsUnCliente"]);
                            objCatalogoDeUsuarios.UsuarioCASA = dr["UsuarioCASA"].ToString();
                            objCatalogoDeUsuarios.SolicitarCambioDePassword = Convert.ToBoolean(dr["SolicitarCambioDePassword"]);
                            objCatalogoDeUsuarios.MostrarTodosLosEventos = Convert.ToBoolean(dr["MostrarTodosLosEventos"]);
                            objCatalogoDeUsuarios.Iniciales = dr["Iniciales"].ToString();
                            objCatalogoDeUsuarios.CURP = dr["CURP"].ToString();
                            objCatalogoDeUsuarios.IDGenero = Convert.ToInt32(dr["IDGenero"]);
                            objCatalogoDeUsuarios.IDPuesto = Convert.ToInt32(dr["IDPuesto"]);
                            objCatalogoDeUsuarios.NumeroDeEmpleado = Convert.ToInt32(dr["NumeroDeEmpleado"]);
                            objCatalogoDeUsuarios.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                            objCatalogoDeUsuarios.ExtensionTel = dr["ExtensionTel"].ToString();
                            objCatalogoDeUsuarios.IdEstacionDefault = Convert.ToInt32(dr["IdEstacionDefault"]);
                            objCatalogoDeUsuarios.IngresoReal = Convert.ToDateTime(dr["IngresoReal"]);
                            objCatalogoDeUsuarios.IngresoANomina = Convert.ToDateTime(dr["IngresoANomina"]);
                            objCatalogoDeUsuarios.TomarTelPart = Convert.ToBoolean(dr["TomarTelPart"]);
                            objCatalogoDeUsuarios.TelParticular = dr["TelParticular"].ToString();
                            objCatalogoDeUsuarios.Baja = Convert.ToDateTime(dr["Baja"]);
                            objCatalogoDeUsuarios.UsuarioBaja = Convert.ToInt32(dr["UsuarioBaja"]);
                            objCatalogoDeUsuarios.FechaUsuarioBaja = Convert.ToDateTime(dr["FechaUsuarioBaja"]);
                            objCatalogoDeUsuarios.MotivoBaja = dr["MotivoBaja"].ToString();




                        }
                        else
                        {
                            objCatalogoDeUsuarios = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " - Error en Buscar Usuario");
                }
            }

            return objCatalogoDeUsuarios;
        }


        public UsuarioPerfil ValidaUsuario(UsuarioLogin request)
        {
            CatalogoDeUsuarios CUsuario = new();
            UsuarioPerfil userDb = new();
            UsuarioRepository UsuarioD = new(_configuration);

            try
            {
                CUsuario =  UsuarioD.Buscar(request.Username);
                if (request.PasssEncryp != CUsuario.Psw)
                {
                    throw new Exception("La contraseña es incorrecta");
                }

                userDb.IdUsuario = CUsuario.IdUsuario;
                userDb.Username = CUsuario.Usuario;
                userDb.Password = CUsuario.Psw;
                userDb.EmailAddress = CUsuario.Email;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return userDb;
        }

        public List<DropDownListDatos> CargaridDepaartamento(int idDepartamento)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_USUARIOS_DESARROLLO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
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
