namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesTranscar.Interfaces
{
    public interface ITransCarRepository
    {
        string SConexion { get; set; }

        List<string> EnviarTransCar(int idPredoda);
    }
}