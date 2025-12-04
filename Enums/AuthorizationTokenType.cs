using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  Represents the type of auth token used by the SDK, either the normal tokens produced by the
    ///  Discord desktop app, or an oauth2 bearer token. Only the latter can be used by the SDK.
    /// </summary>
    public enum AuthorizationTokenType
    {
        User = 0,
        Bearer = 1,
    }
}
