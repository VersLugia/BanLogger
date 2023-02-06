using System.Collections.Generic;
using System.ComponentModel;
using BanLogger.Enums;
using Exiled.API.Interfaces;

namespace BanLogger
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        #region Keys
        [Description("Steam Api key to get the nickname of obanned users (Get your api key in https://steamcommunity.com/dev/apikey)")]
        public string SteamApiKey { get; set; } = "000000";
        [Description("Private Webhooks (Mute/Kick/Ban/OBan)")]
        public Dictionary<PunishmentType, string> PublicWebhooks { get; set; } = new Dictionary<PunishmentType, string>
        {
            [PunishmentType.Mute] = "https://discord.com/api/webhooks/webhook.id/webhook.token",
            [PunishmentType.Kick] = "https://discord.com/api/webhooks/webhook.id/webhook.token",
            [PunishmentType.Ban] = "https://discord.com/api/webhooks/webhook.id/webhook.token"
        };
        [Description("Private Webhooks (Mute/Kick/Ban/OBan) Contains IDs and more info")]
        public Dictionary<PunishmentType, string> PrivateWebhooks { get; set; } = new Dictionary<PunishmentType, string>
        {
            [PunishmentType.OfflineBan] = "https://discord.com/api/webhooks/webhook.id/webhook.token"
        };
        #endregion
        #region Webhook Settings
        [Description("Webhook Username")]
        public string Username { get; set; } = "My Server #1 | Security";
        [Description("Webhook avatar image")]
        public string AvatarUrl { get; set; } = "https://imgur.com/FpPpR90.png";
        [Description("Tss discord message")]
        public bool Tts { get; set; } = false;
        [Description("Hex Color of the webhook")]
        public string HexColor { get; set; } = "#D10E11";
        [Description("Content out of the embed, you can for example ping a role")]
        public string Content { get; set; } = "<@000000000000000000>";
        [Description("Title of the embed")]
        public string Title { get; set; } = "BAN LOGGER";
        [Description("Webhook image URL")]
        public string ImageUrl { get; set; } = "https://imgur.com/spXs9Bq.png";
        [Description("Footer Text")]
        public string FooterText { get; set; } = "";
        [Description("Footer IconUrl")]
        public string FooterIconUrl { get; set; } = "https://imgur.com/spXs9Bq.png";
        #endregion
        #region Translations
        [Description("Default: User banned:")]
        public string UserBannedText { get; set; } = "User Banned:";
        [Description("Default: Issuing staff:")]
        public string IssuingStaffText { get; set; } = "Issuing staff:";
        [Description("Default: Reason:")]
        public string ReasonText { get; set; } = "Reason:";
        [Description("Default: Time banned:")]
        public string TimeBannedText { get; set; } = "Time banned:";
        #endregion
    }
}