using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  Represents the various identity providers that can be used to authenticate a provisional
    ///  account user for public clients.
    /// </summary>
    public enum AuthenticationExternalAuthType
    {
        OIDC = 0,
        EpicOnlineServicesAccessToken = 1,
        EpicOnlineServicesIdToken = 2,
        SteamSessionTicket = 3,
        UnityServicesIdToken = 4,
        DiscordBotIssuedAccessToken = 5,
        AppleIdToken = 6,
        PlayStationNetworkIdToken = 7,
    }
}
