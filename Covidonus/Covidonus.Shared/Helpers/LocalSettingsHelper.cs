using Newtonsoft.Json;
using Windows.Storage;

namespace Covidonus.Shared.Helpers
{
    public static class LocalSettingsHelper
    {
        private static readonly ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

        public static void MarkContainer<T>(SettingContainer container, string containerValue, T value)
        {
#if !NETFX_CORE
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            LocalSettings.Values[containerValue] = json;
#else
            _ = LocalSettings.CreateContainer(container.ToString(), ApplicationDataCreateDisposition.Always);
            LocalSettings.Containers[container.ToString()].Values[containerValue] = value != null ? JsonConvert.SerializeObject(value) : null;
#endif
        }

        public static T GetContainerValue<T>(SettingContainer container, string containerValue)
        {
#if !NETFX_CORE
            var json = (string)ApplicationData.Current.LocalSettings.Values[containerValue];
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
#else
            _ = LocalSettings.CreateContainer(container.ToString(), ApplicationDataCreateDisposition.Always);
            if (!(LocalSettings.Containers[container.ToString()].Values[containerValue] is string currentValue))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(currentValue);
#endif
        }
    }

}
