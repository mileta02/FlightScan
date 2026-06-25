namespace FlightScan.Core.Exceptions
{
    public class GlobalException : Exception
    {
        public string Code { get; set; }

        public GlobalException(string code, string message) : base(message)
        {
            Code = code;
        }
    }
}
