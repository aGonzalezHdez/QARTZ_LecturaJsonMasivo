using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface IBitacoraRepository
    {
        string SConexion { get; set; }

        List<BitacoraDeClientesDataGridView> Cargar(int MyIDCliente);

        int Insertar(Bitacora objBitacora);

        Bitacora LoadBitacora(int IDCliente, int IDCapturo, string Observaciones, string Metodo);
    }
}