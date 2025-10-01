using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Services.Equivalencia
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using LibreriaClasesAPIExpertti.Entities.EntitiesTurnadoImpo;
    using LibreriaClasesAPIExpertti.Entities.EntitiesEquialencia;

    public partial class Equivalencia
    {
        public IConfiguration _configuration;
        public string SConexion { get; set; }
        public Equivalencia(IConfiguration configuration)
        {
            _configuration = configuration;
            SConexion = _configuration.GetConnectionString("dbCASAEI")!;
        }

        public async Task<EquivalenciaR> ProcesarXml(string Xml)
        {
            var RespuestaImpo = new EquivalenciaR();




            return RespuestaImpo;
        }
    }
}
