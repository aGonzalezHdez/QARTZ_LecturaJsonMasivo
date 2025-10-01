using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Repositories.MSManifiestos.RepositoriesCMF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesCMF
{
    public class cmfCompleto: CustomerMasterFile
    {
        public List<CartaInstruccionesIdEmpresa> CartaInstrucciones { get; set; }

        public CatalogoDeCategorias Categoria { get; set; }

        public CatalogodeRieles Riel { get; set; }

        public List<CMFPartidas> CMFPartidas { get; set; }
    }
}
