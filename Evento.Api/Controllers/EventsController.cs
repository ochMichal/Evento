using Evento.Infrastructure.Commands.Events;
using Evento.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Evento.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        // /events -> HTTP GET
        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            var events = await _eventService.BrowseAsync(name);

            return Json(events);
        }

        // /events -> HTTP GET
        [HttpGet("{eventId}")]
        public async Task<IActionResult> Get(Guid eventId)
        {
            var @event = await _eventService.GetAsync(eventId);
            if(@event == null)
            {
                return NotFound();
            }
            return Json(@event);
        }


        // /events -> HTTP POST
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEvent command)
        {
            command.EventId = Guid.NewGuid();
            await _eventService.CreateAsync(command.EventId, command.Name, command.Description, command.StartDate, command.EndDate);

            await _eventService.AddTicketsAsync(command.EventId, command.Tickets, command.Price);
            // status 201
            return Created($"/events/{command.EventId}", null);
        }

        // /events/{id} -> HTTP PUT

        [HttpPut("{eventId}")]
        public async Task<IActionResult> Put(Guid eventId,  [FromBody] UpdateEvent command)
        {
            await _eventService.UpdateAsync(eventId, command.Name, command.Description);

            //status 204
            return NoContent();
        }

        // /events/{id} -> HTTP DELETE
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> Delete(Guid eventId)
        {
            await _eventService.DeleteAsync(eventId);

            //status 204
            return NoContent();
        }
    }
}
