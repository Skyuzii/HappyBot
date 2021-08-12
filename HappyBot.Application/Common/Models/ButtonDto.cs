using AutoMapper;
using HappyBot.Application.Common.Mappings;
using HappyBot.Domain.Entities;

namespace HappyBot.Application.Common.Models
{
    public class ButtonDto : IMapFrom<Button>
    {
        public int Id { get; set; }
        
        public string HeaderName { get; set; }
        
        public string Name { get; set; }

        public int Type { get; set; }
        
        public string MessageText { get; set; }
        
        public int TelegramBotId { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Button, ButtonDto>();
            profile.CreateMap<Button, ButtonDto>().ReverseMap();
        }
    }
}