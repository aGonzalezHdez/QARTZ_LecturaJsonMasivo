namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogoDeOficinas
    {
        public int IdOficina { get; set; }

        public string nombre { get; set; } = null!;

        public string idAduana { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public decimal LimiteDePagoElectronico { get; set; }

        public string E_Mail { get; set; } = null!;

        public string PassMail { get; set; } = null!;

        public string Host { get; set; } = null!;

        public int Puerto { get; set; }

        public int AutomaticoConsol { get; set; }

        public bool DHL { get; set; }

        public int IDDatosDeEmpresa { get; set; }

        public string VersionModuloTrafico { get; set; } = null!;

        public string RutaDigitalizadosVucem { get; set; } = null!;

        public bool UsarWsDigitalizados { get; set; }

        public bool ValidaUltimaVersion { get; set; }

        public decimal SalarioMinimo { get; set; }

        public string RutaPantallas { get; set; } = null!;

        public bool PantallaActiva { get; set; }

        public bool ActImprimePed { get; set; }

        public bool ActCCFirmas { get; set; }

        public bool ActImprimeManif { get; set; }

        public bool HabilitarWeb { get; set; }

        public bool GenImprimePed { get; set; }

        public bool GenImprimeManif { get; set; }

        public string PatenteDefault { get; set; } = null!;

        public string AduDesp { get; set; } = null!;

        public string AduEntr { get; set; } = null!;

        public int DesOrig { get; set; }

        public int MtrEntrImp { get; set; }

        public int MtrArriImp { get; set; }

        public int MtrSaliImp { get; set; }

        public int MtrEntrExp { get; set; }

        public int MtrArriExp { get; set; }

        public int MtrSaliExp { get; set; }

        public int SecDesp { get; set; }

        public string CveMant { get; set; } = null!;

        public string CvePrev { get; set; } = null!;

        public string EmpFac { get; set; } = null!;

        public int OperacionDefault { get; set; }

        public int IdOficinaExtra { get; set; }

        public string ClaveGateway { get; set; } = null!;

        public bool UtilizaGP { get; set; }

        public bool FacturaExpertti { get; set; }

        public bool UsarWSWec { get; set; }

        public bool GenerarConagtadus { get; set; }

        public bool ValidacionAutomatica { get; set; }

        public bool ironPDF { get; set; }

        public string TelConmutador { get; set; } = null!;

        public int ClienteMensajeria { get; set; }

        public string ClavePece { get; set; } = null!;

        public bool MostrarBloque { get; set; }

        public bool wsDODAOld { get; set; }

        public bool FotosPlacas { get; set; }

        public bool ModificarEstacion { get; set; }

        public int LimiteDePedimentos { get; set; }

        public bool RevImpoGlobal { get; set; }

        public bool ValidaCFDI { get; set; }

        public bool MandarCB { get; set; }

        //public string Identificador { get; set; } = null!;
        public string GTWFac { get; set; }

    }
}
