using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BepInEx.DiscordSocialSDK
{
    internal class DiscordController : MonoBehaviour
    {
        [DllImport(DiscoerdSDKPlugin.DISCORD_LIBRARY_NAME,
               EntryPoint = "Discord_RunCallbacks",
               CallingConvention = CallingConvention.Cdecl)]
        public static extern void Discord_RunCallbacks();

        [DllImport(DiscoerdSDKPlugin.DISCORD_LIBRARY_NAME,
              EntryPoint = "Discord_ResetCallbacks",
              CallingConvention = CallingConvention.Cdecl)]
        public static extern void Discord_ResetCallbacks();


        public static DiscordController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            Discord_RunCallbacks();
        }
        private void OnDestroy()
        {
            Discord_ResetCallbacks();
        }
        private void OnApplicationQuit()
        {
            Discord_ResetCallbacks();
        }
    }
}
