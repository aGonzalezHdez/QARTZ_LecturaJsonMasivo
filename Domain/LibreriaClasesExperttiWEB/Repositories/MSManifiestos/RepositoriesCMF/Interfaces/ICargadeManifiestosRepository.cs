using LibreriaClasesAPIExpertti.Entities.EntitiesOperaciones;

namespace LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF.Interfaces
{
    public interface ICargadeManifiestosRepository
    {
        string SConexion { get; set; }

        ManifiestosErrores CargarManifiestoDHL(ManifiestoFile objManifiestoDHLFile);

        ManifiestosErrores CargarManifiestoFedex(ManifiestoFile objManifiestoFedexFile);

        ManifiestosErrores CargarManifiestoPieceIDs(ManifiestoFile objManifiestoFile);

        ManifiestosErrores CargarSafety(ManifiestoFile objSafety);

        Task<ManifiestosErrores> CargarManifiestoJCJFAsync(ManifiestoFile objJCJF);

        ManifiestosErrores CargarManifiestoCMF(ManifiestoFile objManifiesto);

    }
}
