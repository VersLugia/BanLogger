using System;
using System.Globalization;
using BanLogger.Enums;
using BanLogger.Structs;
using BanLogger.Utils;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace BanLogger
{
    class EventHandlers
    {
        #region Mute Logger
        public void OnIssuingMute(IssuingMuteEventArgs ev)
        {
            if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.Mute, out string privateToken) &&
                privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token.DefaultUrl")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(ev.Player.Nickname, ev.Player.UserId),
                    new UserInfo("n/a", "n/a"), "mute", -1), PunishmentType.Mute, WebhookType.Private);
            }
            
            if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.Mute, out string publicToken) &&
                publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(ev.Player.Nickname, ev.Player.UserId),
                    new UserInfo("n/a", "n/a"), "mute", -1), PunishmentType.Mute, WebhookType.Public);
            }
        }
        #endregion

        #region Kick Logger
        public void OnKicking(KickingEventArgs ev)
        {
            if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.Kick, out string privateToken) &&
                privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(ev.Target.Nickname, ev.Target.UserId), 
                    new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"),
                    ev.Reason, -1), PunishmentType.Kick, WebhookType.Private);
            }
            
            if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.Kick, out string publicToken) &&
                publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(ev.Target.Nickname, ev.Target.UserId),
                        new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"),
                        ev.Reason, -1), PunishmentType.Kick, WebhookType.Public);
            }
        }
        #endregion

        #region Ban Logger
        public void OnBanning(BanningEventArgs ev)
        {
            if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.Ban, out string privateToken) &&
                privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(ev.Target.Nickname, ev.Target.UserId),
                    new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"),
                    ev.Reason, ev.Duration), PunishmentType.Ban, WebhookType.Private);
            }
                
            if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.Ban, out string publicToken) &&
                publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(ev.Target.Nickname, ev.Target.UserId),
                    new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"),
                    ev.Reason, ev.Duration), PunishmentType.Ban, WebhookType.Public);
            }
        }
        #endregion
        
        #region Offline Ban Logger
        public void OnBanned(BannedEventArgs ev)
        {
            if (ev.Type != BanHandler.BanType.UserId)
                return;

            if (ev.Details.OriginalName != "Unknown - offline ban")
                return;
            
            if (ev.Details.Id.Contains("@steam") && !string.IsNullOrEmpty(Plugin.Singleton.Config.SteamApiKey))
            {
                try
                {
                    if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.OfflineBan, out string privateToken) &&
                        privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                    {
                        Methods.CreateEmbed(new BanInfo(new UserInfo(Methods.GetUserName(ev.Details.Id), ev.Details.Id),
                            new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"), 
                            ev.Details.Reason, -1), PunishmentType.OfflineBan, WebhookType.Private);
                    }

                    if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.OfflineBan, out string publicToken) &&
                        publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                    {
                        Methods.CreateEmbed(new BanInfo(new UserInfo(Methods.GetUserName(ev.Details.Id), ev.Details.Id),
                            new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"),
                            ev.Details.Reason, -1), PunishmentType.OfflineBan, WebhookType.Public);
                    }
                }
                catch (Exception)
                {
                    Log.Error("An error has ocurred trying to get the username of an obaned user.");

                    if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.OfflineBan, out string privateToken) &&
                        privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                    {
                        Methods.CreateEmbed(new BanInfo(new UserInfo("Unknown (Incorrect API Key)", ev.Details.Id),
                            new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"),
                            ev.Details.Reason, -1), PunishmentType.OfflineBan, WebhookType.Private);
                    }

                    if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.OfflineBan, out string publicToken) &&
                        publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                    {
                        Methods.CreateEmbed(new BanInfo(new UserInfo("Unknown (Incorrect API Key)", ev.Details.Id),
                            new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"),
                            ev.Details.Reason, -1), PunishmentType.OfflineBan, WebhookType.Public);
                    }
                }
            }
            else
            {
                if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.OfflineBan, out string privateToken) &&
                    privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                {
                    Methods.CreateEmbed(new BanInfo(new UserInfo("Unknown (OBan)", ev.Details.Id),
                        new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"),
                        ev.Details.Reason, -1), PunishmentType.OfflineBan, WebhookType.Private);
                }

                if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.OfflineBan,
                        out string publicToken) &&
                    publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                {
                    Methods.CreateEmbed(new BanInfo(new UserInfo("Unknown (OBan)", ev.Details.Id),
                        new UserInfo(ev.Player?.Nickname ?? "Server Console", ev.Player?.UserId ?? "n/a"),
                        ev.Details.Reason, -1), PunishmentType.OfflineBan, WebhookType.Public);
                }
            }
        }
        #endregion
    }
}