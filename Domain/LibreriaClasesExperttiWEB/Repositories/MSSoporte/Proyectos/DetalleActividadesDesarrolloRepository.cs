using LibreriaClasesAPIExpertti.Entities.EntitiesProyectos;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using DocumentFormat.OpenXml.Office.Word;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Proyectos
{
    public class DetalleActividadesDesarrolloRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        
        public DetalleActividadesDesarrolloRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public DetalleActividadesDesarrollo Buscar(int IdDetalle)
        {
            DetalleActividadesDesarrollo objDetalleActividaDesdesarrollo = new();
            var objREFERENCIAS = new Referencias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CASAEI_DETALLEACTIVIDADESDESARROLLO_WEB";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdDetalle", SqlDbType.Int, 4);
            @param.Value = IdDetalle;



            // @IDDatosDeEmpresa
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objDetalleActividaDesdesarrollo.IdDetalle = Convert.ToInt32(dr["IdDetalle"]);
                    objDetalleActividaDesdesarrollo.IdProyecto = Convert.ToInt32(dr["IdProyecto"]);
                    objDetalleActividaDesdesarrollo.Tarea = (dr["Tarea"]).ToString();
                    objDetalleActividaDesdesarrollo.Porcentaje = Convert.ToDouble(dr["Porcentaje"]);
                    objDetalleActividaDesdesarrollo.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                    objDetalleActividaDesdesarrollo.FechaTermino = DBNull.Value.Equals(dr["FechaTermino"]) ? null : Convert.ToDateTime(dr["FechaTermino"]);
                    objDetalleActividaDesdesarrollo.IdEstatus = Convert.ToInt32(dr["IdEstatus"]);
                    objDetalleActividaDesdesarrollo.Seguimiento = (dr["Seguimiento"]).ToString();
                    int IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                    CatalogoDeUsuariosCorto objuser = new();
                    CatalogoDeUsuariosRepository objuserD = new(_configuration);
                    objuser= objuserD.BuscarPorIdCorto(IdUsuario);

                    objDetalleActividaDesdesarrollo.Usuario = objuser;
                    objDetalleActividaDesdesarrollo.FechaCompromiso = DBNull.Value.Equals(dr["FechaCompromiso"]) ? null : Convert.ToDateTime(dr["FechaCompromiso"]);
                    objDetalleActividaDesdesarrollo.TipoDePrioridad = Convert.ToInt32(dr["TipoDePrioridad"]);
                    objDetalleActividaDesdesarrollo.IdProyectoNuevo = DBNull.Value.Equals(dr["IdProyectoNuevo"]) ? null : Convert.ToInt32(dr["IdProyectoNuevo"]);
                    
                }
                else
                {
                    objDetalleActividaDesdesarrollo = default;
                }
                dr.Close();
                // cn.Close()
                // SqlConnection.ClearPool(cn)
                // cn.Dispose()
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            cn.Dispose();
            return objDetalleActividaDesdesarrollo;
        }
        /******************************************************************************************************
        Usuario Crea: Cubits
        Funcionalidad: Insertar tarea de proyectos.
        Fecha de Modificación: 2025-09-24
        Usuario Modifica: Edward - Cubits
        Motivo del Cambio: Se agrega mapeo de usuario realizo cambios en tarea.
        ******************************************************************************************************/
        public int Insertar(DetalleActividadesDesarrollo objDetalleActividadesDesarrollo)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CASAEI_DETALLEACTIVIDADESDESARROLLO_1";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdProyecto", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.IdProyecto;

                @param = cmd.Parameters.Add("@Tarea", SqlDbType.VarChar, 250);
                @param.Value = objDetalleActividadesDesarrollo.Tarea;

                @param = cmd.Parameters.Add("@Porcentaje", SqlDbType.Decimal);
                @param.Value = objDetalleActividadesDesarrollo.Porcentaje;

                @param = cmd.Parameters.Add("@IdEstatus", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.IdEstatus;

                @param = cmd.Parameters.Add("@Seguimiento", SqlDbType.VarChar, 500);
                @param.Value = objDetalleActividadesDesarrollo.Seguimiento;

                @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.Usuario.IdUsuario;

                @param = cmd.Parameters.Add("@FechaCompromiso", SqlDbType.DateTime);
                @param.Value = objDetalleActividadesDesarrollo.FechaCompromiso;

                @param = cmd.Parameters.Add("@TipoDePrioridad", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.TipoDePrioridad;

                @param = cmd.Parameters.Add("@IdUsuarioEvento", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.IdUsuarioEvento;

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
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CASAEI_DETALLEACTIVIDADESDESARROLLO_1");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }
        /******************************************************************************************************
        Usuario Crea: Cubits
        Funcionalidad: Modificar tarea de proyectos.
        Fecha de Modificación: 2025-09-24
        Usuario Modifica: Edward - Cubits
        Motivo del Cambio: Se agrega mapeo de usuario realizo cambios en tarea.
        ******************************************************************************************************/
        public int Modificar(DetalleActividadesDesarrollo objDetalleActividadesDesarrollo)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_UPDATE_CASAEI_DETALLEACTIVIDADESDESARROLLO_1";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdDetalle", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.IdDetalle;

                @param = cmd.Parameters.Add("@IdProyecto", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.IdProyecto;

                @param = cmd.Parameters.Add("@Tarea", SqlDbType.VarChar, 250);
                @param.Value = objDetalleActividadesDesarrollo.Tarea;

                @param = cmd.Parameters.Add("@Porcentaje", SqlDbType.Decimal);
                @param.Value = objDetalleActividadesDesarrollo.Porcentaje;

                @param = cmd.Parameters.Add("@IdEstatus", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.IdEstatus;

                @param = cmd.Parameters.Add("@Seguimiento", SqlDbType.VarChar, 500);
                @param.Value = objDetalleActividadesDesarrollo.Seguimiento;

                @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.Usuario.IdUsuario;

                @param = cmd.Parameters.Add("@FechaCompromiso", SqlDbType.DateTime);
                @param.Value = objDetalleActividadesDesarrollo.FechaCompromiso;

                @param = cmd.Parameters.Add("@TipoDePrioridad", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.TipoDePrioridad;

                @param = cmd.Parameters.Add("@IdUsuarioEvento", SqlDbType.SmallInt);
                @param.Value = objDetalleActividadesDesarrollo.IdUsuarioEvento;

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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CASAEI_DETALLEACTIVIDADESDESARROLLO_1");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }
        public int ModificarProyecto(int IdDetalle, int IdProyecto)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_UPDATE_CASAEI_DETALLEACTIVIDADESDESARROLLO_IdProyecto";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdDetalle", SqlDbType.SmallInt);
                @param.Value = IdDetalle;

                @param = cmd.Parameters.Add("@IdProyecto", SqlDbType.SmallInt);
                @param.Value = IdProyecto;

       

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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CASAEI_DETALLEACTIVIDADESDESARROLLO_1");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }

        public int ModificarTermino(int IdDetalle,int IdUsuario)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_UPDATE_CASAEI_DETALLEACTIVIDADESDESARROLLO_FECHATERMINO";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdDetalle", SqlDbType.SmallInt);
                @param.Value = IdDetalle;

                @param = cmd.Parameters.Add("@IdUsuario", SqlDbType.SmallInt);
                @param.Value = IdUsuario;

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
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CASAEI_DETALLEACTIVIDADESDESARROLLO_FECHATERMINO");
            }
            cn.Close();
            cn.Dispose();
            return id;
        }
        
        public List<DropDownListDatos> CargarEstatus(int TipoDeActividad)
        {
           List<DropDownListDatos> comboList = new();
           try
           {
               using (con = new(sConexion))
               using (SqlCommand cmd = new("NET_LOAD_ESTATUSDESARROLLO", con))
               {
                   con.Open();
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.Add("@TipoDeActividad", SqlDbType.Int, 4).Value = TipoDeActividad;
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

        // public List<EstatusDesarrollo> CargarEstatus(int TipoDeActividad)
        // {
        //     List<EstatusDesarrollo> lst = new();
        //     var cn = new SqlConnection();
        //     var cmd = new SqlCommand();
        //     SqlParameter @param;
        //     SqlDataReader dr;

        //     cn.ConnectionString = sConexion;
        //     cn.Open();

        //     cmd.CommandText = "NET_LOAD_ESTATUSDESARROLLO";
        //     cmd.Connection = cn;
        //     cmd.CommandType = CommandType.StoredProcedure;

        //     @param = cmd.Parameters.Add("@TipoDeActividad", SqlDbType.Int, 4);
        //     @param.Value = TipoDeActividad;

        //     try
        //     {
        //         dr = cmd.ExecuteReader();
        //         if (dr.HasRows)
        //         {

        //             while (dr.Read())
        //             {
        //                 EstatusDesarrollo obj = new();
        //                 obj.IdEstatus = Convert.ToInt32(dr["IdEstatus"]);
        //                 obj.Estatus = (dr["Estatus"]).ToString();
        //                 obj.FechaRequerida = Convert.ToBoolean(dr["FechaRequerida"]);
        //                 obj.IdDepartamento = Convert.ToInt32(dr["IdDepartamento"]);
        //                 lst.Add(obj);
        //             }
        //         }
        //         else
        //         {
        //             lst.Clear();
        //         }
        //         dr.Close();
        //         cmd.Parameters.Clear();
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new Exception(ex.Message.ToString());
        //     }
        //     return lst;
        // }

        public List<DetalleActividadesDesarrollo> Cargar(int IdProyecto, int? idUsuario, bool? Pendientes, int? Prioridad, int? Responsable, int? Estatus)
        {
            List<DetalleActividadesDesarrollo> lst = new();
            var objREFERENCIAS = new Referencias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_DETALLEACTIVIDADESDESARROLLO_WEB";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdProyecto", SqlDbType.Int, 4);
            @param.Value = IdProyecto;

            @param = cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 4);
            @param.Value = idUsuario;

            @param = cmd.Parameters.Add("@Pendientes", SqlDbType.Bit);
            @param.Value = Pendientes;

            @param = cmd.Parameters.Add("@Prioridad", SqlDbType.Int,4);
            @param.Value = Prioridad;
            @param = cmd.Parameters.Add("@Responsable", SqlDbType.Int,4);
            @param.Value = Responsable;
            @param = cmd.Parameters.Add("@Estatus", SqlDbType.Int,4);
            @param.Value = Estatus;

            // @IDDatosDeEmpresa
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        DetalleActividadesDesarrollo objDetalleActividaDesdesarrollo = new();
                        objDetalleActividaDesdesarrollo.IdDetalle = Convert.ToInt32(dr["IdDetalle"]);
                        objDetalleActividaDesdesarrollo.IdProyecto = Convert.ToInt32(dr["IdProyecto"]);
                        objDetalleActividaDesdesarrollo.Tarea = (dr["Tarea"]).ToString();
                        objDetalleActividaDesdesarrollo.Porcentaje = Convert.ToDouble(dr["Porcentaje"]);
                        objDetalleActividaDesdesarrollo.FechaAlta = Convert.ToDateTime(dr["FechaAlta"]);
                        objDetalleActividaDesdesarrollo.FechaTermino = DBNull.Value.Equals(dr["FechaTermino"]) ? null : Convert.ToDateTime(dr["FechaTermino"]);
                        objDetalleActividaDesdesarrollo.IdEstatus = Convert.ToInt32(dr["IdEstatus"]);
                        objDetalleActividaDesdesarrollo.Estatus = (dr["Estatus"]).ToString();
                        objDetalleActividaDesdesarrollo.FechaCompromiso = DBNull.Value.Equals(dr["FechaCompromiso"]) ? null : Convert.ToDateTime(dr["FechaCompromiso"]);
                        objDetalleActividaDesdesarrollo.Desarrolladores = (dr["Desarrolladores"]).ToString();
                        objDetalleActividaDesdesarrollo.TipoDePrioridad= Convert.ToInt32(dr["TipoDePrioridad"]);
                        objDetalleActividaDesdesarrollo.EstatusColor = (dr["EstatusColor"]).ToString();
                        lst.Add(objDetalleActividaDesdesarrollo);
                    }
                }
                else
                {
                    lst.Clear();
                }
                dr.Close();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString());
            }
            cn.Close();
            cn.Dispose();
            return lst;
        }
        public List<DetalleActividadesDesarrollo> CargarPorUsuario(int idUsuario, DateTime? fechaInicial = null, DateTime? fechaFinal = null)
        {
            List<DetalleActividadesDesarrollo> lst = new();
            var cn = new SqlConnection(sConexion);
            var cmd = new SqlCommand();
            SqlDataReader dr;

            try
            {
                cn.Open();
                cmd.CommandText = "NET_LOAD_TAREAS_PENDIENTES_WEB";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetro obligatorio
                cmd.Parameters.Add("@IdDesarrollador", SqlDbType.Int).Value = idUsuario;

                // Parámetros opcionales (si son null, se envía DBNull.Value)
                cmd.Parameters.Add("@FechaInicial", SqlDbType.Date).Value = (object)fechaInicial ?? DBNull.Value;
                cmd.Parameters.Add("@FechaFinal", SqlDbType.Date).Value = (object)fechaFinal ?? DBNull.Value;

                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    DetalleActividadesDesarrollo objDetalle = new()
                    {
                        Folio = dr["Folio"].ToString(),
                        IdProyecto = Convert.ToInt32(dr["IdProyecto"]),
                        Solicitud = dr["Solicitud"].ToString(),
                        Tarea = dr["Tarea"].ToString(),
                        Porcentaje = Convert.ToDouble(dr["Porcentaje"]),
                        FechaAlta = Convert.ToDateTime(dr["FechaAlta"]),
                        FechaCompromiso = DBNull.Value.Equals(dr["FechaCompromiso"]) ? null : Convert.ToDateTime(dr["FechaCompromiso"]),
                        Seccion = dr["seccion"].ToString(),
                    };

                    lst.Add(objDetalle);
                }

                dr.Close();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
                cn.Dispose();
            }

            return lst;
        }


    }
}