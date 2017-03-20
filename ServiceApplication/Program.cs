using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using AppServiceConfiguration;
using NLog;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;
using ServiceLibrary.Interfaces;
using StorageManager;

namespace ServiceApplication
{
    using ServiceLibrary;

    public class Program
    {
        public static void Main(string[] args)
        {

            var usm = new UserStorageManager();
            var master = usm.GetMasterStorage(new UserIdGenerator(), null, null);

            master.Add(new User { FirstName = "name", LastName = "surename", DateOfBirth = DateTime.Now });
            master.Add(new User { FirstName = "name1", LastName = "surename1", DateOfBirth = DateTime.Now });
            Console.WriteLine("Master");
            foreach (var user in master.Search(u => true))
            {
                Console.WriteLine(user.FirstName + " " + user.LastName);
            }
            Console.ReadLine();

            master.Delete(0);
            Console.WriteLine("Master");
            foreach (var user in master.Search(u => true))
            {
                Console.WriteLine(user.FirstName + " " + user.LastName);
            }
            Console.ReadLine();
            usm.UnloadDomains();



            //var usm = new UserStorageManager();
            //var slaves = usm.GetSlaveServices();

            //Console.ReadLine();
            //Console.WriteLine("Slave");
            //foreach (var user in slaves.First(a => true).Search(u => true))
            //{
            //    Console.WriteLine(user.FirstName + " " + user.LastName);
            //}
            //Console.ReadLine();


            //Console.WriteLine("Slave");
            //foreach (var user in slaves.First().Search(u => true))
            //{
            //    Console.WriteLine(user.FirstName + " " + user.LastName);
            //}
            //Console.ReadLine();
            //usm.UnloadDomains();

        }

        [Serializable]
        class UserIdGenerator : IUserIdGenerator
        {
            private static int id = 0;

            public int Generate(User user)
            {
                return id++;
            }
        }

    }
}
