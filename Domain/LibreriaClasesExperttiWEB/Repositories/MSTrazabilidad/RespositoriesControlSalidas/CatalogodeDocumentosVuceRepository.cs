using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using System.Data;
using System.Data.SqlClient;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class CatalogodeDocumentosVuceRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public ControldeEventosRepository eventosRepository;

        public CatalogodeDocumentosVuceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public CatalogodeDocumentosVuce BuscarId(int IdDocumento)
        {
            var objCatalogodeDocumentosVuce = new CatalogodeDocumentosVuce();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;


            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_CATALOGODEDOCUMENTOSVUCE_Id";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            @param = cmd.Parameters.Add("@IdDocumento", SqlDbType.Int, 4);
            @param.Value = IdDocumento;
            // Insertar Parametro de busqueda

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCatalogodeDocumentosVuce.IdDocumento = Convert.ToInt32(dr["IdDocumento"]);
                    objCatalogodeDocumentosVuce.NoOficial = Convert.ToInt32(dr["NoOficial"]);
                    objCatalogodeDocumentosVuce.Documento = dr["Documento"].ToString();
                    objCatalogodeDocumentosVuce.ActivoVucem = Convert.ToBoolean(dr["ActivoVucem"]);
                    objCatalogodeDocumentosVuce.Orden = Convert.ToInt32(dr["Orden"]);
                }
                else
                {
                    objCatalogodeDocumentosVuce = default;
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

            return objCatalogodeDocumentosVuce;
        }

    }
}
