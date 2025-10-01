namespace LibreriaClasesAPIExpertti.Repositories.MSConsumoExternos.RepositoriesVUCEM
{
    using System.IO;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.ServiceModel;
    using System.Text;
    using System.Xml;
    using System.ServiceModel.Channels;

    public class InspectorMensajesCliente : IClientMessageInspector, IEndpointBehavior
    {

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new InspectorMensajesCliente());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            //respuestaCorrecta = reply.ToString();

            var encoding = Encoding.UTF8;

            var config = new XmlWriterSettings();
            config.Encoding = encoding;

            var streamToWrite = new MemoryStream();
            XmlDictionaryWriter wrt = XmlDictionaryWriter.CreateTextWriter(streamToWrite);
            reply.WriteBodyContents(wrt);
            wrt.Flush();

            string XMLaString;
            XMLaString = encoding.GetString(streamToWrite.ToArray());
            XMLaString = XMLaString.Replace("ns3:", "ns2:");
            streamToWrite = new MemoryStream(encoding.GetBytes(XMLaString));
            XmlReader nuevo = XmlReader.Create(streamToWrite);
            var respaldo = reply;
            reply = Message.CreateMessage(respaldo.Version, default, nuevo);
            reply.Headers.CopyHeadersFrom(respaldo);
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var xml = request.ToString;

            return xml;
        }

    }
}
