namespace LibreriaClasesAPIExpertti.Entities.EntitiesClientes
{
    public class ClientesEmailExpediente
    {
        public int IdEmail { get; set; }

        public int IdCliente { get; set; }

        public string Email { get; set; } = string.Empty;

        public int IDDatosDeEmpresa { get; set; }

        public int IdOperacion { get; set; }

        public int IdOficina { get; set; }

        public string Empresa { get; set; } = string.Empty;

        public string Operacion { get; set; } = string.Empty;

        public string Oficina { get; set; } = string.Empty;


    }
}
