using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesOperaciones
{
    public class EnvioEnTransitoRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public EnvioEnTransitoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DetalleDeRelacionEnTransito Buscar(string GuiaHouse)
        {
            var objDetalleDeRelacionEnTransito = new DetalleDeRelacionEnTransito();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            try
            {

                cmd.CommandText = "NET_SEARCH_CASAEI_DetalleDeRelacionEnTransito";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                @param = cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25);
                @param.Value = GuiaHouse;
                // Insertar Parametro de busqueda

                cn.ConnectionString = SConexion;
                cn.Open();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objDetalleDeRelacionEnTransito.IdDetalleR = Convert.ToInt32(dr["IdDetalleR"]);
                    objDetalleDeRelacionEnTransito.IdRelacionT = Convert.ToInt32(dr["IdRelacionT"]);
                    objDetalleDeRelacionEnTransito.GuiaHouse = dr["GuiaHouse"].ToString();
                    objDetalleDeRelacionEnTransito.PieceID = dr["PieceID"].ToString();
                    objDetalleDeRelacionEnTransito.FechaDeScaneo = Convert.ToDateTime(dr["FechaDeScaneo"]);
                    objDetalleDeRelacionEnTransito.IDUsuarioScaneaSalida = Convert.ToInt32(dr["IDUsuarioScaneaSalida"]);
                    objDetalleDeRelacionEnTransito.LlegadaAduanaDespacho = Convert.ToDateTime(dr["LlegadaAduanaDespacho"]);
                    objDetalleDeRelacionEnTransito.IdUsuarioScaneaLlegada = Convert.ToInt32(dr["IdUsuarioScaneaLlegada"]);
                    objDetalleDeRelacionEnTransito.IdOficinaLlegada = dr["IdOficinaLlegada"] == null ? 0 : Convert.ToInt32(dr["IdOficinaLlegada"]);
                    objDetalleDeRelacionEnTransito.IdOficinaSalida = dr["IdOficinaSalida"] == null ? 0 : Convert.ToInt32(dr["IdOficinaSalida"]);
                }


                else
                {
                    objDetalleDeRelacionEnTransito = default;
                }
                dr.Close();
                cn.Close();
                cn.Dispose();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return objDetalleDeRelacionEnTransito;
        }

    }
}
