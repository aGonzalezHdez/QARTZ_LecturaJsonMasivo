using LibreriaClasesAPIExpertti.Entities.EntitiesCasa;
using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesCasa;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesControlDeEventos;
using LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesDocumentosPorGuia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesConsultasWsExternos
{
    public class SDTConsultaWec
    {
        public string Consecutivo { get; set; }
        public string GuiaHouse { get; set; }
        public string GuiaMaster { get; set; }
        public string REFI { get; set; }
        public string RegistroEntrada { get; set; }
        public string EntradaAduana { get; set; }
        public string AlmacenArribo { get; set; }
        public string AlmacenNuevo { get; set; }
        public string RevalidadaxAgteExterno { get; set; }
        public string MercanciaAlertada { get; set; }
        public string ClavePedimento { get; set; }
        public string Bultos { get; set; }
        public string Peso { get; set; }
        public string Salida { get; set; }
        public string RevalidaOtroAgenteAduanal { get; set; }
        public string Ubicacion { get; set; }
        public string FechadeConsulta { get; set; }
        public string IdWec { get; set; }
        public List<Pallets> Pallets { get; set; }

    }//Clase
}
