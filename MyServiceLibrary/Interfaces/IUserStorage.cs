namespace MyServiceLibrary.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IUserStorage
    {
        /// <summary>
        /// Add a single user to storage.
        /// </summary>
        /// <param name="user">User to add</param>
        void Add(User user);

        /// <summary>
        /// Delete a user from storage by id.
        /// </summary>
        /// <param name="userId">Id of user to delete</param>
        void Delete(int userId);

        /// <summary>
        /// Search for users in storage using predicate.
        /// </summary>
        /// <param name="predicate">Search predicate</param>
        /// <returns></returns>
        IEnumerable<User> Search(Func<User, bool> predicate);

        /// <summary>
        /// Saves to specified storage loader.
        /// </summary>
        /// <param name="storageLoader">The storage loader.</param>
        void Save(IStorageLoader storageLoader);

        /// <summary>
        /// Loads from specified storage loader.
        /// </summary>
        /// <param name="storageLoader">The storage loader.</param>
        void Load(IStorageLoader storageLoader);

    }
}