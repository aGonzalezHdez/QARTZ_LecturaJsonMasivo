using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa.Insterfaces;
using LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa
{
    public class SaaioNoIncrRepository : ISaaioNoIncrRepository
    {
        public string SConexion { get; set; }
        string ISaaioNoIncrRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public SaaioNoIncrRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
      
        public DataTable Cargar(string NUM_REFE)
        {
            DataTable dtb = new();
            try
            {
                using SqlConnection con = new(SConexion);
                using SqlCommand cmd = new("NET_LOAD_SAAIO_NOINCR", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add("@NUM_REFE", SqlDbType.VarChar, 15).Value = NUM_REFE;

                using SqlDataAdapter dap = new()
                {
                    SelectCommand = cmd
                };

                con.Open();
                dap.Fill(dtb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + "NET_LOAD_SAAIO_NOINCR");
            }

            return dtb;
        }
    }
}
