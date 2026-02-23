namespace RecruitmentInterviewManagementSystem.API.GlobalExceptions
{
    public class NotFoundUserException : Exception
    {
        public NotFoundUserException()
        {
        }

        public NotFoundUserException(string? message) : base(message)
        {
        }
    }
}
