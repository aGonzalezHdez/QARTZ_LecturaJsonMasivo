namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado.PredodaDHL
{
    public class CPorteConsultaIs
    {
        public string payload { get; set; }

        public List<errorsModel> errorsModel { get; set; }
        public List<string> errors { get; set; }
        public string statusCode { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }
    public class errorsModel
    {
        public string column { get; set; }
        public string value { get; set; }
        public string error { get; set; }
    }

}
