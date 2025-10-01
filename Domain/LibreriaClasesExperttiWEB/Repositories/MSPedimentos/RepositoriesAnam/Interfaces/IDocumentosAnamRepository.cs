using LibreriaClasesAPIExpertti.Entities.EntitiesAnam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSPedimentos.RepositoriesAnam.Interfaces
{
    public interface IDocumentosAnamRepository
    {
        Task GuardarRequestResponse(RequestAnam request, string message, int idReferencia, int idDocumento, int? folioNumero, string folio, string errorLoc, string errorMsg, string errorType, int Consecutivo);
        List<ArchivosActualizarMasivo> ObtenerArchivos();
        List<ArchivosActualizarMasivo> ObtenerMasterSinFolios();
        Task ActualizarRequestResponse(string folioMaster, int? folioNumero, string folio, int IdDocumento);
        Task<bool> EliminarFolio(string Folio);
        Task<ResultadoOperacion> ProcesarStatusBatchAsync(string master, string patente);
        Task<ResultadoOperacion> ProcesarRenombrarBatchAsync(string master, string patente, int idReferencia, int idUsuario);
    }
}
