using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces
{
    public interface ICatalogoDeDocumentosRepository
    {
        bool BuscarPorIDReferencia(int IDReferencia, int IDTipoReferencia);
    }
}
