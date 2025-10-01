namespace LibreriaClasesAPIExpertti.Entities.DespachoIntegrado.PredodaDHL
{
    public class UUIDCartaPorteDHL
    {
        public CFDICartaPorteDHL payload { get; set; }

        public List<errorsModel> errorsModel { get; set; }
        public List<string> errors { get; set; }
        public string statusCode { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }

}
