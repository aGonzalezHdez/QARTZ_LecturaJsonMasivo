using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
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
    public class SopRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public SopRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public CatalogoSOPEncabezado BuscarEncabezado(int idCliente, int idOficina)
        {
            CatalogoSOPEncabezado encabezado = null;

            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand("NET_SEARCH_CATALOGOSOPENCABEZADO_OFICINA", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                    cmd.Parameters.AddWithValue("@IdOficina", idOficina);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            encabezado = new CatalogoSOPEncabezado
                            {
                                IDSOP = Convert.ToInt32(dr["IDSOP"]),
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                DescripcionDelObjetivo = dr["DescripcionDelObjetivo"].ToString(),
                                Antecedentes = dr["Antecedentes"].ToString(),
                                Autor = dr["Autor"].ToString(),
                                Documento = dr["Documento"].ToString(),
                                FechaDeImplementacion = Convert.ToDateTime(dr["FechaDeImplementacion"]),
                                FechaDeModificacion = Convert.ToDateTime(dr["FechaDeModificacion"]),
                            };
                        }
                    }
                }
            }

            return encabezado;
        }

        public CatalogoSOPProcesos BuscarProceso(int idTipoDeProceso, int idDepartamento)
        {
            CatalogoSOPProcesos proceso = null;

            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand("NET_SEARCH_CATALOGOSOPPROCESOSXDEPA", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDTipoDeProceso", idTipoDeProceso);
                    cmd.Parameters.AddWithValue("@IDDepartamento", idDepartamento);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            proceso = new CatalogoSOPProcesos
                            {
                                IDProceso = Convert.ToInt32(dr["IDProceso"]),
                                IDSOP = Convert.ToInt32(dr["IDSOP"]),
                                IDTipoDeProceso = Convert.ToInt32(dr["IDTipoDeProceso"]),
                                IDDepartamento = Convert.ToInt32(dr["IDDepartamento"]),
                                Responsables = dr["Responsables"].ToString(),
                                TiempoDeEntregaAlSProceso = dr["TiempoDeEntregaAlSProceso"].ToString(),
                            };
                        }
                    }
                }
            }

            return proceso;
        }
    }
}
