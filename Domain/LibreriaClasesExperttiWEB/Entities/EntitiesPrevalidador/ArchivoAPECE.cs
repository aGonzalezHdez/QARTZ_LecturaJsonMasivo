using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{
    public class ArchivoAPECE
    {
        private string BancoField;
        public string Banco
        {
            get { return BancoField; }
            set { BancoField = value; }
        }

        private string LineaField;
        public string Linea
        {
            get { return LineaField; }
            set { LineaField = value; }
        }

        private string AduanaField;
        public string Aduana
        {
            get { return AduanaField; }
            set { AduanaField = value; }
        }

        private string PatenteField;
        public string Patente
        {
            get { return PatenteField; }
            set { PatenteField = value; }
        }

        private string PedimentoField;
        public string Pedimento
        {
            get { return PedimentoField; }
            set { PedimentoField = value; }
        }

        private string RFCField;
        public string RFC
        {
            get { return RFCField; }
            set { RFCField = value; }
        }

        private string IDField;
        public string ID
        {
            get { return IDField; }
            set { IDField = value; }
        }

        private string ImpuestosField;
        public string Impuestos
        {
            get { return ImpuestosField; }
            set { ImpuestosField = value; }
        }

        private string FechaDePagoField;
        public string FechaDePago
        {
            get { return FechaDePagoField; }
            set { FechaDePagoField = value; }
        }

        private string HoraDePagoField;
        public string HoraDePago
        {
            get { return HoraDePagoField; }
            set { HoraDePagoField = value; }
        }

        private string OperacionField;
        public string Operacion
        {
            get { return OperacionField; }
            set { OperacionField = value; }
        }

        private string TransaccionField;
        public string Transaccion
        {
            get { return TransaccionField; }
            set { TransaccionField = value; }
        }
    }

}
