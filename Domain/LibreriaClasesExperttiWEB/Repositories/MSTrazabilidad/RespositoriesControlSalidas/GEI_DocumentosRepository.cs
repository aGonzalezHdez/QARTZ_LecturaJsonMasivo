using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
using LibreriaClasesAPIExpertti.Entities.EntitiesGei;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas
{
    public class GEI_DocumentosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public GEI_DocumentosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public GEI_Documentos Buscar(int IdTipoDocumento)
        {
            var objGEIDocumentos = new GEI_Documentos();

            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlDataReader dr;
            SqlParameter param;

            cn.ConnectionString = SConexion;
            cn.Open();

            cmd.CommandText = "NET_SEARCH_GEI_Documentos";
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;

            param = cmd.Parameters.Add("@ID_DOCUMENTO", SqlDbType.Int, 4);
            param.Value = IdTipoDocumento;

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        objGEIDocumentos.Id_Documento = Convert.ToInt32(dr["Id_Documento"]);
                        objGEIDocumentos.Nombre = dr["Nombre"].ToString();
                    }
                }

                else
                {
                    objGEIDocumentos = default;
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

            return objGEIDocumentos;
        }

    }
}
