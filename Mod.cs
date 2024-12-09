using System.Collections.Generic;
using System.IO;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Colossal.IO.AssetDatabase;
using Colossal.PSI.Environment;

namespace RegionFlagIcons
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(RegionFlagIcons)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);

        private static Setting m_Setting;
        private static string m_AssetPath;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            {
                log.Info($"Region Flag Icons loaded");
                m_AssetPath = asset.path;
            }

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

            AssetDatabase.global.LoadSettings(nameof(RegionFlagIcons), m_Setting, new Setting(this));
            CheckFlagStyles();
        }


        public static void CheckFlagStyles()
        {
            var baseDir = Path.Combine(new FileInfo(m_AssetPath).Directory.FullName, "ail", "flags");

            var naDir = Path.Combine(baseDir, "North American");
            string fileName = "";
            switch (m_Setting.NorthAmericanFlagStyle)
            {
                case Setting.FlagStyle_NA.USA:
                    fileName = "USA.svg";
                    break;
                case Setting.FlagStyle_NA.Canada:
                    fileName = "Canada.svg";
                    break;
            }

            var sourceFile = Path.Combine(naDir, fileName);
            var destFile = Path.Combine(baseDir, "North American.svg");

            File.Copy(sourceFile, destFile, true);
        }


        public void OnDispose()
        {
            /*if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }*/
        }
    }
}