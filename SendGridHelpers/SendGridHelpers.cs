using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;

namespace SendGridHelpers
{
    /// <summary>
    /// Helper methods for use with the SendGrid email service.
    /// </summary>
    public class SendGridHelpers
    {
        /// <summary>
        /// Sends a message with the SendGrid API.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>0 to indicate success.</returns>
        public static Task Send(SendGridMessage message)
        {
            // Get credentials.
            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["SendGrid.Username"], ConfigurationManager.AppSettings["SendGrid.Password"]);
            var transportWeb = new Web(credentials);

            // Send it.
            transportWeb.Deliver(message);

            // Return success.
            return Task.FromResult(0);
        }

        /// <summary>
        /// Sends an html formatted email using the default "from" address.
        /// </summary>
        /// <param name="to">The destination email address.</param>
        /// <param name="subject">Email subject line.</param>
        /// <param name="body">Html formatted body.</param>
        /// <returns>0 to indicate success.</returns>
        //public static Task SendHtml(string to, string subject, string body)
        //{
        //    return SendHtml(to, subject, body, ConfigurationManager.AppSettings["SendGrid.DefaultFrom"]);
        //}

        /// <summary>
        /// Sends an html formatted email.
        /// </summary>
        /// <param name="to">The destination email address.</param>
        /// <param name="subject">Email subject line.</param>
        /// <param name="body">Html formatted body.</param>
        /// <param name="from">The source email address.</param>
        /// <returns>0 to indicate success.</returns>
        public static Task SendHtml(string to, string subject, string body, string from = "")
        {
            // Set up SendGrid message.
            var sgm = new SendGridMessage()
            {
                Subject = subject,
                Html = body
            };

            if (string.IsNullOrEmpty(from))
                sgm.From = new MailAddress(ConfigurationManager.AppSettings["SendGrid.DefaultFrom"]);
            else
                sgm.From = new MailAddress(from);

            sgm.AddTo(to);

            // Send it.
            return Send(sgm);
        }

        /// <summary>
        /// Sends a plain text email using the default "from" address.
        /// </summary>
        /// <param name="to">The destination email address.</param>
        /// <param name="subject">Email subject line.</param>
        /// <param name="body">Plain text body.</param>
        /// <returns>0 to indicate success.</returns>
        public static Task SendText(string to, string subject, string body)
        {
            return SendHtml(to, subject, body, ConfigurationManager.AppSettings["Mail.DefaultFrom"]);
        }

        /// <summary>
        /// Sends a plain text email.
        /// </summary>
        /// <param name="to">The destination email address.</param>
        /// <param name="subject">Email subject line.</param>
        /// <param name="body">Plain text body.</param>
        /// <param name="from">The source email address.</param>
        /// <returns>0 to indicate success.</returns>
        public static Task SendText(string to, string subject, string body, string from)
        {
            // Set up SendGrid message.
            var sgm = new SendGridMessage()
            {
                From = new MailAddress(from),
                Subject = subject,
                Text = body
            };
            sgm.AddTo(to);

            // Send it.
            return Send(sgm);
        }
    }
}
