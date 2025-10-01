using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUsuarios;

namespace LibreriaClasesAPIExpertti.Repositories.MSUsuarios.Interfaces
{
    public interface ICrearUsuarioRepository
    {
        string SConexion { get; set; }
        void CrearUsuario(CrearUsuario objCrearUsuario);
        List<CatalogoDeUsuarios> ValidaSiExisteUsuario(ValidarSiExiste objValidarSiExiste);
    }
}
