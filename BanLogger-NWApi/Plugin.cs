using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace BanLogger_NWApi
{
    public class Plugin
    {
        public static Plugin Singleton { get; private set; }
        [PluginConfig] public Config Config;
        
        [PluginEntryPoint("BanLogger", "1.0.0",
            "Sends configurable webhook messages.",
            "VersLugia (original exiled version Jesus-QC)")]
        void OnLoad()
        {
            if (!Config.IsEnabled)
                return;

            Singleton = this;
            EventManager.RegisterEvents<EventHandlers>(this);
        }

        [PluginUnload]
        void OnUnload()
        {
            Singleton = null;
            EventManager.UnregisterEvents<EventHandlers>(this);
        }
    }
}