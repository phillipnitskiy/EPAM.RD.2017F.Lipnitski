using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppServiceConfiguration
{
    public static class AppServiceConfigurator
    {
        private static readonly UserServiceSettingsSectionGroup sectionGroup;

        static AppServiceConfigurator()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var m = config.GetSectionGroup("userServiceSettings");
            sectionGroup = (UserServiceSettingsSectionGroup)config.GetSectionGroup("userServiceSettings");
        }

        public static string GetMaserStorageFileName
        {
            get
            {
                if (ReferenceEquals(sectionGroup, null))
                {
                    throw new ArgumentNullException(nameof(sectionGroup));
                }

                return sectionGroup.MasterSettings.Storage.FileName;
            }
        }

        public static bool IsMasterStorageEnabled
        {
            get
            {
                if (sectionGroup == null)
                {
                    throw new ArgumentNullException(nameof(sectionGroup));
                }

                return !string.IsNullOrEmpty(sectionGroup.MasterSettings.Master.Port);
            }
        }

        public static SlavesElementCollection GetMasterStorageSlaveElements
        {
            get
            {
                if (sectionGroup == null)
                {
                    throw new ArgumentNullException(nameof(sectionGroup));
                }

                return sectionGroup.MasterSettings.Slaves;
            }
        }

        public static bool IsSlaveStoragesEnabled
        {
            get
            {
                if (sectionGroup == null)
                {
                    throw new ArgumentNullException(nameof(sectionGroup));
                }

                return sectionGroup.SlavesSettings.Slaves.Count > 0;
            }
        }

        public static SlavesElementCollection GetSlaveStorageElements
        {
            get
            {
                if (sectionGroup == null)
                {
                    throw new ArgumentNullException(nameof(sectionGroup));
                }

                return sectionGroup.SlavesSettings.Slaves;
            }
        }
    }
}
