using System;
using BanLogger_NWApi.Enums;
using BanLogger_NWApi.Structs;
using BanLogger_NWApi.Utils;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;

namespace BanLogger_NWApi
{
    class EventHandlers
    {
        #region Mute Logger
        [PluginEvent(ServerEventType.PlayerMuted)]
        public void OnPlayerMuted(Player player, bool isIntercom)
        {
            if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.Mute, out string privateToken) &&
                privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token.DefaultUrl")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(player.Nickname, player.UserId),
                    new UserInfo("n/a", "n/a"), "mute", -1), PunishmentType.Mute, WebhookType.Private);
            }
            
            if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.Mute, out string publicToken) &&
                publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(player.Nickname, player.UserId),
                    new UserInfo("n/a", "n/a"), "mute", -1), PunishmentType.Mute, WebhookType.Public);
            }
        }
        #endregion

        #region Kick Logger
        [PluginEvent(ServerEventType.PlayerKicked)]
        public void OnPlayerKicked(Player player, CommandSender issuer, string reason)
        {
            if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.Kick, out string privateToken) &&
                privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(player.Nickname, player.UserId), 
                    new UserInfo(issuer?.Nickname ?? "Server Console", issuer?.SenderId ?? "n/a"),
                    reason, -1), PunishmentType.Kick, WebhookType.Private);
            }
            
            if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.Kick, out string publicToken) &&
                publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(player.Nickname, player.UserId),
                        new UserInfo(issuer?.Nickname ?? "Server Console", issuer?.SenderId ?? "n/a"),
                        reason, -1), PunishmentType.Kick, WebhookType.Public);
            }
        }
        #endregion

        #region Ban Logger
        [PluginEvent(ServerEventType.PlayerBanned)]
        public void OnPlayerBanned(Player player, CommandSender issuer, string reason, long duration)
        {
            if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.Ban, out string privateToken) &&
                privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(player.Nickname, player.UserId),
                    new UserInfo(issuer?.Nickname ?? "Server Console", issuer?.SenderId ?? "n/a"),
                    reason, duration), PunishmentType.Ban, WebhookType.Private);
            }
                
            if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.Ban, out string publicToken) &&
                publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
            {
                Methods.CreateEmbed(new BanInfo(new UserInfo(player.Nickname, player.UserId),
                    new UserInfo(issuer?.Nickname ?? "Server Console", issuer?.SenderId ?? "n/a"),
                    reason, duration), PunishmentType.Ban, WebhookType.Public);
            }
        }
        #endregion
        
        #region Offline Ban 
        [PluginEvent(ServerEventType.BanIssued)]
        public void OnBanIssued(BanDetails banDetails, BanHandler.BanType banType)
        {
            if (banType != BanHandler.BanType.UserId)
                return;

            if (banDetails.OriginalName != "Unknown - offline ban")
                return;
            
            if (banDetails.Id.Contains("@steam") && !string.IsNullOrEmpty(Plugin.Singleton.Config.SteamApiKey))
            {
                try
                {
                    if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.OfflineBan, out string privateToken) &&
                        privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                    {
                        Methods.CreateEmbed(new BanInfo(new UserInfo(Methods.GetUserName(banDetails.Id), banDetails.Id),
                            new UserInfo(banDetails.Issuer ?? "Server Console", "n/a"), 
                            banDetails.Reason, -1), PunishmentType.OfflineBan, WebhookType.Private);
                    }

                    if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.OfflineBan, out string publicToken) &&
                        publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                    {
                        Methods.CreateEmbed(new BanInfo(new UserInfo(Methods.GetUserName(banDetails.Id), banDetails.Id),
                            new UserInfo(banDetails.Issuer ?? "Server Console", "n/a"),
                            banDetails.Reason, -1), PunishmentType.OfflineBan, WebhookType.Public);
                    }
                }
                catch (Exception)
                {
                    Log.Error("An error has ocurred trying to get the username of an obaned user.");

                    if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.OfflineBan, out string privateToken) &&
                        privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                    {
                        Methods.CreateEmbed(new BanInfo(new UserInfo("Unknown (Incorrect API Key)", banDetails.Id),
                            new UserInfo(banDetails.Issuer ?? "Server Console", "n/a"),
                            banDetails.Reason, -1), PunishmentType.OfflineBan, WebhookType.Private);
                    }

                    if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.OfflineBan, out string publicToken) &&
                        publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                    {
                        Methods.CreateEmbed(new BanInfo(new UserInfo("Unknown (Incorrect API Key)", banDetails.Id),
                            new UserInfo(banDetails.Issuer ?? "Server Console", "n/a"),
                            banDetails.Reason, -1), PunishmentType.OfflineBan, WebhookType.Public);
                    }
                }
            }
            else
            {
                if (Plugin.Singleton.Config.PrivateWebhooks.TryGetValue(PunishmentType.OfflineBan, out string privateToken) &&
                    privateToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                {
                    Methods.CreateEmbed(new BanInfo(new UserInfo("Unknown (OBan)", banDetails.Id),
                        new UserInfo(banDetails.Issuer?? "Server Console", "n/a"),
                        banDetails.Reason, -1), PunishmentType.OfflineBan, WebhookType.Private);
                }
                
                if (Plugin.Singleton.Config.PublicWebhooks.TryGetValue(PunishmentType.OfflineBan,
                        out string publicToken) &&
                    publicToken != "https://discord.com/api/webhooks/webhook.id/webhook.token")
                {
                    Methods.CreateEmbed(new BanInfo(new UserInfo("Unknown (OBan)", banDetails.Id),
                        new UserInfo(banDetails.Issuer ?? "Server Console", "n/a"),
                        banDetails.Reason, -1), PunishmentType.OfflineBan, WebhookType.Public);
                }
            }
        }
        #endregion
    }
}