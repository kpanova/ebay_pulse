using eBayPulse.Tools;

namespace eBayPulse.eBayApi.Call
{
    public class GeteBayOfficialTime: Call
    {
        public GeteBayOfficialTime(Context context)
            : base(context, CallName.GeteBayOfficialTime)
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
