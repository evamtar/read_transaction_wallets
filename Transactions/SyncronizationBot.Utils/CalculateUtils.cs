

namespace SyncronizationBot.Utils
{
    public class CalculateUtils
    {
        public static decimal? DoCalculateMarketCap(decimal? price, decimal supply) 
        {
            return price * supply;
        }

        public static decimal? DoCalculatePrice(decimal? marketcap, decimal supply)
        {
            return supply / marketcap;
        }
    }
}
