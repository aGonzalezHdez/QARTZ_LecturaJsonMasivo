using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos
{
    public class CatalogoDeFPPorClienteRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public CatalogoDeFPPorClienteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogoDeFPPorCliente Buscar(string MiRfc, string MiClaveDePedimento, int MiOperacion)
        {
            var objCATALOGODEFPPORCLIENTE = new CatalogoDeFPPorCliente();
            var cn = new SqlConnection();
            var cmd = new SqlCommand();
            SqlParameter @param;
            SqlDataReader dr;

            try
            {
                cmd.CommandText = "NET_LOAD_CATALOGODEFPPORCLIENTE";
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;

                // @RFC CHAR(13) ,
                @param = cmd.Parameters.Add("@RFC", SqlDbType.Char, 13);
                @param.Value = MiRfc;

                // @ClaveDePedimento VARCHAR(2),
                @param = cmd.Parameters.Add("@ClaveDePedimento", SqlDbType.VarChar, 2);
                @param.Value = MiClaveDePedimento;

                // @Operacion INT
                @param = cmd.Parameters.Add("@Operacion", SqlDbType.Int, 4);
                @param.Value = MiOperacion;


                cn.ConnectionString = SConexion;
                cn.Open();


                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    dr.Read();
                    objCATALOGODEFPPORCLIENTE.IDFP = Convert.ToInt32(dr["IDFP"]);
                    objCATALOGODEFPPORCLIENTE.RFC = dr["RFC"].ToString();
                    objCATALOGODEFPPORCLIENTE.ClaveDePedimento = dr["ClaveDePedimento"].ToString();
                    objCATALOGODEFPPORCLIENTE.Operacion = Convert.ToInt32(dr["Operacion"]);
                    objCATALOGODEFPPORCLIENTE.AplicaDTA = Convert.ToBoolean(dr["AplicaDTA"]);
                    objCATALOGODEFPPORCLIENTE.FPDTA = Convert.ToInt32(dr["FPDTA"]);
                    objCATALOGODEFPPORCLIENTE.AplicaIVA = Convert.ToBoolean(dr["AplicaIVA"]);
                    objCATALOGODEFPPORCLIENTE.FPIVA = Convert.ToInt32(dr["FPIVA"]);
                    objCATALOGODEFPPORCLIENTE.AplicaADV = Convert.ToBoolean(dr["AplicaADV"]);
                    objCATALOGODEFPPORCLIENTE.FPADV = Convert.ToInt32(dr["FPADV"]);
                }
                else
                {
                    objCATALOGODEFPPORCLIENTE = default;
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

            return objCATALOGODEFPPORCLIENTE;
        }

    }
}
