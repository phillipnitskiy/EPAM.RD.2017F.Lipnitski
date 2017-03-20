namespace ServiceLibrary
{
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// XML Storage Loader
    /// </summary>
    /// <seealso cref="IStorageLoader" />
    public class XmlStorageLoader : IStorageLoader
    {

        #region Fields

        /// <summary>
        /// The path.
        /// </summary>
        private readonly string path;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStorageLoader"/> class.
        /// </summary>
        public XmlStorageLoader()
        {
            var storageFileName = ConfigurationManager.AppSettings["storageFileName"];
            this.path = Path.Combine(Environment.CurrentDirectory, storageFileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlStorageLoader"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentNullException">path</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">path</exception>
        public XmlStorageLoader(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path == string.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(path));
            }

            this.path = path;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves a storage to file.
        /// </summary>
        /// <param name="storage"></param>
        /// <exception cref="System.ArgumentNullException">storage</exception>
        public void Save(IEnumerable<User> storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException(nameof(storage));
            }

            var formatter = new XmlSerializer(storage.GetType());

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, storage);
            }
        }

        /// <summary>
        /// Loads a storage from file.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> Load()
        {
            IEnumerable<User> storage;

            var formatter = new XmlSerializer(typeof(List<User>));

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                storage = formatter.Deserialize(fileStream) as IEnumerable<User>;
            }

            return storage;
        }

        #endregion
    }
}
