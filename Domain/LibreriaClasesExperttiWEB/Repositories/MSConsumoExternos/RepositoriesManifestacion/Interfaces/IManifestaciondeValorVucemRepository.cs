using LibreriaClasesAPIExpertti.Entities.EntitiesManifestacion;
using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;

namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesManifestacion.Interfaces
{
    public interface IManifestaciondeValorVucemRepository
    {
        string SConexion { get; set; }

        int Insertar(ManifestaciondeValorVucem objManifestaciondeValorVucem);

        ManifestaciondeValorVucem Buscar(string NumerodeReferencia);

        bool Modificar(ManifestaciondeValorVucem objManifestaciondeValorVucem);

        bool Borrar(int idMV);

        int getMetododeValoracion(string numerodeReferencia);
        bool getVinculacion(string numerodeReferencia);

    }
}
