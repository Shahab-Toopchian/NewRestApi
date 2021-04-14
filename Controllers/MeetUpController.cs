using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MeetUpAPI.Entities;
using MeetUpAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MeetUpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetUpController : ControllerBase
    {
        private readonly MeetUpContext _meetUpContext;
        private readonly IMapper _mapper;
        private readonly ILogger<MeetUpController> _logger;

        public MeetUpController(MeetUpContext meetUpContext, IMapper mapper,ILogger<MeetUpController> logger)
        {
            _meetUpContext = meetUpContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<List<MeetUpDetailsDto>> Get()
        {
            var meetupList = _meetUpContext.MeetUps.Include(m=>m.Location).ToList();
            var meetupDto = _mapper.Map<List<MeetUpDetailsDto>>(meetupList);
            return Ok(meetupDto);
        }

        [HttpGet("{name}")]
        public ActionResult<MeetUpDetailsDto> Get(string name)
        {
            var meetup = _meetUpContext.MeetUps.Include(m => m.Location).Include(m=>m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            var meetupDto = _mapper.Map<MeetUpDetailsDto>(meetup);
            return Ok(meetupDto);
        }

        [HttpPost]
        public ActionResult Post([FromBody]MeetUpDto model)
        {
            if (ModelState.IsValid)
            {
                var meetup = _mapper.Map<MeetUp>(model);
                _meetUpContext.MeetUps.Add(meetup);
                _meetUpContext.SaveChanges();
                var key = meetup.Name.Replace(" ", "-").ToLower();
                return Created("api/meetup/" + key, null);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{name}")]
        public ActionResult Put(string name, [FromBody] MeetUpDto model)
        {
            var meetup = _meetUpContext.MeetUps.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            meetup.Name = model.Name;
            meetup.Organizer = model.Organizer;
            meetup.Date = model.Date;
            meetup.IsPrivate = model.IsPrivate;

            _meetUpContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{name}")]
        public ActionResult Delete(string name)
        {
            //nlog test
           //_logger.LogWarning($"MeetUp's {name} were Del");
            var meetup = _meetUpContext.MeetUps.FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == name.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            _meetUpContext.Remove(meetup);
            _meetUpContext.SaveChanges();
            return NoContent();
        }

    }
}
