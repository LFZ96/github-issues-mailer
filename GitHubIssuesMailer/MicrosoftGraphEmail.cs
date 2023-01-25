using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIssuesMailer
{
    public class MicrosoftGraphEmail
    {
        public Message Message { get; set; }
        public Recipients ToRecipients { get; set; }
    }

    public class Message
    {
        public string Subject { get; set; }
        public Body Body { get; set; }
        public Recipients ToRecipients { get; set; }
    }

    public class Recipients
    {
        public List<EmailAddress> EmailAddresses { get; set; }

        public Recipients(List<string> emailAddresses)
        {
            EmailAddresses = new List<EmailAddress>();
            foreach (string emailAddress in emailAddresses)
            {
                EmailAddresses.Add(new EmailAddress(emailAddress));
            }
        }
    }

    public class EmailAddress
    {
        public string Address { get; set; }

        public EmailAddress(string address)
        {
            Address = address;
        }
    }

    public class Body
    {
        public string ContentType { get; set; }
        public string Content { get; set; }
    }
}
