using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using BanLogger_NWApi.Enums;
using BanLogger_NWApi.Structs;
using MEC;
using PluginAPI.Core;

namespace BanLogger_NWApi.Utils
{
    public static class Methods
    {
        public static void CreateEmbed(BanInfo banInfo, PunishmentType punishmentType, WebhookType webhookType)
        {
            Embed embed = new Embed
            {
                title = Plugin.Singleton.Config.Title,
                color = int.Parse(Plugin.Singleton.Config.HexColor.Replace("#", ""), NumberStyles.HexNumber),
                author = new EmbedAuthor(Plugin.Singleton.Config.Username, null, Plugin.Singleton.Config.ImageUrl),
                description = CreateEmbedDescription(banInfo, punishmentType, webhookType),
                image = new EmbedImage(Plugin.Singleton.Config.ImageUrl),
                footer = new EmbedFooter(Plugin.Singleton.Config.FooterText, Plugin.Singleton.Config.FooterIconUrl)
            };

            string webhook = webhookType == WebhookType.Public ? Plugin.Singleton.Config.PublicWebhooks[punishmentType] : Plugin.Singleton.Config.PrivateWebhooks[punishmentType];
                
            if (DiscordHandler.Queue.ContainsKey(webhook))
                DiscordHandler.Queue[webhook].Add(embed);
            else
                DiscordHandler.Queue.Add(webhook, new List<Embed> { embed });

            if (!DiscordHandler.CoroutineHandle.IsRunning)
                DiscordHandler.CoroutineHandle = Timing.RunCoroutine(DiscordHandler.HandleQueue());
        }

