using System.Globalization;

namespace ASPNETLIVE.Services.ThaiDate
{
    public class ThaiDate : IThaiDate
    {
        public string ShowThaiDate()
        {
            return DateTime.Now.ToString("dd MMM yyyy", new CultureInfo("th-TH"));
        }
    }
}
