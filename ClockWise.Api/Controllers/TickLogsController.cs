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
        public async Task<ActionResult<IEnumerable<TickLogDto>>> GetTickLogsByEmployeeId(int employeeId)
        {
            var tickLogs = await _tickLogRepository.GetTickLogsByEmployeeIdAsync(employeeId);

            if (tickLogs == null || tickLogs.Count == 0)
            {
                return NotFound("No Logs found");
            }

            var tickLogDtos = _mapper.Map<List<TickLogDto>>(tickLogs);
            return Ok(tickLogDtos);
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<IEnumerable<TickLogDto>>> GetTickLogsByEmployeeIdWithRange(int employeeId, DateTime dateFrom, DateTime dateTo)
        {
            var tickLogs = await _tickLogRepository.GetTickLogsByEmployeeIdWithRangeAsync(employeeId, dateFrom, dateTo);

            if (tickLogs == null || tickLogs.Count == 0)
            {
                return NotFound("No Logs found in the specified range");
            }

            var tickLogDtos = _mapper.Map<List<TickLogDto>>(tickLogs);
            return Ok(tickLogDtos);
        }

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<TickLogDto>>> GetTodayTickLogsByEmployeeId(int employeeId)
        {
            DateTime dateFrom = DateTime.Today; // 00:00:00
            DateTime dateTo = DateTime.Today.AddDays(1).AddTicks(-1); // 23:59:59

            var tickLogs = await _tickLogRepository.GetTickLogsByEmployeeIdWithRangeAsync(employeeId, dateFrom, dateTo);

            if (tickLogs == null || tickLogs.Count == 0)
            {
                return NotFound("No Logs found for today");
            }

            var tickLogDtos = _mapper.Map<List<TickLogDto>>(tickLogs);
            return Ok(tickLogDtos);
        }

        [HttpGet("workedtoday/{employeeId}")]
        public async Task<ActionResult<TimeSpan>> GetTodayWorkedTimeByEmployeeId(int employeeId)
        {
            DateTime dateFrom = DateTime.Today; // 00:00:00
            DateTime dateTo = DateTime.Today.AddDays(1).AddTicks(-1); // 23:59:59

            var tickLogs = await _tickLogRepository.GetTickLogsByEmployeeIdWithRangeAsync(employeeId, dateFrom, dateTo);

            if (tickLogs == null || tickLogs.Count == 0)
            {
                return NotFound("00:00h");
            }

            TimeSpan totalTimeWorked = TimeSpan.Zero;
            DateTime? lastTickIn = null; // Keep track of the last "in" time

            foreach (var tickLog in tickLogs.OrderBy(t => t.Tick)) // Order by Tick time
            {
                if (lastTickIn == null) 
                {
                    lastTickIn = tickLog.Tick;
                }
                else 
                {
                    if (tickLog.Tick > lastTickIn) 
                    {
                        totalTimeWorked += tickLog.Tick - lastTickIn.Value;
                        lastTickIn = null;
                    }
                    else 
                    {
                        lastTickIn = tickLog.Tick; 
                    }
                }
            }

            if (lastTickIn != null)
            {
                totalTimeWorked += DateTime.Now - lastTickIn.Value;
            }

            string formattedTime = string.Format("{0:hh\\:mm}h", totalTimeWorked); 

            return Ok(formattedTime);
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
            tickLog.IsApproved = true;
            tickLog.Tick = DateTime.Now;

            await _tickLogRepository.CreateTickLogAsync(tickLog);

            var tickLogDto = _mapper.Map<TickLogDto>(tickLog);
            return CreatedAtAction(nameof(GetTickLogById), new { id = tickLog.Id }, tickLogDto);
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestTickLog(TickLogDto requestTickLogDto)
        {
            var tickLog = _mapper.Map<TickLog>(requestTickLogDto);
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

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveTickLog(int id)
        {
            var tickLog = await _tickLogRepository.GetTickLogByIdAsync(id);

            if (tickLog == null)
            {
                return NotFound();
            }

            tickLog.IsApproved = true;

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