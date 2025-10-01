using System;
using System.Collections.Generic;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPreasignarRieles;

public partial class Preasignacionderieles
{
    public int IdPreasignacion { get; set; }

    public string GuiaHouse { get; set; }

    public string Categoria { get; set; }

    public string Usuario { get; set; }

    public DateTime Fecha { get; set; }
}
