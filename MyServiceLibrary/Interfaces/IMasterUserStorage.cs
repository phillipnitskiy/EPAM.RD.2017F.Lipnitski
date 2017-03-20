using System;

namespace ServiceLibrary.Interfaces
{
    public interface IMasterUserStorage : IUserStorage, ILoadableStorage
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
        /// Occurs when user added.
        /// </summary>
        event EventHandler<UserAddedRemovedEventArgs> UserAdded;

        /// <summary>
        /// Occurs when user removed.
        /// </summary>
        event EventHandler<UserAddedRemovedEventArgs> UserDeleted;
    }

}