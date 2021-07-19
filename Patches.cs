using HarmonyLib;
using Owoify;
using TMPro;

namespace Muwuck
{
    [HarmonyPatch(typeof(TMP_Text))]
    class Text
    {
        [HarmonyPatch("ParseInputText"), HarmonyPrefix]
        static void ParseInputText(TMP_Text __instance)
        {
            __instance.textPreprocessor = __instance.textPreprocessor ?? new MuwuckPreprocessor();
        }
    }

    [HarmonyPatch]
    class OwoChat
    {
        [HarmonyPatch(typeof(ChatBox), nameof(ChatBox.SendMessage)), HarmonyPrefix]
        static void SendMessage(ref string message)
        {
            message = Owoifier.Owoify(message, Main.intensity.Value);
        }
    }

    class MuwuckPreprocessor : ITextPreprocessor
    {
        public string PreprocessText(string orig) => Owoifier.Owoify(orig, Main.intensity.Value);
    }

    [HarmonyPatch(typeof(MuckSettings.Settings))]
    class Settings
    {
        [HarmonyPatch(nameof(MuckSettings.Settings.Gameplay)), HarmonyPostfix]
        static void Controls(MuckSettings.Settings.Page page)
        {
            page.AddScrollSetting("OwOification intensity", Main.intensity);
        }
    }
}