namespace LibreriaClasesAPIExpertti.Entities.EntitiesS3
{
    public class ent_CredencialesAmazonS3
    {
        public string AccessKeyID { get; set; }
        public string SecretKey { get; set; }
        public ent_RegionesAmazonS3 RegionEndpoint { get; set; }
    }
}
