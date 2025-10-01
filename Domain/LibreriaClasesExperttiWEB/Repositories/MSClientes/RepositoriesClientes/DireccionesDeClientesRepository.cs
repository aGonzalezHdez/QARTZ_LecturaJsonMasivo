using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class DireccionesDeClientesRepository : IDireccionesDeClientesRepository
    {
        string IDireccionesDeClientesRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string SConexion { get; set; }
        public IConfiguration _configuration;  
        
        public DireccionesDeClientesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }


        public List<DireccionesDeClientes> BuscarCliente(int IDCliente)
        {
            List<DireccionesDeClientes> list = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_DIRECCIONESDECLIENTESEXPERTTI", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int).Value = IDCliente;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        list = SqlDataReaderToList.DataReaderMapToList<DireccionesDeClientes>(sdr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public DireccionesDeClientes BuscarDireccion(int IDDireccion)
        {
            DireccionesDeClientes objDireccion = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_DIRECCIONESDECLIENTESEXPERTTI_POR_IDDIRECCION", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDDireccion", SqlDbType.Int).Value = IDDireccion;
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objDireccion.IDDireccion = Convert.ToInt32(dr["IDDireccion"]);
                            objDireccion.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                            objDireccion.Direccion = string.Format("{0}", dr["Direccion"]);
                            objDireccion.Colonia = string.Format("{0}", dr["Colonia"]);
                            objDireccion.Poblacion = string.Format("{0}", dr["Poblacion"]);
                            objDireccion.CodigoPostal = string.Format("{0}", dr["CodigoPostal"]);
                            objDireccion.NumeroExt = string.Format("{0}", dr["NumeroExt"]);
                            objDireccion.NumeroInt = string.Format("{0}", dr["NumeroInt"]);
                            objDireccion.EntreLaCalleDe = string.Format("{0}", dr["EntreLaCalleDe"]);
                            objDireccion.YDe = string.Format("{0}", dr["YDe"]);
                            objDireccion.Entidad = string.Format("{0}", dr["Entidad"]);
                            objDireccion.Activo = Convert.ToBoolean(dr["Activo"]);
                            objDireccion.Orden = Convert.ToInt32(dr["Orden"]);
                            objDireccion.Localidad = string.Format("{0}", dr["Localidad"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objDireccion;
        }

        public DireccionesDeClientes BuscarDireccionActiva(int IDCliente)
        {
            DireccionesDeClientes objDireccion = new();

            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_CASAEI_SEARCH_DIRECCIONESDECLIENTESEXPERTTI_ACTIVA", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int).Value = IDCliente;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objDireccion.IDDireccion = Convert.ToInt32(dr["IDDireccion"]);
                        objDireccion.IDCliente = Convert.ToInt32(dr["IDCliente"]);
                        objDireccion.Direccion = string.Format("{0}", dr["Direccion"]);
                        objDireccion.Colonia = string.Format("{0}", dr["Colonia"]);
                        objDireccion.Poblacion = string.Format("{0}", dr["Poblacion"]);
                        objDireccion.CodigoPostal = string.Format("{0}", dr["CodigoPostal"]);
                        objDireccion.NumeroExt = string.Format("{0}", dr["NumeroExt"]);
                        objDireccion.NumeroInt = string.Format("{0}", dr["NumeroInt"]);
                        objDireccion.EntreLaCalleDe = string.Format("{0}", dr["EntreLaCalleDe"]);
                        objDireccion.YDe = string.Format("{0}", dr["YDe"]);
                        objDireccion.Entidad = string.Format("{0}", dr["Entidad"]);
                        objDireccion.Activo = Convert.ToBoolean(dr["Activo"]);
                        objDireccion.Orden = Convert.ToInt32(dr["Orden"]);
                        objDireccion.Localidad = string.Format("{0}", dr["Localidad"]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return objDireccion;
        }

        public int Insertar(DireccionesDeClientes objDireccionesDeClientes)
        {
            Errores Errores = new();
            int id = 0;
            try
            {

                using (SqlConnection con = new SqlConnection(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_DIRECCIONESDECLIENTESEXPERTTI_NEW", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@IDCliente", SqlDbType.Int, 4).Value = objDireccionesDeClientes.IDCliente;
                    cmd.Parameters.Add("@Direccion", SqlDbType.VarChar, 80).Value = objDireccionesDeClientes.Direccion + " COL. " + objDireccionesDeClientes.Colonia;
                    cmd.Parameters.Add("@Colonia", SqlDbType.VarChar, 120).Value = objDireccionesDeClientes.Colonia;
                    cmd.Parameters.Add("@Poblacion", SqlDbType.VarChar, 80).Value = objDireccionesDeClientes.Poblacion;
                    cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 6).Value = objDireccionesDeClientes.CodigoPostal;
                    cmd.Parameters.Add("@NumeroExt", SqlDbType.VarChar, 10).Value = objDireccionesDeClientes.NumeroExt;
                    cmd.Parameters.Add("@NumeroInt", SqlDbType.VarChar, 10).Value = objDireccionesDeClientes.NumeroInt;
                    cmd.Parameters.Add("@EntreLaCalleDe", SqlDbType.VarChar, 120).Value = objDireccionesDeClientes.EntreLaCalleDe;
                    cmd.Parameters.Add("@YDe", SqlDbType.VarChar, 120).Value = objDireccionesDeClientes.YDe;
                    cmd.Parameters.Add("@ClaveEntidadFederativa", SqlDbType.VarChar, 2).Value = objDireccionesDeClientes.Entidad;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit, 4).Value = objDireccionesDeClientes.Activo;
                    cmd.Parameters.Add("@Localidad", SqlDbType.VarChar, 120).Value = objDireccionesDeClientes.Localidad;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;


                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != 0)
                            {
                                id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                            }
                            else
                            {
                                Errores.IdError = 2;
                                Errores.Error = $"No fue posible guardar la dirección";
                                var json = JsonConvert.SerializeObject(Errores);
                                throw new Exception(json);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message.ToString());
                Errores.IdError = 2;
                Errores.Error = $"No fue posible guardar la dirección por : " + ex.Message.ToString();
                var json = JsonConvert.SerializeObject(Errores);
                throw new Exception(json);
            }

            return id;
        }

        public class Errores
        {
            public int IdError { get; set; }
            public string Error { get; set; }
        }

        public int Modificar(DireccionesDeClientes objDireccionesDeClientes)
        {
            int id;
            try
            {
                using (SqlConnection con = new SqlConnection(SConexion))
                using (var cmd = new SqlCommand("NET_UPDATE_DIRECCIONESDECLIENTESEXPERTTI_NEW", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDDireccion", SqlDbType.Int, 4).Value = objDireccionesDeClientes.IDDireccion;
                    cmd.Parameters.Add("@Direccion", SqlDbType.VarChar, 80).Value = objDireccionesDeClientes.Direccion;
                    cmd.Parameters.Add("@Colonia", SqlDbType.VarChar, 120).Value = objDireccionesDeClientes.Colonia;
                    cmd.Parameters.Add("@Poblacion", SqlDbType.VarChar, 80).Value = objDireccionesDeClientes.Poblacion;
                    cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 6).Value = objDireccionesDeClientes.CodigoPostal;
                    cmd.Parameters.Add("@NumeroExt", SqlDbType.VarChar, 10).Value = objDireccionesDeClientes.NumeroExt;
                    cmd.Parameters.Add("@NumeroInt", SqlDbType.VarChar, 10).Value = objDireccionesDeClientes.NumeroInt;
                    cmd.Parameters.Add("@EntreLaCalleDe", SqlDbType.VarChar, 120).Value = objDireccionesDeClientes.EntreLaCalleDe;
                    cmd.Parameters.Add("@YDe", SqlDbType.VarChar, 120).Value = objDireccionesDeClientes.YDe;
                    cmd.Parameters.Add("@ClaveEntidadFederativa", SqlDbType.VarChar, 2).Value = objDireccionesDeClientes.Entidad;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = objDireccionesDeClientes.Activo;
                    cmd.Parameters.Add("@Orden", SqlDbType.Int, 4).Value = objDireccionesDeClientes.Orden;
                    cmd.Parameters.Add("@Localidad", SqlDbType.VarChar, 120).Value = objDireccionesDeClientes.Localidad;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return id;
        }

        public void ValidarObjeto(DireccionesDeClientes objDireccionesDeClientes, bool isInsert)
        {
            if (isInsert == false)
            {
                DireccionesDeClientesRepository direccionesRepository = new(_configuration);
                DireccionesDeClientes objDirecciones = direccionesRepository.BuscarDireccion(objDireccionesDeClientes.IDDireccion);

                if (objDirecciones == null)
                {
                    //Error = $"La direccion con IDDireccion = {objDireccionesDeClientes.IDDireccion} no existe.";
                    throw new Exception($"La direccion con IDDireccion = {objDireccionesDeClientes.IDDireccion} no existe.");
                }
            }
        }
    }
}
