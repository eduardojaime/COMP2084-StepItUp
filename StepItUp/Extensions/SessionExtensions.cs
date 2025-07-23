using Newtonsoft.Json;

namespace StepItUp.Extensions
{
    // Make this a static class
    // .NET will know automatically which class to extend by looking at this class name
    // Session + Extensions
    // From https://talkingdotnet.com/store-complex-objects-in-asp-net-core-session/
    public static class SessionExtensions
    {
        // Also, specify the interface you are extending in each method you create
        public static void SetObject(this ISession session, string key, object value)
        {
            // Seralizes the object to a JSON string and stores it in session
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            // Gets the string representation of the object stored in session
            var value = session.GetString(key);
            // Returns the deserialized object of type T
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
