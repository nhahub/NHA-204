using System;
using System.ComponentModel.DataAnnotations;

namespace MAlex.Models
{
    public class ContactMessage
    {
        // Primary Key
        public int Id { get; set; }

        // Full Name
        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(100)]
        public string FullName { get; set; }

        // Email
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100)]
        public string Email { get; set; }

        // Subject
        [Required(ErrorMessage = "Please enter a subject.")]
        [StringLength(200)]
        public string Subject { get; set; }

        // Message
        [Required(ErrorMessage = "Please enter your message content.")]
        public string Message { get; set; }

        // Submission Date
        public DateTime SentAt { get; set; } = DateTime.Now;
    }
}