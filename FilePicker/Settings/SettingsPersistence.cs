using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePicker.Settings
{
    public static class SettingsPersistence
    {
        private static readonly string appDataFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TomsFilePicker/settings.json");
        private static JsonSerializer serializer = new JsonSerializer();

        public static SettingsModel LoadFromAppdata() => Load(appDataFile);

        public static void StoreToAppdata(SettingsModel s) => Store(s, appDataFile);

        public static SettingsModel Load(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    string raw = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<SettingsModel>(raw);

                } catch
                {
                    return SettingsModel.GetDefaultInstance();
                }
            }
            else
            {
                return SettingsModel.GetDefaultInstance();
            }
        }

        public static void Store(SettingsModel s, string path)
        {
            if (!File.Exists(path))
            {
                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var fs = File.Create(path);
                fs.Close();
            }

            using (StreamWriter sw = new StreamWriter(path))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, s);
            }
        }
    }
}
