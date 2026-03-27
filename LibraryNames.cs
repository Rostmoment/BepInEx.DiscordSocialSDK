using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK
{
    internal class LibraryNames
    {

        public const string DISCORD_LIBRARY_NAME = "discord_partner_sdk";
        public const string DISCORD_LIBRARY_NAME_DLL = DISCORD_LIBRARY_NAME + ".dll";
        public const string DISCORD_LIBRARY_RESOURE = "BepInEx.DiscordSocialSDK.libs." + DISCORD_LIBRARY_NAME_DLL;

        public const string SYSTEM_MEMORY_NAME = "System.Memory";
        public const string SYSTEM_MEMORY_NAME_DLL = SYSTEM_MEMORY_NAME + ".dll";
        public const string SYSTEM_MEMORY_RESOURCE = "BepInEx.DiscordSocialSDK.libs." + SYSTEM_MEMORY_NAME_DLL;

        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME = "System.Runtime.CompilerServices.Unsafe";
        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL = SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME + ".dll";
        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_RESOURCE = "BepInEx.DiscordSocialSDK.libs." + SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL;
    }
}
