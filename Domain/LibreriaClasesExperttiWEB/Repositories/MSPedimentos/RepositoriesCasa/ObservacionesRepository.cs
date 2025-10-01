using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wsCentralizar;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class ObservacionesRepository
    {
        private readonly string _connectionString;

        public ObservacionesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbCASAEI")!;
        }

        public wsCentralizar.DocumentosPorGuia BuscarUltima(int idReferencia, int idTipoDocumento)
        {
            wsCentralizar.DocumentosPorGuia documento = null;

            using (var cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand("NET_SEARCH_DOCUMENTOSPORGUIA_ULTIMA", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idReferencia", idReferencia);
                    cmd.Parameters.AddWithValue("@IdTipoDocumento", idTipoDocumento);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            documento = new wsCentralizar.DocumentosPorGuia
                            {
                                idDocumento = Convert.ToInt32(dr["idDocumento"]),
                                idTipoDocumento = Convert.ToInt32(dr["idTipoDocumento"]),
                                idReferencia = Convert.ToInt32(dr["idReferencia"]),
                                Consecutivo = Convert.ToInt32(dr["Consecutivo"]),
                                RutaFecha = dr["RutaFecha"].ToString(),
                                Extension = dr["Extension"].ToString(),
                                FechaAlta = Convert.ToDateTime(dr["FechaAlta"])
                            };
                        }
                    }
                }
            }

            return documento;
        }

        public string GenerarObservacionesAutomaticas(int idReferencia)
        {
            var proforma = BuscarUltima(idReferencia, 1); // Tipo Documento 1 = Proforma
            if (proforma == null)
                throw new ArgumentException("No existe proforma");

            string observaciones = $"PROFORMA {proforma.Consecutivo}";

            var factura = BuscarUltima(idReferencia, 14); // Tipo Documento 14 = Factura
            if (factura != null)
            {
                observaciones += $" // FACTURA {factura.Consecutivo}";
            }

            return observaciones;
        }
    }

}
