using Hexa_CSTS.DTOs;
using Hexa_CSTS.Models;
using Hexa_CSTS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Hexa_CSTS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] 
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;

        public AdminController(IUserRepository userRepository, ITicketRepository ticketRepository)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
        }

        // Get all agenst
        [HttpGet("agents")]
        public async Task<ActionResult<List<UserDtos.Dto>>> GetAgents()
        {
            var agents = await _userRepository.GetRole(UserRole.SupportAgent);

            var agentDtos = agents.Select(agent => new UserDtos.Dto
            {
                UserId = agent.UserId,
                Name = agent.Name,
                Email = agent.Email,
                Role = agent.Role.ToString(),
                IsActive = agent.IsActive
            }).ToList();

            return Ok(agentDtos);
        }

// agent progress
        [HttpGet("agents/workload")]
        public async Task<ActionResult<List<UserDtos.AgentWorkload>>> GetAgentWorkload()
        {
            var agents = await _userRepository.GetRole(UserRole.SupportAgent);
            var workload = new List<UserDtos.AgentWorkload>();

            foreach (var agent in agents)
            {
                var assignedTickets = await _ticketRepository.GetByAgent(agent.UserId);
                workload.Add(new UserDtos.AgentWorkload
                {
                    AgentId = agent.UserId,
                    AgentName = agent.Name,
                    OpenTickets = assignedTickets.Count(t => t.Status != TicketStatus.Closed)
                });
            }

            return Ok(workload);
        }
    }
}