namespace MyServiceLibrary.Tests
{
    using System;
    using System.Linq;
    using Moq;
    using MyServiceLibrary;
    using NUnit.Framework;
    using System.Collections.Generic;
    using Interfaces;

    [TestFixture]
    public class XmlStorageLoaderTests
    {
        [Test]
        public void Constructor_NullPath_ExceptionThrown()
        {
            string nullPath = null;

            TestDelegate testDelegate = () =>  new XmlStorageLoader(nullPath);

            Assert.Catch<ArgumentNullException>(testDelegate);
        }

        [Test]
        public void Constructor_EmptyStringPath_ExceptionThrown()
        {
            string emptyStringPath = string.Empty;

            TestDelegate testDelegate = () => new XmlStorageLoader(emptyStringPath);

            Assert.Catch<ArgumentOutOfRangeException>(testDelegate);
        }

        [Test]
        public void Save_NullStorage_ExceptionThrown()
        {
            IEnumerable<User> nullStorage = null;
            IStorageLoader storageLoader = new XmlStorageLoader("defaultName");

            TestDelegate testDelegate = () => storageLoader.Save(nullStorage);

            Assert.Catch<ArgumentNullException>(testDelegate);
        }

    }
}
