using System.Data.SqlClient;
using System.Data;

using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

using NPOI.SS.Formula.Functions;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using Microsoft.Extensions.Configuration;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias
{
    public class DocumentoRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public DocumentoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<Documento> Cargar(string NumeroDeReferencia, int pageNumber, int pageSize, out int totalRecords)
        {
            List<Documento> lstDocumentos = new List<Documento>();

            try
            {
                totalRecords = 0;
                using (con = new SqlConnection(sConexion))
                using (SqlCommand cmd = new SqlCommand("NET_LOAD_DOCUMENTOS_NEW_PAGINADO", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros requeridos
                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25).Value = NumeroDeReferencia;
                    cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            
                            if (dr.Read()) // Primer result set, el total
                            {
                                totalRecords = dr.GetInt32(0);
                            }

                            dr.NextResult();

                            while (dr.Read())
                            {
                                Documento documento = new Documento
                                {
                                    TipodeDocumento = dr["TipodeDocumento"]?.ToString(),
                                    Cons = dr["Cons"]?.ToString(),
                                    FechaAlta = dr["FECHAALTA"]?.ToString(),
                                    Elaboro = dr["Elaboró"]?.ToString(),
                                    RutaFisica = dr["RutaFisica"]?.ToString(),
                                    Encriptado = dr["Encriptado"]?.ToString(),
                                    Identificador = dr["Identificador"]?.ToString(),
                                    IdDocumentoVuce = dr["IdDocumentoVuce"]?.ToString(),
                                    RequisitoCumplido = dr["RequisitoCumplido"]?.ToString(),
                                    IdTipoDocumento = dr["IdTipoDocumento"]?.ToString(),
                                    RutaFisicaAnterior = dr["RutaFisicaAnterior"]?.ToString(),
                                    S3 = dr["S3"]?.ToString(),
                                    RutaS3 = dr["RutaS3"]?.ToString(),
                                    IdDocumento = dr["IdDocumento"]?.ToString(),
                                };
                                lstDocumentos.Add(documento);
                            }
                        }
                        else
                        {
                            lstDocumentos = new List<Documento>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar documentos: " + ex.Message);
            }
            return lstDocumentos;
        }


    }
}
