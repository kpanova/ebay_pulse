using eBayPulse.Tools;

namespace eBayPulse.eBayApi.Call
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

        protected override bool ParseSpecificOutput(XmlReader xmlReader)
        {
            ErrorMessage = "Not implemented.";
            return false;
        }
    }
}
