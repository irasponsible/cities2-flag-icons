using System.Collections.Generic;
using System.IO;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Colossal.IO.AssetDatabase;
using AssetIconLibrary;
using Colossal.PSI.Environment;

namespace RegionFlagIcons
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(RegionFlagIcons)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);

        private static Setting m_Setting;
        private static string m_AssetPath;
        private static string _modDirectory;
        private static string _resourcesDirectory;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            {
                log.Info($"Region Flag Icons loaded");
                m_AssetPath = asset.path;
                _modDirectory = new FileInfo(m_AssetPath).Directory.FullName;
                _resourcesDirectory = Path.Combine(EnvPath.kUserDataPath, "ModsData", "RegionFlagIcons", "Resources");
            }

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));
            
            // Temporary, until AIL fixes this
            var oldAilFolder = Path.Combine(_modDirectory, "ail");
            if (Directory.Exists(oldAilFolder))
            {
                Directory.Delete(oldAilFolder, true);
            }
            AssetDatabase.global.LoadSettings(nameof(RegionFlagIcons), m_Setting, new Setting(this));
            //CheckFlagStyles();
        }

        public static void CheckFlagStyles()
        {
            /*var baseDir = Path.Combine(_dataDirectory, ".ail", "flags");

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

            File.Copy(sourceFile, destFile, true);*/
        }

        public static void ChangePackFlag(string folderName, string flag)
        {
            string folderPath = Path.Combine(_resourcesDirectory, folderName);
            string flagPath = Path.Combine(_resourcesDirectory, "flags", $"{flag}");
            log.Info("Starting ThumbnailProcessor with flagPath: " + flagPath + " and folderPath: " + folderPath);
            
            // Change thumbnails
            ThumbnailProcessor.ApplyFlagToFolder(flagPath, folderPath);
            
            // Change pack icon
            var targetPath = Path.Combine(folderPath, $"{folderName} Pack Filter.svg");
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
            var dir = new DirectoryInfo(Path.Combine(EnvPath.kUserDataPath, "ModsData", "RegionFlagIcons", "Resources"));
            if (!dir.Exists)
                return names.ToArray();
            foreach (var file in dir.GetFiles("*" + "png"))
            {
                names.Add(file);
            }
            return names.ToArray();
        }

        private static void CopyThumbnailsFromAIL()
        {
            // Not used
        }

        private static void CopyThumbnailsFromRFI()
        {
            var source = new DirectoryInfo(Path.Combine(_modDirectory, "Resources"));
            var target = new DirectoryInfo(Path.Combine(EnvPath.kUserDataPath, "ModsData", "RegionFlagIcons", "Resources"));
            CopyRecursively(source, target);
        }
        
        private static void CopyRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            if (!target.Exists)
                target.Create();
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
        
        public static IEnumerable<string> GetIconsRootFolders(string style) // parameter is optional
        {
            CopyThumbnailsFromRFI();
            yield return Path.Combine(EnvPath.kUserDataPath, "ModsData", "RegionFlagIcons", "Resources");
        }
    }
}