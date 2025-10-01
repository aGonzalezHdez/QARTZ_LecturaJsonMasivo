using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeUsuariosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogoDeUsuariosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CatalogoDeUsuarios BuscarPorId(int IDUsuario)
        {
            CatalogoDeUsuarios objCatalogoDeUsuarios = new CatalogoDeUsuarios();

            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CATALOGODEUSUARIOS_POR_ID", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IDUsuario;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
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


                            CatalogoDeOficinas objOficina = new CatalogoDeOficinas();
                            CatalogoDeOficinasRepository objOficinaRepository = new CatalogoDeOficinasRepository(_configuration);
                            objOficina = objOficinaRepository.Buscar(objCatalogoDeUsuarios.IdOficina);

                            objCatalogoDeUsuarios.Oficina = objOficina;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objCatalogoDeUsuarios;
        }
        public CatalogoDeUsuariosCorto BuscarPorIdCorto(int IDUsuario)
        {
            CatalogoDeUsuariosCorto objCatalogoDeUsuarios = new();

            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CATALOGODEUSUARIOS_POR_ID", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = IDUsuario;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCatalogoDeUsuarios.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            objCatalogoDeUsuarios.Nombre = string.Format("{0}", dr["Nombre"]);

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return objCatalogoDeUsuarios;
        }

        public List<CatalogoDeUsuarios> CargarDepartamento(int lIdDepartamento, int lidOficina)
        {
            List<CatalogoDeUsuarios> lstCatalogoDeUsuarios = new List<CatalogoDeUsuarios>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            try
            {
                //cn.ConnectionString = myConnectionString;
                cn.Open();

                cmd.CommandText = "NET_LOAD_CATALOGODEUSUARIOS_DEPARTAMENTO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @@IdDepartamento int 

                param = cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4);
                param.Value = lIdDepartamento;


                param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
                param.Value = lidOficina;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        CatalogoDeUsuarios objCatalogoDeUsuarios = new();


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


                        lstCatalogoDeUsuarios.Add(objCatalogoDeUsuarios);
                    }
                }
                else
                    lstCatalogoDeUsuarios = null;
                dr.Close();
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstCatalogoDeUsuarios;
        }
        public object InsertGuia2d(string GuiaHouse, string Texto)
        {


            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "Pocket.NET_INSERT_GUIA2D";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {

                @param = cmd.Parameters.Add("@Texto", SqlDbType.Text);
                @param.Value = Texto;

                @param = cmd.Parameters.Add("@Guia", SqlDbType.VarChar, 13);
                @param.Value = GuiaHouse;

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
            }

            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "Pocket.NET_INSERT_GUIA2D");
            }
            cn.Close();
            cn.Dispose();
            return default;

        }

        public List<DropDownListDatos> UsuariosparaProyecto(int idDepartamento)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                SqlParameter @param;
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_USUARIOS_PROYECTO", con))
                {

                    @param = cmd.Parameters.Add("@idDepartamento", SqlDbType.Int, 4);
                    @param.Value = idDepartamento;


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

        public List<DropDownListDatos> UsuariosparaTareas(int idDepartamento)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                SqlParameter @param;
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_USUARIOS_DESARROLLADORES", con))
                {

                    @param = cmd.Parameters.Add("@idDepartamento", SqlDbType.Int, 4);
                    @param.Value = idDepartamento;


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
        public List<DropDownListDatos> UsuariosPorEmpresa(int idDatosDeEmpresa)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                SqlParameter @param;
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_USUARIOS_EMPRESA", con))
                {

                    @param = cmd.Parameters.Add("@idDatosDeEmpresa", SqlDbType.Int, 4);
                    @param.Value = idDatosDeEmpresa;

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

        public List<CatalogoDeUsuarios> CargarDisponiblesPorOperacion(int idDepartamento, int idOficina, int idOperacion)
        {

            List<CatalogoDeUsuarios> lstCatalogoDeUsuarios = new List<CatalogoDeUsuarios>();
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;

            try
            {
                cn.Open();
                cmd.CommandText = "NET_LOAD_CATALOGODEUSUARIOS_DISPONIBLES_POR_OPERACION";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                param = cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4);
                param.Value = idDepartamento;
                param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
                param.Value = idOficina;
                param = cmd.Parameters.Add("@IdOperacion", SqlDbType.Int, 4);
                param.Value = idOperacion;
                dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        CatalogoDeUsuarios objCatalogoDeUsuarios = new();
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
                        lstCatalogoDeUsuarios.Add(objCatalogoDeUsuarios);
                    }
                }
                else
                {
                    lstCatalogoDeUsuarios = null;
                    dr.Close();
                    cn.Close();
                    // SqlConnection.ClearPool(cn)
                    cn.Dispose();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                    cn.Dispose();
                }
            }
            return lstCatalogoDeUsuarios;
        }
    }
}
