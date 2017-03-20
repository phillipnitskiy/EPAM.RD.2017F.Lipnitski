namespace ServiceLibrary.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IUserStorage
    {

        /// <summary>
        /// Search for users in storage using predicate.
        /// </summary>
        /// <param name="predicate">Search predicate</param>
        /// <returns></returns>
        IEnumerable<User> Search(Func<User, bool> predicate);

    }
}