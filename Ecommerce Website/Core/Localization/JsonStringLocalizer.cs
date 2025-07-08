using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace Ecommerce_Website.Core.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly JsonSerializer _serializer = new ();
        public LocalizedString this[string name] 
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value);
            }
        }


        public LocalizedString this[string name, params object[] arguments] 
        {  
            get
            {
                var actualValue = this[name];
                return !actualValue.ResourceNotFound 
                    ? new LocalizedString(name, string.Format(actualValue.Value, arguments)) 
                    : actualValue;

            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filePath = $"Core/Localization/Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";

            using FileStream Stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader StramReader = new StreamReader(Stream);
            using JsonTextReader Reader = new JsonTextReader(StramReader);

            while (Reader.Read())
            {
                if (Reader.TokenType != JsonToken.PropertyName)
                    continue;
                
                var propertyName = Reader.Value as string;
                Reader.Read(); 
                var value = _serializer.Deserialize<string>(Reader);
                yield return new LocalizedString(propertyName, value, false);
               
            }
        }

        private string GetString (string key)
        {
            var filePath = $"Core/Localization/Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
            var fullFilePath = Path.GetFullPath(filePath);

            if (File.Exists(fullFilePath))
            {
                var Result = GetValueFromJson(key, fullFilePath);
                return Result;
            }
            return string.Empty;
        }

        private string GetValueFromJson(string PropertyName,string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(PropertyName))
                return string.Empty;

            using FileStream Stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader StramReader = new StreamReader(Stream);
            using JsonTextReader Reader = new JsonTextReader(StramReader);

            while (Reader.Read())
            {
                if (Reader.TokenType == JsonToken.PropertyName && Reader.Value as string== PropertyName)
                {
                    Reader.Read(); 
                    return _serializer.Deserialize<string>(Reader);
                }
            }
            return string.Empty;

        }
    }
}
