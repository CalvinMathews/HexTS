using System.ComponentModel.DataAnnotations;

namespace Hexa_CSTS.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [Required, MaxLength(1200)]
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}
