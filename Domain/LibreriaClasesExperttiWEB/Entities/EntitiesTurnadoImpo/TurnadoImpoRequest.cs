using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesTurnadoImpo
{
    public class TurnadoImpoRequest
    {
        public string GuiaHouse { get; set; }
        public int IdUsuario { get; set; }
        public int IDDatosDeEmpresa { get; set; }
        public int IdOficina { get; set; }
        public int IdDepartamento { get; set; }
        public string Guia2d { get; set; }
        public bool rdbUnitarias { get; set; }
        public bool rdbPartidas { get; set; }
        public bool rdbMiscelaneas { get; set; }
        public bool rdbVolumen { get; set; }
        public bool rdbTransito { get; set; }
        public bool chkPreAlertas { get; set; }
        public bool chkDiasAnteriores { get; set; }
        public bool chkInformaciones { get; set; }
        public bool validaNoManisfestado { get; set; }
        public bool validaEncargoCliente { get; set; }
        public int avisarDiffAgenteAdu { get; set; }
        public bool chkbSubdivision { get; set; }
        public int IDGrupodeTrabajoSelected { get; set; }
        public int validaNubePrepago { get; set; }
        public CatalogoDeUsuarios ObjUsuario { get; set; }
        public bool RevDsicrepancia { get; set; }
        public string MyConnectionString { get; set; }
        public string MyConnectionStringGP { get; set; }
        public bool chkFis { get; set; }
        public List<string> piecesIds { get; set; }
        public bool reasignar { get; set; }
        public bool escaneoCompleto { get; set; }
        public int turnadoParcialOAsignacion { get; set; }
        public bool validarReferencia { get; set; }
        public int avisarPrevioOConsolidado { get; set; }
    }

}
