using AutoMapper;
using BadgeProject.DTO;
using BadgeProject.Model;

namespace BadgeProject.AutoMapper
{
    public class Auto_Mapper:Profile
    {
        public Auto_Mapper()
        {
            CreateMap<User, RegisterDTO>().ReverseMap();

        }

    }
}
