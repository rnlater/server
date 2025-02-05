using Shared.Types;

namespace Application.Interfaces
{
    public interface IMailService
    {
        /// <summary>
        /// Send email to the specified email address
        /// </summary>
        /// <param name="to"></param>
        /// <param name="name"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        /// <exception cref="ErrorMessage.EmailNotSent">Email not sent</exception>
        Task<Result<string>> SendEmail(string to, string name, string subject, string body);
    }
}