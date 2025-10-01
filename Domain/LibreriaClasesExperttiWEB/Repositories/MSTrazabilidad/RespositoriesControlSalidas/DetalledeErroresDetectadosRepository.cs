using LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class DetalledeErroresDetectadosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public DetalledeErroresDetectadosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(List<DetalledeErroresDetectados> lstDetalledeErroresDetectados)
        {
            int id = 0;
            int IdReferencia = 0;

            try
            {

                //Elimina IdReferencia               
                foreach (DetalledeErroresDetectados objDetalledeErroresDetectados in lstDetalledeErroresDetectados)
                {
                    if (lstDetalledeErroresDetectados.Count >= 1)
                    {
                        IdReferencia = objDetalledeErroresDetectados.IdReferencia;
                        break;
                    }
                }

                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_INSERT_CASAEI_DetalledeErroresDetectados", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    Eliminar(IdReferencia);

                    foreach (DetalledeErroresDetectados objDetalledeErroresDetectados in lstDetalledeErroresDetectados)
                    {
                        cmd.Parameters.Add("@idError", SqlDbType.Int, 4).Value = objDetalledeErroresDetectados.idError;
                        cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4).Value = objDetalledeErroresDetectados.IdReferencia;
                        cmd.Parameters.Add("@idUsuarioDetecta", SqlDbType.Int, 4).Value = objDetalledeErroresDetectados.idUsuarioDetecta;

                        cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        cmd.Parameters.Clear();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

        public int Eliminar(int idReferencia)
        {
            int id;

            try
            {
                using (con = new(SConexion))
                using (var cmd = new SqlCommand("NET_DELETE_CASAEI_DetalledeErroresDetectados", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4).Value = idReferencia;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
            return id;
        }

    }
}
