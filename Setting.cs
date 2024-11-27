using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;
using Game.UI.Widgets;
using System.Collections.Generic;

namespace RegionFlagIcons
{
    [FileLocation($"ModsSettings/{nameof(RegionFlagIcons)}/{nameof(RegionFlagIcons)}")]
    [SettingsUIGroupOrder(kButtonGroup)]
    [SettingsUIShowGroupName(kButtonGroup)]
    public class Setting : ModSetting
    {
        public const string kSection = "Main";

        public const string kButtonGroup = "Group";

        [SettingsUIButton]
        [SettingsUIConfirmation]
        [SettingsUISection(kSection)]
        public bool DeleteChangedIcons
        {
            set
            {
                Mod.DeleteChangedIcons();
            }
        }

        public Setting(IMod mod) : base(mod)
        {
        }

        public override void SetDefaults()
        {
            throw new System.NotImplementedException();
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "Region Flag Icons" },
                { m_Setting.GetOptionTabLocaleID(Setting.kSection), "Main" },

                { m_Setting.GetOptionGroupLocaleID(Setting.kButtonGroup), "Settings" },
                {m_Setting.GetOptionLabelLocaleID(nameof(Setting.DeleteChangedIcons)), "Uninstall"},
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.DeleteChangedIcons)),
                    $"This will remove the changed thumbnails from your AIL folder. Do this before uninstalling the mod, otherwise it will not have an effect."
                },
                {
                    m_Setting.GetOptionWarningLocaleID(nameof(Setting.DeleteChangedIcons)),
                    $"Uninstall the icons? Please make sure to also remove the mod from your playset to fully uninstall it"
                },
            };
        }

        public void Unload()
        {
        }
    }
}