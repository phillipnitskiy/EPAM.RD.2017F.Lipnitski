using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AppServiceConfiguration
{
    public class MasterSettingsSection : ConfigurationSection
    {

        [ConfigurationProperty("master", IsRequired = true)]
        public MasterElement Master
        {
            get { return (MasterElement)base["master"]; }
        }

        [ConfigurationProperty("storage", IsRequired = true)]
        public StorageElement Storage
        {
            get { return (StorageElement)base["storage"]; }
        }

        [ConfigurationProperty("slaves")]
        public SlavesElementCollection Slaves
        {
            get { return (SlavesElementCollection)base["slaves"]; }
        }

    }

    public class MasterElement : ConfigurationElement
    {
        [ConfigurationProperty("port", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Port
        {
            get { return (string)base["port"]; }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["ipAddress"] = value; }
        }
    }

    public class StorageElement : ConfigurationElement
    {
        [ConfigurationProperty("fileName", DefaultValue = "storage.xml", IsKey = false, IsRequired = true)]
        public string FileName
        {
            get { return (string)base["fileName"]; }
            set { base["fileName"] = value; }
        }
    }

    public class SlaveElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = false)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["ipAddress"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Port
        {
            get { return (string)base["port"]; }
            set { base["port"] = value; }
        }

    }

    [ConfigurationCollection(typeof(SlaveElement), AddItemName = "slave")]
    public class SlavesElementCollection : ConfigurationElementCollection
    {
        public SlaveElement this[int index]
        {
            get { return (SlaveElement)base.BaseGet(index); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SlaveElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SlaveElement)element).Name;
        }
    }
}
