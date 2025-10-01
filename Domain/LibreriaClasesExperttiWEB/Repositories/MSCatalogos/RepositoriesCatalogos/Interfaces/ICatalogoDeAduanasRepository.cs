using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces
{
    public interface ICatalogoDeAduanasRepository
    {
        string SConexion { get; set; }
        public List<DropDownListDatos> Cargar(int IdOficina);
    }
}
