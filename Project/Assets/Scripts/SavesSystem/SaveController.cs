using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Entity.Player;
using UnityEngine;

namespace SavesSystem {
    public static class SaveController {
        public static string Path = Application.persistentDataPath + "/test.dat";
        public static bool LoadOnStart = false;
        public static void SaveGame() {
            var formatter = new BinaryFormatter();
            var fs = new FileStream(Path, FileMode.Create);

            var data = new SaveObject();

            formatter.Serialize(fs, data);
            fs.Close();
        }

        public static void LoadGame() {
            if (File.Exists(Path)) {
                var formatter = new BinaryFormatter();
                var fs = new FileStream(Path, FileMode.Open);

                var data = formatter.Deserialize(fs) as SaveObject;
                fs.Close();
                data.Load();
            } else {
              Debug.LogError("File not found " + Path);  
            }
        }

        public static bool UpdatePath(int saveIndex) {
            Path = Application.persistentDataPath + "/save" + saveIndex + ".save";
            return File.Exists(Path);
        }
    }
}
