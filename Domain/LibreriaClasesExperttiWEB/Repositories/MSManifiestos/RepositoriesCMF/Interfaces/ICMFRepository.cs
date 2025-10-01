//using Chilkat;
using LibreriaClasesAPIExpertti.Entities.EntitiesCMF;
using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;
using System.Data;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF.Interfaces
{
    public interface ICMFRepository
    {
        string sConexion { get; set; }

        List<string> Guardar(cmfEncabezado objNuevo);
        List<string> Subir(string ArchivoCMF, string NombreArchivo);
        ManifiestosErrores SubirSDAAAporFTP();
        ManifiestosErrores SubirSDAAAporDirectorio();
        DataTable CargarDatosContactoCliente(int IdDtosEmpresa, string Guia);
        bool ModificarDatosContactoCliente(int IdCMF, string RFC, string Telefono, string Email);

    }
}