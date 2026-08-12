using Ecom.Application.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _logPath = @"c:\Users\hp\Desktop\Shiwansh_Solution\All_Backend Api\Ecom_OnionProject\EmailsLog.txt";

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var emailRecord = $@"
========================================================================
TIMESTAMP: {DateTime.Now}
TO: {toEmail}
SUBJECT: {subject}
------------------------------------------------------------------------
{htmlContent}
========================================================================
";
            Console.WriteLine($"[EMAIL SENT] To: {toEmail}, Subject: {subject}");

            try
            {
                await File.AppendAllTextAsync(_logPath, emailRecord);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EMAIL LOG ERROR] Failed to write to log file: {ex.Message}");
            }
        }
    }
}
