using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesPrevalidador
{
    public class ArchivoA
    {
        private string bancoField;
        public string Banco
        {
            get { return bancoField; }
            set { bancoField = value; }
        }

        private string cajaField;
        public string Caja
        {
            get { return cajaField; }
            set { cajaField = value; }
        }

        private DateTime diaPagoField;
        public DateTime DiaPago
        {
            get { return diaPagoField; }
            set { diaPagoField = value; }
        }

        private string operacionField;
        public string Operacion
        {
            get { return operacionField; }
            set { operacionField = value; }
        }

        private string firmaField;
        public string Firma
        {
            get { return firmaField; }
            set { firmaField = value; }
        }

        private string pedimentoField;
        public string Pedimento
        {
            get { return pedimentoField; }
            set { pedimentoField = value; }
        }

        private string referenciaField;
        public string Referencia
        {
            get { return referenciaField; }
            set { referenciaField = value; }
        }

        private string transaccionField;
        public string Transaccion
        {
            get { return transaccionField; }
            set { transaccionField = value; }
        }

        private string idField;
        public string ID
        {
            get { return idField; }
            set { idField = value; }
        }

        private string rfcField;
        public string RFC
        {
            get { return rfcField; }
            set { rfcField = value; }
        }
    }
}
