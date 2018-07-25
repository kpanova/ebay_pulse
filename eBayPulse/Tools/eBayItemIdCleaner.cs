using System;
using System.Text.RegularExpressions;

namespace eBayPulse.Tools
{
    public class eBayItemIdCleaner
    {
        public String Value { get; private set; } = String.Empty;
        public bool IsValid { get; private set; } = false;
        public const uint Length = 12;

        public eBayItemIdCleaner(String dirtyString)
        {
            if (String.IsNullOrEmpty(dirtyString)) {
                return;
            }

            String [] patterns = {
                @"^(\d{12})$",
                @".*itm\/(\d{12})($|\?.+)",
                @".*itm\/.+\/(\d{12})($|\?.+)",
                @".*iid=(\d{12})($|\&.+)",
            };

            foreach (var pattern in patterns) {
                Match match = Regex.Match(dirtyString, pattern);

                if (!match.Success) {
                    continue;
                }

                Value = match.Groups[1].Value;
                IsValid = true;

                if (match.NextMatch().Success) {
                    Value = String.Empty;
                    IsValid = false;
                }

                return;
            }
        }
    }
}
