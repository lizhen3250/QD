using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace QD_Tour_Web.Services
{
    public class MailService
    {
        private string receiver;
        private string subject;
        private string content;
        public MailService(string Receiver, string Subject, string Content)
        {
            this.receiver = Receiver;
            this.subject = Subject;
            this.content = Content;
        }

        public bool isSent()
        {
            System.Net.Mail.SmtpClient client = null;

            bool isSent = false;
            try
            {
                //163邮箱发送配置                   

                client = new System.Net.Mail.SmtpClient();

                client.Host = "smtp.163.com";

                client.Port = 25;

                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                client.EnableSsl = true;

                client.UseDefaultCredentials = true;

                client.Credentials = new System.Net.NetworkCredential("zhenli325@163.com", "flwls32500");

                System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();

                Message.SubjectEncoding = System.Text.Encoding.UTF8;

                Message.BodyEncoding = System.Text.Encoding.UTF8;

                Message.Priority = System.Net.Mail.MailPriority.High;

                Message.From = new System.Net.Mail.MailAddress("zhenli325@163.com");

                //添加邮件接收人地址

                string[] receivers = this.receiver.Split(new char[] { ',' });

                Array.ForEach(receivers.ToArray(), ToMail => { Message.To.Add(ToMail); });

                Message.Subject = this.subject;

                Message.Body = this.content;

                Message.IsBodyHtml = true;

                client.Send(Message);

                isSent = true;

            }

            catch (Exception e)
            {

            }

            return isSent;
        }
    }
}