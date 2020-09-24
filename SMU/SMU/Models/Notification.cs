using System.ComponentModel.DataAnnotations.Schema;


namespace SMU.Models
{
    [NotMapped]
    public class Notification
    {

        public string To { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

    }
}
