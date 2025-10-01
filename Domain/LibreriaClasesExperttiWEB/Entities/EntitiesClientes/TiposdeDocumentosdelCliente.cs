namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class TiposdeDocumentosdelCliente
    {

        public int IDTiposDeDocumentosDeCliente { get; set; }

        public string TipoDeDocumento { get; set; } = null!;

        public bool AplicaVigencia { get; set; }

        public int FechaFinalObligada { get; set; }

        public int FiguraFiscal { get; set; }

        public int SubTipoDeDocumento { get; set; }

        public int ID_Requisitos { get; set; }

        public string Ayuda { get; set; } = null!;

        public string NombreDocumento { get; set; } = null!;
    }
}