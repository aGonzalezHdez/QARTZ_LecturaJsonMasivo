namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos
{
    public class AsignarGuiasRespuesta
    {

        public int IdUsuarioAsignado { get; set; }
        public string Pedimento { get; set; } = null!;
        public int Disponibles { get; set; }
        public bool Prevalidar { get; set; }
        public int IdEvento { get; set; }

        public int IdReferencia { get; set; }
        public int IdOficina { get; set; }

        public int IdUsuario { get; set; }
        public string Patente { get; set; }

        public string UsuarioAsignado { get; set; }

    }
}
