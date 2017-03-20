using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AppServiceConfiguration;
using ServiceLibrary;
using ServiceLibrary.Interfaces;
using ServiceTcpComunication;

namespace StorageManager
{
    public class UserStorageManager
    {
        private AppDomain masterStorageDomain;
        private List<AppDomain> slaveStorageDomains = new List<AppDomain>();

        private MasterTcpComunicator masterTcpComunicator;
        private List<SlaveTcpComunicator> slaveTcpComunicators = new List<SlaveTcpComunicator>();

        public IMasterUserStorage GetMasterStorage(IUserIdGenerator userIdGenerator, IUserValidator userValidator, IUserStorage userStorage)
        {
            if (!AppServiceConfigurator.IsMasterStorageEnabled)
            {
                return null;
            }

            masterStorageDomain = AppDomain.CreateDomain("MasterDomain");

            var masterStorage = CreateUserStorageInstance(masterStorageDomain, "ServiceLibrary",
                typeof(MasterUserStorage), new object[] { userIdGenerator, userValidator, userStorage }) as MasterUserStorage;

            List<IPEndPoint> slavesEndPoints = new List<IPEndPoint>();
            foreach (SlaveElement slave in AppServiceConfigurator.GetMasterStorageSlaveElements)
            {
                var slaveIpEndPoint = new IPEndPoint(IPAddress.Parse(slave.IpAddress), int.Parse(slave.Port));
                slavesEndPoints.Add(slaveIpEndPoint);
            }

            masterTcpComunicator = new MasterTcpComunicator(masterStorage, slavesEndPoints);

            return masterStorage;
        }
        public IEnumerable<ISlaveUserStorage> GetSlaveServices()
        {
            if (!AppServiceConfigurator.IsSlaveStoragesEnabled)
            {
                return null;
            }

            var slaveStorages = new List<ISlaveUserStorage>();
            foreach (SlaveElement slave in AppServiceConfigurator.GetSlaveStorageElements)
            {
                AppDomain slaveDomain = AppDomain.CreateDomain(slave.Name);
                slaveStorageDomains.Add(slaveDomain);
                var slaveUserStorage = CreateUserStorageInstance(slaveDomain, "ServiceLibrary",
                    typeof(SlaveUserStorage), null) as SlaveUserStorage;
                slaveStorages.Add(slaveUserStorage);

                var slaveIpEndPoint = new IPEndPoint(IPAddress.Parse(slave.IpAddress), int.Parse(slave.Port));
                slaveTcpComunicators.Add(new SlaveTcpComunicator(
                    slaveUserStorage, slaveIpEndPoint));
            }

            return slaveStorages;
        }

        private IUserStorage CreateUserStorageInstance(AppDomain storageDomain, string assemblyName,
            Type serviceType, object[] activationAttributes)
        {
            var instance = storageDomain.CreateInstanceAndUnwrap(assemblyName,
                serviceType.FullName, false, BindingFlags.CreateInstance, null, activationAttributes,
                CultureInfo.CurrentCulture, null) as IUserStorage;

            return instance;
        }

        public void UnloadDomains()
        {
            foreach (var slaveStorageDomain in slaveStorageDomains)
            {
                AppDomain.Unload(slaveStorageDomain);
            }
            if (masterStorageDomain != null)
            {
                AppDomain.Unload(masterStorageDomain);
            }
        }
    }
}
