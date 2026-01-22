using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hexa_CSTS.DTOs
{
  
    public class TicketDtos
    {
        //  ticket details
        public class Dto
        {
            public int TicketId { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string Priority { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; }
            public string CreatorName { get; set; } = string.Empty;
            public string? AgentName { get; set; }
            public int? AssignedToId { get; set; }
            public List<CommentDtos.Dto> Comments { get; set; } = new();
        }

        // Create
        public class Create
        {
            [Required, MaxLength(200)]
            public string Title { get; set; } = string.Empty;

            [Required]
            public string Description { get; set; } = string.Empty;

            [Required]
            public string Priority { get; set; } = string.Empty;
        }

        // Update Ticket
        public class Update
        {
            [Required]
            public string Status { get; set; } = string.Empty;

            public int? AssignedToId { get; set; }
        }
    }
}