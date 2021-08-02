namespace HappyBot.Application.Common.Models
{
    public class SettingsTelegramBot
    {
        public string Url { get; set; }

        public SettingsMainBot SettingsMainBot { get; set; }
    }

    public class SettingsMainBot
    {
        public string Token { get; set; }
    }
}