using AutoMapper;
using Entities.Dtos;
using Entities.Models;

namespace Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public MappingProfile()
        {
            _ = CreateMap<User, UserDto>();
        }
    }
}
