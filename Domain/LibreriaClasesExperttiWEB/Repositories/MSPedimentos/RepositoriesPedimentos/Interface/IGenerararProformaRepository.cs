using LibreriaClasesAPIExpertti.Entities.EntitiesPedimentos;
using System.Threading.Tasks;

public interface IGenerararProformaRepository
{
    Task<(string salida, string error, int codigoSalida, string rutaArchivoDestino)> GenerarProformaAsync(GeneraProformaRequest request);
    Task<string> EjecutarProcesoAsync(string referencia);


}