using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceConfiguration
{
    public class UserServiceSettingsSectionGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty("masterSettings")]
        public MasterSettingsSection MasterSettings
        {
            get { return (MasterSettingsSection)base.Sections["masterSettings"]; }
        }

        [ConfigurationProperty("slavesSettings")]
        public SlavesSettingsSection SlavesSettings
        {
            get { return (SlavesSettingsSection)base.Sections["slavesSettings"]; }
        }
    }
}
