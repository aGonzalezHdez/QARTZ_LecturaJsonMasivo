namespace LibreriaClasesAPIExpertti.Entities.EntitiesReferencias
{
    public class Referencias
    {

        public int IDReferencia { get; set; }

        public string NumeroDeReferencia { get; set; }

        public DateTime FechaApertura { get; set; }

        public string AduanaEntrada { get; set; }

        public string AduanaDespacho { get; set; }

        public string Patente { get; set; }

        public int IDCliente { get; set; }

        public int Operacion { get; set; }

        public int IdDuenoDeLaReferencia { get; set; }

        public bool Subdivision { get; set; }

        public int IdOficina { get; set; }

        public bool PendientePorRectificar { get; set; }

        public int IdClienteDestinatario { get; set; }

        public string ReferenciaDestinatario { get; set; }

        public int IdGrupo { get; set; }

        public int IdMasterConsol { get; set; }

        public string ReferenciaEncriptada { get; set; }

        public int PaperLess { get; set; }

        public int IDDatosDeEmpresa { get; set; }

        public int IDEscalacion { get; set; }

        public string NoCuenta { get; set; }

        public int TipodeNotificacion { get; set; }

        public bool Remesa { get; set; }

        public bool CapturaenCasa { get; set; }

        public int IdEstacion { get; set; }

        public int IdReferenciaMadre { get; set; }

        public int NoRemesa { get; set; }

        public int Prioridad { get; set; }

        public bool InterOficinas { get; set; }

        public string Prefijo { get; set; }


        public string XMLCoveAnt { get; set; } = null!;
        public int S3 { get; set; }
        public string RUTAPDFS3PEDIMENTO { get; set; } = null!;
        public string RUTAPDFS3PEDIMENTOSIMPLE { get; set; } = null!;
        public string Pedimento { get; set; } = null!;
        public int IdDocumento { get; set; }

        public bool FraccionRojo { get; set; }
        public string PDFExpediente { get; set; }
        public string PDFPedimento { get; set; }
        public string PDFPedimentoSipl { get; set; }

        public string PDFFactura { get; set; }
        public string XMLFactura { get; set; }
        public string RUTAPDF { get; set; }

        public string RUTASIMPLIFICADOS { get; set; }
        public string RUTAEXPEDIENTE { get; set; }
        public string XMLCove { get; set; }
        public bool InterOficina { get; set; }
        public string LineaCierre { get; set; }
        public bool PedimentoGlobal { get; set; }

        public string DODA { get; set; }
        public string RutaDODA { get; set; }
        public string RutaFisicaDODA { get; set; }



    }

}