        public static string CreateEmbedDescription(BanInfo banInfo, PunishmentType punishmentType, WebhookType webhookType)
        {
            var fields = new List<Field>();
            
            switch (punishmentType)
            {
                case PunishmentType.Mute when webhookType == WebhookType.Public:
                    fields = new List<Field>
                    {
                        new Field(Plugin.Singleton .Config.UserBannedText, banInfo.BannedUserInfo.PublicInfo),
                        new Field(Plugin.Singleton.Config.IssuingStaffText, banInfo.IssuerUserInfo.PublicInfo),
                        new Field(Plugin.Singleton.Config.ReasonText, banInfo.Reason),
                        new Field(Plugin.Singleton.Config.TimeBannedText, "Mute"),
                    };
                    break;
                case PunishmentType.Mute when  webhookType == WebhookType.Private:
                    fields = new List<Field>
                    {
                        new Field(Plugin.Singleton.Config.UserBannedText, banInfo.BannedUserInfo.PrivateInfo),
                        new Field(Plugin.Singleton.Config.IssuingStaffText, banInfo.IssuerUserInfo.PrivateInfo),
                        new Field(Plugin.Singleton.Config.ReasonText, banInfo.Reason),
                        new Field(Plugin.Singleton.Config.TimeBannedText, "Mute"),
                    };
                    break;
                case PunishmentType.Kick when webhookType == WebhookType.Public:
                    fields = new List<Field>
                    {
                        new Field(Plugin.Singleton.Config.UserBannedText, banInfo.BannedUserInfo.PublicInfo),
                        new Field(Plugin.Singleton.Config.IssuingStaffText, banInfo.IssuerUserInfo.PublicInfo),
                        new Field(Plugin.Singleton.Config.ReasonText, banInfo.Reason),
                        new Field(Plugin.Singleton.Config.TimeBannedText, "Kick"),
                    };
                    break;
                case PunishmentType.Kick when webhookType == WebhookType.Private:
                    fields = new List<Field>
                    {
                        new Field(Plugin.Singleton.Config.UserBannedText, banInfo.BannedUserInfo.PrivateInfo),
                        new Field(Plugin.Singleton.Config.IssuingStaffText, banInfo.IssuerUserInfo.PrivateInfo),
                        new Field(Plugin.Singleton.Config.ReasonText, banInfo.Reason),
                        new Field(Plugin.Singleton.Config.TimeBannedText, "Kick"),
                    };
                    break;
                case PunishmentType.Ban when webhookType == WebhookType.Public:
                    fields = new List<Field>
                    {
                        new Field(Plugin.Singleton.Config.UserBannedText, banInfo.BannedUserInfo.PublicInfo),
                        new Field(Plugin.Singleton.Config.IssuingStaffText, banInfo.IssuerUserInfo.PublicInfo),
                        new Field(Plugin.Singleton.Config.ReasonText, banInfo.Reason),
                        new Field(Plugin.Singleton.Config.TimeBannedText, banInfo.ReadableDuration),
                    };
                    break;
                case PunishmentType.Ban when webhookType == WebhookType.Private:
                    fields = new List<Field>
                    {
                        new Field(Plugin.Singleton.Config.UserBannedText, banInfo.BannedUserInfo.PrivateInfo),
                        new Field(Plugin.Singleton.Config.IssuingStaffText, banInfo.IssuerUserInfo.PrivateInfo),
                        new Field(Plugin.Singleton.Config.ReasonText, banInfo.Reason),
                        new Field(Plugin.Singleton.Config.TimeBannedText, banInfo.ReadableDuration),
                    };
                    break;
                case PunishmentType.OfflineBan when webhookType == WebhookType.Public:
                    fields = new List<Field>
                    {
                        new Field(Plugin.Singleton.Config.UserBannedText, banInfo.BannedUserInfo.PublicInfo),
                        new Field(Plugin.Singleton.Config.IssuingStaffText, banInfo.IssuerUserInfo.PublicInfo),
                        new Field(Plugin.Singleton.Config.ReasonText, banInfo.Reason),
                        new Field(Plugin.Singleton.Config.TimeBannedText, banInfo.ReadableDuration),
                    };
                    break;
                case PunishmentType.OfflineBan when webhookType == WebhookType.Private:
                    fields = new List<Field>
                    {
                        new Field(Plugin.Singleton.Config.UserBannedText, banInfo.BannedUserInfo.PrivateInfo),
                        new Field(Plugin.Singleton.Config.IssuingStaffText, banInfo.IssuerUserInfo.PrivateInfo),
                        new Field(Plugin.Singleton.Config.ReasonText, banInfo.Reason),
                        new Field(Plugin.Singleton.Config.TimeBannedText, banInfo.ReadableDuration),
                    };
                    break;
            }

            return fields.Join();
        }

        public static string GetUserName(string userid)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest) WebRequest.Create($"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={Plugin.Singleton.Config.SteamApiKey}&steamids={userid}");
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return Regex.Match(result, @"\x22personaname\x22:\x22(.+?)\x22").Groups[1].Value;
                }
            }
            catch (Exception)
            {
                Log.Error("An error has occured while contacting steam servers (Are they down? Invalid API key?)");
            }

            return "Unknown";
        }
        
        public static string Join(this List<Field> fields)
        {
            return fields.Aggregate("", (current, field) => current + field);
        }

        public static string TimeFormatter(long duration)
        {
            var timespan = new TimeSpan(0, 0, (int)duration);
            var finalFormat = "";

            if (timespan.TotalDays >= 365)
                finalFormat += $" {timespan.TotalDays / 365}y";
            else if (timespan.TotalDays >= 30)
                finalFormat += $" {timespan.TotalDays / 30}mon";
            else if (timespan.TotalDays >= 1)
                finalFormat += $" {timespan.TotalDays}d";
            else if (timespan.Hours > 0)
                finalFormat += $" {timespan.Hours}h";
            if (timespan.Minutes > 0)
                finalFormat += $" {timespan.Minutes}min";
            if (timespan.Seconds > 0)
                finalFormat += $" {timespan.Seconds}s";

            return finalFormat;
        }
    }
}