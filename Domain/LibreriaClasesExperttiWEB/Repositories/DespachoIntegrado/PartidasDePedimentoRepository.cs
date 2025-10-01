using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado
{
    public class PartidasDePedimentoRepository
    {
        IConfiguration _configuration;
        public string SConexion { get; set; }
        public PartidasDePedimentoRepository(IConfiguration configuracion)
        {
            _configuration = configuracion;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public DataTable CargarFraccionesCPorteFx(int IdReferencia)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            var dtb = new DataTable();
            var dap = new SqlDataAdapter();
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_LOAD_CARTA_PORTE_FEDEX_IDREFERENCIA";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDREFERENCIA", IdReferencia);

                dap.SelectCommand = cmd;
                dap.Fill(dtb);
                    
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                cn.Dispose();
                throw new Exception(ex.Message.ToString() + " NET_LOAD_CARTA_PORTE_FEDEX_IDREFERENCIA");

            }
            cn.Close();
            cn.Dispose();
            return dtb;
        }
    }
}
