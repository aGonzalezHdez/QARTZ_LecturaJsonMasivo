using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos
{
    public class DetalleJulianosRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public DetalleJulianosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public int Insertar(DetalleJulianos objDetalleJulianos)
        {
            int idDetalle;
            try
            {
                using (con = new SqlConnection(SConexion))
                {
                    con.Open();
                    var cmd = new SqlCommand("", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add("@idJuliano", SqlDbType.Int, 4).Value = objDetalleJulianos.idJuliano;
                    cmd.Parameters.Add("@idReferencia", SqlDbType.Int, 4).Value = objDetalleJulianos.idReferencia;
                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            idDetalle = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                        else
                        {
                            idDetalle = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                idDetalle = 0;
                con.Close();
                con.Dispose();
                throw new Exception(ex.Message.ToString());
            }

            con.Close();
            con.Dispose();
            return idDetalle;
        }


    }
}
