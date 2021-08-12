using System.Collections.Generic;
using AutoMapper;
using HappyBot.Application.Common.Mappings;
using HappyBot.Domain.Entities;
using Telegram.Bot;

namespace HappyBot.Application.Common.Models
{
    public class TelegramBotDto : IMapFrom<TelegramBot>
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Token { get; set; }

        public bool IsMainBot { get; set; }

        public int UserId { get; set; }
        
        public ITelegramBotClient Client { get; set; }

        public bool IsEnable { get; set; }

        public IList<ButtonDto> Buttons { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TelegramBot, TelegramBotDto>();
            profile.CreateMap<TelegramBot, TelegramBotDto>().ReverseMap();
        }
    }
}