using System.Collections.Generic;
using System.IO;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Colossal.IO.AssetDatabase;

namespace RegionFlagIcons
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(RegionFlagIcons)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);

        private static Setting m_Setting;
        private static string m_AssetPath;
        private static string _baseDirectory;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            {
                log.Info($"Region Flag Icons loaded");
                m_AssetPath = asset.path;
                _baseDirectory = new FileInfo(m_AssetPath).Directory.FullName;
            }

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            
            // Temporary, until AIL fixes this
            var oldAilFolder = Path.Combine(_baseDirectory, "ail");
            if (Directory.Exists(oldAilFolder))
            {
                Directory.Delete(oldAilFolder, true);
            }
            AssetDatabase.global.LoadSettings(nameof(RegionFlagIcons), m_Setting, new Setting(this));
            CheckFlagStyles();
        }

        public static void CheckFlagStyles()
        {
            var baseDir = Path.Combine(_baseDirectory, ".ail", "flags");

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

        public static void ChangePackFlag(string folderName, string flag)
        {
            string folderPath = Path.Combine(_baseDirectory, ".ail", folderName);
            string flagPath = Path.Combine(_baseDirectory, ".ail", "flags", $"{flag.Replace(".png", ".svg")}");
            log.Info("Starting ThumbnailProcessor with flagPath: " + flagPath + " and folderPath: " + folderPath);
            
            // Change thumbnails
            ThumbnailProcessor.ApplyFlagToFolder(flagPath, folderPath);
            
            // Change pack icon
            var targetPath = Path.Combine(folderPath, $"{folderName} Pack Filter.svg");
            log.Info($"Copying pack icon from {flagPath} to {targetPath}");
            File.Copy(flagPath, targetPath, true);
        }


        public void OnDispose()
        {
            /*if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }*/
        }

        public static FileInfo[] GetFlagFiles()
        {
            List<FileInfo> names = [];
            var dir = new DirectoryInfo(Path.Combine(_baseDirectory, ".ail", "flags"));
            foreach (var file in dir.GetFiles("*" + "png"))
            {
                names.Add(file);
            }
            return names.ToArray();
        }
    }
}