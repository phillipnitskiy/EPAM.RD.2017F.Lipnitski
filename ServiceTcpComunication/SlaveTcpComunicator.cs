using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using PostSharp.Patterns.Diagnostics;
using ServiceLibrary.Interfaces;

namespace ServiceTcpComunication
{
    [Serializable]
    public class SlaveTcpComunicator : MarshalByRefObject
    {
        private ISlaveUserStorage slaveUserStorage;
        private IPEndPoint slaveIpEndPoint;
        private TcpListener serverListener;
        private TcpClient serverClient;

        [Log]
        public SlaveTcpComunicator(
            ISlaveUserStorage slaveUserStorage, IPEndPoint slaveIpEndPoint)
        {
            if (slaveUserStorage == null)
            {
                throw new ArgumentNullException(nameof(slaveUserStorage));
            }
            if (slaveIpEndPoint == null)
            {
                throw new ArgumentNullException(nameof(slaveIpEndPoint));
            }

            this.slaveUserStorage = slaveUserStorage;
            this.slaveIpEndPoint = slaveIpEndPoint;

            serverListener = new TcpListener(slaveIpEndPoint);

            UserAdded += slaveUserStorage.UserAddedHandler;
            UserDeleted += slaveUserStorage.UserDeletedHandler;

            ListenServer();

        }

        [Log]
        private async void ListenServer()
        {

            serverListener.Start();
            serverClient = await serverListener.AcceptTcpClientAsync();
            NetworkStream stream = serverClient.GetStream();
            BinaryFormatter formatter = new BinaryFormatter();
            while (true)
            {
                ServiceMessage message = (ServiceMessage)formatter.Deserialize(stream);
                if (message.Operation == ServiceOperation.Add)
                {
                    OnUserAdded(new UserAddedRemovedEventArgs { User = message.User });
                }
                else
                {
                    OnUserDeleted(new UserAddedRemovedEventArgs { User = message.User });
                }
            }

        }

        protected virtual void OnUserAdded(UserAddedRemovedEventArgs eventArgs)
        {
            EventHandler<UserAddedRemovedEventArgs> t = UserAdded;
            t?.Invoke(this, eventArgs);
        }

        protected virtual void OnUserDeleted(UserAddedRemovedEventArgs eventArgs)
        {
            EventHandler<UserAddedRemovedEventArgs> t = UserDeleted;
            t?.Invoke(this, eventArgs);
        }


        public event EventHandler<UserAddedRemovedEventArgs> UserAdded = delegate { };
        public event EventHandler<UserAddedRemovedEventArgs> UserDeleted = delegate { };

    }
}