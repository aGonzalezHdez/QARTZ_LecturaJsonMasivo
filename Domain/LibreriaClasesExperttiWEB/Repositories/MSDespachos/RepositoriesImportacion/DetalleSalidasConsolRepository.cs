using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibreriaClasesAPIExpertti.Entities.EntitiesDespacho;

namespace LibreriaClasesAPIExpertti.Repositories.MSDespachos.RepositoriesImportacion
{
    public class DetalleSalidasConsolRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;

        public DetalleSalidasConsolRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Contar(int idAnexo)
        {
            int escaneos = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_SEARCH_DETALLESALIDASCONSOL", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IDAnexo", SqlDbType.Int).Value = idAnexo;

                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                escaneos = Convert.ToInt32(dr["ESCANEADOS"]);
                            }
                            else
                            {
                                escaneos = 0; // Return 0 if no rows found
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            return escaneos;
        }
        public int Insertar(DetalleSalidasConsol ldetallesalidasconsol)
        {
            int id = 0;
            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "NET_INSERT_DETALLESALIDASCONSOL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    // @IDAnexo int
                    cmd.Parameters.Add("@IDAnexo", SqlDbType.Int).Value = ldetallesalidasconsol.IDAnexo;

                    // @IDSalidasConsol int
                    cmd.Parameters.Add("@IDSalidasConsol", SqlDbType.Int).Value = ldetallesalidasconsol.IDSalidasConsol;

                    SqlParameter newIdParam = cmd.Parameters.Add("@newid_registro", SqlDbType.Int);
                    newIdParam.Direction = ParameterDirection.Output;

                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();

                        if ((int)cmd.Parameters["@newid_registro"].Value != -1)
                        {
                            id = (int)cmd.Parameters["@newid_registro"].Value;
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        id = 0;
                        throw new Exception(ex.Message + " NET_INSERT_DETALLESALIDASCONSOL");
                    }
                }
            }

            return id;
        }

    }
}
