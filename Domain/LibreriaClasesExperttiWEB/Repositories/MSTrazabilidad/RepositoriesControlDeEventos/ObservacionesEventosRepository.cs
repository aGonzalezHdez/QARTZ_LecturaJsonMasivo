using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos
{
    public class ObservacionesEventosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public ObservacionesEventosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DataTable CargarDepartamentos(int IdReferencia)
        {
            DataTable dtb = new DataTable();

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter();
                    SqlParameter param;

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD__OBSERVACIONES_DEPARTAMENTOS";

                    param = dap.SelectCommand.Parameters.Add("@idReferencia", SqlDbType.Int);
                    param.Value = IdReferencia;

                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_LOAD__OBSERVACIONES_DEPARTAMENTOS");
                }
            }

            return dtb;
        }
        public DataTable CargarChecks(int IdReferencia, int IdDepartamentoDestino)
        {
            DataTable dtb = new DataTable();

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter();
                    SqlParameter param;

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_OBSERVACIONES_CHECK";

                    param = dap.SelectCommand.Parameters.Add("@idReferencia", SqlDbType.Int);
                    param.Value = IdReferencia;


                    param = dap.SelectCommand.Parameters.Add("@IdDepartamentoDestino", SqlDbType.Int);
                    param.Value = IdDepartamentoDestino;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_LOAD_OBSERVACIONES_CHECK");
                }
            }

            return dtb;
        }

        public DataTable CargarObservacionesCheckpoint(int IdReferencia, int IdDepartamentoDestino, int IDCheckPoint)
        {
            DataTable dtb = new DataTable();

            using (SqlConnection cn = new SqlConnection(SConexion))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter dap = new SqlDataAdapter();
                    SqlParameter param;

                    dap.SelectCommand = new SqlCommand();
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.CommandText = "NET_LOAD_OBSERVACIONES_OBS";

                    param = dap.SelectCommand.Parameters.Add("@idReferencia", SqlDbType.Int);
                    param.Value = IdReferencia;


                    param = dap.SelectCommand.Parameters.Add("@IdDepartamentoDestino", SqlDbType.Int);
                    param.Value = IdDepartamentoDestino;

                    param = dap.SelectCommand.Parameters.Add("@IDCheckPoint", SqlDbType.Int);
                    param.Value = IDCheckPoint;


                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;


                    dap.Fill(dtb);

                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception ex)
                {
                    cn.Close();
                    cn.Dispose();

                    throw new Exception(ex.Message.ToString() + "NET_LOAD_OBSERVACIONES_OBS");
                }
            }

            return dtb;
        }


        public List<CatalogoObservaciones> CargarObservaciones(int IdReferencia)
        {
            List<CatalogoObservaciones> observaciones = new List<CatalogoObservaciones>();
            DataTable departamentos = CargarDepartamentos(IdReferencia);

            foreach (DataRow row in departamentos.Rows)
            {

                CatalogoObservaciones observ = new CatalogoObservaciones();
                observ.Departamento = row["NombreDepartamento"].ToString();
                observ.IdDepartamento = Convert.ToInt32(row["IdDepartamentoDestino"].ToString());
                DataTable checkpoints = CargarChecks(IdReferencia, observ.IdDepartamento);
                List<CatalogodeCheckPoints> Checkpoints = new List<CatalogodeCheckPoints>();
                foreach (DataRow rowCheck in checkpoints.Rows)
                {
                    CatalogodeCheckPoints checkpoint = new CatalogodeCheckPoints();
                    List<ObservacionCheckpoint> observacionCheckpoints = new List<ObservacionCheckpoint>();

                    checkpoint.IDCheckPoint = Convert.ToInt32(rowCheck["IDCHECKPOINT"].ToString());
                    checkpoint.Descripcion = rowCheck["Descripcion"].ToString();

                    DataTable observacionesCheck = CargarObservacionesCheckpoint(IdReferencia, observ.IdDepartamento, checkpoint.IDCheckPoint);
                    foreach (DataRow rowObserv in observacionesCheck.Rows)
                    {
                        ObservacionCheckpoint obs = new ObservacionCheckpoint();
                        obs.Observacion = rowObserv["Observacion"].ToString();
                        observacionCheckpoints.Add(obs);
                    }
                    checkpoint.Observaciones = observacionCheckpoints;
                    Checkpoints.Add(checkpoint);
                }
                observ.CheckPoints = Checkpoints;
                observaciones.Add(observ);
            }
            return observaciones;
        }
    }
}
