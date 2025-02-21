using AutoMapper;
using ClockWise.Api.DTOs;
using ClockWise.Api.Models;
using ClockWise.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClockWise.Api.Controllers
{
    [ApiController]
    [Route("api/tickLogs")]
    public class TickLogsController : ControllerBase
    {
        private readonly ITickLogsInterface _tickLogRepository;
        private readonly IMapper _mapper;

        public TickLogsController(ITickLogsInterface tickLogsInterface, IMapper mapper)
        {
            _tickLogRepository = tickLogsInterface;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TickLogDto>>> GetAllTickLogsByEmployeeId(int employeeId)
        {
            var tickLogs = await _tickLogRepository.GetAllTickLogsByEmployeeIdAsync(employeeId);

            if (tickLogs == null || tickLogs.Count == 0)
            {
                return NotFound("No Logs found");
            }

            var tickLogDtos = _mapper.Map<List<TickLogDto>>(tickLogs);
            return Ok(tickLogDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TickLogDto>> GetTickLogById(int id)
        {
            var tickLog = await _tickLogRepository.GetTickLogByIdAsync(id);

            if (tickLog == null)
            {
                return NotFound("Log not found");
            }

            var tickLogDto = _mapper.Map<TickLogDto>(tickLog);
            return Ok(tickLogDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTickLog(TickLogDto createTickLogDto)
        {
            var tickLog = _mapper.Map<TickLog>(createTickLogDto);
            tickLog.IsDeleted = false;
            tickLog.IsApproved = false;

            await _tickLogRepository.CreateTickLogAsync(tickLog);

            var tickLogDto = _mapper.Map<TickLogDto>(tickLog);
            return CreatedAtAction(nameof(GetTickLogById), new { id = tickLog.Id }, tickLogDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTickLog(int id, TickLogDto updateTickLogDto)
        {
            var tickLog = await _tickLogRepository.GetTickLogByIdAsync(id);

            if (tickLog == null)
            {
                return NotFound();
            }

            _mapper.Map(updateTickLogDto, tickLog);

            try
            {
                await _tickLogRepository.UpdateTickLogAsync(tickLog);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _tickLogRepository.TickLogExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTickLog(int id)
        {
            var tickLog = await _tickLogRepository.GetTickLogByIdAsync(id);

            if (tickLog == null)
            {
                return NotFound();
            }

            await _tickLogRepository.DeleteTickLogAsync(tickLog);
            return NoContent();
        }
    }
}