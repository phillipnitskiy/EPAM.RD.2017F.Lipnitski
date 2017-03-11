namespace MyServiceLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class UserStorage : IUserStorage
    {
        #region Fields

        /// <summary>
        /// The user identifier generator
        /// </summary>
        private readonly IUserIdGenerator userIdGenerator;

        /// <summary>
        /// The user validator
        /// </summary>
        private readonly IUserValidator userValidator;

        /// <summary>
        /// The storage
        /// </summary>
        private ICollection<User> storage;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStorage"/> class.
        /// </summary>
        /// <param name="userIdGenerator">The user identifier generator.</param>
        /// <param name="userValidator">The user validator.</param>
        /// <exception cref="System.ArgumentNullException">userIdGenerator</exception>
        public UserStorage(IUserIdGenerator userIdGenerator, IUserValidator userValidator = null)
        {
            if (userIdGenerator == null)
            {
                throw new ArgumentNullException(nameof(userIdGenerator));
            }

            this.userIdGenerator = userIdGenerator;
            this.userValidator = userValidator;
            this.storage = new List<User>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add a single user to storage.
        /// </summary>
        /// <param name="user">User to add</param>
        /// <exception cref="System.ArgumentNullException">user</exception>
        /// <exception cref="System.ArgumentException">Invalid user data. - user</exception>
        public void Add(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (userValidator != null && !this.userValidator.Validate(user))
            {
                throw new ArgumentException("Invalid user data.", nameof(user));
            }

            user.Id = this.userIdGenerator.Generate(user);

            this.storage.Add(user);
        }

        /// <summary>
        /// Delete a user from storage by id.
        /// </summary>
        /// <param name="userId">Id of user to delete</param>
        /// <exception cref="System.InvalidOperationException">
        /// User does not exist.
        /// </exception>
        public void Delete(int userId)
        {
            if (this.storage.Count <= 0)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            var user = this.Search(u => u.Id == userId).FirstOrDefault();
            if (user == null)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            this.storage.Remove(user);
        }

        /// <summary>
        /// Search for users in storage using predicate.
        /// </summary>
        /// <param name="predicate">Search predicate</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">predicate</exception>
        public IEnumerable<User> Search(Func<User, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var result = new List<User>();

            if (this.storage.Count <= 0)
            {
                return result;
            }

            result = this.storage.Where(predicate).ToList();

            return result;
        }

        /// <summary>
        /// Saves to specified storage loader.
        /// </summary>
        /// <param name="storageLoader">The storage loader.</param>
        public void Save(IStorageLoader storageLoader)
        {
            if (storageLoader == null)
            {
                throw  new ArgumentNullException(nameof(storageLoader));
            }

            storageLoader.Save(storage);
        }

        /// <summary>
        /// Loads from specified storage loader.
        /// </summary>
        /// <param name="storageLoader">The storage loader.</param>
        public void Load(IStorageLoader storageLoader)
        {
            if (storageLoader == null)
            {
                throw new ArgumentNullException(nameof(storageLoader));
            }

            this.storage = storageLoader.Load()?.ToList();
        }

        #endregion
    }
}
