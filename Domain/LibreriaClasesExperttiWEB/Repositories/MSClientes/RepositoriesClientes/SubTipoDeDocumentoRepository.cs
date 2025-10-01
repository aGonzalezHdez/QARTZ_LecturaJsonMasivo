using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes
{
    public class SubTipoDeDocumentoRepository : ISubTipoDeDocumentoRepository
    {
        public string SConexion { get; set; }
        string ISubTipoDeDocumentoRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public SubTipoDeDocumentoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<List<SubTipoDeDocumento>> CargarCombo()
        {
            List<SubTipoDeDocumento> lstSubTipodeDocumento = new();

            try
            {
                using (SqlConnection con = new(SConexion))

                using (var cmd = new SqlCommand("NET_LOAD_SUBTIPODEDOCUMENTO", con))
                {

                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SubTipoDeDocumento objSubTipodeDocumento = new()
                            {
                                IDSubTipoDeDocumento = Convert.ToInt32(dr["IDSubTipoDeDocumento"]),
                                DescripcionSubTipo = string.Format("{0}", dr["DescripcionSubTipo"])
                            };
                            lstSubTipodeDocumento.Add(objSubTipodeDocumento);
                        }
                    }
                    else
                    {
                        lstSubTipodeDocumento = null!;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return lstSubTipodeDocumento;
        }
    }
}
