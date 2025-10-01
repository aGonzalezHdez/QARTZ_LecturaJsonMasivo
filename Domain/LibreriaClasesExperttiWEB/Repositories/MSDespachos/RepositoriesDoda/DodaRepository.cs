using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesDoda
{
    public class DodaRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public DodaRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public Doda Buscar(int idDODA)
        {
            Doda objDoda = new Doda();
            using (SqlConnection cn = new SqlConnection(SConexion))
            using (SqlCommand cmd = new SqlCommand("NET_SEARCH_DODA_WEB", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@IDDODA", SqlDbType.Int, 4).Value = idDODA;

                try
                {
                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objDoda.IdDODA = Convert.ToInt32(dr["IdDODA"]);
                            objDoda.N_Ticket = (dr["N_Ticket"]).ToString();
                            objDoda.N_Integracion = (dr["N_Integracion"]).ToString();
                            objDoda.Patente = (dr["Patente"]).ToString();
                            objDoda.FechaEmision = Convert.ToDateTime(dr["FechaEmision"]);
                            objDoda.N_Pedimentos = Convert.ToInt32(dr["N_Pedimentos"]);
                            objDoda.NAduana = Convert.ToInt32(dr["NAduana"]);
                            objDoda.NoAduana = (dr["NoAduana"]).ToString();
                            objDoda.CadenaOriginal = (dr["CadenaOriginal"]).ToString();
                            objDoda.NSerieCertificado = (dr["NSerieCertificado"]).ToString();
                            objDoda.SelloDigital = (dr["SelloDigital"]).ToString();
                            objDoda.NSerieCertificadoSAT = Convert.ToString(dr["NSerieCertificadoSAT"]);
                            objDoda.SelloDigitalSAT = (dr["SelloDigitalSAT"]).ToString();
                            objDoda.CadenaOriginalSAT = (dr["CadenaOriginalSAT"]).ToString();
                            objDoda.UsuarioCiec = (dr["UsuarioCiec"]).ToString();
                            objDoda.Link = (dr["Link"]).ToString();
                            objDoda.RutaArchivo = (dr["RutaArchivo"]).ToString();
                            objDoda.DespachoAduanero = Convert.ToInt32(dr["DespachoAduanero"]);
                            objDoda.NumeroGafeteUnico = (dr["NumeroGafeteUnico"]).ToString();
                            objDoda.ACTIVO = Convert.ToBoolean(dr["ACTIVO"]);
                            objDoda.ERRORES = Convert.ToBoolean(dr["ERRORES"]);
                            objDoda.DESCRIPCIONERROR = (dr["DESCRIPCIONERROR"]).ToString();
                            objDoda.Placas = (dr["Placas"]).ToString();
                            objDoda.CAAT = (dr["CAAT"]).ToString();
                            objDoda.Seccion = (dr["Seccion"]).ToString();
                            objDoda.IdPredoda = Convert.ToInt32(dr["IdPredoda"]);
                            objDoda.NumerodeTag = (dr["NumerodeTag"]).ToString();
                            objDoda.tipo_documento_id = Convert.ToInt32(dr["tipo_documento_id"]);
                            objDoda.Fast_Id = (dr["Fast_Id"]).ToString();
                            objDoda.datos_adicionales = (dr["datos_adicionales"]).ToString();
                            objDoda.ModalidadCruce = Convert.ToInt32(dr["ModalidadCruce"]);
                            objDoda.Candados = (dr["Candados"]).ToString();
                            objDoda.AVC = (dr["AVC"]).ToString();
                            objDoda.FechaVigencia = Convert.ToDateTime(dr["FechaVigencia"]);
                            objDoda.ValidacionAgencia = (dr["ValidacionAgencia"]).ToString();
                            objDoda.Operacion = Convert.ToInt32(dr["Operacion"]);
                            objDoda.ModulacionAVC = Convert.ToBoolean(dr["ModulacionAVC"]);
                            objDoda.IDCHECKPOINT = Convert.ToInt32(dr["IDCHECKPOINT"]);
                            objDoda.FECHADESPACHO = Convert.ToDateTime(dr["FECHADESPACHO"]);
                            
                           



                        }
                        else
                        {
                            objDoda = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objDoda;
        }

        public bool  DODA_Modulado(int idDoda, int IdCheckPoint, DateTime FechaDespacho)
        {
            bool modifico=false;
            SqlConnection cn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;

            cn.ConnectionString = SConexion;
            cn.Open();
            cmd.CommandText = "NET_UPDATE_CASAEI_DODA_MODULADO";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;


            // @idPreDoda INT,
            param = cmd.Parameters.Add("@idDoda", SqlDbType.Int, 4);
            param.Value = idDoda;

            param = cmd.Parameters.Add("@IDCHECKPOINT", SqlDbType.Int, 4);
            param.Value = IdCheckPoint;

            param = cmd.Parameters.Add("@FECHADESPACHO", SqlDbType.DateTime);
            param.Value = FechaDespacho;


            param = cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4);
            param.Direction = ParameterDirection.Output;


            try
            {
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                modifico= true;
            }
            catch (Exception ex)
            {
                modifico = false;
                cn.Close();
                // SqlConnection.ClearPool(cn)
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + "NET_UPDATE_CASAEI_DODA_AVC_MODULADO");
            }
            cn.Close();
            // SqlConnection.ClearPool(cn)
            cn.Dispose();
            return modifico;
        }


    }
}
