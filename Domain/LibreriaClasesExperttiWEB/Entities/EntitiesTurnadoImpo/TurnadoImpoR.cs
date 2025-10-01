using LibreriaClasesAPIExpertti.Entities.EntitiesControlDeEventos;
using LibreriaClasesAPIExpertti.Entities.EntitiesWs;
using LibreriaClasesAPIExpertti.Entities.EntitiesClientes;
using LibreriaClasesAPIExpertti.Entities.EntitiesWs;
using System.Data;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesTurnadoImpo
{

    public partial class TurnadoImpoR : BaseRespuestaWS
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks> 0.- Drawing.Color.Green
        /// 1.- Drawing.Color.Red
        /// 2.- Drawing.Color.Yellow
        /// 3.- Drawing.Color.Orange
        /// 4.- Drawing.Color.Blue
        /// 5.- Drawing.Color.White
        /// </remarks>
        private int BackColorField;
        public int BackColor
        {
            get
            {
                return BackColorField;
            }
            set
            {
                BackColorField = value;
            }
        }

        private bool AvisarNoManifestadoField;
        public bool AvisarNoManifestado
        {
            get
            {
                return AvisarNoManifestadoField;
            }
            set
            {
                AvisarNoManifestadoField = value;
            }
        }

        private bool AvisarEncargoClienteField;
        public bool AvisarEncargoCliente
        {
            get
            {
                return AvisarEncargoClienteField;
            }
            set
            {
                AvisarEncargoClienteField = value;
            }
        }

        private bool AvisarDiffAgenteAduField;
        public bool AvisarDiffAgenteAdu
        {
            get
            {
                return AvisarDiffAgenteAduField;
            }
            set
            {
                AvisarDiffAgenteAduField = value;
            }
        }

        private List<CartaInstrucciones> CARTADEINSTRUCCIONESField;
        public List<CartaInstrucciones> CARTADEINSTRUCCIONES
        {
            get
            {
                return CARTADEINSTRUCCIONESField;
            }
            set
            {
                CARTADEINSTRUCCIONESField = value;
            }
        }


        private string PrioridadField;
        public string Prioridad
        {
            get
            {
                return PrioridadField;
            }
            set
            {
                PrioridadField = value;
            }
        }


        private bool AvisarNubePrepagoField;
        public bool AvisarNubePrepago
        {
            get
            {
                return AvisarNubePrepagoField;
            }
            set
            {
                AvisarNubePrepagoField = value;
            }
        }

        private bool SolicitarPieceIdField;
        public bool SolicitarPieceId
        {
            get
            {
                return SolicitarPieceIdField;
            }
            set
            {
                SolicitarPieceIdField = value;
            }
        }

        private bool NotifyIsSubdivisionField;
        public bool NotifyIsSubdivision
        {
            get
            {
                return NotifyIsSubdivisionField;
            }
            set
            {
                NotifyIsSubdivisionField = value;
            }
        }
        private List<ControldeEventos> EventosField;
        public List<ControldeEventos> Eventos
        {
            get
            {
                return EventosField;
            }
            set
            {
                EventosField = value;
            }
        }
        private bool EscaneoCompletoField;
        public bool EscaneoCompleto
        {
            get
            {
                return EscaneoCompletoField;
            }
            set
            {
                EscaneoCompletoField = value;
            }
        }
        private bool SolicitarAlmacenField;
        public bool SolicitarAlmacen
        {
            get
            {
                return SolicitarAlmacenField;
            }
            set
            {
                SolicitarAlmacenField = value;
            }
        }
        private bool EnviarPoolAsignacionField;
        public bool EnviarPoolAsignacion
        {
            get
            {
                return EnviarPoolAsignacionField;
            }
            set
            {
                EnviarPoolAsignacionField = value;
            }
        }
        private bool NotifyPiezasCompDifOficinasField;
        public bool NotifyPiezasCompDifOficinas
        {
            get
            {
                return NotifyPiezasCompDifOficinasField;
            }
            set
            {
                NotifyPiezasCompDifOficinasField = value;
            }
        }
        private bool TurnadoParcialField;
        public bool TurnadoParcial
        {
            get
            {
                return TurnadoParcialField;
            }
            set
            {
                TurnadoParcialField = value;
            }
        }
        private bool NotifyTurnadoParcialOAsignacionField;
        public bool NotifyTurnadoParcialOAsignacion
        {
            get
            {
                return NotifyTurnadoParcialOAsignacionField;
            }
            set
            {
                NotifyTurnadoParcialOAsignacionField = value;
            }
        }
        private bool ReferenciaValidadaField;
        public bool ReferenciaValidada
        {
            get
            {
                return ReferenciaValidadaField;
            }
            set
            {
                ReferenciaValidadaField = value;
            }
        }

        private int TurnadoParcialOAsignacionField;
        public int TurnadoParcialOAsignacion
        {
            get
            {
                return TurnadoParcialOAsignacionField;
            }
            set
            {
                TurnadoParcialOAsignacionField = value;
            }
        }
        private int AvisarPrevioOConsolidadoField;
        public int AvisarPrevioOConsolidado
        {
            get
            {
                return AvisarPrevioOConsolidadoField;
            }
            set
            {
                AvisarPrevioOConsolidadoField = value;
            }
        }

        private int TotalPieceIdField;
        public int TotalPieceId
        {
            get
            {
                return TotalPieceIdField;
            }
            set
            {
                TotalPieceIdField = value;
            }
        }
    }
}
