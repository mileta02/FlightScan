namespace FlightScan.Core.Config
{
    public class JwtSettings
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public int ExpirationHours { get; set; }
    }
}
