using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wsVentanillaUnica;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{

    public class DireccionesdeDestinatariosRepository : IDireccionesdeDestinatariosRepository
    {
        public string sConexion { get; set; }

        public IConfiguration _configuration;

        public DireccionesdeDestinatariosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbMSG")!;
        }
        public int Insertar(DireccionesdeDestinatarios objDireccionesdeDestinatarios)
        {
            int id = 0;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {

                Validar(objDireccionesdeDestinatarios);


                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_DIRECCIONESDEDESTINATARIOS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdDestinatario", SqlDbType.Int, 4);
                @param.Value = objDireccionesdeDestinatarios.IdDestinatario;

                @param = cmd.Parameters.Add("@IDDireccion", SqlDbType.Int, 4);
                @param.Value = objDireccionesdeDestinatarios.IDDireccion;

                @param = cmd.Parameters.Add("@Calle", SqlDbType.VarChar, 50);
                @param.Value = objDireccionesdeDestinatarios.Calle;

                @param = cmd.Parameters.Add("@Colonia", SqlDbType.VarChar, 120);
                @param.Value = objDireccionesdeDestinatarios.Colonia;

                @param = cmd.Parameters.Add("@MunicipioAlcandia", SqlDbType.VarChar, 50);
                @param.Value = objDireccionesdeDestinatarios.MunicipioAlcandia;

                @param = cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 6);
                @param.Value = objDireccionesdeDestinatarios.CodigoPostal;

                @param = cmd.Parameters.Add("@NumeroExt", SqlDbType.VarChar, 10);
                @param.Value = objDireccionesdeDestinatarios.NumeroExt;

                @param = cmd.Parameters.Add("@NumeroInt", SqlDbType.VarChar, 10);
                @param.Value = objDireccionesdeDestinatarios.NumeroInt;

                @param = cmd.Parameters.Add("@Ciudad", SqlDbType.VarChar, 50);
                @param.Value = objDireccionesdeDestinatarios.Ciudad;

                @param = cmd.Parameters.Add("@ClaveEntidadFederativa", SqlDbType.VarChar, 2);
                @param.Value = objDireccionesdeDestinatarios.ClaveEntidadFederativa;


                @param = cmd.Parameters.Add("@Localidad", SqlDbType.VarChar, 120);
                @param.Value = objDireccionesdeDestinatarios.Localidad;

                @param = cmd.Parameters.Add("@IdCaptura", SqlDbType.SmallInt);
                @param.Value = objDireccionesdeDestinatarios.IdCaptura;

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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_DIRECCIONESDEDESTINATARIOS");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public int Modificar(DireccionesdeDestinatarios objDireccionesdeDestinatarios)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                //if (objDireccionesdeDestinatarios.IdDestinatario== 0)
                //{
                //    throw new Exception("para modificar es necesario seleccionar una direccion ");
                //}
                Validar(objDireccionesdeDestinatarios);

                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_UPDATE_DIRECCIONESDEDESTINATARIOS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IDDireccion", SqlDbType.SmallInt);
                @param.Value = objDireccionesdeDestinatarios.IDDireccion;

                @param = cmd.Parameters.Add("@Calle", SqlDbType.VarChar, 50);
                @param.Value = objDireccionesdeDestinatarios.Calle;

                @param = cmd.Parameters.Add("@Colonia", SqlDbType.VarChar, 120);
                @param.Value = objDireccionesdeDestinatarios.Colonia;

                @param = cmd.Parameters.Add("@MunicipioAlcandia", SqlDbType.VarChar, 50);
                @param.Value = objDireccionesdeDestinatarios.MunicipioAlcandia;

                @param = cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 6);
                @param.Value = objDireccionesdeDestinatarios.CodigoPostal;

                @param = cmd.Parameters.Add("@NumeroExt", SqlDbType.VarChar, 10);
                @param.Value = objDireccionesdeDestinatarios.NumeroExt;

                @param = cmd.Parameters.Add("@NumeroInt", SqlDbType.VarChar, 10);
                @param.Value = objDireccionesdeDestinatarios.NumeroInt;

                @param = cmd.Parameters.Add("@Ciudad", SqlDbType.VarChar, 50);
                @param.Value = objDireccionesdeDestinatarios.Ciudad;

                @param = cmd.Parameters.Add("@ClaveEntidadFederativa", SqlDbType.VarChar, 2);
                @param.Value = objDireccionesdeDestinatarios.ClaveEntidadFederativa;



                @param = cmd.Parameters.Add("@Localidad", SqlDbType.VarChar, 120);
                @param.Value = objDireccionesdeDestinatarios.Localidad;

                @param = cmd.Parameters.Add("@IdCaptura", SqlDbType.SmallInt);
                @param.Value = objDireccionesdeDestinatarios.IdCaptura;

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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_DIRECCIONESDEDESTINATARIOS");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public DireccionesdeDestinatarios Buscar(int IDDireccion)
        {
            DireccionesdeDestinatarios objDireccionesdeDestinatarios = new();

            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_SEARCH_DIRECCIONESDEDESTINATARIOS", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IDDireccion", SqlDbType.Int, 4).Value = IDDireccion;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        objDireccionesdeDestinatarios.IDDireccion = Convert.ToInt32(dr["IDDireccion"]);
                        objDireccionesdeDestinatarios.Calle = dr["Calle"].ToString();
                        objDireccionesdeDestinatarios.Colonia = dr["Colonia"].ToString();
                        objDireccionesdeDestinatarios.MunicipioAlcandia = dr["MunicipioAlcandia"].ToString();
                        objDireccionesdeDestinatarios.CodigoPostal = dr["CodigoPostal"].ToString();
                        objDireccionesdeDestinatarios.NumeroExt = dr["NumeroExt"].ToString();
                        objDireccionesdeDestinatarios.NumeroInt = dr["NumeroInt"].ToString();
                        objDireccionesdeDestinatarios.Ciudad = dr["Ciudad"].ToString();
                        objDireccionesdeDestinatarios.ClaveEntidadFederativa = dr["ClaveEntidadFederativa"].ToString();
                        objDireccionesdeDestinatarios.Activo = Convert.ToBoolean(dr["Activo"]);
                        objDireccionesdeDestinatarios.Localidad = dr["Localidad"].ToString();
                        objDireccionesdeDestinatarios.IdCaptura = Convert.ToInt32(dr["IdCaptura"]);
                        objDireccionesdeDestinatarios.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);

                    }
                    else
                    {
                        objDireccionesdeDestinatarios = null!;
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objDireccionesdeDestinatarios;
        }


        public List<DireccionesdeDestinatarios> Cargar(int IdDestinatario)
        {
            List<DireccionesdeDestinatarios> lstDirecciones = new List<DireccionesdeDestinatarios>();

            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_LOAD_DIRECCIONESDEDESTINATARIOS", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDestinatario", SqlDbType.Int, 4).Value = IdDestinatario;
                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DireccionesdeDestinatarios objDireccionesdeDestinatarios = new();
                            objDireccionesdeDestinatarios.IDDireccion = Convert.ToInt32(dr["IDDireccion"]);
                            objDireccionesdeDestinatarios.Calle = dr["Calle"].ToString();
                            objDireccionesdeDestinatarios.Colonia = dr["Colonia"].ToString();
                            objDireccionesdeDestinatarios.MunicipioAlcandia = dr["MunicipioAlcandia"].ToString();
                            objDireccionesdeDestinatarios.CodigoPostal = dr["CodigoPostal"].ToString();
                            objDireccionesdeDestinatarios.NumeroExt = dr["NumeroExt"].ToString();
                            objDireccionesdeDestinatarios.NumeroInt = dr["NumeroInt"].ToString();
                            objDireccionesdeDestinatarios.Ciudad = dr["Ciudad"].ToString();
                            objDireccionesdeDestinatarios.ClaveEntidadFederativa = dr["ClaveEntidadFederativa"].ToString();
                            objDireccionesdeDestinatarios.Activo = Convert.ToBoolean(dr["Activo"]);
                            objDireccionesdeDestinatarios.Localidad = dr["Localidad"].ToString();
                            objDireccionesdeDestinatarios.IdCaptura = Convert.ToInt32(dr["IdCaptura"]);
                            objDireccionesdeDestinatarios.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);

                            lstDirecciones.Add(objDireccionesdeDestinatarios);
                        }



                    }
                    else
                    {
                        lstDirecciones.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstDirecciones;
        }


        private bool Validar(DireccionesdeDestinatarios objDireccionesdeDestinatarios)
        {
            bool valida = false;

            try
            {
                if (objDireccionesdeDestinatarios.Calle == "")
                {
                    throw new Exception("La calle es obligatoria, favor de ingresarla");
                }

                if (objDireccionesdeDestinatarios.MunicipioAlcandia == "")
                {
                    throw new Exception("El municipio/alcaldia es obligatorio, favor de ingresarlo");
                }

                if (objDireccionesdeDestinatarios.Colonia == "")
                {
                    throw new Exception("La colonia es obligatoria, favor de ingresarla");
                }

                if (objDireccionesdeDestinatarios.Ciudad == "")
                {
                    throw new Exception("La ciudad es obligatoria, favor de ingresarla");
                }

                if (objDireccionesdeDestinatarios.CodigoPostal == "")
                {
                    throw new Exception("El Codigo Postal es obligatorio, favor de ingresarlo");
                }

                if (objDireccionesdeDestinatarios.NumeroExt == "")
                {
                    throw new Exception("El número exterior es obligatorio, favor de ingresarlo");
                }

                if (objDireccionesdeDestinatarios.IdCaptura == 0)
                {
                    throw new Exception("El usuario de captura es obligatorio");
                }

                if (objDireccionesdeDestinatarios.ClaveEntidadFederativa == "")
                {
                    throw new Exception("la entidad federativa es obligatoria, favor de ingresarla");
                }

                valida = true;
            }
            catch (Exception ex)
            {
                valida = false;
                throw new Exception(ex.Message.ToString());
            }

            return valida;
        }

        public List<DireccionesdeDestinatarios> Coincidencia(string CodigoPostal, string Calle, string NumeroExt)
        {
            List<DireccionesdeDestinatarios> lstDireccionesdeDestinatarios = new();

            try
            {
                using (SqlConnection cn = new SqlConnection(sConexion))

                using (SqlCommand cmd = new("NET_LOAD_DIRECCIONESDEDESTINATARIOS_COINCIDENCIAS", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CodigoPostal", SqlDbType.VarChar, 6).Value = CodigoPostal;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Calle", SqlDbType.VarChar, 50).Value = Calle;


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NumeroExt", SqlDbType.VarChar, 10).Value = NumeroExt;

                    using SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            DireccionesdeDestinatarios objDireccionesdeDestinatarios = new();

                            objDireccionesdeDestinatarios.IDDireccion = Convert.ToInt32(dr["IDDireccion"]);
                            objDireccionesdeDestinatarios.Calle = dr["Calle"].ToString();
                            objDireccionesdeDestinatarios.Colonia = dr["Colonia"].ToString();
                            objDireccionesdeDestinatarios.MunicipioAlcandia = dr["MunicipioAlcandia"].ToString();
                            objDireccionesdeDestinatarios.CodigoPostal = dr["CodigoPostal"].ToString();
                            objDireccionesdeDestinatarios.NumeroExt = dr["NumeroExt"].ToString();
                            objDireccionesdeDestinatarios.NumeroInt = dr["NumeroInt"].ToString();
                            objDireccionesdeDestinatarios.Ciudad = dr["Ciudad"].ToString();
                            objDireccionesdeDestinatarios.ClaveEntidadFederativa = dr["ClaveEntidadFederativa"].ToString();
                            objDireccionesdeDestinatarios.Activo = Convert.ToBoolean(dr["Activo"]);
                            objDireccionesdeDestinatarios.Localidad = dr["Localidad"].ToString();
                            objDireccionesdeDestinatarios.IdCaptura = Convert.ToInt32(dr["IdCaptura"]);
                            objDireccionesdeDestinatarios.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);

                            lstDireccionesdeDestinatarios.Add(objDireccionesdeDestinatarios);
                        }






                    }
                    else
                    {
                        lstDireccionesdeDestinatarios.Clear();
                    }


                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return lstDireccionesdeDestinatarios;
        }


    }
}
