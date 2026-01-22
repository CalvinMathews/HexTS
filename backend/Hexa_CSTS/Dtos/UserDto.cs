using System.ComponentModel.DataAnnotations;


public class UserDtos
{
    // Users public profile
    public class Dto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    public class ChangeRole
    {
        public string Role { get; set; } = string.Empty;
    }
    // Registert
    public class Register
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }

    // Login request
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;


        public class Response
        {
            public int UserId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string Token { get; set; } = string.Empty;
        }
    }

    // Admin Update role 
    public class AdminUpdate
    {
        [Required]
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    // agent
    public class AgentWorkload
    {
        public int AgentId { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public int OpenTickets { get; set; }
    }

}