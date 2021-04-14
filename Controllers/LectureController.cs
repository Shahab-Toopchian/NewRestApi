using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MeetUpAPI.Entities;
using MeetUpAPI.Models;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.EntityFrameworkCore;

namespace MeetUpAPI.Controllers
{
    [Route("api/meetup/{meetupName}/lecture")]
    [ApiController]
    public class LectureController : ControllerBase
    {
        private readonly MeetUpContext _meetUpContext;
        private readonly IMapper _mapper;

        public LectureController(MeetUpContext meetUpContext, IMapper mapper)
        {
            _meetUpContext = meetUpContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Get(string meetupName)
        {
            var meetup = _meetUpContext.MeetUps.Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());
            if (meetup == null)
            {
                return NotFound();
            }

            var lectures = _mapper.Map<List<LectureDto>>(meetup.Lectures);
            return Ok(lectures);
        }

        [HttpPost]
        public ActionResult Post(string meetupName, [FromBody] LectureDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var meetup = _meetUpContext.MeetUps.Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            var lecture = _mapper.Map<Lecture>(model);
            meetup.Lectures.Add(lecture);
            _meetUpContext.SaveChanges();
            return Created($"api/meetup/{meetupName}", null);
        }

        [HttpDelete]
        public ActionResult Delete(string meetupName)
        {
            var meetup = _meetUpContext.MeetUps.Include(m=>m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            _meetUpContext.Lectures.RemoveRange(meetup.Lectures);
            _meetUpContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string meetupName,int id)
        {


            var meetup = _meetUpContext.MeetUps.Include(m => m.Lectures)
                .FirstOrDefault(m => m.Name.Replace(" ", "-").ToLower() == meetupName.ToLower());

            if (meetup == null)
            {
                return NotFound();
            }

            var lecture = meetup.Lectures.FirstOrDefault(x => x.Id == id);
            if (lecture == null)
            {
                return NotFound();
            }
            _meetUpContext.Lectures.Remove(lecture);
            _meetUpContext.SaveChanges();
            return NoContent();
        }
    }
}
