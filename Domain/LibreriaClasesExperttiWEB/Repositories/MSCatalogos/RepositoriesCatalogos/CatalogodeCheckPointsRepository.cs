using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using LibreriaClasesAPIExpertti.Utilities.Helper;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogodeCheckPointsRepository: ICatalogodeCheckPointsRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public CatalogodeCheckPointsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<int> Insertar(CatalogodeCheckPoints objCatalogodeCheckPoints)
        {
            int id;
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_INSERT_CATALOGODECHECKPOINTS", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCheckPoint", SqlDbType.Int, 4).Value = objCatalogodeCheckPoints.IDCheckPoint;
                    cmd.Parameters.Add("@ClaveCheckPoint", SqlDbType.Char, 4).Value = objCatalogodeCheckPoints.ClaveCheckPoint;
                    cmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 150).Value = objCatalogodeCheckPoints.Descripcion;
                    cmd.Parameters.Add("@Obligatorio", SqlDbType.Bit, 4).Value = objCatalogodeCheckPoints.Obligatorio;
                    cmd.Parameters.Add("@Duplicidad", SqlDbType.Bit, 4).Value = objCatalogodeCheckPoints.Duplicidad;
                    cmd.Parameters.Add("@ListaDeIDPrecedencias", SqlDbType.VarChar, 50).Value = objCatalogodeCheckPoints.ListaDeIDPrecedencias;
                    cmd.Parameters.Add("@PrecedenciaObligatoria", SqlDbType.Bit, 4).Value = objCatalogodeCheckPoints.PrecedenciaObligatoria;
                    cmd.Parameters.Add("@Automatico", SqlDbType.Bit, 4).Value = objCatalogodeCheckPoints.Automatico;
                    cmd.Parameters.Add("@IDDepartamento", SqlDbType.Int, 4).Value = objCatalogodeCheckPoints.IDDepartamento;
                    cmd.Parameters.Add("@Idoficina", SqlDbType.Int, 4).Value = objCatalogodeCheckPoints.Idoficina;
                    cmd.Parameters.Add("@TipoDeEvento", SqlDbType.Int, 4).Value = objCatalogodeCheckPoints.TipoDeEvento;
                    cmd.Parameters.Add("@StatusWeb", SqlDbType.Bit, 4).Value = objCatalogodeCheckPoints.StatusWeb;
                    cmd.Parameters.Add("@OrdenDeDespliegue", SqlDbType.Int, 4).Value = objCatalogodeCheckPoints.OrdenDeDespliegue;
                    cmd.Parameters.Add("@Activo", SqlDbType.Bit, 4).Value = objCatalogodeCheckPoints.Activo;
                    cmd.Parameters.Add("@MostrarEnRegistros", SqlDbType.Bit, 4).Value = objCatalogodeCheckPoints.MostrarEnRegistros;
                    cmd.Parameters.Add("@ObservacionObligatoria", SqlDbType.Bit, 4).Value = objCatalogodeCheckPoints.ObservacionObligatoria;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                    using (await cmd.ExecuteReaderAsync())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                        else
                        {
                            id = 0;
                        }
                    }



                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString() + "NET_INSERT_CATALOGODECHECKPOINTS");
            }
            return id;
        }
        public CatalogodeCheckPoints BuscarId(int IDCheckPoint, int Idoficina, int IdReferencia)
        {

            var objCATALOGODECHECKPOINTS = new CatalogodeCheckPoints();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            // cmd.CommandText = "NET_SEARCH_CATALOGODECHECKPOINTS_Id"
            cmd.CommandText = "NET_SEARCH_CATALOGODECHECKPOINTSNEW_Id";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            // @IDCheckPoint INT
            param = cmd.Parameters.Add("@IDCheckPoint", SqlDbType.Int, 4);
            param.Value = IDCheckPoint;

            // @Idoficina  INT
            param = cmd.Parameters.Add("@Idoficina", SqlDbType.Int, 4);
            param.Value = Idoficina;

            param = cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4);
            param.Value = IdReferencia;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    Helper helper = new Helper();

                    dr.Read();
                    objCATALOGODECHECKPOINTS.IDCheckPoint = Convert.ToInt32(dr["IDCheckPoint"]);
                    objCATALOGODECHECKPOINTS.ClaveCheckPoint = dr["ClaveCheckPoint"].ToString();
                    objCATALOGODECHECKPOINTS.Descripcion = dr["Descripcion"].ToString();
                    objCATALOGODECHECKPOINTS.Obligatorio = Convert.ToBoolean(dr["Obligatorio"]);
                    objCATALOGODECHECKPOINTS.Duplicidad = Convert.ToBoolean(dr["Duplicidad"]);
                    objCATALOGODECHECKPOINTS.ListaDeIDPrecedencias = dr["ListaDeIDPrecedencias"].ToString();
                    objCATALOGODECHECKPOINTS.PrecedenciaObligatoria = Convert.ToBoolean(dr["PrecedenciaObligatoria"]);
                    objCATALOGODECHECKPOINTS.Automatico = Convert.ToBoolean(dr["Automatico"]);
                    objCATALOGODECHECKPOINTS.IDDepartamento = Convert.ToInt32(dr["IDDepartamento"]);
                    objCATALOGODECHECKPOINTS.Idoficina = Convert.ToInt32(dr["Idoficina"]);
                    objCATALOGODECHECKPOINTS.TipoDeEvento = Convert.ToInt32(dr["TipoDeEvento"]);
                    objCATALOGODECHECKPOINTS.StatusWeb = Convert.ToBoolean(dr["StatusWeb"]);
                    objCATALOGODECHECKPOINTS.OrdenDeDespliegue = Convert.ToInt32(dr["OrdenDeDespliegue"]);
                    objCATALOGODECHECKPOINTS.Activo = Convert.ToBoolean(dr["Activo"]);
                    objCATALOGODECHECKPOINTS.MostrarEnRegistros = Convert.ToBoolean(dr["MostrarEnRegistros"]);
                    objCATALOGODECHECKPOINTS.ObservacionObligatoria = Convert.ToBoolean(dr["ObservacionObligatoria"]);
                    objCATALOGODECHECKPOINTS.ListaDeIDPrecedencias1 = helper.Parsear(dr["ListaDeIDPrecedencias"].ToString());
                    objCATALOGODECHECKPOINTS.IdDepartamentoDestino = Convert.ToInt32(dr["IdDepartamentoDestino"]);
                    objCATALOGODECHECKPOINTS.ValidaPedimento = Convert.ToBoolean(dr["ValidaPedimento"]);
                    objCATALOGODECHECKPOINTS.ValidaProforma = Convert.ToBoolean(dr["ValidaProforma"]);
                    objCATALOGODECHECKPOINTS.Prevalida = Convert.ToBoolean(dr["Prevalida"]);
                    objCATALOGODECHECKPOINTS.PreValidaCove = Convert.ToBoolean(dr["PreValidaCove"]);
                    objCATALOGODECHECKPOINTS.ValidaPartidas = Convert.ToBoolean(dr["ValidaPartidas"]);
                    objCATALOGODECHECKPOINTS.AsignarPedimento = Convert.ToBoolean(dr["AsignarPedimento"]);
                    objCATALOGODECHECKPOINTS.AsignacionAutomatica = Convert.ToBoolean(dr["AsignacionAutomatica"]);
                    objCATALOGODECHECKPOINTS.Reproceso = Convert.ToBoolean(dr["Reproceso"]);
                    objCATALOGODECHECKPOINTS.DepOrigen = Convert.ToBoolean(dr["DepOrigen"]);
                }

                else
                {
                    objCATALOGODECHECKPOINTS = default;
                }
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

            return objCATALOGODECHECKPOINTS;
        }
        public CatalogodeCheckPoints Buscar(int IDCheckPoint, int Idoficina)
        {
            CatalogodeCheckPoints objCATALOGODECHECKPOINTS = new();

            try
            {

                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODECHECKPOINTSNEW_Id", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCheckPoint", SqlDbType.Int, 4).Value = IDCheckPoint;
                    cmd.Parameters.Add("@Idoficina", SqlDbType.Int, 4).Value = Idoficina;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCATALOGODECHECKPOINTS.IDCheckPoint = Convert.ToInt32(dr["IDCheckPoint"]);
                            objCATALOGODECHECKPOINTS.ClaveCheckPoint = string.Format("{0}", dr["ClaveCheckPoint"]);
                            objCATALOGODECHECKPOINTS.Descripcion = string.Format("{0}", dr["Descripcion"]);
                            objCATALOGODECHECKPOINTS.Obligatorio = Convert.ToBoolean(dr["Obligatorio"]);
                            objCATALOGODECHECKPOINTS.Duplicidad = Convert.ToBoolean(dr["Duplicidad"]);
                            objCATALOGODECHECKPOINTS.ListaDeIDPrecedencias = string.Format("{0}", dr["ListaDeIDPrecedencias"]);
                            objCATALOGODECHECKPOINTS.PrecedenciaObligatoria = Convert.ToBoolean(dr["PrecedenciaObligatoria"]);
                            objCATALOGODECHECKPOINTS.Automatico = Convert.ToBoolean(dr["Automatico"]);
                            objCATALOGODECHECKPOINTS.IDDepartamento = Convert.ToInt32(dr["IDDepartamento"]);
                            objCATALOGODECHECKPOINTS.Idoficina = Convert.ToInt32(dr["Idoficina"]);
                            objCATALOGODECHECKPOINTS.TipoDeEvento = Convert.ToInt32(dr["TipoDeEvento"]);
                            objCATALOGODECHECKPOINTS.StatusWeb = Convert.ToBoolean(dr["StatusWeb"]);
                            objCATALOGODECHECKPOINTS.OrdenDeDespliegue = Convert.ToInt32(dr["OrdenDeDespliegue"]);
                            objCATALOGODECHECKPOINTS.Activo = Convert.ToBoolean(dr["Activo"]);
                            objCATALOGODECHECKPOINTS.MostrarEnRegistros = Convert.ToBoolean(dr["MostrarEnRegistros"]);
                            objCATALOGODECHECKPOINTS.ObservacionObligatoria = Convert.ToBoolean(dr["ObservacionObligatoria"]);
                            //objCATALOGODECHECKPOINTS.ListaDeIDPrecedencias1 = dr["ListaDeIDPrecedencias"]).ConvertAll(int.Parse);
                            objCATALOGODECHECKPOINTS.IdDepartamentoDestino = Convert.ToInt32(dr["IdDepartamentoDestino"]);
                            objCATALOGODECHECKPOINTS.ValidaPedimento = Convert.ToBoolean(dr["ValidaPedimento"]);
                            objCATALOGODECHECKPOINTS.ValidaProforma = Convert.ToBoolean(dr["ValidaProforma"]);
                            objCATALOGODECHECKPOINTS.Prevalida = Convert.ToBoolean(dr["Prevalida"]);
                            objCATALOGODECHECKPOINTS.PreValidaCove = Convert.ToBoolean(dr["PreValidaCove"]);
                            objCATALOGODECHECKPOINTS.ValidaPartidas = Convert.ToBoolean(dr["ValidaPartidas"]);
                            objCATALOGODECHECKPOINTS.AsignarPedimento = Convert.ToBoolean(dr["AsignarPedimento"]);
                            objCATALOGODECHECKPOINTS.AsignacionAutomatica = Convert.ToBoolean(dr["AsignacionAutomatica"]);
                            objCATALOGODECHECKPOINTS.Reproceso = Convert.ToBoolean(dr["Reproceso"]);
                            objCATALOGODECHECKPOINTS.DepOrigen = Convert.ToBoolean(dr["DepOrigen"]);
                            objCATALOGODECHECKPOINTS.CrearRef = Convert.ToBoolean(dr["CrearRef"]);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCATALOGODECHECKPOINTS;
        }

        public async Task<CatalogodeCheckPoints> BuscarPorDepto(int IDCheckPoint, int Idoficina, int IdDepartamento, int idReferencia)
        {
            CatalogodeCheckPoints objCATALOGODECHECKPOINTS = new();


            try
            {
                using (cn = new SqlConnection(sConexion))
                using (SqlCommand cmd = new("NET_SEARCH_CATALOGODECHECKPOINTSNEW_ID_DEPTO", cn))
                {
                    cn.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDCheckPoint", SqlDbType.Int, 4).Value = IDCheckPoint;
                    cmd.Parameters.Add("@Idoficina", SqlDbType.Int, 4).Value = Idoficina;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = IdDepartamento;
                    cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4).Value = idReferencia;

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCATALOGODECHECKPOINTS.IDCheckPoint = Convert.ToInt32(dr["IDCheckPoint"]);
                            objCATALOGODECHECKPOINTS.ClaveCheckPoint = string.Format("{0}", dr["ClaveCheckPoint"]);
                            objCATALOGODECHECKPOINTS.Descripcion = string.Format("{0}", dr["Descripcion"]);
                            objCATALOGODECHECKPOINTS.Obligatorio = Convert.ToBoolean(dr["Obligatorio"]);
                            objCATALOGODECHECKPOINTS.Duplicidad = Convert.ToBoolean(dr["Duplicidad"]);
                            objCATALOGODECHECKPOINTS.ListaDeIDPrecedencias = string.Format("{0}", dr["ListaDeIDPrecedencias"]);
                            objCATALOGODECHECKPOINTS.PrecedenciaObligatoria = Convert.ToBoolean(dr["PrecedenciaObligatoria"]);
                            objCATALOGODECHECKPOINTS.Automatico = Convert.ToBoolean(dr["Automatico"]);
                            objCATALOGODECHECKPOINTS.IDDepartamento = Convert.ToInt32(dr["IDDepartamento"]);
                            objCATALOGODECHECKPOINTS.Idoficina = Convert.ToInt32(dr["Idoficina"]);
                            objCATALOGODECHECKPOINTS.TipoDeEvento = Convert.ToInt32(dr["TipoDeEvento"]);
                            objCATALOGODECHECKPOINTS.StatusWeb = Convert.ToBoolean(dr["StatusWeb"]);
                            objCATALOGODECHECKPOINTS.OrdenDeDespliegue = Convert.ToInt32(dr["OrdenDeDespliegue"]);
                            objCATALOGODECHECKPOINTS.Activo = Convert.ToBoolean(dr["Activo"]);
                            objCATALOGODECHECKPOINTS.MostrarEnRegistros = Convert.ToBoolean(dr["MostrarEnRegistros"]);
                            objCATALOGODECHECKPOINTS.ObservacionObligatoria = Convert.ToBoolean(dr["ObservacionObligatoria"]);
                            //objCATALOGODECHECKPOINTS.ListaDeIDPrecedencias1 = string.Format("{0}", dr["ListaDeIDPrecedencias"]);
                            objCATALOGODECHECKPOINTS.IdDepartamentoDestino = Convert.ToInt32(dr["IdDepartamentoDestino"]);
                            objCATALOGODECHECKPOINTS.ValidaPedimento = Convert.ToBoolean(dr["ValidaPedimento"]);
                            objCATALOGODECHECKPOINTS.ValidaProforma = Convert.ToBoolean(dr["ValidaProforma"]);
                            objCATALOGODECHECKPOINTS.Prevalida = Convert.ToBoolean(dr["Prevalida"]);
                            objCATALOGODECHECKPOINTS.PreValidaCove = Convert.ToBoolean(dr["PreValidaCove"]);
                            objCATALOGODECHECKPOINTS.ValidaPartidas = Convert.ToBoolean(dr["ValidaPartidas"]);
                            objCATALOGODECHECKPOINTS.AsignarPedimento = Convert.ToBoolean(dr["AsignarPedimento"]);
                            objCATALOGODECHECKPOINTS.AsignacionAutomatica = Convert.ToBoolean(dr["AsignacionAutomatica"]);
                            objCATALOGODECHECKPOINTS.DepOrigen = Convert.ToBoolean(dr["DepOrigen"]);
                        }
                        else
                            objCATALOGODECHECKPOINTS = null;

                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objCATALOGODECHECKPOINTS;
        }

        public List<DropDownListDatos> CargarEventosdeSalida(int IdDepartamento, int IdOficina, int Operacion, int IdCliente)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODECHECKPOINTSNEW_SALIDA_OPERACION", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = IdDepartamento;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = Operacion;
                    cmd.Parameters.Add("@IdCliente", SqlDbType.Int,4).Value= IdCliente;


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

        public List<DropDownListDatos> CargarEventosdeWECWEB(int IdDepartamento, int IdOficina)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODECHECKPOINTSWEB_WEC_WEB", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = IdDepartamento;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
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

        public List<DropDownListDatos> CargarEventosdeSalidaDetenidos(int IdDepartamento, int IdOficina, int Operacion)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODECHECKPOINTSNEW_SALIDA_OPERACION_DETENIDAS", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = IdDepartamento;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = Operacion;
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
        public List<DropDownListDatos> CargarRectificacion()
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                DropDownListDatos item = new DropDownListDatos();
                item.Id = 232;
                item.Descripcion = "SALIDAS TURNA GUIA A RECTIFICACION";
                comboList.Add(item);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return comboList;
        }

        public List<DropDownListDatos> CargarEventosdeSalidaDetenidosWEB(int IdDepartamento, int IdOficina, int Operacion)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODECHECKPOINTSNEW_SALIDA_OPERACION_DETENIDAS_NOTI_WEB", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = IdDepartamento;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = Operacion;
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


        public List<DropDownListDatos> CargarEventosdeSalidaRegreso(int IdDepartamento, int IdOficina, int Operacion)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODECHECKPOINTSNEW_SALIDA_OPERACION_REGRESO", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = IdDepartamento;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = Operacion;
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

        public List<DropDownListDatos> CargarEventosdeSalidaRegresoWEB(int IdDepartamento, int IdOficina, int Operacion)
        {
            List<DropDownListDatos> comboList = new();
            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_LOAD_CATALOGODECHECKPOINTSNEW_SALIDA_OPERACION_REGRESO_NOTI_WEB", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4).Value = IdDepartamento;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = IdOficina;
                    cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4).Value = Operacion;
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
