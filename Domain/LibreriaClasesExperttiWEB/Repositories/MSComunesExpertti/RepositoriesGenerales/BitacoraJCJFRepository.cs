using LibreriaClasesAPIExpertti.Entities.EntitiesGenerales;
using LibreriaClasesAPIExpertti.Entities.EntitiesJCJF;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LibreriaClasesAPIExpertti.Repositories.MSComunesExpertti.RepositoriesGenerales
{
    public class BitacoraJCJFRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;

        public BitacoraJCJFRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public int Insertar(BitacoraJCJF objBitacoraJCJF)
        {
            int id;

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CASAEI_BITACORA_JCJF", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@GuiaHouse", SqlDbType.VarChar, 25).Value = objBitacoraJCJF.GuiaHouse;
                    cmd.Parameters.Add("@Respuesta", SqlDbType.Bit).Value = objBitacoraJCJF.Respuesta;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 512).Value = objBitacoraJCJF.Mensaje;
                    cmd.Parameters.Add("@Tipo", SqlDbType.Int, 4).Value = objBitacoraJCJF.Tipo;
                    cmd.Parameters.Add("@TipoPrevio", SqlDbType.VarChar, 10).Value = objBitacoraJCJF.TipoPrevio;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (Convert.ToInt32(cmd.Parameters["@newid_registro"].Value) != -1)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                        else
                        {
                            id = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }


        public int InsertarManifieto(ResposeJCJFManifiesto objBitacoraJCJF, ManifiestoFile objJCJF)
        {
            int id = 0;

            try
            {
                using (con = new(SConexion))
                using (SqlCommand cmd = new("NET_INSERT_CASAEI_BITACORA_JCJF_MANIFIESTO", con))
                {
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@IdSubidaManifiestoEi", SqlDbType.Int, 4).Value = objBitacoraJCJF?.Datos?.IdSubidaManifiestoEi ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Master", SqlDbType.VarChar, 25).Value = objJCJF.TxtGuiaMaster;
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 512).Value = objBitacoraJCJF?.Datos?.Nombre ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Procesado", SqlDbType.Bit).Value = objBitacoraJCJF?.Datos?.Procesado ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@RegistrosProcesados", SqlDbType.Int, 4).Value = objBitacoraJCJF?.Datos?.RegistrosProcesados ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@FechaProcesado", SqlDbType.DateTime).Value = objBitacoraJCJF?.Datos?.FechaProcesado ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Creado", SqlDbType.DateTime).Value = objBitacoraJCJF?.Datos?.Creado ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, -1).Value = objBitacoraJCJF?.Error ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@IdDatosDeEmpresa", SqlDbType.Int, 4).Value = objJCJF.IDDatosDeEmpresa;
                    cmd.Parameters.Add("@IdOficina", SqlDbType.Int, 4).Value = objJCJF.IdOficina;
                    cmd.Parameters.Add("@IdUsuario", SqlDbType.Int, 4).Value = objJCJF.IdUsuario;

                    cmd.Parameters.Add("@newid_registro", SqlDbType.Int, 4).Direction = ParameterDirection.Output;

                    using (cmd.ExecuteReader())
                    {
                        if (cmd.Parameters["@newid_registro"].Value != DBNull.Value)
                        {
                            id = Convert.ToInt32(cmd.Parameters["@newid_registro"].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return id;
        }



    }
}
