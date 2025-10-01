using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones
{
    public class GruposdeTrabajoRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public GruposdeTrabajoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public GruposdeTrabajo Buscar(int IDGrupo)
        {

            var objGRUPOSDETRABAJO = new GruposdeTrabajo();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_GRUPOSDETRABAJO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("IDGrupo", SqlDbType.Int, 4);
            @param.Value = IDGrupo;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    objGRUPOSDETRABAJO.IdGrupo = Convert.ToInt32(dr["IDGrupo"]);
                    objGRUPOSDETRABAJO.Nombre = dr["Nombre"].ToString();
                    objGRUPOSDETRABAJO.IdDepartamento = Convert.ToInt32(dr["IDDepartamento"]);
                    objGRUPOSDETRABAJO.Estatus = Convert.ToInt32(dr["Estatus"]);
                    objGRUPOSDETRABAJO.UsuarioCASA = dr["UsuarioCASA"].ToString();
                    objGRUPOSDETRABAJO.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                }
                else
                {
                    objGRUPOSDETRABAJO = default;
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

            return objGRUPOSDETRABAJO;
        }

        /// <summary>
        /// BUSCAR GRUPO POR IDUSUARIO
        /// </summary>
        /// <param name="IDUsuario">IDUSUARIO</param>
        /// <param name="MyConnectionString">CADENA DE CONEXION</param>
        /// <returns>IDGRUPO</returns>
        /// <remarks>NET_SEARCH_MIEMBROSDEGRUPOPORIDUSUARIO</remarks>
        public int BuscarporGrupo(int IDUsuario)
        {

            int IdGrupo;

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_MIEMBROSDEGRUPOPORIDUSUARIO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("IDUsuario", SqlDbType.Int, 4);
            @param.Value = IDUsuario;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    IdGrupo = Convert.ToInt32(dr["IDGrupo"]);
                }

                else
                {
                    IdGrupo = default;
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

            return IdGrupo;
        }

        public GruposdeTrabajo BuscarTop(int IdOficina, int IdDepartamento)
        {

            var objGRUPOSDETRABAJO = new GruposdeTrabajo();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = sConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_GRUPOSDETRABAJO_TOP";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
            @param.Value = IdOficina;

            @param = cmd.Parameters.Add("@IdDepartamento", SqlDbType.Int, 4);
            @param.Value = IdDepartamento;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objGRUPOSDETRABAJO.IdGrupo = Convert.ToInt32(dr["IDGrupo"]);
                    objGRUPOSDETRABAJO.Nombre = dr["Nombre"].ToString();
                    objGRUPOSDETRABAJO.IdDepartamento = Convert.ToInt32(dr["IDDepartamento"]);
                    objGRUPOSDETRABAJO.Estatus = Convert.ToInt32(dr["Estatus"]);
                    objGRUPOSDETRABAJO.UsuarioCASA = dr["UsuarioCASA"].ToString();
                    objGRUPOSDETRABAJO.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                }
                else
                {
                    objGRUPOSDETRABAJO = default;
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

            return objGRUPOSDETRABAJO;
        }
        public List<GruposdeTrabajo> Cargar(int idOficina)
        {

            var lstGruposdeTrabajo = new List<GruposdeTrabajo>();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;
            SqlParameter @param;

            try
            {
                cn.ConnectionString = sConexion;
                cn.Open();

                cmd.CommandText = "NET_LOAD_GRUPOSDETRABAJO_POR_OFICINA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @IDOficina INT 
                @param = cmd.Parameters.Add("@IDOficina", SqlDbType.Int, 4);
                @param.Value = idOficina;

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        var objGruposdeTrabajo = new GruposdeTrabajo();

                        objGruposdeTrabajo.IdGrupo = Convert.ToInt32(dr["IDGrupo"]);
                        objGruposdeTrabajo.Nombre = dr["Nombre"].ToString();
                        objGruposdeTrabajo.IdDepartamento = Convert.ToInt32(dr["IDDepartamento"]);
                        objGruposdeTrabajo.Estatus = Convert.ToInt32(dr["Estatus"]);

                        lstGruposdeTrabajo.Add(objGruposdeTrabajo);
                    }
                }

                else
                {
                    lstGruposdeTrabajo = null;
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

            return lstGruposdeTrabajo;
        }

        /// <summary>
        /// Buscar
        /// </summary>
        /// <param name="IDGrupo">Id Grupo</param>
        /// <param name="IdOficina">Id Oficina</param>
        /// <param name="IdDepartamento">Id Departamento</param>
        /// <param name="MyConnectionString">Cadena de conexion</param>
        /// <returns>Grupo de trabajo</returns>
        /// <remarks>NET_SEARCH_GRUPOSDETRABAJOPOROFNADEPTO</remarks>
        public GruposdeTrabajo BuscarporGrupoOfnaDepto(int IDGrupo, int IdOficina, int IdDepartamento, string MyConnectionString)
        {

            var objGRUPOSDETRABAJO = new GruposdeTrabajo();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = MyConnectionString;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_GRUPOSDETRABAJOPOROFNADEPTO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IDGrupo", SqlDbType.Int, 4);
            @param.Value = IDGrupo;

            @param = cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4);
            @param.Value = IdOficina;

            @param = cmd.Parameters.Add("@IDDepartamento", SqlDbType.Int, 4);
            @param.Value = IdDepartamento;
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objGRUPOSDETRABAJO.IdGrupo = Convert.ToInt32(dr["IDGrupo"]);
                    objGRUPOSDETRABAJO.Nombre = dr["Nombre"].ToString();
                    objGRUPOSDETRABAJO.IdDepartamento = Convert.ToInt32(dr["IDDepartamento"]);
                    objGRUPOSDETRABAJO.Estatus = Convert.ToInt32(dr["Estatus"]);
                    objGRUPOSDETRABAJO.UsuarioCASA = dr["UsuarioCASA"].ToString();
                    objGRUPOSDETRABAJO.IdOficina = Convert.ToInt32(dr["IdOficina"]);
                }
                else
                {
                    objGRUPOSDETRABAJO = default;
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

            return objGRUPOSDETRABAJO;
        }
    }
}
