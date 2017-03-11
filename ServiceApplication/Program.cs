using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Extensibility;

namespace ServiceApplication
{
    using MyServiceLibrary;

    public class Program
    {
        public static void Main(string[] args)
        {
            //ICollection<User> storageToSave = new List<User>
            //{
            //    new User {FirstName = "name", LastName = "surname", DateOfBirth = DateTime.Now},
            //    new User {FirstName = "name1", LastName = "surname1", DateOfBirth = DateTime.Now},
            //    new User {FirstName = "name12", LastName = "surname12", DateOfBirth = DateTime.Now}
            //};

            ////string path = Path.Combine(Environment.CurrentDirectory, "storage.txt");
            //XmlStorageLoader xmlStorageLoader = new XmlStorageLoader();

            //xmlStorageLoader.Save(storageToSave);

            //ICollection<User> storageToLoad = null;

            //storageToLoad = xmlStorageLoader.Load().ToList();

            //foreach (var user in storageToLoad)
            //{
            //    Console.WriteLine($"{user.FirstName} {user.LastName} {user.DateOfBirth}");
            //}


            try
            {
                TestMethod("heheeh");

            }
            catch (Exception)
            {
                
            }
        }

        [Log]
        [LogException]
        public static void TestMethod(string sdsdsd)
        {
            throw new ArgumentNullException();
        }
    }
}
