using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSClientes.RepositoriesClientes.Interfaces
{
    public interface ICartaDeInstruccionesRepository
    {
        string SConexion { get; set; }

        List<CartaInstruccionesIdEmpresa> CargarCartadeInstruccionesIdCliente(int IdCliente, int IDDatosDeEmpresa);

        double UltimoValor(int IdCliente, int IdOficina);


        bool RangoValorInicial(int IdCliente, double MyValor, int IdOficina);


        int Insertar(CartaInstrucciones objCartaDeInstrucciones);


        int Modificar(CartaInstrucciones objCartaDeInstrucciones);


        List<CartaInstrucciones> CargarporOficina(int IdCliente, int IdOficina);

        bool Eliminar(int IDCarta);  
 
    }
}