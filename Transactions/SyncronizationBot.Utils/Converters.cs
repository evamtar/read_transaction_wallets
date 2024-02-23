

namespace SyncronizationBot.Utils
{
    public static class Converters
    {
        public static Dictionary<string, object> GetParameters(object[]? args)
        {
            var parameters = new Dictionary<string, object>();
            if (args != null && args.Any())
            {
                foreach (var obj in args)
                {
                    if (obj != null)
                        parameters.Add(obj?.ToString() ?? string.Empty, obj!);
                }
            }
            return parameters;
        }
    }
}
