namespace Apotheek.Base.Identity.Api.Dtos.Accounts
{
    public class PharmacyInfoDto
    {
        public int Id { get; set; }
        public string AGB { get; set; }
        public string Name { get; set; }
        public bool IsDepot { get; set; }
        public int CustomerId { get; set; }
    }
}
