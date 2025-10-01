namespace LibreriaClasesAPIExpertti.Entities.EntitiesWs
{

    public enum TipoDeRespuesta
    {
        TR_Informacion,
        TR_Advertencia,
        TR_Error,
        TR_Excepcion,
        TR_Candado,
        TR_Normal,
        TR_Moneda
    }

    public abstract partial class BaseRespuestaWS
    {
        private TipoDeRespuesta m_tipoDeRespuesta;
        // Contiene el tipo de respuesta para esta instancia
        public TipoDeRespuesta TipoDeRespuesta
        {
            get
            {
                return m_tipoDeRespuesta;
            }
            set
            {
                m_tipoDeRespuesta = value;
            }
        }

        private string m_mensaje;

        // Contiene el mensaje interno de la respuesta asociada con la el tipo de naturaleza de la respuesta
        public string Mensaje
        {
            get
            {
                return m_mensaje;
            }
            set
            {
                m_mensaje = value;
            }
        }

    }
}
