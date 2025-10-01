using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesTransCar;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Utilities.Converters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos
{
    public   class CalculodeImpuestosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public ControldeEventosRepository eventosRepository;

        public CalculodeImpuestosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<CalculodeImpuestos> fCalculodeImpuestos(string NumerodeReferencia)
        {
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader reader;
            DataTable dtb = new DataTable();
            List<CalculodeImpuestos> listado;
            try
            {
                cn.ConnectionString = SConexion;
                cn.Open();
                cmd.CommandText = "NET_CALCULO_DE_IMPUESTOS";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NUM_REFE", NumerodeReferencia.Trim());

                reader = cmd.ExecuteReader();

                listado = SqlDataReaderToList.DataReaderMapToList<CalculodeImpuestos>(reader);

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
