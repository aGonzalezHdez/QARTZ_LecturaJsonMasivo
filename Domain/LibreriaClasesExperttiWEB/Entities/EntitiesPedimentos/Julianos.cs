namespace LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos
{
    public class Julianos
    {
        public int IDDatosDeEmpresa { get; set; }
        public int idJuliano { get; set; }
        public string Aduana { get; set; }
        public string Patente { get; set; }
        public int Movimiento { get; set; }
        public string Prevalidador { get; set; }
        public int Consecutivo { get; set; }
        public int Operacion { get; set; }
        public int idOficina { get; set; }
        public int IdUsuario { get; set; }

        public string Representante { get; set; }

        public List<idsReferencias> Referencias { get; set; }


    }

    public class idsReferencias
    {
        public int idReferencia { get; set; }

    }

}
