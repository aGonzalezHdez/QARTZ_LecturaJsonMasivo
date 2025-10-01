using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesBoletin
{
    public class Boletines
    {
        public int IdBoletin { get; set; }
        public string Titulo { get; set; }
        public string NoVersion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
        public int IdDepartamento { get; set; }
        public int IdDatosDeEmpresa { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public bool? TodoslosDeptos { get; set; }
        public bool? TodoslasApps { get; set; }
        public int? IdModulo { get; set; }
        public string? RutaS3 { get; set; }
        public int[] Departamentos { get; set; }
    }

    public class BoletinesInsert
    {
        public string Titulo { get; set; }
        public int IdDepartamento { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public bool? TodoslosDeptos { get; set; }
        public bool? TodoslasApps { get; set; }
        public int? IdModulo { get; set; }
        public int IdDatosDeEmpresa { get; set; }
        public int[] Departamentos { get; set; }
    }

    public class BoletinesUpdate
    {
        public int IdBoletin { get; set; }
        public string Titulo { get; set; }
        public int IdDepartamento { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public bool? TodoslosDeptos { get; set; }
        public bool? TodoslasApps { get; set; }
        public int? IdModulo { get; set; }
        public int IdDatosDeEmpresa { get; set; }
        public int[] Departamentos { get; set; }
    }


    public class Filtros
    {
        public bool? Activo { get; set; }
        public int? IdDepartamento { get; set; }
        public DateTime? FechaVigencia { get; set; }
        public bool? TodoslosDeptos { get; set; }
        public bool? TodoslasApps { get; set; }
        public int? IdModulo { get; set; }
        public int? IdDatosDeEmpresa { get; set; }
        public string? Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }

    }
}
