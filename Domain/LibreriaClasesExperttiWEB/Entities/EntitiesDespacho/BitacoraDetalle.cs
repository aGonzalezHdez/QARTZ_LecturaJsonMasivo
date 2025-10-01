using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaClasesAPIExpertti.Entities.EntitiesDespacho
{
    public class BitacoraDetalle
    {
        public int IdDetalleBitacora { get; set; }
        public int IdRelacionBitacora { get; set; }
        public string GuiaHouse { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionIngles { get; set; }
        public decimal ValorDolares { get; set; }
        public int CantidadDeBultos { get; set; }
        public decimal Peso { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaLlegada { get; set; }
        public DateTime FechaSalida { get; set; }
        public int IdUsuario { get; set; }
        public int Proceso { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string CodigoPostal { get; set; }
        public string Moneda { get; set; }
        public string Cliente { get; set; }
        public int Tipo { get; set; }
        public int TipoServicio { get; set; }
        public bool Embalaje { get; set; }
        public bool Activo { get; set; }
        public int IDDatosDeEmpresa { get; set; }
        public int InsertoEmbalaje { get; set; }
        public int BorroEmbalaje { get; set; }
        public string Flujo { get; set; }
    }

}
