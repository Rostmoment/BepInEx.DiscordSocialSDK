using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  The type of external identity provider.
    /// </summary>
    public enum ExternalIdentityProviderType
    {
        OIDC = 0,
        EpicOnlineServices = 1,
        Steam = 2,
        Unity = 3,
        DiscordBot = 4,
        None = 5,
        Unknown = 6,
    }
}
