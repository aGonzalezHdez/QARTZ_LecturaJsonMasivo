using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.RepositoriesSolicitudUsuarios
{
    public class SolicitudUsuariosRepository: ISolicitudUsuariosRepository
    {
        public string SConexion { get; set; }
        string ISolicitudUsuariosRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public SolicitudUsuariosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public string Insertar(SolicitudUsuarios objSolicitudUsuarios)
        {
            string? Folio = "";         

            var resultadoValidacion = ValidarSolicitud(objSolicitudUsuarios);

            if (string.IsNullOrEmpty(resultadoValidacion))
            { 
                try
                {
                    using (SqlConnection con = new(SConexion))
                    using (SqlCommand cmd = new("usuario.NET_INSERT_CASAEI_SolicitudUsuarios", con))
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@FechaSolicitud", SqlDbType.DateTime, 4).Value = objSolicitudUsuarios.FechaSolicitud;
                        //cmd.Parameters.Add("@idEstatus", SqlDbType.Int, 4).Value = objSolicitudUsuarios.idEstatus;
                        cmd.Parameters.Add("@idUsuarioSolicita", SqlDbType.Int, 4).Value = objSolicitudUsuarios.idUsuarioSolicita;
                        cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IDDatosDeEmpresa;
                        cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IdOficina;
                        cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IdDepartamento;
                        cmd.Parameters.Add("@IDPuesto", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IDPuesto;
                        cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objSolicitudUsuarios.Operacion;
                        cmd.Parameters.Add("@NSS", SqlDbType.VarChar, 20).Value = objSolicitudUsuarios.NSS;
                        cmd.Parameters.Add("@NumeroDeEmpleado", SqlDbType.Int, 4).Value = objSolicitudUsuarios.NumeroDeEmpleado;
                        cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Value = objSolicitudUsuarios.Nombre;
                        cmd.Parameters.Add("@IDGenero", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IDGenero;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar, 60).Value = objSolicitudUsuarios.Email;
                        cmd.Parameters.Add("@Iniciales", SqlDbType.VarChar, 10).Value = objSolicitudUsuarios.Iniciales;
                        cmd.Parameters.Add("@CURP", SqlDbType.VarChar, 20).Value = objSolicitudUsuarios.CURP;
                        cmd.Parameters.Add("@MontodeAutorizacion", SqlDbType.Decimal, 4).Value = objSolicitudUsuarios.MontodeAutorizacion;
                        cmd.Parameters.Add("@IngresoReal", SqlDbType.DateTime).Value = objSolicitudUsuarios.IngresoReal;
                        cmd.Parameters.Add("@IngresoANomina", SqlDbType.DateTime).Value = objSolicitudUsuarios.IngresoANomina;
                        cmd.Parameters.Add("@TelParticular", SqlDbType.VarChar, 50).Value = objSolicitudUsuarios.TelParticular;
                        cmd.Parameters.Add("@ExtensionTel", SqlDbType.VarChar, 10).Value = objSolicitudUsuarios.ExtensionTel;
                        cmd.Parameters.Add("@Movil", SqlDbType.VarChar, 10).Value = objSolicitudUsuarios.Movil;
                        cmd.Parameters.Add("@ObservaciondeSolicitud", SqlDbType.VarChar, -1).Value = objSolicitudUsuarios.ObservaciondeSolicitud;
                        cmd.Parameters.Add("@Autorizaegreso", SqlDbType.Bit, 4).Value = objSolicitudUsuarios.Autorizaegreso;
                        cmd.Parameters.Add("@EsUnCliente", SqlDbType.Bit, 4).Value = objSolicitudUsuarios.EsUnCliente;
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 4).Value = objSolicitudUsuarios.idUsuario;
                        //cmd.Parameters.Add("@FechaAceptado", SqlDbType.DateTime, 4).Value = objSolicitudUsuarios.FechaAceptado;
                        cmd.Parameters.Add("@UsuarioCASA", SqlDbType.VarChar, 10).Value = objSolicitudUsuarios.UsuarioCASA;
                        cmd.Parameters.Add("@Folio", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            Folio = Convert.ToString(cmd.Parameters["@Folio"].Value);
                        }
                    }                
                }
                catch (SqlException sqlEx)
                {                
                    throw new Exception("Database error: " + sqlEx.Message);
                }
                catch (Exception ex)
                {            
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                Folio = resultadoValidacion; 
            }

            return Folio;
        }

        public int Modificar(SolicitudUsuarios objSolicitudUsuarios)
        {
            int Id = 0;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("[usuario].[NET_UPDATE_CASAEI_SolicitudUsuarios]", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdSolicitud", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IdSolicitud;
                    cmd.Parameters.Add("@Folio", SqlDbType.VarChar, 10).Value = objSolicitudUsuarios.Folio;
                    //cmd.Parameters.Add("@FechaSolicitud", SqlDbType.DateTime, 4).Value = objSolicitudUsuarios.FechaSolicitud;
                    cmd.Parameters.Add("@idEstatus", SqlDbType.Int, 4).Value = objSolicitudUsuarios.idEstatus;          
                    cmd.Parameters.Add("@idUsuarioSolicita", SqlDbType.Int, 4).Value = objSolicitudUsuarios.idUsuarioSolicita;
                    cmd.Parameters.Add("@IDDatosDeEmpresa", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IDDatosDeEmpresa;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IdOficina;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IdDepartamento;
                    cmd.Parameters.Add("@IDPuesto", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IDPuesto;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = objSolicitudUsuarios.Operacion;
                    cmd.Parameters.Add("@NSS", SqlDbType.VarChar, 20).Value = objSolicitudUsuarios.NSS;
                    cmd.Parameters.Add("@NumeroDeEmpleado", SqlDbType.Int, 4).Value = objSolicitudUsuarios.NumeroDeEmpleado;
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 100).Value = objSolicitudUsuarios.Nombre;
                    cmd.Parameters.Add("@IDGenero", SqlDbType.Int, 4).Value = objSolicitudUsuarios.IDGenero;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 60).Value = objSolicitudUsuarios.Email;
                    cmd.Parameters.Add("@Iniciales", SqlDbType.VarChar, 10).Value = objSolicitudUsuarios.Iniciales;
                    cmd.Parameters.Add("@CURP", SqlDbType.VarChar, 20).Value = objSolicitudUsuarios.CURP;
                    cmd.Parameters.Add("@MontodeAutorizacion", SqlDbType.Decimal, 4).Value = objSolicitudUsuarios.MontodeAutorizacion;
                    cmd.Parameters.Add("@IngresoReal", SqlDbType.DateTime).Value = objSolicitudUsuarios.IngresoReal;
                    cmd.Parameters.Add("@IngresoANomina", SqlDbType.DateTime).Value = objSolicitudUsuarios.IngresoANomina;
                    cmd.Parameters.Add("@TelParticular", SqlDbType.VarChar, 50).Value = objSolicitudUsuarios.TelParticular;
                    cmd.Parameters.Add("@ExtensionTel", SqlDbType.VarChar, 10).Value = objSolicitudUsuarios.ExtensionTel;
                    cmd.Parameters.Add("@Movil", SqlDbType.VarChar, 10).Value = objSolicitudUsuarios.Movil;
                    cmd.Parameters.Add("@ObservaciondeSolicitud", SqlDbType.VarChar, -1).Value = objSolicitudUsuarios.ObservaciondeSolicitud;
                    cmd.Parameters.Add("@Autorizaegreso", SqlDbType.Bit, 4).Value = objSolicitudUsuarios.Autorizaegreso;
                    cmd.Parameters.Add("@EsUnCliente", SqlDbType.Bit, 4).Value = objSolicitudUsuarios.EsUnCliente;
                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 4).Value = objSolicitudUsuarios.idUsuario;
                    cmd.Parameters.Add("@FechaAceptado", SqlDbType.DateTime, 4).Value = objSolicitudUsuarios.FechaAceptado;
                    cmd.Parameters.Add("@UsuarioCASA", SqlDbType.VarChar, 10).Value = objSolicitudUsuarios.UsuarioCASA;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            Id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                    }
                }
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

        public SolicitudUsuarios Buscar(string Folio)
        {
            SolicitudUsuarios objSolicitudUsuarios = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("[usuario].[NET_SEARCH_CASAEI_SolicitudUsuarios]", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Folio", SqlDbType.VarChar, 10).Value = Folio;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objSolicitudUsuarios.IdSolicitud = Convert.ToInt32(dr["IdSolicitud"]);
                        objSolicitudUsuarios.Folio = dr["Folio"].ToString();
                        objSolicitudUsuarios.FechaSolicitud = Convert.ToDateTime(dr["FechaSolicitud"]);
                        objSolicitudUsuarios.idEstatus = Convert.ToInt32(dr["idEstatus"]);
                        objSolicitudUsuarios.idUsuarioSolicita = Convert.ToInt32(dr["idUsuarioSolicita"]);
                        objSolicitudUsuarios.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                        objSolicitudUsuarios.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                        objSolicitudUsuarios.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);
                        objSolicitudUsuarios.IDPuesto = Convert.ToInt32(dr["IDPuesto"]);
                        objSolicitudUsuarios.Operacion = Convert.ToInt32(dr["Operacion"]);
                        objSolicitudUsuarios.NSS = dr["NSS"].ToString();
                        objSolicitudUsuarios.NumeroDeEmpleado = Convert.ToInt32(dr["NumeroDeEmpleado"]);
                        objSolicitudUsuarios.Nombre = dr["Nombre"].ToString();
                        objSolicitudUsuarios.IDGenero = Convert.ToInt32(dr["IDGenero"]);
                        objSolicitudUsuarios.Email = dr["Email"].ToString();
                        objSolicitudUsuarios.Iniciales = dr["Iniciales"].ToString();
                        objSolicitudUsuarios.CURP = dr["CURP"].ToString();
                        objSolicitudUsuarios.MontodeAutorizacion = Convert.ToDouble(dr["MontodeAutorizacion"]);
                        objSolicitudUsuarios.IngresoReal = DBNull.Value.Equals(dr["IngresoReal"]) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(dr["IngresoReal"]); 
                        objSolicitudUsuarios.IngresoANomina = DBNull.Value.Equals(dr["IngresoANomina"]) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(dr["IngresoANomina"]);
                        objSolicitudUsuarios.TelParticular = dr["TelParticular"].ToString();
                        objSolicitudUsuarios.ExtensionTel = dr["ExtensionTel"].ToString();
                        objSolicitudUsuarios.Movil = dr["Movil"].ToString();
                        objSolicitudUsuarios.ObservaciondeSolicitud = dr["ObservaciondeSolicitud"].ToString();
                        objSolicitudUsuarios.Autorizaegreso = Convert.ToBoolean(dr["Autorizaegreso"]);
                        objSolicitudUsuarios.EsUnCliente = Convert.ToBoolean(dr["EsUnCliente"]);
                        objSolicitudUsuarios.idUsuario = Convert.ToInt32(dr["idUsuario"]);
                        objSolicitudUsuarios.FechaAceptado = DBNull.Value.Equals(dr["FechaAceptado"]) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(dr["FechaAceptado"]); 
                        objSolicitudUsuarios.UsuarioCASA = dr["UsuarioCASA"].ToString(); 
                    }
                    else
                    {
                        objSolicitudUsuarios = null!;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objSolicitudUsuarios;
        }

        public List<SolicitudUsuarios> BuscarEstatus(int idEstatus)
        {
            List<SolicitudUsuarios> listSolicitudUsuarios = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("Usuario.NET_BUSCAR_CASAEI_SOLICITUDUSUARIOS_ESTATUS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idEstatus", SqlDbType.Int, 4).Value = idEstatus;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SolicitudUsuarios objSolicitudUsuarios = new();
                            objSolicitudUsuarios.IdSolicitud = Convert.ToInt32(dr["IdSolicitud"]);
                            objSolicitudUsuarios.Folio = dr["Folio"].ToString();
                            objSolicitudUsuarios.FechaSolicitud = Convert.ToDateTime(dr["FechaSolicitud"]);
                            objSolicitudUsuarios.idEstatus = Convert.ToInt32(dr["idEstatus"]);
                            objSolicitudUsuarios.idUsuarioSolicita = Convert.ToInt32(dr["idUsuarioSolicita"]);
                            objSolicitudUsuarios.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                            objSolicitudUsuarios.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                            objSolicitudUsuarios.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);
                            objSolicitudUsuarios.IDPuesto = Convert.ToInt32(dr["IDPuesto"]);
                            objSolicitudUsuarios.Operacion = Convert.ToInt32(dr["Operacion"]);
                            objSolicitudUsuarios.NSS = dr["NSS"].ToString();
                            objSolicitudUsuarios.NumeroDeEmpleado = Convert.ToInt32(dr["NumeroDeEmpleado"]);
                            objSolicitudUsuarios.Nombre = dr["Nombre"].ToString();
                            objSolicitudUsuarios.IDGenero = Convert.ToInt32(dr["IDGenero"]);
                            objSolicitudUsuarios.Email = dr["Email"].ToString();
                            objSolicitudUsuarios.Iniciales = dr["Iniciales"].ToString();
                            objSolicitudUsuarios.CURP = dr["CURP"].ToString();
                            objSolicitudUsuarios.MontodeAutorizacion = Convert.ToDouble(dr["MontodeAutorizacion"]);
                            objSolicitudUsuarios.IngresoReal = DBNull.Value.Equals(dr["IngresoReal"]) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(dr["IngresoReal"]); 
                            objSolicitudUsuarios.IngresoANomina = DBNull.Value.Equals(dr["IngresoANomina"]) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(dr["IngresoANomina"]); 
                            objSolicitudUsuarios.TelParticular = dr["TelParticular"].ToString();
                            objSolicitudUsuarios.ExtensionTel = dr["ExtensionTel"].ToString();
                            objSolicitudUsuarios.Movil = dr["Movil"].ToString();
                            objSolicitudUsuarios.ObservaciondeSolicitud = dr["ObservaciondeSolicitud"].ToString();
                            objSolicitudUsuarios.Autorizaegreso = Convert.ToBoolean(dr["Autorizaegreso"]);
                            objSolicitudUsuarios.EsUnCliente = Convert.ToBoolean(dr["EsUnCliente"]);
                            objSolicitudUsuarios.idUsuario = Convert.ToInt32(dr["idUsuario"]);
                            objSolicitudUsuarios.FechaAceptado = DBNull.Value.Equals(dr["FechaAceptado"]) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(dr["FechaAceptado"]);  
                            objSolicitudUsuarios.UsuarioCASA = dr["UsuarioCASA"].ToString();

                            listSolicitudUsuarios.Add(objSolicitudUsuarios);
                        }
                    }
                    else
                    {
                        listSolicitudUsuarios = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return listSolicitudUsuarios;
        }

        public List<SolicitudUsuarios> PendientesporUsuario(int idUsuarioSolicita)
        {
            List<SolicitudUsuarios> listSolicitudUsuarios = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("usuario.NET_BUSCAR_CASAEI_SOLICITUDUSUARIOS_PENDIENTESPORUSUARIO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idUsuarioSolicita", SqlDbType.Int, 4).Value = idUsuarioSolicita;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SolicitudUsuarios objSolicitudUsuarios = new();
                            objSolicitudUsuarios.IdSolicitud = Convert.ToInt32(dr["IdSolicitud"]);
                            objSolicitudUsuarios.Folio = dr["Folio"].ToString();
                            objSolicitudUsuarios.FechaSolicitud = Convert.ToDateTime(dr["FechaSolicitud"]);
                            objSolicitudUsuarios.idEstatus = Convert.ToInt32(dr["idEstatus"]);
                            objSolicitudUsuarios.idUsuarioSolicita = Convert.ToInt32(dr["idUsuarioSolicita"]);
                            objSolicitudUsuarios.Solicita = dr["Solicita"].ToString();
                            objSolicitudUsuarios.IDDatosDeEmpresa = Convert.ToInt32(dr["IDDatosDeEmpresa"]);
                            objSolicitudUsuarios.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                            objSolicitudUsuarios.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);
                            objSolicitudUsuarios.IDPuesto = Convert.ToInt32(dr["IDPuesto"]);
                            objSolicitudUsuarios.Operacion = Convert.ToInt32(dr["Operacion"]);
                            objSolicitudUsuarios.NSS = dr["NSS"].ToString();
                            objSolicitudUsuarios.NumeroDeEmpleado = Convert.ToInt32(dr["NumeroDeEmpleado"]);
                            objSolicitudUsuarios.Nombre = dr["Nombre"].ToString();
                            objSolicitudUsuarios.IDGenero = Convert.ToInt32(dr["IDGenero"]);
                            objSolicitudUsuarios.Email = dr["Email"].ToString();
                            objSolicitudUsuarios.Iniciales = dr["Iniciales"].ToString();
                            objSolicitudUsuarios.CURP = dr["CURP"].ToString();
                            objSolicitudUsuarios.MontodeAutorizacion = Convert.ToDouble(dr["MontodeAutorizacion"]);
                            objSolicitudUsuarios.IngresoReal = DBNull.Value.Equals(dr["IngresoReal"]) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(dr["IngresoReal"]); 
                            objSolicitudUsuarios.IngresoANomina = DBNull.Value.Equals(dr["IngresoANomina"]) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(dr["IngresoANomina"]);  
                            objSolicitudUsuarios.TelParticular = dr["TelParticular"].ToString();
                            objSolicitudUsuarios.ExtensionTel = dr["ExtensionTel"].ToString();
                            objSolicitudUsuarios.Movil = dr["Movil"].ToString();
                            objSolicitudUsuarios.ObservaciondeSolicitud = dr["ObservaciondeSolicitud"].ToString();
                            objSolicitudUsuarios.Autorizaegreso = Convert.ToBoolean(dr["Autorizaegreso"]);
                            objSolicitudUsuarios.EsUnCliente = Convert.ToBoolean(dr["EsUnCliente"]);
                            objSolicitudUsuarios.idUsuario = Convert.ToInt32(dr["idUsuario"]);
                            objSolicitudUsuarios.FechaAceptado = DBNull.Value.Equals(dr["FechaAceptado"]) ? Convert.ToDateTime("01/01/1900") : Convert.ToDateTime(dr["FechaAceptado"]);
                         
                            objSolicitudUsuarios.UsuarioCASA = dr["UsuarioCASA"].ToString();

                            listSolicitudUsuarios.Add(objSolicitudUsuarios);
                        }
                    }
                    else
                    {
                        listSolicitudUsuarios = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listSolicitudUsuarios;
        }

        public string CrearUsuario(int IdSolicitud, int IdUsuarioAlta)
        {
            string Folio = "";
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_CASAEI_SolicitudUsuarios", con))
                {
                    con.Open();
                  
                    cmd.Parameters.Add("@FechaSolicitud", SqlDbType.DateTime, 4).Value = IdSolicitud;
                    cmd.Parameters.Add("@idEstatus", SqlDbType.Int, 4).Value = IdUsuarioAlta;
                   
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return Folio;
        }

        public List<DropDownListDatos> Cargar()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_LOAD_TIPODEACTIVIDADES", con))
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

        public string ValidarSolicitud(SolicitudUsuarios objSolicitudUsuarios)
        {
            string Error = string.Empty;
            try
            {
                if (objSolicitudUsuarios.IdDepartamento == 9 && objSolicitudUsuarios.UsuarioCASA == "")
                {
                    Error = "Falta el usuario de Casa";
                }  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return Error;
        }

        public List<SolicitudUsuariosCoincidencias> Coincidencias(int IdSolicitud)
        {
            List<SolicitudUsuariosCoincidencias> listCoincidencias = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("usuario.NET_LOAD_COINCIDENCIAS_USUARIOS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdSolicitud", SqlDbType.Int, 4).Value = IdSolicitud;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SolicitudUsuariosCoincidencias objCoincidencias = new();
                            objCoincidencias.idUsuario = Convert.ToInt32(dr["idUsuario"]);
                            objCoincidencias.Tipo = dr["Tipo"].ToString();                           
                            objCoincidencias.Coincidencias = dr["Coincidencias"].ToString();
                            objCoincidencias.Usuario = dr["Usuario"].ToString();
                            objCoincidencias.Email = dr["Email"].ToString();
                            objCoincidencias.NumeroDeEmpleado = dr["NumeroDeEmpleado"].ToString();
                            objCoincidencias.Departamento = dr["Departamento"].ToString();
                            objCoincidencias.Oficina = dr["Oficina"].ToString();
                          
                            listCoincidencias.Add(objCoincidencias);
                        }
                    }
                    else
                    {
                        listCoincidencias = null!;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listCoincidencias;
        }
    }
}
