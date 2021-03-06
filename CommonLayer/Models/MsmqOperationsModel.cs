using Experimental.System.Messaging;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Models
{
    public class MsmqOperation
    {
        MessageQueue msmq = new MessageQueue();
        

        /// <summary>
        /// Senders the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        public void Sender(string token)
        {
            msmq.Path = @".\private$\Tokens";
            try
            {
                if (!MessageQueue.Exists(msmq.Path))
                {
                    MessageQueue.Create(msmq.Path);
                }
                msmq.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                msmq.ReceiveCompleted += Msmq_ReceiveCompleted;
                msmq.Send(token);
                msmq.BeginReceive();
                msmq.Close();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        /// <summary>
        /// Handles the ReceiveCompleted event of the Msmq control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ReceiveCompletedEventArgs"/> instance containing the event data.</param>
        private void Msmq_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var msg = msmq.EndReceive(e.AsyncResult);
            string token = msg.Body.ToString();
            //mail sending code smtp 
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.gmail.com") 
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("vineethclass250@gmail.com", "dummypassword@class")
                };
                message.From = new MailAddress("vineethclass250@gmail.com");
                message.To.Add(new MailAddress("vineethclass250@gmail.com"));
                string bodymessage = @"<p>Your password has been reset.Please click the link to create new password.</p>" + string.Format("<a href=\"https://localhost:4200/api/User/ResetPassword.aspx?token={0}\">Reset Password Link</a>");
                message.Subject = "Reset password link";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = bodymessage;
                smtp.Send(message);
            }
            catch (Exception) { }

            //For a msmq reciver
            msmq.BeginReceive();
        }

    }
}
