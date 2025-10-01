using LibreriaClasesAPIExpertti.Entities.EntitiesCatalogos;
using LibreriaClasesAPIExpertti.Entities.EntitiesUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSCatalogos.RepositoriesCatalogos.Interfaces
{
    public interface ICatalogodeCheckPointsRepository
    {
        public Task<int> Insertar(CatalogodeCheckPoints objCatalogodeCheckPoints);
        public CatalogodeCheckPoints BuscarId(int IDCheckPoint, int Idoficina, int IdReferencia);
        public CatalogodeCheckPoints Buscar(int IDCheckPoint, int Idoficina);
        public Task<CatalogodeCheckPoints> BuscarPorDepto(int IDCheckPoint, int Idoficina, int IdDepartamento, int idReferencia);
        public List<DropDownListDatos> CargarEventosdeSalida(int IdDepartamento, int IdOficina, int Operacion, int IdCliente);
        public List<DropDownListDatos> CargarEventosdeWECWEB(int IdDepartamento, int IdOficina);
        public List<DropDownListDatos> CargarEventosdeSalidaDetenidos(int IdDepartamento, int IdOficina, int Operacion);
        public List<DropDownListDatos> CargarRectificacion();
        public List<DropDownListDatos> CargarEventosdeSalidaDetenidosWEB(int IdDepartamento, int IdOficina, int Operacion);
        public List<DropDownListDatos> CargarEventosdeSalidaRegreso(int IdDepartamento, int IdOficina, int Operacion);
        public List<DropDownListDatos> CargarEventosdeSalidaRegresoWEB(int IdDepartamento, int IdOficina, int Operacion);

    }
}
