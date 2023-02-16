using BanLogger_NWApi.Utils;

namespace BanLogger_NWApi.Structs
{
    public class BanInfo
    {
        public BanInfo(UserInfo bannedUserInfo, UserInfo issuerUserInfo, string reason, long duration)
        {
            BannedUserInfo = bannedUserInfo;
            IssuerUserInfo = issuerUserInfo;
            Reason = reason;
            Duration = duration;
        }
        
        public UserInfo BannedUserInfo;
        public UserInfo IssuerUserInfo;
        public string Reason;
        public long Duration;

        public string ReadableDuration => Duration == -1 ? "Unknown" : Methods.TimeFormatter(Duration);
    }
}