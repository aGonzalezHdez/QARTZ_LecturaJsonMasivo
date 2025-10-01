using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado
{
    public class CatalogoDeReportingService
    {
        public int IdReporting { get; set; }
        public string NombreReporting { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string PathPdf { get; set; }
        public bool GenerarPdf { get; set; }
        public string NuevoUrl { get; set; }
        public string NuevoPath { get; set; }
    }

}
