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

        private Setting m_Setting;
        private static string targetDirectory = Path.Combine(EnvPath.kUserDataPath, "ModsData", "AssetIconLibrary", "CustomThumbnails");

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            m_Setting = new Setting(this);
            m_Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(m_Setting));

            if (!Directory.Exists(targetDirectory))
            {
                log.Error($"Target directory {targetDirectory} does not exist. Files could not be copied.");
            }
            else
            {
                List<string> directories = ["de_thumbnails", "uk_thumbnails", "fr_thumbnails", "flags"];
                log.Info("AssetPath: " + asset.path);
                foreach (string directory in directories)
                {
                    FileInfo fi = new FileInfo(asset.path);
                    var path = Path.Combine(fi.Directory!.FullName, directory);
                    log.Info("FullPath: " + path);
                    CheckAndCopyFiles(path);
                }
            }

            AssetDatabase.global.LoadSettings(nameof(RegionFlagIcons), m_Setting, new Setting(this));
        }

        private static void CheckAndCopyFiles(string directory)
        {
            DirectoryInfo di = new DirectoryInfo(directory);
            if (!di.Exists)
            {
                log.Error($"Directory {directory} does not exist. Files could not be copied. Please re-download this mod");
                return;
            }

            /*if (!Directory.Exists(Path.Combine(targetDirectory, di.Name)))
            {
                Directory.CreateDirectory(Path.Combine(targetDirectory, di.Name));
            }*/
            foreach (FileInfo fi in di.GetFiles())
            {
                // Check if files are equal, if not, overwrite
                var path = Path.Combine(targetDirectory, fi.Name);
                //var path = Path.Combine(targetDirectory, di.Name, fi.Name);
                if (File.Exists(path))
                {
                    if (fi.Length != new FileInfo(path).Length)
                    {
                        File.Copy(fi.FullName, path, true);
                    }
                }
                else
                {
                    File.Copy(fi.FullName, path, false);
                }
            }
        }

        public static void DeleteChangedIcons()
        {
            // TODO: Fix this
            List<string> directories = ["de_thumbnails", "uk_thumbnails", "fr_thumbnails", "flags"];
            foreach (var directory in directories)
            {
                var path = Path.Combine(targetDirectory, directory);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (m_Setting != null)
            {
                m_Setting.UnregisterInOptionsUI();
                m_Setting = null;
            }
        }
    }
}