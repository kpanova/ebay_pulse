using eBayPulse.Tools;

namespace eBayPulse.eBayApi
{
    public class FetchToken: Call
    {
        public FetchToken(Context context)
            : base(context, CallName.FetchToken)
        {
            CallSpecificInputXml = $"";
        }

        public string eBayAuthToken {get; private set;}
        public string HardExpirationTime {get; private set;}

        protected override bool ParseOutput(XmlReader xmlReader)
        {
            ErrorMessage = "Not implemented.";
            return false;
        }
    }
}
