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
        public string htmlString;

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
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("sj7525316@gmail.com");
                message.To.Add(new MailAddress("sj7525316@gmail.com"));
                string bodymessage = "For reset click here <a href='https://localhost:44320/api/User/resetPassword'> click me</a>" +
               "You can copy the token Provided here to reset your password :  " + token;
                message.Subject = "Reset password link";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = bodymessage
                    ;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("sj7525316@gmail.com", "s789456123");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }

            //For a msmq reciver
            msmq.BeginReceive();
        }

    }
}
