using LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia;
using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion;
using LibreriaClasesAPIExpertti.Entities.EntitiesMV;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Entities.EntitiesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales;
using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesS3;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesReferencias;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RespositoriesControlSalidas;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioCoveRepository : ISaaioCoveRepository
    {
        public string SConexion { get; set; }
        string ISaaioCoveRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public SaaioCoveRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public List<SaaioCove> Cargar(string NUM_REFE)
        {
            List<SaaioCove>? listSaaioCove = new();
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_SAAIO_COVE", con);
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15 ).Value = NUM_REFE;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SaaioCove objSaaioCove = new();

                            objSaaioCove.NUM_REFE = dr["NUM_REFE"].ToString();
                            objSaaioCove.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                            objSaaioCove.FOL_COVE = Convert.ToInt32(dr["FOL_COVE"]);
                            objSaaioCove.CONS_COVE = Convert.ToInt32(dr["CONS_COVE"]);
                            objSaaioCove.FEC_ENV = Convert.ToDateTime(dr["FEC_ENV"]);
                            objSaaioCove.E_DOCUMENT = dr["E_DOCUMENT"].ToString();
                            objSaaioCove.ERR_COVE = dr["ERR_COVE"].ToString();
                            objSaaioCove.ERR_COVE2 = dr["ERR_COVE2"].ToString();
                            objSaaioCove.NUM_CER = dr["NUM_CER"].ToString();
                            objSaaioCove.FIR_DIGIT = dr["FIR_DIGIT"].ToString();
                            objSaaioCove.FIG_FIRM = dr["FIG_FIRM"].ToString();
                            objSaaioCove.FIR_DIGIT2 = dr["FIR_DIGIT2"].ToString();

                           
                            listSaaioCove.Add(objSaaioCove);
                        }
                    }
                    else
                    {
                        listSaaioCove = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listSaaioCove;
        }

        public List<SaaioCoveMV> CargarMV(string NUM_REFE)
        {
            List<SaaioCoveMV>? listSaaioCove = new();
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_SAAIO_COVE", con);
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;
                    using SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SaaioCoveMV objSaaioCove = new();

                            objSaaioCove.NUM_REFE = dr["NUM_REFE"].ToString();
                            objSaaioCove.CONS_FACT = Convert.ToInt32(dr["CONS_FACT"]);
                            objSaaioCove.E_DOCUMENT = dr["E_DOCUMENT"].ToString();
                            

                            Referencias referencias= new Referencias();
                            ReferenciasRepository referenciasRepository = new(_configuration);
                            referencias = referenciasRepository.Buscar(NUM_REFE);

                            if (referencias==null)
                            {
                                throw new Exception("No fue posible asociar la referencia a la bd CASAEI");
                            }
                            FacturasCove facturasCove = new FacturasCove();
                            FacturasCoveRepository facturasCoveRepository = new(_configuration);
                            facturasCove = facturasCoveRepository.Buscar(referencias.IDReferencia, objSaaioCove.CONS_FACT);
                            
                            if (facturasCove != null )
                            {
                                if (facturasCove.idDocumentoCove != 0)
                                {
                                     DocumentosporGuia   documentosporGuia = new DocumentosporGuia();
                                    DocumentosporGuiaRepository documentosporGuiaRepository = new(_configuration);
                                    documentosporGuia = documentosporGuiaRepository.BuscarPorId(facturasCove.idDocumentoCove);

                                    string vURL = string.Empty;                                
                                    BucketsS3Repository objS3 = new BucketsS3Repository(_configuration);

                                    if (documentosporGuia == null)
                                    {
                                        vURL = string.Empty;
                                    }
                                    else
                                    {
                                        vURL = objS3.URL(documentosporGuia.RutaS3, "grupoei.documentos");
                                    }
                            
                                    objSaaioCove.linkCoveS3 = vURL; 
                                }
                                else { objSaaioCove.linkCoveS3 = string.Empty; }
                               
                            }
                            else
                            {
                                objSaaioCove.linkCoveS3 = string.Empty;
                            }

                           listSaaioCove.Add(objSaaioCove);
                        }
                    }
                    else
                    {
                        listSaaioCove = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return listSaaioCove;
        }
    }
}
