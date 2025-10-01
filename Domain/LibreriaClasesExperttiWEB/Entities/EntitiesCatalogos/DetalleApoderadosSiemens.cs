namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class DetalleApoderadosSiemens
    {      

        public int IdDetalle { get; set; }


        public int IdApoderado { get; set; }


        public string? Apoderado { get; set; }


        public string? IataDestino { get; set; }


        public int IdNotificador { get; set; }


        public string? Notificador { get; set; }


        public DateTime Fecha { get; set; }


        public int IdUsuario { get; set; }


        public string? UsuarioCapturo { get; set; }

    }
}

