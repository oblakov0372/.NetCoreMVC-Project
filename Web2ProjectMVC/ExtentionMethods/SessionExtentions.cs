using System.Text.Json;

namespace Web2ProjectMVC.ExtentionMethods
{
    public static class SessionExtentions
    {
        public static void SetObject<T>(this ISession session,string key,T value)
        {
            if(value == null)
            {
                session.Remove(key);
                return;
            }
            string jsonData = JsonSerializer.Serialize(value);

            session.SetString(key, jsonData);
        }
        public static T GetObject<T>(this ISession instance, string key) where T : class
        {
            if (!instance.Keys.Contains(key))
                return null;

            string jsonData = instance.GetString(key);
            if (String.IsNullOrEmpty(jsonData))
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
