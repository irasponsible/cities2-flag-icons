using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace RegionFlagIcons
{
    [FileLocation($"ModsSettings/{nameof(RegionFlagIcons)}/{nameof(RegionFlagIcons)}")]
    [SettingsUIGroupOrder(kRegionSettingsGroup, kPackSettingsGroup)]
    [SettingsUIShowGroupName(kRegionSettingsGroup, kPackSettingsGroup)]
    public class Setting : ModSetting
    {
        public enum FlagStyle_NA
        {
            USA,
            Canada
        }


        public const string kSection = "Main";

        public const string kRegionSettingsGroup = "Region Icon Settings";
        public const string kPackSettingsGroup = "Pack Icon Settings";

        public Setting(IMod mod) : base(mod)
        {
        }

        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public bool ApplyChanges
        {
            set
            {
                Mod.CheckFlagStyles();
                Application.Quit();
            }
        }
        
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public FlagStyle_NA NorthAmericanFlagStyle
        { get; set; } = FlagStyle_NA.USA;
        

        public override void SetDefaults()
        {
            NorthAmericanFlagStyle = FlagStyle_NA.USA;
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

                { m_Setting.GetOptionGroupLocaleID(Setting.kRegionSettingsGroup), "Region Icon Settings" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kPackSettingsGroup), "Asset Pack Icon Settings" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.NorthAmericanFlagStyle)), "North American Flag Style" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.NorthAmericanFlagStyle)),
                    $"Change the icon used for north american flags"
                },

                { m_Setting.GetEnumValueLocaleID(Setting.FlagStyle_NA.USA), "USA" },
                { m_Setting.GetEnumValueLocaleID(Setting.FlagStyle_NA.Canada), "Canada" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ApplyChanges)), "Restart Game to Apply Changes" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.ApplyChanges)),
                    $"Click to close the game. After a restart the changes will be applied. If you don't want to restart right now, the changes will take effect when the game is started next time."
                },
                {
                    m_Setting.GetOptionWarningLocaleID(nameof(Setting.ApplyChanges)),
                    "Game will be closed?"
                },
            };
        }

        public void Unload()
        {
        }
    }
}