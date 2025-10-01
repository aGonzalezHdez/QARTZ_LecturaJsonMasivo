using LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador
{
    public class FechasAirbusRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;        

        public FechasAirbusRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<FechasAirbus> Cargar(string miReferencia)
        {
            var lstFechasAirbus = new List<FechasAirbus>();

            using (var cn = new SqlConnection(SConexion))
            {
                using (var cmd = new SqlCommand("NET_SELECT_SAAIO_PEDIME_FECHAS_AIRBUS", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add parameters to the command
                    var param = cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15);
                    param.Value = miReferencia;
                    try
                    {
                        cn.Open();

                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    var objFechasAirbus = new FechasAirbus
                                    {
                                        ClaveFecha = dr.GetInt32(dr.GetOrdinal("ClaveFecha")),
                                        Fecha = dr.GetDateTime(dr.GetOrdinal("Fecha"))
                                    };

                                    lstFechasAirbus.Add(objFechasAirbus);
                                }
                            }
                            else
                            {
                                lstFechasAirbus = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

            return lstFechasAirbus;
        }
    }
}
