using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
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
    public class AlertaMasivaRepository: IAlertasMasivas
    {
        public string SConexion { get; set; }
        string IAlertasMasivas.SConexion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IConfiguration _configuration;
            
        public AlertaMasivaRepository(IConfiguration configuration) {
           _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public string EnviarAlertasMasivas(AlertaReferenciasRequest request)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(SConexion))
                using (SqlCommand cmd = new SqlCommand("NET_INSERT_ALERTASMASIVAS_NOTI", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ListaReferencias", SqlDbType.NVarChar).Value = request.Referencias ?? "";
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 100).Value = request.Mensaje ?? "";

                    if (DateTime.TryParse(request.Fecha, out DateTime fechaConvertida))
                        cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fechaConvertida;
                    else
                        throw new ArgumentException("Formato de fecha inválido.");

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw new Exception("NET_INSERT_ALERTASMASIVAS_NOTI " + ex.Message.ToString());
            }

            return "Alertas enviadas correctamente.";
        }
    }
}
