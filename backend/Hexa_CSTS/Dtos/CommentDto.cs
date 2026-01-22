using System;
using System.ComponentModel.DataAnnotations;

namespace Hexa_CSTS.DTOs
{
    public class CommentDtos
    {
        public class Dto
        {
            public int CommentId { get; set; }
            public string Message { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; }
            public string AuthorName { get; set; } = string.Empty;
        }

  
        public class Create
        {
            [Required, MinLength(1)]
            public string Message { get; set; } = string.Empty;
        }
    }
}