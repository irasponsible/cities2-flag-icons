using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Colossal.PSI.Environment;
using Game.UI.Widgets;
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
        
        
        
        [SettingsUIMultilineText]
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public string RestartGameText => string.Empty;

        private string _flagFR = "France.png";
        [SettingsUIDropdown(typeof(Setting), nameof(GetFlagDropdownItems))]
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public string FlagFR
        {
            get => _flagFR;
            set
            {
                _flagFR = value;
                Mod.ChangePackFlag("FR", value);
            }
        }
        
        private string _flagDE = "Germany.png";
        [SettingsUIDropdown(typeof(Setting), nameof(GetFlagDropdownItems))]
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public string FlagDE
        {
            get => _flagDE;
            set
            {
                _flagDE = value;
                Mod.ChangePackFlag("DE", value);
            }
        }
        
        private string _flagUK = "United Kingdom.png";
        [SettingsUIDropdown(typeof(Setting), nameof(GetFlagDropdownItems))]
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public string FlagUK
        {
            get => _flagUK;
            set
            {
                _flagUK = value;
                Mod.ChangePackFlag("UK", value);
            }
        }
        
        private string _flagJP = "Japan.png";
        [SettingsUIDropdown(typeof(Setting), nameof(GetFlagDropdownItems))]
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public string FlagJP
        {
            get => _flagJP;
            set
            {
                _flagJP = value;
                Mod.ChangePackFlag("JP", value);
            }
        }
        
        private string _flagEE = "Ukraine.png";
        [SettingsUIDropdown(typeof(Setting), nameof(GetFlagDropdownItems))]
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public string FlagEE
        {
            get => _flagEE;
            set
            {
                _flagEE = value;
                Mod.ChangePackFlag("EE", value);
            }
        }
        
        private string _flagCN = "China.png";
        [SettingsUIDropdown(typeof(Setting), nameof(GetFlagDropdownItems))]
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public string FlagCN
        {
            get => _flagCN;
            set
            {
                _flagCN = value;
                Mod.ChangePackFlag("CN", value);
            }
        }
        
        private string _flagSW = "Arizona.png";
        [SettingsUIDropdown(typeof(Setting), nameof(GetFlagDropdownItems))]
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public string FlagSW
        {
            get => _flagSW;
            set
            {
                _flagSW = value;
                Mod.ChangePackFlag("SW", value);
            }
        }
        
        private string _flagNE = "New York City.png";
        [SettingsUIDropdown(typeof(Setting), nameof(GetFlagDropdownItems))]
        [SettingsUISection(kSection, kRegionSettingsGroup)]
        public string FlagNE
        {
            get => _flagNE;
            set
            {
                _flagNE = value;
                Mod.ChangePackFlag("NE", value);
            }
        }
        
        
        
        

        public DropdownItem<string>[] GetFlagDropdownItems()
        {
            var names = Mod.GetFlagFiles();

            List<DropdownItem<string>> items = new List<DropdownItem<string>>();
            foreach(var s in names)
            {
                items.Add(new DropdownItem<string>()
                {
                    value = s.Name,
                    displayName = s.Name.Replace(".png", ""),
                });
            }

            return items.ToArray();
        }
        
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
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.RestartGameText)), "All changes to the flags require a game restart to work" },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FlagFR)), "FR Pack Flag" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.FlagFR)),
                    $"Change the icon used for French Region Pack"
                },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FlagDE)), "DE Pack Flag" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.FlagDE)),
                    $"Change the icon used for German Region Pack"
                },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FlagUK)), "UK Pack Flag" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.FlagUK)),
                    $"Change the icon used for UK Region Pack"
                },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FlagJP)), "JP Pack Flag" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.FlagJP)),
                    $"Change the icon used for Japan Region Pack"
                },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FlagEE)), "EE Pack Flag" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.FlagEE)),
                    $"Change the icon used for Eastern Europe Region Pack"
                },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FlagCN)), "CN Pack Flag" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.FlagCN)),
                    $"Change the icon used for China Region Pack"
                },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FlagSW)), "USSW Pack Flag" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.FlagSW)),
                    $"Change the icon used for US South West Region Pack"
                },
                
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.FlagNE)), "USNE Pack Flag" },
                {
                    m_Setting.GetOptionDescLocaleID(nameof(Setting.FlagNE)),
                    $"Change the icon used for US North East Region Pack"
                },
                

                
                
            };
        }

        public void Unload()
        {
        }
    }
}