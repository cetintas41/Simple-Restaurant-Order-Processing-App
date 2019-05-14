using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public static class Extensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string OrderStatusTranslator(int status, string lang)
        {
            if (lang == "tr")
            {
                switch (status)
                {
                    case 1:
                        return "Hazırlanıyor";
                    case 2:
                        return "Tamamlandı";
                    default:
                        return "Bekliyor";
                }
            }

            if (lang == "en")
            {
                switch (status)
                {
                    case 1:
                        return "Preparing";
                    case 2:
                        return "Done";
                    default:
                        return "Waiting";
                }
            }

            return null;
        }
    }
}
