namespace LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos
{
    public class CatalogodePuestos
    {        
        public int IDPuesto { get; set; }    
        
        public string Puesto { get; set; }
        public int IdPuestoJefe { get; set; }
        public int IdNivelPuesto { get; set; }

        public bool TodasOficinas { get; set; }
        public bool TodosDepartamentos { get; set; }
        public bool TodasAduanas { get; set; }
        public bool AmbasOperaciones { get; set; }
    }
}
