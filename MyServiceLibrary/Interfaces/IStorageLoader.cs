namespace MyServiceLibrary.Interfaces
{
    using System.Collections.Generic;

    public interface IStorageLoader
    {
        /// <summary>
        /// Saves a storage.
        /// </summary>
        void Save(IEnumerable<User> storage);

        /// <summary>
        /// Loads a storage.
        /// </summary>
        IEnumerable<User> Load();
    }
}
