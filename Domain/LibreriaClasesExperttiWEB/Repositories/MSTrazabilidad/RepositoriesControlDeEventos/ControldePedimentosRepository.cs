using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos
{
    public class ControldePedimentosRepository
    {
        public string sConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection cn;

        public ControldePedimentosRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            sConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }
        public ControldePedimentos Buscar(int idPedimento)
        {
            ControldePedimentos objCONTROLDEPEDIMENTOS = null;

            using (var cn = new SqlConnection(sConexion))
            using (var cmd = new SqlCommand("NET_SEARCH_CONTROLDEPEDIMENTOS", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdPedimento", SqlDbType.Int) { Value = idPedimento });

                try
                {
                    cn.Open();

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            dr.Read();
                            objCONTROLDEPEDIMENTOS = new ControldePedimentos
                            {
                                IdPedimento = Convert.ToInt32(dr["IdPedimento"]),
                                IdReferencia = Convert.ToInt32(dr["IdReferencia"]),
                                IdOficina = Convert.ToInt32(dr["IdOficina"]),
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                Patente = dr["Patente"].ToString(),
                                Pedimento = dr["Pedimento"].ToString(),
                                Fecha = Convert.ToDateTime(dr["Fecha"])
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return objCONTROLDEPEDIMENTOS;
        }

        public async Task<AsignarGuiasRespuesta> AsignarPedimento(ControldePedimentos lControldePedimentos, bool Extraordinario, string Aduana)
        {
            AsignarGuiasRespuesta objControldePedimentos = new();

            try
            {
                using (cn = new(sConexion))
                using (SqlCommand cmd = new("NET_CREATE_NUMERODEPEDIMENTO_EXTRAORDINARIO_ADUANA", cn))
                {
                    cn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Patente", SqlDbType.Int, 4).Value = lControldePedimentos.Patente;
                    cmd.Parameters.Add("@idOficina", SqlDbType.Int, 4).Value = lControldePedimentos.IdOficina;
                    cmd.Parameters.Add("@IdReferencia", SqlDbType.Int, 4).Value = lControldePedimentos.IdReferencia;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = lControldePedimentos.IdUsuario;
                    cmd.Parameters.Add("@Extraordinario", SqlDbType.Bit).Value = Extraordinario;
                    cmd.Parameters.Add("@Aduana", SqlDbType.VarChar, 3).Value = Aduana;

                    using SqlDataReader dr = await cmd.ExecuteReaderAsync();
                    if (dr.HasRows)
                    {
                        dr.Read();

                        objControldePedimentos.IdReferencia = lControldePedimentos.IdReferencia;
                        objControldePedimentos.IdOficina = lControldePedimentos.IdOficina;
                        objControldePedimentos.IdUsuario = lControldePedimentos.IdUsuario;
                        objControldePedimentos.Patente = lControldePedimentos.Patente;
                        objControldePedimentos.Pedimento = dr["Pedimento"].ToString();
                        objControldePedimentos.Disponibles = Convert.ToInt32(dr["Disponibles"]);
                    }

                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }

            return objControldePedimentos;
        }
    }
}
