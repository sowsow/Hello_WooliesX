using System;
using Newtonsoft.Json;

namespace WooliesX.Common.Extensions
{
    public static class Extensions
    {
        public static T FromJsonString<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJsonString<T>(this T @object)
        {
            if (@object == null)
            {
                throw new ArgumentNullException(nameof(@object));
            }

            return JsonConvert.SerializeObject(
                @object,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}
