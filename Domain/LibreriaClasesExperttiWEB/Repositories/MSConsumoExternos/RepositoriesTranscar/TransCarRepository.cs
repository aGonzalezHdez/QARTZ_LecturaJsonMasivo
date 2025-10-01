using LibreriaClasesAPIExpertti.Entities.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Entities.EntitiesTransCar;
using LibreriaClasesAPIExpertti.Repositories.DespachoIntegrado;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesTranscar.Interfaces;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesTranscar
{
    public class TransCarRepository : ITransCarRepository
    {
        public string SConexion { get; set; }
        public IConfiguration _configuration;
        public SqlConnection con;
        public ControldeEventosRepository eventosRepository;
        public TransCarRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public List<string> EnviarTransCar(int idPredoda)
        {
            var list = new List<string>();
            PredodaRepository objPreD = new(_configuration);

            try
            {
                List<TransCarPredoda> lst = new List<TransCarPredoda>();
                ApiTransCar apiTrans = new(_configuration);

                var sessionCookie = apiTrans.getToken();
                if (apiTrans.GeneroToken == true)
                {
                    lst = objPreD.DetallePredodaTransCar(idPredoda);
                    foreach (TransCarPredoda d in lst)
                    {
                        TransCar_Mercancias objTrasc = new();
                        objTrasc.Code = d.Code;
                        objTrasc.Name = d.Code;
                        objTrasc.U_TS_Bienes = "31181701";
                        objTrasc.U_TS_Claves = "";
                        objTrasc.U_TS_Descripcion = "Empaques";
                        objTrasc.U_TS_Cantidad = "1";
                        objTrasc.U_TS_ClaveUnidad = "XPK";
                        objTrasc.U_TS_Unidad = "Paquete";
                        objTrasc.U_TS_Dimensiones = "";
                        objTrasc.U_TS_MaterialPeligroso = "NO";
                        objTrasc.U_TS_ClaveMaterialPeligroso = "";
                        objTrasc.U_TS_Embalaje = "";
                        objTrasc.U_TS_DescriEmbalaje = "";
                        objTrasc.U_TS_PesosKG = d.Prorrateo.ToString();
                        objTrasc.U_TS_ValorMercancia = "";
                        objTrasc.U_TS_Moneda = "";
                        objTrasc.U_TS_Fraccion = d.Fraccion.Trim();
                        objTrasc.U_TS_UUIDComercio = "";
                        objTrasc.U_TS_Pedimento = d.Pedimento.Trim();
                        objTrasc.U_TS_Guias = "";// d.Guias.Trim(); por petición de Transcar(no enviar guías) minuta 16/10/2024
                        objTrasc.U_TS_CantidadTransporta = d.bultos.ToString();
                        objTrasc.U_TS_DetalleMercancia = "";// d.Descripcion.Trim();
                        objTrasc.U_TS_TipoMaterial = "05";
                        objTrasc.U_TS_Pedroda = idPredoda.ToString();

                        string json = string.Empty;
                        json = Newtonsoft.Json.JsonConvert.SerializeObject(objTrasc);


                        string Respuesta = d.Pedimento.Trim() + ": " + apiTrans.sendApiTranscar(json, sessionCookie);
                        list.Add(Respuesta);
                    }
                }
                else { throw new Exception("No fue posible establecer conexión al servicio de TransCar"); }



            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }



            return list;
        }

    }
}
