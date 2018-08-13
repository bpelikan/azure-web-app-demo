using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace azure_web_app_demo.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
