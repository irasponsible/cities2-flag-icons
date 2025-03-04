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
        public static ILog log = LogManager.GetLogger($"{nameof(RegionFlagIcons)}")
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
            CheckFlagStyles();
        }

        public static void CheckFlagStyles()
        {
            var baseDir = Path.Combine(_resourcesDirectory, "flags");

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
            string folderPath = Path.Combine(_resourcesDirectory, folderName);
            string flagPath = Path.Combine(_resourcesDirectory, "flags", $"{flag}");
            log.Debug("Starting ThumbnailProcessor with flagPath: " + flagPath + " and folderPath: " + folderPath);
            
            // Change thumbnails
            ThumbnailProcessor.ApplyFlagToFolder(flagPath, folderPath);
            
            // Change pack icon
            var flagPathSvg = Path.Combine(_resourcesDirectory, "flags", $"{flag.Replace(".png", ".svg")}");
            var targetPath = Path.Combine(folderPath, $"{folderName} Pack Filter.svg");
            log.Debug($"Copying Pack Filter {flagPathSvg} to {targetPath}");
            File.Copy(flagPathSvg, targetPath, true);
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
            var dir = new DirectoryInfo(Path.Combine(EnvPath.kUserDataPath, "ModsData", "RegionFlagIcons", "Resources", "flags"));
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
            CopyAll(source, target);
        }
        
        private static void CopyAll(DirectoryInfo directory, DirectoryInfo target)
        {
            if (!directory.Exists)
            {
                return;
            }

            target.Create();

            //Now Create all of the directories
            foreach (var dirPath in Directory.GetDirectories(directory.FullName, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(directory.FullName, target.FullName));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (var newPath in Directory.GetFiles(directory.FullName, "*.*", SearchOption.AllDirectories))
            {
                var path = newPath.Replace(directory.FullName, target.FullName);
                if (!File.Exists(path))
                    File.Copy(newPath, path, false);
            }
        }
        
        public static IEnumerable<string> GetIconsRootFolders(string style) // parameter is optional
        {
            CopyThumbnailsFromRFI();
            yield return Path.Combine(EnvPath.kUserDataPath, "ModsData", "RegionFlagIcons", "Resources");
        }
    }
}