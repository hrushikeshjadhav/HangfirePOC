using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SendMail
{
    public class EmailService
    {
        private readonly string smtpHost = "smtp.example.com"; // Your SMTP server
        private readonly int smtpPort = 587; // SMTP port, typically 587 for TLS
        private readonly string smtpUser = "your-email@example.com"; // Your email
        private readonly string smtpPass = "your-email-password"; // Your email password

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress(smtpUser, "Your Name");
            var toAddress = new MailAddress(toEmail);
            using var smtpClient = new SmtpClient
            {
                Host = smtpHost,
                Port = smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            using var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Set to true if the body is HTML
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }
}
