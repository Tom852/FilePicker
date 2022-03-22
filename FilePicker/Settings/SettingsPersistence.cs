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
        public static SettingsModel Load()
        {
            if (File.Exists(appDataFile))
            {
                try
                {
                    string raw = File.ReadAllText(appDataFile);
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

        public static void Store(SettingsModel s)
        {
            if (!File.Exists(appDataFile))
            {
                var dir = Path.GetDirectoryName(appDataFile);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var fs = File.Create(appDataFile);
                fs.Close();
            }

            using (StreamWriter sw = new StreamWriter(appDataFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, s);
            }
        }
    }
}
