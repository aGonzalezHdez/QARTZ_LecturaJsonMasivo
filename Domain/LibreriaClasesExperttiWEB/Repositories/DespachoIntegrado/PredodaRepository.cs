using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado.PredodaDHL;
using LibreriaClasesAPIExpertti.Entities.EntitiesTransCar;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using NPOI.OpenXmlFormats.Dml;
using System.Data;
using System.Data.SqlClient;


namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class PredodaRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public ControldeEventosRepository eventosRepository;

        public PredodaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public Predoda Insertar(Predoda predoda)
        {
            int id;
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;


            try
            {

                cn.ConnectionString = SConexion;
                cn.Open();

                cmd.CommandText = "NET_INSERT_CASAEI_PreDODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Aduana",predoda.Aduana);
                cmd.Parameters.AddWithValue("@Patente",predoda.Patente);
                cmd.Parameters.AddWithValue("@Operacion",predoda.Operacion);
                cmd.Parameters.AddWithValue("@Origen",predoda.Origen);
                cmd.Parameters.AddWithValue("@Placas",predoda.Placas);
                cmd.Parameters.AddWithValue("@NumeroGafeteUnico",predoda.NumeroGafeteUnico);
                cmd.Parameters.AddWithValue("@CAAT",predoda.CAAT);
                cmd.Parameters.AddWithValue("@IdOficina",predoda.IdOficina);
                cmd.Parameters.AddWithValue("@ModalidadCruce",predoda.ModalidadCruce);
                cmd.Parameters.AddWithValue("@Fast_Id",predoda.FastId);
                cmd.Parameters.AddWithValue("@datos_adicionales",predoda.DatosAdicionales);
                cmd.Parameters.AddWithValue("@Candados",predoda.Candados);
                cmd.Parameters.AddWithValue("@NumerodeTag",predoda.NumerodeTag);

                //Salida
                param = cmd.Parameters.Add("@PreDODA", SqlDbType.VarChar, 18);
                param.Direction = ParameterDirection.Output;

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                predoda.IdPredoda = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                predoda.PreDODA = cmd.Parameters["@PreDODA"].Value.ToString();
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_INSERT_CASAEI_PreDODA");
            }
            cn.Close();
            cn.Dispose();
            return predoda;

        }
        public bool Modificar(CFDICartaPorteDHL predoda)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter param;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_UPDATE_CASAEI_PREDODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPreDoda",predoda.preDodaId);
                cmd.Parameters.AddWithValue("@cancelado", predoda.cancelado);
                cmd.Parameters.AddWithValue("@liberado", predoda.liberado);
                cmd.Parameters.AddWithValue("@tipoOperacionId", predoda.tipoOperacionId);
                cmd.Parameters.AddWithValue("@estatusId", predoda.estatusId);
                cmd.Parameters.AddWithValue("@uuid", predoda.uuid);
                cmd.Parameters.AddWithValue("@uuidRelacionado", predoda.uuidRelacionado);
                cmd.Parameters.AddWithValue("@fechaCFDI", DBNull.Value);
                cmd.Parameters.AddWithValue("@endPointPdf", predoda.endPointPdf);
                cmd.Parameters.AddWithValue("@procesoId", predoda.procesoId);

                param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
                param.Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message + " NET_UPDATE_CASAEI_PREDODA");
            }
            cn.Close();
            cn.Dispose();
            return true;
        }
        public Predoda Buscar(int idPredoda)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader reader;
            Predoda item = new Predoda();
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_SEARCH_CASAEI_PreDODA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPreDoda", idPredoda);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    item = new Predoda
                    {
                        IdPredoda = Convert.ToInt32(reader["idPreDoda"]),
                        Aduana = reader["Aduana"].ToString(),
                        Patente = reader["Patente"].ToString(),
                        Operacion = Convert.ToInt32(reader["Operacion"].ToString()),
                        Origen = reader["Origen"].ToString(),
                        Placas = reader["Placas"].ToString(),
                        NumeroGafeteUnico = reader["NumeroGafeteUnico"].ToString(),
                        CAAT = reader["CAAT"].ToString(),
                        Cancelado = Convert.ToBoolean(reader["Cancelado"]),
                        Liberado = Convert.ToBoolean(reader["Liberado"]),
                        TipoOperacionId = reader["tipoOperacionId"].ToString(),
                        EstatudId = reader["estatusId"].ToString(),
                        uuid = reader["uuid"].ToString(),
                        uuidRelacionado = reader["uuidRelacionado"].ToString(),
                        fechaCFDI = Convert.ToDateTime(reader["fechaCFDI"]),
                        endPointPdf = reader["endPointPdf"].ToString(),
                        procesoId = reader["procesoId"].ToString(),
                        fechaSolicitud = Convert.ToDateTime(reader["fechaSolicitud"]),
                        FechaAlta = Convert.ToDateTime(reader["FechaAlta"]),
                        Consecutivo = Convert.ToInt32(reader["Consecutivo"]),
                        PreDODA = reader["PreDODA"].ToString(),
                        IdOficina = Convert.ToInt32(reader["idOficina"]),
                        ModalidadCruce = Convert.ToInt32(reader["ModalidadCruce"]),
                        NumerodeTag = reader["NumerodeTag"].ToString(),
                        tipoDocumentoId = reader["tipo_documento_id"].ToString(),
                        FastId = reader["fast_id"].ToString(),
                        DatosAdicionales = reader["datos_adicionales"].ToString(),
                        Candados = reader["Candados"].ToString(),
                    };
                }
        
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_SEARCH_CASAEI_PreDODA");

            }
            cn.Close();
            cn.Dispose();
            return item;
        }

        public List<PredodaDetalle> DetallePredoda(int idPredoda)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader reader;
            DataTable dtb = new DataTable();
            List<PredodaDetalle> listado = new List<PredodaDetalle>();
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_LOAD_CASAEI_PreDODADetalle";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPreDoda", idPredoda);

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        listado.Add(new PredodaDetalle
                        {
                            idPreDodaDetalle = Convert.ToInt32( reader["idPreDodaDetalle"]),
                            idPreDoda = idPredoda,
                            IdReferencia = Convert.ToInt32(reader["IdReferencia"]),
                            Referencia = reader["Referencia"].ToString(),
                            NumeroDeCOVE = reader["COVE"].ToString(),
                            Pedimento = reader["Pedimento"].ToString(),
                            Remesa = Convert.ToInt32(reader["Remesa"]),
                            RFC = reader["RFC"].ToString()
                        });
                    }
                }

                cn.Close() ;
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_LOAD_CASAEI_PreDODADetalle");
                
            }
            cn.Close();
            cn.Dispose();
            return listado;
        }

        public List<TransCarPredoda> DetallePredodaTransCar(int idPredoda)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader reader;
            DataTable dtb = new DataTable();
            List<TransCarPredoda> listado;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_LOAD_CARTA_PORTE_TransCar";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idPreDoda", idPredoda);

                reader = cmd.ExecuteReader();

                listado = SqlDataReaderToList.DataReaderMapToList<TransCarPredoda>(reader);

                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_LOAD_CARTA_PORTE_TransCar");

            }
            cn.Close();
            cn.Dispose();
            return listado;
        }
    }
}
