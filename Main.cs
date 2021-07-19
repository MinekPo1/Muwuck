using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Owoify;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine.SceneManagement;

namespace Muwuck
{
    [BepInPlugin(Guid, Name, Version), BepInDependency("Terrain.MuckSettings")]
    public class Main : BaseUnityPlugin
    {
        public const string
            Name = "Muwuck",
            Author = "Terrain",
            Guid = Author + "." + Name,
            Version = "1.0.0.0";

        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;

        public static ConfigFile config = new ConfigFile(Path.Combine(Paths.ConfigPath, "owo.cfg"), true);
        public static ConfigEntry<Owoifier.OwoifyLevel> intensity = config.Bind("Muwuck", "intensity", Owoifier.OwoifyLevel.Owo, "Ascend when flying.");

        Main()
        {
            log = Logger;
            harmony = new Harmony(Guid);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);
            intensity.SettingChanged += (_1, _2) => {
                var texts = FindObjectsOfType<TMP_Text>();
                foreach (var text in texts) text.ForceMeshUpdate(true, true);
            };

            // SceneManager.sceneLoaded += (_1, _2) => {
            //     var texts = FindObjectsOfType<TMP_Text>();
            //     foreach (var text in texts) {
            //         text.textPreprocessor = text.textPreprocessor ?? new MuwuckPreprocessor();
            //         text.ForceMeshUpdate(true, true);
            //     }
            // };

            config.SaveOnConfigSet = true;
            harmony.PatchAll(assembly);
        }

        
    }
}