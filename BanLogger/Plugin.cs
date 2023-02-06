using System;
using Exiled.API.Features;

namespace BanLogger
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "BanLogger";
        public override string Author => "Jesus-QC (reborn by VersLugia)";
        public override Version Version { get; } = new Version(1, 0, 0);
        public static Plugin Singleton { get; private set; }
        private EventHandlers Events { get; set; }

        public override void OnEnabled()
        {
            Singleton = this;
            Events = new EventHandlers();
            Exiled.Events.Handlers.Player.IssuingMute += Events.OnIssuingMute;
            Exiled.Events.Handlers.Player.Kicking += Events.OnKicking;
            Exiled.Events.Handlers.Player.Banning += Events.OnBanning;
            Exiled.Events.Handlers.Player.Banned += Events.OnBanned;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.IssuingMute -= Events.OnIssuingMute;
            Exiled.Events.Handlers.Player.Kicking -= Events.OnKicking;
            Exiled.Events.Handlers.Player.Banning -= Events.OnBanning;
            Exiled.Events.Handlers.Player.Banned -= Events.OnBanned;
            Singleton = null;
            Events = null;
            base.OnDisabled();
        }
    }
}