using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceConfiguration
{
    public class SlavesSettingsSection : ConfigurationSection
    {

        [ConfigurationProperty("master", IsRequired = true)]
        public MasterElement Master
        {
            get { return (MasterElement)base["master"]; }
        }

        [ConfigurationProperty("slaves")]
        public SlavesElementCollection Slaves
        {
            get { return (SlavesElementCollection)base["slaves"]; }
        }

    }
}
