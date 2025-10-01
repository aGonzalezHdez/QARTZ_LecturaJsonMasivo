namespace LibreriaClasesAPIExpertti.Entities.EntitiesBuscador
{
    public class BuscadorBuscador
    {
        public int idBuscar { get; set; }
        public string PalabraClave { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
        public int Operacion { get; set; }
        public int idTipodeMercancia { get; set; }
        public List<Imagenes_Bsc> Imagenes { get; set; }
        public List< DetalleSinonimosdeRiesgo_Bsc> SinonimosdeRiesgo { get; set; }

    }
}
