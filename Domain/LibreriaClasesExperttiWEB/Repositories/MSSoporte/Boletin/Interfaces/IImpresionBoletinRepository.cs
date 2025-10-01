namespace LibreriaClasesAPIExpertti.Repositories.MSSoporte.Boletin.Interfaces
{
    public interface IImpresionBoletinRepository
    {
        string SConexion { get; set; }  

        Task<string> GenerarBoletin(int IdBoletin);
    }
}
