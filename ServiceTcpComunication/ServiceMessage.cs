using System;
using ServiceLibrary;

namespace ServiceTcpComunication
{
    public enum ServiceOperation
    {
        Add = 1,
        Delete = 2,
    }

    [Serializable]
    public class ServiceMessage
    {
        public ServiceOperation Operation { get; set; }

        public User User { get; set; }
    }
}