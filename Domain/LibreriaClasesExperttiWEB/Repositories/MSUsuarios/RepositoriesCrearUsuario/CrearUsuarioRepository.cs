using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;
using LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.RepositoriesCrearUsuario
{
    public class CrearUsuarioRepository : ICrearUsuarioRepository
    {
        public string SConexion { get; set; }
        string ICrearUsuarioRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CrearUsuarioRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public void CrearUsuario(CrearUsuario objCrearUsuario)
        {
            
            try
            {

                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("Perfiles.NET_CREATE_USUARIO", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idSolicitud", SqlDbType.Int, 4).Value = objCrearUsuario.IdSolicitud;
                    cmd.Parameters.Add("@idUsuarioAlta", SqlDbType.Int, 4).Value = objCrearUsuario.IdUsuarioAlta;


                    cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }           
        }

        public List<CatalogoDeUsuarios> ValidaSiExisteUsuario(ValidarSiExiste objValidarSiExiste)
        {
            List<CatalogoDeUsuarios> lstCatalogoDeUsuarios = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("usuario.NET_SELECT_CASAEI_CATALOGODEUSUARIOS_VALIDA_EXISTA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Value = objValidarSiExiste.Nombre;
                    cmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 30).Value = objValidarSiExiste.Usuario;
                    cmd.Parameters.Add("@NumeroDeEmpleado", SqlDbType.BigInt).Value = objValidarSiExiste.NumeroDeEmpleado;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 60).Value = objValidarSiExiste.Email;
                    cmd.Parameters.Add("@CURP", SqlDbType.VarChar, 20).Value = objValidarSiExiste.CURP;

                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CatalogoDeUsuarios objCatalogoDeUsuarios = new();

                            objCatalogoDeUsuarios.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objCatalogoDeUsuarios.Nombre = string.Format("{0}", dr["Nombre"]);
                            objCatalogoDeUsuarios.Usuario = string.Format("{0}", dr["Usuario"]);
                            objCatalogoDeUsuarios.Psw = string.Format("{0}", dr["Psw"]);
                            objCatalogoDeUsuarios.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                            objCatalogoDeUsuarios.Email = string.Format("{0}", dr["Email"]);
                            objCatalogoDeUsuarios.Autorizaegreso = Convert.ToBoolean(dr["Autorizaegreso"]);
                            objCatalogoDeUsuarios.MontodeAutorizacion = Convert.ToDouble(dr["MontodeAutorizacion"]);
                            objCatalogoDeUsuarios.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);
                            objCatalogoDeUsuarios.IdModulo = Convert.ToInt32(dr["IdModulo"]);
                            objCatalogoDeUsuarios.UsuarioActivo = Convert.ToBoolean(dr["UsuarioActivo"]);
                            objCatalogoDeUsuarios.CapturoUsuario = Convert.ToInt32(dr["CapturoUsuario"]);
                            objCatalogoDeUsuarios.FechaDeCaptura = Convert.ToDateTime(dr["FechaDeCaptura"]);
                            objCatalogoDeUsuarios.EsUnCliente = Convert.ToBoolean(dr["EsUnCliente"]);
                            objCatalogoDeUsuarios.UsuarioCASA = string.Format("{0}", dr["UsuarioCASA"]);
                            objCatalogoDeUsuarios.SolicitarCambioDePassword = Convert.ToBoolean(dr["SolicitarCambioDePassword"]);
                            objCatalogoDeUsuarios.MostrarTodosLosEventos = Convert.ToBoolean(dr["MostrarTodosLosEventos"]);
                            objCatalogoDeUsuarios.Iniciales = string.Format("{0}", dr["Iniciales"]);
                            objCatalogoDeUsuarios.CURP = string.Format("{0}", dr["CURP"]);
                            objCatalogoDeUsuarios.IDGenero = Convert.ToInt32(dr["IDGenero"]);
                            objCatalogoDeUsuarios.IDPuesto = Convert.ToInt32(dr["IDPuesto"]);
                            objCatalogoDeUsuarios.NumeroDeEmpleado = Convert.ToInt32(dr["NumeroDeEmpleado"]);
                            objCatalogoDeUsuarios.PoolPendientes = Convert.ToBoolean(dr["PoolPendientes"]);
                            objCatalogoDeUsuarios.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                            objCatalogoDeUsuarios.ExtensionTel = string.Format("{0}", dr["ExtensionTel"]);
                            objCatalogoDeUsuarios.IdEstacionDefault = Convert.ToInt32(dr["IdEstacionDefault"]);
                            objCatalogoDeUsuarios.IngresoReal = Convert.ToDateTime(dr["IngresoReal"]);
                            objCatalogoDeUsuarios.IngresoANomina = Convert.ToDateTime(dr["IngresoANomina"]);
                            objCatalogoDeUsuarios.Baja = Convert.ToDateTime(dr["Baja"]);
                            objCatalogoDeUsuarios.Operacion = Convert.ToInt32(dr["Operacion"]);
                            objCatalogoDeUsuarios.TomarTelPart = Convert.ToBoolean(dr["TomarTelPart"]);
                            objCatalogoDeUsuarios.TelParticular = string.Format("{0}", dr["TelParticular"]);
                            objCatalogoDeUsuarios.UsuarioBaja = Convert.ToInt32(dr["UsuarioBaja"]);
                            objCatalogoDeUsuarios.FechaUsuarioBaja = Convert.ToDateTime(dr["FechaUsuarioBaja"]);
                            objCatalogoDeUsuarios.MotivoBaja = string.Format("{0}", dr["MotivoBaja"]);
                            
                            lstCatalogoDeUsuarios.Add(objCatalogoDeUsuarios);
                        }                    
                    }
                    else
                    {
                    lstCatalogoDeUsuarios = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return lstCatalogoDeUsuarios;
        }
    }
}
