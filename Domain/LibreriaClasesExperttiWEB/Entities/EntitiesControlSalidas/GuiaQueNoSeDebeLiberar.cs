namespace LibreriaClasesAPIExpertti.Entities.EntitiesControlSalidas
{
    using System;

    public class GuiaQueNoseDebeLiberar
    {
        public int IDNoLiberar { get; set; }
        public string NumeroDeGuia { get; set; }
        public string Motivo { get; set; }
        public DateTime FechaCaptura { get; set; }
        public bool Estatus { get; set; }
        public bool Unitarias { get; set; }
        public int IdUsuarioAlta { get; set; }
        public int IDUsuarioBaja { get; set; }
        public string Posicion { get; set; }
        public DateTime FechaBaja { get; set; }
        public bool Activa { get; set; }
        public bool TodaslasAreas { get; set; }
        public int IDDepartamento { get; set; }
        public int IdTipoError { get; set; }
        public int IdTipoErrorFiltro { get; set; }

    }

}
