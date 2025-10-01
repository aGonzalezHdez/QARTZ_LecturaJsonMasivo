using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesProyectos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Proyectos
{
    public class ActividadesdeDesarrolloRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public IUbicaciondeArchivosRepository _ubicaciondeArchivosRepository;
        BucketsS3Repository _bucketRepo;

        public ActividadesdeDesarrolloRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
            _ubicaciondeArchivosRepository = new UbicaciondeArchivosRepository(_configuration);
            _bucketRepo = new(_configuration);
        }

        public ActividadesdeDesarrollo Buscar(int IdProyecto)
        {
            ActividadesdeDesarrollo objActividadesDesarrollo = new();
            var objREFERENCIAS = new Referencias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CASAEI_ACTIVIDADESDESARROLLO_WEB";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdProyecto", SqlDbType.Int, 4);
            @param.Value = IdProyecto;



            // @IDDatosDeEmpresa
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objActividadesDesarrollo.IdProyecto = Convert.ToInt32(dr["IdProyecto"]);
                    objActividadesDesarrollo.IdDesarrollador = Convert.ToInt32(dr["IdDesarrollador"]);
                    objActividadesDesarrollo.TipoDePrioridad = Convert.ToInt32(dr["TipoDePrioridad"]);
                    objActividadesDesarrollo.Solicitud = (dr["Solicitud"]).ToString();
                    objActividadesDesarrollo.App = (dr["App"]).ToString();
                    objActividadesDesarrollo.Estatus = Convert.ToDouble(dr["Estatus"]);
                    objActividadesDesarrollo.MotivosDeDemora = (dr["MotivosDeDemora"]).ToString();
                    objActividadesDesarrollo.FechaSolicitud = Convert.ToDateTime(dr["FechaSolicitud"]);
                    objActividadesDesarrollo.FechaCompromido = DBNull.Value.Equals(dr["FechaCompromido"]) ? null : Convert.ToDateTime(dr["FechaCompromido"]);
                    objActividadesDesarrollo.FechaInicio = DBNull.Value.Equals(dr["FechaInicio"]) ? null : Convert.ToDateTime(dr["FechaInicio"]);
                    objActividadesDesarrollo.FechaFin = DBNull.Value.Equals(dr["FechaFin"]) ? null : Convert.ToDateTime(dr["FechaFin"]);
                    int IdUsuarioSolicita = Convert.ToInt32(dr["IdUsuarioSolicita"]);
                    int IdUsuarioPruebas = Convert.ToInt32(dr["IdUsuarioPruebas"]);
                    CatalogoDeUsuariosRepository objuserD = new(_configuration);
                    CatalogoDeUsuariosCorto objuserSol = new();
                    objuserSol = objuserD.BuscarPorIdCorto(IdUsuarioSolicita);
                    objActividadesDesarrollo.UsuarioSolicita = objuserSol;

                    CatalogoDeUsuariosCorto objuserPruebas = new();
                    objuserPruebas = objuserD.BuscarPorIdCorto(IdUsuarioPruebas);
                    objActividadesDesarrollo.UsuarioPruebas = objuserPruebas;

                    objActividadesDesarrollo.VersionPublicada = (dr["VersionPublicada"]).ToString();
                    objActividadesDesarrollo.Comentarios = (dr["Comentarios"]).ToString();
                    objActividadesDesarrollo.TipoDeActividad = Convert.ToInt32(dr["TipoDeActividad"]);
                    objActividadesDesarrollo.idDepartamento = Convert.ToInt32(dr["idDepartamento"]);
                    objActividadesDesarrollo.IdEstatus = Convert.ToInt32(dr["IdEstatus"]);
                    objActividadesDesarrollo.Folio = dr["Folio"].ToString();

                    string idInvolucrados = dr["IdsUsuariosInvolucrados"]?.ToString();

                    if (!string.IsNullOrWhiteSpace(idInvolucrados))
                    {
                        objActividadesDesarrollo.IdUsuariosInvolucrados = idInvolucrados
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => int.Parse(s))
                            .ToList();
                    }

                }
                else
                {
                    objActividadesDesarrollo = default;
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
            return objActividadesDesarrollo;
        }

        public ActividadesdeDesarrollo Insertar(ActividadesdeDesarrollo objActividadesDesarrollo)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.CommandText = "NET_INSERT_CASAEI_ACTIVIDADESDESARROLLO_WEB";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;


                @param = cmd.Parameters.Add("@IdDesarrollador", SqlDbType.Int, 4);
                @param.Value = objActividadesDesarrollo.IdDesarrollador;

                @param = cmd.Parameters.Add("@TipoDePrioridad", SqlDbType.SmallInt);
                @param.Value = objActividadesDesarrollo.TipoDePrioridad;

                @param = cmd.Parameters.Add("@Solicitud", SqlDbType.VarChar, int.MaxValue);
                @param.Value = objActividadesDesarrollo.Solicitud;

                @param = cmd.Parameters.Add("@App", SqlDbType.VarChar, 50);
                @param.Value = objActividadesDesarrollo.App;

                @param = cmd.Parameters.Add("@Estatus", SqlDbType.Decimal);
                @param.Value = objActividadesDesarrollo.Estatus;

                @param = cmd.Parameters.Add("@MotivosDeDemora", SqlDbType.VarChar, 500);
                @param.Value = objActividadesDesarrollo.MotivosDeDemora;


                @param = cmd.Parameters.Add("@IdUsuarioSolicita", SqlDbType.Int);
                @param.Value = objActividadesDesarrollo.UsuarioSolicita.IdUsuario;

                @param = cmd.Parameters.Add("@IdUsuarioPruebas", SqlDbType.Int);
                @param.Value = objActividadesDesarrollo.UsuarioPruebas.IdUsuario;

                @param = cmd.Parameters.Add("@VersionPublicada", SqlDbType.VarChar, 10);
                @param.Value = objActividadesDesarrollo.VersionPublicada;

                @param = cmd.Parameters.Add("@Comentarios", SqlDbType.VarChar, int.MaxValue);
                @param.Value = objActividadesDesarrollo.Comentarios;

                @param = cmd.Parameters.Add("@TipoDeActividad", SqlDbType.Int);
                @param.Value = objActividadesDesarrollo.TipoDeActividad;

                @param = cmd.Parameters.Add("@IdEstatus", SqlDbType.Int);
                @param.Value = objActividadesDesarrollo.IdEstatus;

                @param = cmd.Parameters.Add("@idDepartamento", SqlDbType.Int);
                @param.Value = objActividadesDesarrollo.idDepartamento;

                @param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                @param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                {
                    id = (int)cmd.Parameters["@newid_registro"].Value;

                    objActividadesDesarrollo = Buscar(id);
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
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CASAEI_ACTIVIDADESDESARROLLO_NEW");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return objActividadesDesarrollo;
        }

        public int Modificar(ActividadesdeDesarrollo objActividadesDesarrollo)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();
                cmd.CommandText = "NET_UPDATE_CASAEI_ACTIVIDADESDESARROLLO_WEB";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@IdProyecto", SqlDbType.SmallInt);
                @param.Value = objActividadesDesarrollo.IdProyecto;

                @param = cmd.Parameters.Add("@IdDesarrollador", SqlDbType.Int, 4);
                @param.Value = objActividadesDesarrollo.IdDesarrollador;

                @param = cmd.Parameters.Add("@TipoDePrioridad", SqlDbType.SmallInt);
                @param.Value = objActividadesDesarrollo.TipoDePrioridad;

                @param = cmd.Parameters.Add("@Solicitud", SqlDbType.VarChar, int.MaxValue);
                @param.Value = objActividadesDesarrollo.Solicitud;

                @param = cmd.Parameters.Add("@App", SqlDbType.VarChar, 50);
                @param.Value = objActividadesDesarrollo.App;

                @param = cmd.Parameters.Add("@Estatus", SqlDbType.Decimal);
                @param.Value = objActividadesDesarrollo.Estatus;

                @param = cmd.Parameters.Add("@MotivosDeDemora", SqlDbType.VarChar, 500);
                @param.Value = objActividadesDesarrollo.MotivosDeDemora;

                @param = cmd.Parameters.Add("@FechaSolicitud", SqlDbType.DateTime);
                @param.Value = objActividadesDesarrollo.FechaSolicitud;

                @param = cmd.Parameters.Add("@FechaCompromido", SqlDbType.DateTime);
                @param.Value = objActividadesDesarrollo.FechaCompromido;

                @param = cmd.Parameters.Add("@FechaInicio", SqlDbType.DateTime);
                @param.Value = objActividadesDesarrollo.FechaInicio;

                @param = cmd.Parameters.Add("@FechaFin", SqlDbType.DateTime);
                @param.Value = objActividadesDesarrollo.FechaFin;

                @param = cmd.Parameters.Add("@IdUsuarioSolicita", SqlDbType.Int);
                @param.Value = objActividadesDesarrollo.UsuarioSolicita.IdUsuario;

                @param = cmd.Parameters.Add("@IdUsuarioPruebas", SqlDbType.Int);
                @param.Value = objActividadesDesarrollo.UsuarioPruebas.IdUsuario;

                @param = cmd.Parameters.Add("@VersionPublicada", SqlDbType.VarChar, 10);
                @param.Value = objActividadesDesarrollo.VersionPublicada;

                @param = cmd.Parameters.Add("@Comentarios", SqlDbType.VarChar, int.MaxValue);
                @param.Value = objActividadesDesarrollo.Comentarios;

                @param = cmd.Parameters.Add("@TipoDeActividad", SqlDbType.Int);
                @param.Value = objActividadesDesarrollo.TipoDeActividad;

                @param = cmd.Parameters.Add("@IdEstatus", SqlDbType.Int);
                @param.Value = objActividadesDesarrollo.IdEstatus;

                if (objActividadesDesarrollo.IdUsuariosInvolucrados!=null)
                {
                    @param = cmd.Parameters.Add("@IdsUsuariosInvolucrados", SqlDbType.Structured);
                    @param.TypeName = "IntListType";
                    @param.Value = ConvertListToDataTable(objActividadesDesarrollo.IdUsuariosInvolucrados);
                }
            

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
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_INSERT_CASAEI_ACTIVIDADESDESARROLLO_WEB");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return id;
        }


        public List<ProyectosPendientes> Pendientes(int idUsuario)
        {
            List<ProyectosPendientes> lstProyectosPendientes = new();
            var objREFERENCIAS = new Referencias();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_LOAD_PROYECTOS_PENDIENTES_WEB";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@idUsuario", SqlDbType.Int, 4);
            @param.Value = idUsuario;



            // @IDDatosDeEmpresa
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        ProyectosPendientes objProyectosPendientes = new();
                        objProyectosPendientes.idproyecto = Convert.ToInt32(dr["IdProyecto"]);
                        objProyectosPendientes.Prioridad = (dr["Prioridad"]).ToString();
                        objProyectosPendientes.TipodeActividad = (dr["TipoDeActividad"]).ToString();
                        objProyectosPendientes.Solicitud = (dr["Solicitud"]).ToString();
                        objProyectosPendientes.Folio = (dr["Folio"]).ToString();

                        objProyectosPendientes.Estatus = (dr["Estatus"]).ToString();
                        objProyectosPendientes.Porcentaje = Convert.ToDouble(dr["Porcentaje"]);
                        objProyectosPendientes.FechadeSolicitud = Convert.ToDateTime(dr["FechaSolicitud"]);


                        objProyectosPendientes.FechaCompromiso = DBNull.Value.Equals(dr["FechaCompromiSo"]) ? null : Convert.ToDateTime(dr["FechaCompromiSo"]);
                        objProyectosPendientes.FechaFin = DBNull.Value.Equals(dr["FechaFin"]) ? null : Convert.ToDateTime(dr["FechaFin"]);

                        objProyectosPendientes.Solicitud = (dr["Solicitud"]).ToString();

                        objProyectosPendientes.Usuario_Solicita = (dr["Solicito"]).ToString();
                        objProyectosPendientes.LiderdelProyecto = (dr["LiderdeProyecto"]).ToString();

                        objProyectosPendientes.IdEstatus = Convert.ToInt32(dr["IdEstatus"].ToString());

                        lstProyectosPendientes.Add(objProyectosPendientes);
                    }


                }
                else
                {
                    lstProyectosPendientes.Clear();
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
            return lstProyectosPendientes;
        }


        public List<DropDownListDatos> TipodeSolicitud()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (con = new(sConexion))
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
        public List<UsuarioInvolucrados> CatalogoUsuariosProyectos(int idEmpresa)
        {
            List<UsuarioInvolucrados> usuarios = new List<UsuarioInvolucrados>();
            try
            {
                using (SqlConnection con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGO_INVOLUCRADOS_PROYECTOS", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEmpresa", idEmpresa);
                    using SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            usuarios.Add(new UsuarioInvolucrados()
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Nombre = reader["Nombre"].ToString(),
                                IdDatoEmpresa = Convert.ToInt32(reader["IDDatosDeEmpresa"]),
                                NombreDepartamento = reader["NombreDepartamento"].ToString()
                            });
                        }
                    }

                            
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return usuarios;
        }
        public string GetFile(string name)
        {
            string UrlS3 = string.Empty;
            string filename = string.Empty;
            switch (name)
            {
                case "FDO-01":
                    filename = "Formatos/(FDO-01) Solicitud de proyecto.doc";
                    break;
                case "FDO-02":
                    filename = "Formatos/(FDO-02)Solicitud de Pool y Check Point.doc";
                    break;
                default:
                    return "Not Found";
            }
            UrlS3 = _bucketRepo.URL(filename, "grupoei.proyectos");
            return UrlS3;
        }

        public static DataTable ConvertListToDataTable(List<int> lista)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            foreach (var item in lista)
            {
                table.Rows.Add(item);
            }
            return table;
        }

        public List<DropDownListDatosColor> CargarEstatus()
        {
            List<DropDownListDatosColor> comboList = new();
            try
            {
                using (con = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_ESTATUSPROYECTO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    using SqlDataReader reader = cmd.ExecuteReader();


                    comboList = SqlDataReaderToDropDownList.DropDownListColor<DropDownListDatosColor>(reader);
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
