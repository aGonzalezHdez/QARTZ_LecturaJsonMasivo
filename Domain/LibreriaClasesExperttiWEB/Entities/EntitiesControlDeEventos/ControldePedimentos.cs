namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos
{
    public class ControldePedimentos
    {
        public int IdPedimento { get; set; }

        public int IdReferencia { get; set; }

        public int IdOficina { get; set; }

        public int IdUsuario { get; set; }

        public string Patente { get; set; }

        public string Pedimento { get; set; }

        public DateTime Fecha { get; set; }

        public string Aduana { get; set; }

        public int Disponibles { get; set; }

    }
}
