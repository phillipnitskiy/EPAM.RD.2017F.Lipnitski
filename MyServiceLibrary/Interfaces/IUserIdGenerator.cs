namespace MyServiceLibrary.Interfaces
{
    public interface IUserIdGenerator
    {
        /// <summary>
        /// Generete user id.
        /// </summary>
        /// <param name="user">User for id generation</param>
        /// <returns></returns>
        int Generate(User user);
    }
}