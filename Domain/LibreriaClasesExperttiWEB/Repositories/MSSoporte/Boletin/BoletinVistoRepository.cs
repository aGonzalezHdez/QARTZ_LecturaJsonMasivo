using LibreriaClasesAPIExpertti.Entities.EntitiesBoletin;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Boletin
{
    public class BoletinVistoRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public BoletinVistoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int InsertBoletinVisto(BoletinVisto bv)
        {
            int newId = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_INSERT_CASAEI_BOLETINESVISTOS", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@idBoletin", bv.idBoletin);
                    cmd.Parameters.AddWithValue("@idUsuario", bv.idUsuario);
                    if (bv.FechaVisto.HasValue)
                        cmd.Parameters.AddWithValue("@FechaVisto", bv.FechaVisto);
                    else
                        cmd.Parameters.AddWithValue("@FechaVisto", DBNull.Value);

                    try
                    {
                        cn.Open();
                        newId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al insertar boletín visto: " + ex.Message);
                    }
                }
            }

            return newId;
        }
    }
}
