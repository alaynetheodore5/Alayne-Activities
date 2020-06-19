using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class ActivityParty
    {
        [Key]
        public int ActivityPartyId { get; set; }

        public int UserId  {get; set;}
        public int ActivityId { get; set; }

        public User ActivityGoer { get; set;}
        public AnActivity Act { get; set; }

    }
}