namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogodeCheckPoints
    {
        public int IDCheckPoint { get; set; } = 0;
        public string ClaveCheckPoint { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public bool Obligatorio { get; set; } = false;
        public bool Duplicidad { get; set; } = false;
        public string ListaDeIDPrecedencias { get; set; } = null!;
        public bool PrecedenciaObligatoria { get; set; } = false;
        public bool Automatico { get; set; } = false;
        public int IDDepartamento { get; set; } = 0;
        public int Idoficina { get; set; } = 0;
        public int TipoDeEvento { get; set; } = 0;
        public bool StatusWeb { get; set; } = false;
        public int OrdenDeDespliegue { get; set; } = 0;
        public bool Activo { get; set; } = false;
        public bool MostrarEnRegistros { get; set; } = false;
        public bool ObservacionObligatoria { get; set; } = false;
        public string Ultimo { get; set; } = null!;
        public string Primero { get; set; } = null!;
        public string Siguiente { get; set; } = null!;
        public string Anterior { get; set; } = null!;
        public List<int> ListaDeIDPrecedencias1 { get; set; } = new List<int>();
        public int IdDepartamentoDestino { get; set; }
        public bool ValidaPedimento { get; set; } = false;
        public bool Prevalida { get; set; } = false;
        public bool PreValidaCove { get; set; } = false;
        public bool ValidaProforma { get; set; } = false;
        public bool ValidaPartidas { get; set; } = false;
        public bool AsignarPedimento { get; set; } = false;
        public bool AsignacionAutomatica { get; set; } = false;
        public bool Reproceso { get; set; } = false;
        public bool DepOrigen { get; set; } = false;

        public bool CrearRef { get; set; }
        public List<ObservacionCheckpoint> Observaciones { get; set; }

    }
    public class ObservacionCheckpoint
    {
        public string Observacion { get; set; }
    }
}
