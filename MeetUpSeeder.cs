using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeetUpAPI.Entities;

namespace MeetUpAPI
{
    public class MeetUpSeeder
    {
        private readonly MeetUpContext _meetUpContext;

        public MeetUpSeeder(MeetUpContext meetUpContext)
        {
            _meetUpContext = meetUpContext;
        }

        public void Seed()
        {
            if (_meetUpContext.Database.CanConnect())
            {
                if (!_meetUpContext.MeetUps.Any())
                {
                    InsertSampleData();
                } 
            }

        }

        private void InsertSampleData()
        {
            var meetups = new List<MeetUp>
            {
                new MeetUp
                {
                    Name = "Web Api",
                    Date = DateTime.Now.AddDays(7),
                    IsPrivate = false,
                    Organizer = "Microsoft",
                    Location = new Location
                    {
                        City = "Tehran",
                        Street = "Azadi",
                        PostCode = "31-227"
                    },
                    Lectures = new List<Lecture>
                    {
                        new Lecture
                        {
                            Author = "Shahab",
                            Topic = ".net5",
                            Description = "Deep Dive"
                        }
                    }
                }
            };

           _meetUpContext.AddRange(meetups);
           _meetUpContext.SaveChanges();
        }
    }
}
