using Hexa_CSTS.DTOs;
using Hexa_CSTS.Models;
using Hexa_CSTS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hexa_CSTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ICommentRepository _commentRepository;

        public TicketsController(ITicketRepository ticketRepository, ICommentRepository commentRepository)
        {
            _ticketRepository = ticketRepository;
            _commentRepository = commentRepository;
        }

        private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        private static TicketDtos.Dto ToTicketDto(Ticket ticket) => new()
        {
            TicketId = ticket.TicketId,
            Title = ticket.Title,
            Description = ticket.Description,
            Status = ticket.Status.ToString(),
            Priority = ticket.Priority.ToString(),
            CreatedDate = ticket.CreatedDate,
            CreatorName = ticket.CreatedBy?.Name ?? "Unknown",
            AgentName = ticket.AssignedToAgnt?.Name,
            AssignedToId = ticket.AssignedToId,
            Comments = ticket.Comments?.Select(c => new CommentDtos.Dto
            {
                CommentId = c.CommentId,
                Message = c.Message,
                CreatedDate = c.CreatedDate,
                AuthorName = c.User?.Name ?? "Unknown"
            }).ToList() ?? new List<CommentDtos.Dto>()
        };

        [HttpGet]
        public async Task<ActionResult<List<TicketDtos.Dto>>> Get()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            return Ok(tickets.Select(ToTicketDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDtos.Dto>> Get(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ToTicketDto(ticket));
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<TicketDtos.Dto>> Create([FromBody] TicketDtos.Create request)
        {
            var newTicket = new Ticket
            {
                Title = request.Title,
                Description = request.Description,
                Status = TicketStatus.New,
                CreatedById = CurrentUserId,
                CreatedDate = DateTime.UtcNow
            };

            if (Enum.TryParse<TicketPriority>(request.Priority, true, out var priority))
            {
                newTicket.Priority = priority;
            }

            var createdTicket = await _ticketRepository.AddAsync(newTicket);
            var ticketToReturn = await _ticketRepository.GetByIdAsync(createdTicket.TicketId);

            return CreatedAtAction(nameof(Get), new { id = ticketToReturn.TicketId }, ToTicketDto(ticketToReturn));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] TicketDtos.Update request)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.AssignedToId = request.AssignedToId;

            if (Enum.TryParse<TicketStatus>(request.Status, true, out var status))
            {
                ticket.Status = status;
            }

            await _ticketRepository.UpdateAsync(ticket);
            return NoContent();
        }
        [HttpPut("{id}/status")]
        [Authorize] // everyone must be logged in; we'll check role inside
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] TicketDtos.Update request)
        {
            // 1. get ticket
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
                return NotFound();

            // 2. current user
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role); // "Admin", "SupportAgent", "Customer"

         
            if (userRole == "Admin")
            {
                if (Enum.TryParse<TicketStatus>(request.Status, true, out var adminStatus))
                    ticket.Status = adminStatus;

                ticket.AssignedToId = request.AssignedToId;
                await _ticketRepository.UpdateAsync(ticket);
                return NoContent();
            }

            if (userRole == "SupportAgent")
            {
                if (ticket.AssignedToId != userId)
                    return Forbid();

                if (!Enum.TryParse<TicketStatus>(request.Status, true, out var agentStatus))
                    return BadRequest("Invalid status.");

                if (agentStatus != TicketStatus.InProgress && agentStatus != TicketStatus.Resolved)
                    return BadRequest("Agents can only set InProgress or Resolved.");

                ticket.Status = agentStatus;
                await _ticketRepository.UpdateAsync(ticket);
                return NoContent();
            }

            return Forbid();
        }

        [HttpPut("{id}/close")]
        [Authorize(Roles = "Customer")] 
        public async Task<IActionResult> CloseTicket(int id)
        {
            
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

           
            if (ticket.CreatedById != CurrentUserId)
            {
                return Forbid("You are not authorized to close this ticket."); 
            }

            
            ticket.Status = TicketStatus.Closed;

            await _ticketRepository.UpdateAsync(ticket);

            // success
            return NoContent();
        }
        [HttpGet("by-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<TicketDtos.Dto>>> GetByUser(int userId)
        {
            var tickets = await _ticketRepository.GetByUser(userId);
            return Ok(tickets.Select(ToTicketDto).ToList());
        }

        [HttpGet("by-agent/{agentId}")]
        [Authorize(Roles = "Admin,SupportAgent")]
        public async Task<ActionResult<List<TicketDtos.Dto>>> GetByAgent(int agentId)
        {
            var tickets = await _ticketRepository.GetByAgent(agentId);
            return Ok(tickets.Select(ToTicketDto).ToList());
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult<CommentDtos.Dto>> AddComment(int id, [FromBody] CommentDtos.Create request)
        {
            if (!await _ticketRepository.ExistsAsync(id))
            {
                return NotFound("Ticket not found.");
            }

            var newComment = new Comment
            {
                Message = request.Message,
                TicketId = id,
                UserId = CurrentUserId,
                CreatedDate = DateTime.UtcNow
            };

            var createdComment = await _commentRepository.AddAsync(newComment);
            var commentToReturn = await _commentRepository.GetByIdAsync(createdComment.CommentId);

            var commentResponse = new CommentDtos.Dto
            {
                CommentId = commentToReturn.CommentId,
                Message = commentToReturn.Message,
                CreatedDate = commentToReturn.CreatedDate,
                AuthorName = commentToReturn.User?.Name ?? "Unknown"
            };

            return CreatedAtAction(nameof(Get), new { id = commentToReturn.TicketId }, commentResponse);
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<List<TicketDtos.Dto>>> GetDashboardTickets()
        {
            List<Ticket> tickets;

            if (User.IsInRole("Admin"))
            {
                tickets = await _ticketRepository.GetAllAsync();
            }
            else if (User.IsInRole("SupportAgent"))
            {
                tickets = await _ticketRepository.GetByAgent(CurrentUserId);
            }
            else
            {
                tickets = await _ticketRepository.GetByUser(CurrentUserId);
            }

            return Ok(tickets.Select(ToTicketDto).ToList());
        }

        [HttpGet("unassigned")]
        [Authorize(Roles = "Admin,SupportAgent")]
        public async Task<ActionResult<List<TicketDtos.Dto>>> GetUnassignedTickets()
        {
            var tickets = await _ticketRepository.GetUnassignedAsync();
            return Ok(tickets.Select(ToTicketDto).ToList());
        }
    }
}