using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using ServiceLibrary.Interfaces;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;

namespace ServiceTcpComunication
{
    [Serializable]
    public class MasterTcpComunicator : MarshalByRefObject
    {
        private readonly IMasterUserStorage masterUserStorage;
        private BlockingCollection<TcpClient> slaveStoragesTcpClients;

        [Log]
        public MasterTcpComunicator(IMasterUserStorage masterUserStorage, IEnumerable<IPEndPoint> slavesEndPoints)
        {
            if (masterUserStorage == null)
            {
                throw new ArgumentNullException(nameof(masterUserStorage));
            }

            if (slavesEndPoints == null)
            {
                throw new ArgumentNullException(nameof(slavesEndPoints));
            }

            this.masterUserStorage = masterUserStorage;
            this.slaveStoragesTcpClients = new BlockingCollection<TcpClient>();

            foreach (var slavesEndPoint in slavesEndPoints)
            {
                TcpClient slave = new TcpClient();
                slave.Connect(slavesEndPoint);
                slaveStoragesTcpClients.Add(slave);
            }

            masterUserStorage.UserAdded += UserAddedHandler;
            masterUserStorage.UserDeleted += UserDeletedHandler;
        }

        [Log]
        private void UserAddedHandler(object sender, UserAddedRemovedEventArgs eventArgs)
        {
            SendUserMessage(
                new ServiceMessage { Operation = ServiceOperation.Add, User = eventArgs.User });
        }

        [Log]
        private void UserDeletedHandler(object sender, UserAddedRemovedEventArgs eventArgs)
        {
            SendUserMessage(
                new ServiceMessage { Operation = ServiceOperation.Delete, User = eventArgs.User });
        }

        [Log]
        private void SendUserMessage(ServiceMessage message)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            foreach (TcpClient slave in slaveStoragesTcpClients)
            {
                if (!slave.Connected)
                    continue;

                NetworkStream stream = slave.GetStream();
                formatter.Serialize(stream, message);
            }
        }

    }
}
