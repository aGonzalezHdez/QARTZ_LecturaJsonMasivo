using Chilkat;
using LibreriaClasesAPIExpertti.Entities.EntitiesTurnadoImpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class ProcesarGuiaRespuesta
    {
        private int EnUnidadField;

        private int TotalField;

        private int PendientesField;

        private string IATAField;
        private string GuiaHouseField;

        private List<PieceId> pieceIdsField;

        private int IdRelacionBitacoraField;
        private String ErrorField;
        private Boolean HasErrorField;

        public int EnUnidad
        {
            get
            {
                return EnUnidadField;
            }
            set
            {
                EnUnidadField = value;
            }
        }
        public Boolean HasError
        {
            get
            {
                return HasErrorField;
            }
            set
            {
                HasErrorField = value;
            }
        }
        public string Error
        {
            get
            {
                return ErrorField;
            }
            set
            {
                ErrorField = value;
            }
        }
        public string GuiaHouse
        {
            get
            {
                return GuiaHouseField;
            }
            set
            {
                GuiaHouseField = value;
            }
        }

        public int Total
        {
            get
            {
                return TotalField;
            }
            set
            {
                TotalField = value;
            }
        }

        public int Pendientes
        {
            get
            {
                return PendientesField;
            }
            set
            {
                PendientesField = value;
            }
        }

        public string IATA
        {
            get
            {
                return IATAField;
            }
            set
            {
                IATAField = value;
            }
        }

        public List<PieceId> pieceIds
        {
            get
            {
                return pieceIdsField;
            }
            set
            {
                pieceIdsField = value;
            }
        }

        public int IdRelacionBitacora
        {
            get
            {
                return IdRelacionBitacoraField;
            }
            set
            {
                IdRelacionBitacoraField = value;
            }
        }
    }
}
