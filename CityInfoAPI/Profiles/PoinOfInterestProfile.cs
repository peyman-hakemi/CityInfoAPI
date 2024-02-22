using AutoMapper;

namespace CityInfoAPI.Profiles
{
    public class PoinOfInterestProfile: Profile
    {
        public PoinOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
        }
    }
}
