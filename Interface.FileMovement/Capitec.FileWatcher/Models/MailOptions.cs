namespace Interface.FileMovement.Models
{
    public class MailOptions
    {
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string SMTP { get; set; }
    }
}