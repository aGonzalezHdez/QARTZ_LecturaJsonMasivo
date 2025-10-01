using LibreriaClasesAPIExpertti.Entities.EntitiesTrazabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Repositories.MSTrazabilidad.RepositoriesNotificacion.Interfaces
{
    public interface IAlertasMasivas
    {
        string SConexion { get; set; }
        string EnviarAlertasMasivas(AlertaReferenciasRequest request);

    }
}
