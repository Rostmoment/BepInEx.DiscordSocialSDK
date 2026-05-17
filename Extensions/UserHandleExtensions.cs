using BepInEx.DiscordSocialSDK.Enums;
using BepInEx.DiscordSocialSDK.Handles;
using System;
using System.Collections;
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
        /// <returns>Bytes of avatar</returns>
        public static byte[] GetAvatarAsBytes(this UserHandle userHandle, AvatarSize size)
        {
            string url = GetUrl(userHandle, size);

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
        /// <returns>Avatar as <see cref="Texture2D"/></returns>
        public static Texture2D GetAvatarAsTexture(this UserHandle userHandle, AvatarSize size)
        {
            string url = GetUrl(userHandle, size);

            using (WWW www = new WWW(url))
            {
                while (!www.isDone) { }

                Texture2D texture = www.texture;
                texture.filterMode = FilterMode.Point;
                texture.name = $"{userHandle.Username()}Avatar";
                return texture;
            }
        }

        /// <summary>
        /// Coroutine for getting avatar as bytes of png
        /// </summary>
        /// <param name="userHandle">User</param>
        /// <param name="size">Size of avatar</param>
        /// <param name="onFinished">Action to invoke when finished</param>
        public static IEnumerator GetAvatarAsBytes(this UserHandle userHandle, AvatarSize size, Action<byte[]> onFinished)
        {
            string url = GetUrl(userHandle, size);

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.Send();
                onFinished.Invoke(request.downloadHandler.data);
            }
        }

        /// <summary>
        /// Coroutine for getting avatar as <see cref="Texture2D"/>
        /// </summary>
        /// <param name="userHandle">User</param>
        /// <param name="size">Size of avatar</param>
        /// <param name="onFinished">Action to invoke when finished</param>
        public static IEnumerator GetAvatarAsTexture(this UserHandle userHandle, AvatarSize size, Action<Texture2D> onFinished)
        {
            string url = GetUrl(userHandle, size);

            using (UnityWebRequest request = UnityWebRequest.GetTexture(url))
            {
                yield return request.Send();

                if (request.isError || request.responseCode != 200)
                {
                    onFinished?.Invoke(null);
                }
                else
                {
                    Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    if (texture != null)
                    {
                        texture.filterMode = FilterMode.Point;
                        texture.name = userHandle.Username() + "Avatar";
                    }
                    onFinished?.Invoke(texture);
                }
            }
        }

        private static string GetUrl(UserHandle userHandle, AvatarSize size)
        {
            string avatarUrl = userHandle.AvatarUrl(UserHandle.AvatarType.Png, UserHandle.AvatarType.Png);
            return $"{avatarUrl}?size={(int)size}";
        }
    }
}
