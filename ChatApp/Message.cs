using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApp
{
    public class Message
    {
        [Required(ErrorMessage = "Sender is required.")]
        [Display(Name = "Sender")]
        public string Sender { get; init; } = string.Empty; // Default to an empty string for safety

        [Required(ErrorMessage = "Message content cannot be empty.")]
        [StringLength(1000, ErrorMessage = "Message content cannot exceed 1000 characters.")]
        [Display(Name = "Message Content")]
        public string Content { get; init; } = string.Empty;

        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; init; } = DateTime.Now;

        // Helper to format timestamp as a string (e.g., "HH:mm")
        public string FormattedTimestamp => Timestamp.ToString("HH:mm");
    }
}