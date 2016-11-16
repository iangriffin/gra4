namespace GRA.Data.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Model.Site, Domain.Model.Site>().ReverseMap();
        }
    }
}
