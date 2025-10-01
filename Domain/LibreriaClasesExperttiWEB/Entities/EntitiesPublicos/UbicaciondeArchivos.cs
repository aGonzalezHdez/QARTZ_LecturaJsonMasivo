namespace LibreriaClasesAPIExpertti.Entities.EntitiesPublicos
{
    public class UbicaciondeArchivos
    {
        public int IdUbicacion { get; set; }
        public int IdOficina { get; set; }

        public string Patente { get; set; }

        public string Descripcion { get; set; }

        public string Ubicacion { get; set; }

        public string UbicacionAnterior { get; set; }
    }
}

