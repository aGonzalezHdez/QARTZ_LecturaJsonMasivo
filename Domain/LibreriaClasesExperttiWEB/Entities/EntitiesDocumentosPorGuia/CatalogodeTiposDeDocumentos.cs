namespace LibreriaClasesAPIExpertti.Entities.EntitiesDocumentosPorGuia
{
    public class CatalogodeTiposDeDocumentos
    {
        public int IdTipoDocumento { get; set; }
        public string TipodeDocumento { get; set; }
        public string Identificador { get; set; }
        public bool Bajar_Web { get; set; }
        public bool Subir_WEB { get; set; }
        public int OrdenDeDespliegue { get; set; }
        public bool Activo { get; set; }
        public int IdDocumentoVuce { get; set; }
        public int IDRequisitos { get; set; }

        public bool AdjuntarExp { get; set; }

        public int OrdenExp { get; set; }

        public int IdDocumentoGP { get; set; }

        public bool MostrarWEB { get; set; }

        public bool DHL { get; set; }

        public bool Elimina { get; set; }
        public string EliminaWEB { get; set; }
        public string TipoArchivo { get; set; }
        public int Operacion { get; set; }
        public bool EXPEDIENTEDIGITAL { get; set; }
        public string NombreExpediente { get; set; }
        public int OrdenExAA { get; set; }
        public int TipoDoc { get; set; }
        public bool ComplementoObligatorio { get; set; }
        public string ComplementoDesc { get; set; }
        public int Consecutivo { get; set; }

    }
}
