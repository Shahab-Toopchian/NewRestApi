using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MeetUpAPI.Entities;
using MeetUpAPI.Models;

namespace MeetUpAPI
{
    public class MeetUpProfile :Profile
    {
        public MeetUpProfile()
        {
            CreateMap<MeetUp, MeetUpDetailsDto>()
                .ForMember(m => m.City, map => map.MapFrom(meetup => meetup.Location.City))
                .ForMember(m => m.PostCode, map => map.MapFrom(meetup => meetup.Location.PostCode))
                .ForMember(m => m.Street, map => map.MapFrom(meetup => meetup.Location.Street));

            CreateMap<MeetUpDto, MeetUp>();
            CreateMap<LectureDto, Lecture>().ReverseMap();

        }
    }
}
