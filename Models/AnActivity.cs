using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class AnActivity
    {
        [Key]
        public int ActivityId { get;set; }


        [Required(ErrorMessage="Activity Title is required")]
        [MinLength(2,ErrorMessage="Activity Title must be at least 2 characters")]
        public string Title { get;set; }


        [Required(ErrorMessage="Start Date is required")]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage="Start Time is required")]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }

        public int Duration { get; set; }

        public string MinHrDay { get; set; }


        [Required(ErrorMessage="Activity Description is required")]
        [MinLength(10,ErrorMessage="Activity Description must be at least 10 characters")]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // This is the foreign key
        public int UserId { get; set; }

        // An activity can have only one user that plans it.
        public User Planner { get; set; }

        // many to many
        public List<ActivityParty> Guests { get; set; }

    }
}