using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{

    public class CatalogoDeClientesDeDeMensajeriaRepository : ICatalogoDeClientesDeDeMensajeriaRepository
    {
        //public string SConexion { get; set; }
        //public IConfiguration _configuration;
        public SqlConnection con;
        public string ClaveCliente;
        public int idCliente;
        public string SConexion { get; set; }
        string ICatalogoDeClientesDeDeMensajeriaRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public CatalogoDeClientesDeDeMensajeriaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogoDeClientesDeDeMensajeria Buscar(int idCliente)
        {
            CatalogoDeClientesDeDeMensajeria objCatalogoDeClientesDeDeMensajeria = new();
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CATALOGODECLIENTESDEDEMENSAJERIA_POR_ID", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idCliente", SqlDbType.VarChar).Value = idCliente;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        dr.Read();


                        objCatalogoDeClientesDeDeMensajeria.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objCatalogoDeClientesDeDeMensajeria.Clave = dr["Clave"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.Nombre = dr["Nombre"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.ApellidoPaterno = dr["ApellidoPaterno"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.ApellidoMaterno = dr["ApellidoMaterno"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.RFC = dr["RFC"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.IDCapturo = Convert.ToInt32(dr["IDCapturo"]);
                        objCatalogoDeClientesDeDeMensajeria.Activo = Convert.ToBoolean(dr["Activo"]);
                        objCatalogoDeClientesDeDeMensajeria.Telefono = dr["Telefono"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.EmailContacto = dr["EmailContacto"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.Atencion = dr["Atencion"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.eMailCFD = dr["eMailCFD"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.TipoDeFigura = Convert.ToInt32(dr["TipoDeFigura"]);
                        objCatalogoDeClientesDeDeMensajeria.EmailPdfCASA = dr["EmailPdfCASA"].ToString();
                        objCatalogoDeClientesDeDeMensajeria.FechaDeAlta = Convert.ToDateTime(dr["FechaDeAlta"]);
                        objCatalogoDeClientesDeDeMensajeria.TipoDePersona = Convert.ToBoolean(dr["TipoDePersona"]);
                        objCatalogoDeClientesDeDeMensajeria.SoloExportacion = Convert.ToBoolean(dr["SoloExportacion"]);
                        objCatalogoDeClientesDeDeMensajeria.Prospecto = Convert.ToBoolean(dr["Prospecto"]);



                        DireccionesDeClientesRepository objDireccionesDeClientesRepository = new DireccionesDeClientesRepository(_configuration);

                        objCatalogoDeClientesDeDeMensajeria.Direcciones = objDireccionesDeClientesRepository.BuscarDireccionActiva(objCatalogoDeClientesDeDeMensajeria.IDCliente);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objCatalogoDeClientesDeDeMensajeria;
        }

        public string Insertar(CatalogoDeClientesDeDeMensajeria objCatalogoDeClientesDeDeMensajeria)
        {

            try
            {
                ValidarCliente(objCatalogoDeClientesDeDeMensajeria);

                using (SqlConnection con = new(SConexion))
                {
                    con.Open();
                    var cmd = new SqlCommand("NET_INSERT_CATALOGODECLIENTESDEDEMENSAJERIA_WEB", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 120).Value = objCatalogoDeClientesDeDeMensajeria.Nombre;
                    cmd.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.ApellidoPaterno;
                    cmd.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.ApellidoMaterno;
                    cmd.Parameters.Add("@RFC", SqlDbType.Char, 13).Value = objCatalogoDeClientesDeDeMensajeria.RFC;
                    cmd.Parameters.Add("@IDCapturo", SqlDbType.Int, 4).Value = objCatalogoDeClientesDeDeMensajeria.IDCapturo;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit, 4).Value = objCatalogoDeClientesDeDeMensajeria.Activo;
                    cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.Telefono;
                    cmd.Parameters.Add("@EmailContacto", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.EmailContacto;
                    cmd.Parameters.Add("@Atencion", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.Atencion;
                    cmd.Parameters.Add("@TipoDePersona", SqlDbType.Bit, 4).Value = objCatalogoDeClientesDeDeMensajeria.TipoDePersona;
                    cmd.Parameters.Add("@SoloExportacion", SqlDbType.Bit, 4).Value = objCatalogoDeClientesDeDeMensajeria.SoloExportacion;
                    cmd.Parameters.Add("@Prospecto", SqlDbType.Bit, 4).Value = objCatalogoDeClientesDeDeMensajeria.Prospecto;
                    cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4).Value = objCatalogoDeClientesDeDeMensajeria.idReferencia;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Clave", SqlDbType.VarChar, 6).Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    idCliente = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    ClaveCliente = cmd.Parameters["@Clave"].Value.ToString();

                    cmd.Parameters.Clear();

                    DireccionesDeClientesRepository objdir = new DireccionesDeClientesRepository(_configuration);
                    objCatalogoDeClientesDeDeMensajeria.Direcciones.IDCliente = idCliente;
                    objdir.Insertar(objCatalogoDeClientesDeDeMensajeria.Direcciones);

                    Bitacora objBitacora = new Bitacora();
                    BitacoraRepository InsertarBitacoraRepository = new(_configuration);

                    objBitacora.IDCliente = idCliente;
                    objBitacora.Estatus = 3;
                    objBitacora.IDUsuario = objCatalogoDeClientesDeDeMensajeria.IDCapturo;
                    objBitacora.Observaciones = "ALTA DE CLIENTE";

                    int IDBitacora = InsertarBitacoraRepository.Insertar(objBitacora);

                }
            }
            catch (Exception ex)
            {
                ClaveCliente = "";
                con.Close();
                con.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            con.Close();
            con.Dispose();
            return ClaveCliente;


        }


        public string Modificar(CatalogoDeClientesDeDeMensajeria objCatalogoDeClientesDeDeMensajeria)
        {
            ValidarCliente(objCatalogoDeClientesDeDeMensajeria);

            try
            {
                using (SqlConnection con = new(SConexion))
                {
                    con.Open();
                    var cmd = new SqlCommand("NET_UPDATE_CATALOGODECLIENTESDEDEMENSAJERIA", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objCatalogoDeClientesDeDeMensajeria.IDCliente;
                    cmd.Parameters.Add("@Clave", SqlDbType.VarChar, 6).Value = objCatalogoDeClientesDeDeMensajeria.Clave;
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 120).Value = objCatalogoDeClientesDeDeMensajeria.Nombre;
                    cmd.Parameters.Add("@ApellidoPaterno", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.ApellidoPaterno;
                    cmd.Parameters.Add("@ApellidoMaterno", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.ApellidoMaterno;
                    cmd.Parameters.Add("@RFC", SqlDbType.Char, 13).Value = objCatalogoDeClientesDeDeMensajeria.RFC;
                    cmd.Parameters.Add("@IDCapturo", SqlDbType.Int, 4).Value = objCatalogoDeClientesDeDeMensajeria.IDCapturo;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit, 4).Value = objCatalogoDeClientesDeDeMensajeria.Activo;
                    cmd.Parameters.Add("@Telefono", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.Telefono;
                    cmd.Parameters.Add("@EmailContacto", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.EmailContacto;
                    cmd.Parameters.Add("@Atencion", SqlDbType.VarChar, 80).Value = objCatalogoDeClientesDeDeMensajeria.Atencion;
                    cmd.Parameters.Add("@eMailCFD", SqlDbType.VarChar).Value = objCatalogoDeClientesDeDeMensajeria.eMailCFD;
                    cmd.Parameters.Add("@TipoDeFigura", SqlDbType.Int, 4).Value = objCatalogoDeClientesDeDeMensajeria.TipoDeFigura;
                    cmd.Parameters.Add("@EmailPdfCASA", SqlDbType.VarChar).Value = objCatalogoDeClientesDeDeMensajeria.EmailPdfCASA;
                    cmd.Parameters.Add("@TipoDePersona", SqlDbType.Bit, 4).Value = objCatalogoDeClientesDeDeMensajeria.TipoDePersona;
                    cmd.Parameters.Add("@SoloExportacion", SqlDbType.Bit, 4).Value = objCatalogoDeClientesDeDeMensajeria.SoloExportacion;
                    cmd.Parameters.Add("@Prospecto", SqlDbType.Bit, 4).Value = objCatalogoDeClientesDeDeMensajeria.Prospecto;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    ClaveCliente = objCatalogoDeClientesDeDeMensajeria.Clave;
                    idCliente = objCatalogoDeClientesDeDeMensajeria.IDCliente;

                    if (objCatalogoDeClientesDeDeMensajeria.Direcciones.IDDireccion != 0)
                    {
                        DireccionesDeClientesRepository objdir = new DireccionesDeClientesRepository(_configuration);
                        objdir.Modificar(objCatalogoDeClientesDeDeMensajeria.Direcciones);
                    }

                    Bitacora objBitacora = new Bitacora();
                    BitacoraRepository InsertarBitacoraRepository = new(_configuration);

                    objBitacora.IDCliente = idCliente;
                    objBitacora.Estatus = 2;
                    objBitacora.IDUsuario = objCatalogoDeClientesDeDeMensajeria.IDCapturo;
                    objBitacora.Observaciones = objCatalogoDeClientesDeDeMensajeria.Observaciones;

                    int IDBitacora = InsertarBitacoraRepository.Insertar(objBitacora);

                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {

                con.Close();
                con.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            con.Close();
            con.Dispose();

            return ClaveCliente;

        }

        public string Mover(string Clave, string RFC, string CURP)
        {

            try
            {
                using (SqlConnection con = new(SConexion))
                {
                    con.Open();
                    var cmd = new SqlCommand("NET_MOVER_CLIENTE_DE_MENSAJERIA_A_FORMAL", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Clave", SqlDbType.VarChar, 6).Value = Clave;
                    cmd.Parameters.Add("@RFC", SqlDbType.VarChar, 13).Value = RFC;
                    cmd.Parameters.Add("@CURP", SqlDbType.VarChar, 18).Value = CURP;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                ClaveCliente = "";
                con.Close();
                con.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            con.Close();
            con.Dispose();

            return Clave;

        }

        public void ValidarCliente(CatalogoDeClientesDeDeMensajeria objCliente)
        {
            try
            {
                CatalogoDeUsuarios objUsuario = new CatalogoDeUsuarios();
                CatalogoDeUsuariosRepository objUsuarioD = new CatalogoDeUsuariosRepository(_configuration);

                objUsuario = objUsuarioD.BuscarPorId(objCliente.IDCapturo);

                if (objUsuario.IdDepartamento != 21)
                {
                    if (objCliente.idReferencia == 0)
                    {
                        throw new Exception("Es necesario ingresar el  número de referencia");
                    }
                }

                if (objCliente.Nombre == null)
                {
                    throw new Exception("Es necesario ingresar el  nombre del cliente");
                }

                if (objCliente.Nombre == "")
                {
                    throw new Exception("Es necesario ingresar el  nombre del cliente");
                }

                if (objCliente.RFC == null)
                {
                    throw new Exception("Es necesario ingresar el  RFC del cliente");
                }

                if (objCliente.RFC == "")
                {
                    throw new Exception("Es necesario ingresar el  RFC del cliente");
                }

                if (objCliente.Direcciones.Direccion != null)
                {
                    if (objCliente.Direcciones.Direccion == "")
                    {
                        throw new Exception("Es necesario ingresar la direccion del cliente");
                    }

                    if (objCliente.Direcciones.CodigoPostal == "")
                    {
                        throw new Exception("Es necesario ingresar el codigo postal del cliente");
                    }
                }



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }



        }
    }
}
