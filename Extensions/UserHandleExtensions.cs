using BepInEx.DiscordSocialSDK.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace BepInEx.DiscordSocialSDK.Extensions
{
    public static class UserHandleExtensions
    {
        /// <summary>
        /// Returns avatar of user as bytes of png file
        /// </summary>
        /// <param name="userHandle">User</param>
        /// <param name="size">Size of avatar</param>
        /// <returns></returns>
        public static byte[] GetAvatarAsBytes(this UserHandle userHandle, AvatarSize size)
        {
            string avatarUrl = userHandle.AvatarUrl(UserHandle.AvatarType.Png, UserHandle.AvatarType.Png);
            int i = (int)size;
            string url = $"{avatarUrl}?size={i}";
            Logger.LogInfo($"Size={i}, url={url}");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                UnityEngine.AsyncOperation operation = request.Send();
                while (!operation.isDone) { }
                return request.downloadHandler.data;
            }
        }

        /// <summary>
        /// Returns avatar of user as <see cref="Texture2D"/>
        /// </summary>
        /// <param name="userHandle">User</param>
        /// <param name="size">Size of avatar</param>
        /// <returns></returns>
        public static Texture2D GetAvatarAsTexture(this UserHandle userHandle, AvatarSize size)
        {
            string avatarUrl = userHandle.AvatarUrl(UserHandle.AvatarType.Png, UserHandle.AvatarType.Png);
            int i = (int)size;
            string url = $"{avatarUrl}?size={i}";
            Logger.LogInfo($"Size={i}, url={url}");

            using (WWW www = new WWW(url))
            {
                while (!www.isDone) { }

                Texture2D texture = www.texture;
                texture.filterMode = FilterMode.Point;
                texture.name = $"{userHandle.Username()}Avatar";
                return texture;
            }
        }
    }
}
