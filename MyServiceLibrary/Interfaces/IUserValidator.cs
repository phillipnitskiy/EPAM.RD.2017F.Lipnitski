namespace ServiceLibrary.Interfaces
{
    public interface IUserValidator
    {
        /// <summary>
        /// Validate user info.
        /// </summary>
        /// <param name="user">User to validate</param>
        /// <returns></returns>
        bool Validate(User user);
    }
}