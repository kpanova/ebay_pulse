using eBayPulse.Tools;

namespace eBayPulse.eBayApi.Call
{
    public class GetSessionID: Call
    {
        public GetSessionID(Context context)
            : base(context, CallName.GetSessionID)
        {
            CallSpecificInputXml = $"<RuName>{context.RuName}</RuName>";
        }

        public string SessionId { get; private set; }

        protected override bool ParseOutput(XmlReader xmlReader)
        {
            SessionId = xmlReader.GetValue("SessionID");
            if (SessionId == null)
            {
                return false;
            }
            return true;
        }
    }
}
