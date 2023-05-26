using Newtonsoft.Json;
using System.Text;

namespace SDATools;

internal static class Utils
{
    internal static async Task<string> ReadStringAsync(Stream stream)
    {
        byte[] tmp = new byte[stream.Length];
        await stream.ReadAsync(tmp);
        var json = Encoding.UTF8.GetString(tmp);
        return json;
    }

    internal static async Task<T?> TryParseJson<T>(Stream stream)
    {
        try
        {
            var json = await ReadStringAsync(stream);
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
        catch
        {
            return default;
        }
    }
}
