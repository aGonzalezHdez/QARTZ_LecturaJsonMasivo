using LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesPublicos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesPrevalidador
{
    public class CatalogoDeClientesInterRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;        

        public CatalogoDeClientesInterRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public void EnviaEstaReferencia(ref string miReferencia)
        {
            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_ENVIAR_PEDIMENTO_SOCIOS", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NUM_REFE", miReferencia ?? (object)DBNull.Value);

                try
                {
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        // Assuming no data needs to be read
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public void EnviaDespuesDelPago(ref string misPedimentos, int miIdUsuario, string pMisDocumentos)
        {
            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_SABER_SI_ENVIA_A_EDI_SOCIOS", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ArrayPedimentos", misPedimentos ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@IdUsuario", miIdUsuario);

                try
                {
                    cn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var referencia = dr["Referencia"].ToString();
                                EnviaEstaReferencia(ref referencia);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
