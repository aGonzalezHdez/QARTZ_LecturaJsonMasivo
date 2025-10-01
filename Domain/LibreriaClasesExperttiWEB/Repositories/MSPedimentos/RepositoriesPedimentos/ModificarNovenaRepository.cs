using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPedimentos
{
    public class ModificarNovenaRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public ModificarNovenaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<SaaioIdeFra> Cargar(string numRefe)
        {
            var list = new List<SaaioIdeFra>();

            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand("NET_LOAD_SAAIO_IDEFRA", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NUM_REFE", numRefe);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new SaaioIdeFra
                            {
                                NUM_REFE = reader["NUM_REFE"].ToString(),
                                NUM_PART = Convert.ToInt32(reader["NUM_PART"]),
                                NUM_IDE = Convert.ToInt32(reader["NUM_IDE"]),
                                CVE_PERM = reader["CVE_PERM"]?.ToString(),
                                FEC_PERM = reader["FEC_PERM"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["FEC_PERM"]),
                                NUM_PERM2 = reader["NUM_PERM2"]?.ToString(),
                                NUM_PERM = reader["NUM_PERM"]?.ToString(),
                                NUM_PERM3 = reader["NUM_PERM3"]?.ToString()
                            };
                            list.Add(item);
                        }
                    }
                }
            }

            return list;
        }

        public int Modificar(SaaioIdeFra lSaaioIdeFra)
        {
            try
            {
                using (var cn = new SqlConnection(_connectionString))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("NET_UPDATE_SAAIO_IDEFRA", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@NUM_REFE", lSaaioIdeFra.NUM_REFE);
                        cmd.Parameters.AddWithValue("@NUM_PART", lSaaioIdeFra.NUM_PART);
                        cmd.Parameters.AddWithValue("@NUM_IDE", lSaaioIdeFra.NUM_IDE);
                        cmd.Parameters.AddWithValue("@CVE_PERM", (object?)lSaaioIdeFra.CVE_PERM ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FEC_PERM", (object?)lSaaioIdeFra.FEC_PERM ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NUM_PERM2", (object?)lSaaioIdeFra.NUM_PERM2 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NUM_PERM", (object?)lSaaioIdeFra.NUM_PERM ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@NUM_PERM3", (object?)lSaaioIdeFra.NUM_PERM3 ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message} - Error en NET_UPDATE_SAAIO_IDEFRA");
            }
        }

        public void ModificarNovena(string numRefe)
        {
            var lstIdeFra = Cargar(numRefe);

            if (lstIdeFra != null && lstIdeFra.Count > 0)
            {
                foreach (var item in lstIdeFra)
                {
                    if (item.CVE_PERM == "EN" && (item.NUM_PERM == "VII" || item.NUM_PERM == "VIII"))
                    {
                        item.NUM_PERM = "IX";
                        Modificar(item);
                    }
                }
            }
        }
    }
}
