using System.Data.SqlClient;
using System.Data;
using LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos.AdientDXSACI;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesAdientDX
{
    public class BitacoraAdienDXRepository
    {

        public string SConexion { get; set; }
        public IConfiguration _configuration;

        public BitacoraAdienDXRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(BitacoraAdienDX objBitacoraAdienDX)
        {
            int idBuscar;

            try
            {
                using (SqlConnection con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CASAEI_BITACORA_ADIENTDX", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@NumerodeReferencia", SqlDbType.VarChar, 15).Value = objBitacoraAdienDX.NumerodeReferencia;
                    cmd.Parameters.Add("@CompanyId", SqlDbType.VarChar, 250).Value = objBitacoraAdienDX.CompanyId;
                    //cmd.Parameters.Add("@FechaEnvio", SqlDbType.DateTime, 4).Value = objBitacoraAdienDX.FechaEnvio;
                    cmd.Parameters.Add("@Menssaje", SqlDbType.VarChar, 250).Value = objBitacoraAdienDX.Menssaje;
                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                        {
                            idBuscar = (int)cmd.Parameters["@newid_registro"].Value;
                        }
                        else
                        {
                            idBuscar = 0;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return idBuscar;
        }
    }
}
