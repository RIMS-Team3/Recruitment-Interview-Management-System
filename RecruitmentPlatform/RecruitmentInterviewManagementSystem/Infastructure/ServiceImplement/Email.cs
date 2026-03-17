using RecruitmentInterviewManagementSystem.Applications.Notifications.DTO;
using RecruitmentInterviewManagementSystem.Applications.Notifications.Interfaces;
using System.Net.Mail;
using System.Net;

namespace RecruitmentInterviewManagementSystem.Infastructure.ServiceImplement
{
    public class Email : INotification
    {

        public string TypeService => "Email";

        public async Task<bool> SendRegisterAccount(RequestSendMessage request)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("hotelluxurytrungduc@gmail.com", "ykbg blmo tqxy hrld");
                    smtp.EnableSsl = true;

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress("hotelluxurytrungduc@gmail.com", "PTD Corporation");
                        message.To.Add(request.To);
                        message.Subject = request.Subject;
                        message.Body = request.Body;
                        message.IsBodyHtml = true;

                        await smtp.SendMailAsync(message);
                    }
                }

                Console.WriteLine("Email sent successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }
    }
}
