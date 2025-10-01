namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class Bitacora
    {
        public int IDBitacora { get; set; }
        public int IDCliente { get; set; }
        public int Estatus { get; set; }
        public int IDUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
    }

}

