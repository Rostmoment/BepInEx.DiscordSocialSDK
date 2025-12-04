using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Enums
{
    /// <summary>
    ///  ActivityActionTypes represents the type of invite being sent to a user.
    /// </summary>
    /// <remarks>
    ///  There are essentially two types of invites:
    ///  1: A user with an existing activity party can invite another user to join that existing party
    ///  2: A user can request to join the existing activity party of another user
    ///
    ///  See https://discord.com/developers/docs/rich-presence/overview for more information.
    ///
    /// </remarks>
    public enum ActivityActionTypes
    {
        Invalid = 0,
        Join = 1,
        JoinRequest = 5,
    }
}
