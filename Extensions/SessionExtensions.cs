using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Web.Extensions
{
    // Extension class hỗ trợ lưu và đọc đối tượng (Object) từ Session dưới dạng chuỗi JSON
    public static class SessionExtensions
    {
        // Ghi đối tượng (Object) vào Session
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Đọc đối tượng (Object) từ Session
        public static T? GetObject<T>(this ISession session, string key)
        {
            var jsonString = session.GetString(key);
            return jsonString == null ? default : JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}
