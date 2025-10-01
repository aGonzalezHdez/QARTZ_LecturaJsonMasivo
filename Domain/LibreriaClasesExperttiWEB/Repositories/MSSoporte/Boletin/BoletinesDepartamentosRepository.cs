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
    public class BoletinesDepartamentosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public BoletinesDepartamentosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<BoletinesDepartamentos> GetBoletinesDepartamentos()
        {
            List<BoletinesDepartamentos> lista = new List<BoletinesDepartamentos>();

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_LIST_CASAEI_BOLETINESDEPARTAMENTOS", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    try
                    {
                        cn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                BoletinesDepartamentos bd = new BoletinesDepartamentos
                                {
                                    IdBoletinDetalleDep = Convert.ToInt32(dr["IdBoletinDetalleDep"]),
                                    IdBoletin = Convert.ToInt32(dr["IdBoletin"]),
                                    IdDepartamento = Convert.ToInt32(dr["IdDepartamento"])
                                };

                                lista.Add(bd);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al listar boletines departamentos: " + ex.Message);
                    }
                }
            }

            return lista;
        }

        public int InsertBoletinesDepartamentos(BoletinesDepartamentos bd)
        {
            int newId = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_INSERT_CASAEI_BOLETINESDEPARTAMENTOS", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletin", bd.IdBoletin);
                    cmd.Parameters.AddWithValue("@IdDepartamento", bd.IdDepartamento);

                    try
                    {
                        cn.Open();
                        newId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al insertar boletines departamento: " + ex.Message);
                    }
                }
            }

            return newId;
        }

        public int UpdateBoletinesDepartamentos(BoletinesDepartamentos bd)
        {
            int rowsAffected = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_UPDATE_CASAEI_BOLETINESDEPARTAMENTOS", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletinDetalleDep", bd.IdBoletinDetalleDep);
                    cmd.Parameters.AddWithValue("@IdBoletin", bd.IdBoletin);
                    cmd.Parameters.AddWithValue("@IdDepartamento", bd.IdDepartamento);

                    try
                    {
                        cn.Open();
                        rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al actualizar boletines departamento: " + ex.Message);
                    }
                }
            }

            return rowsAffected;
        }

        public int DeleteBoletinesDepartamento(int id)
        {
            int rowsAffected = 0;

            using (SqlConnection cn = new SqlConnection(sConexion))
            {
                using (SqlCommand cmd = new SqlCommand("NET_DELETE_CASAEI_BOLETINESDEPARTAMENTOS", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdBoletinDetalleDep", id);

                    try
                    {
                        cn.Open();
                        rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al eliminar boletines departamento: " + ex.Message);
                    }
                }
            }

            return rowsAffected;
        }
    }
}
