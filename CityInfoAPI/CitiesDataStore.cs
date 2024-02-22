using CityInfoAPI.Models;

namespace CityInfoAPI
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }

        //public static CitiesDataStore current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto() { Id = 1, Name = "Tehran", Description = "this is tehran",
                PointOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 1,
                        Name = "jaye didani tehran",
                        Description = "this is awsome 1"
                    }
                }
                },
                new CityDto() { Id = 2, Name = "Kerman", Description = "this is kerman",
                PointOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 2,
                        Name = "jaye didani kerman",
                        Description = "this is awsome 2"
                    }
                }},
                new CityDto() { Id = 3, Name = "Shiraz", Description = "this is shiraz",
                PointOfInterest = new List<PointOfInterestDto>()
                {
                    new PointOfInterestDto()
                    {
                        Id = 3,
                        Name = "jaye didani shiraz",
                        Description = "this is awsome 3"
                    }
                }},
            };
        }
    }
}
