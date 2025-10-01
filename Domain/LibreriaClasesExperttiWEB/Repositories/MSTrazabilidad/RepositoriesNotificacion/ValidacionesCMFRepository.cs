using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion
{
    public class ValidacionesCMFRepository: IValidacionesCMFRepository
    {
        public string SConexion { get; set; }
        string IValidacionesCMFRepository.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IConfiguration _configuration;
        public ValidacionesCMFRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public string BuscarCuentaFOC(string GuiaHouse)
        {
            string Mensaje = string.Empty;
            try
            {
                using (SqlConnection con = new(SConexion))
                using (var cmd = new SqlCommand("NET_SEARCH_CUENTA_FOC ", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@GUIAHOUSE", SqlDbType.VarChar, 13).Value = GuiaHouse;
                    cmd.Parameters.Add("@MENSAJE", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    using (cmd.ExecuteReader())
                    {
                        Mensaje = cmd.Parameters["@MENSAJE"].Value.ToString();
                        cmd.Parameters.Clear();
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
            return Mensaje;
        }
    }
}
