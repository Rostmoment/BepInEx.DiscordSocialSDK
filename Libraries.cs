using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace BepInEx.DiscordSocialSDK
{
    internal class Libraries
    {
        #region constants
        private const string PATH_PREFIX = "BepInEx.DiscordSocialSDK.libs.";
        private const string DLL_POSTFIX = ".dll";

        public const string DISCORD_LIBRARY_NAME = "discord_partner_sdk";

        public const string SYSTEM_MEMORY_NAME = "System.Memory";
        public const string SYSTEM_MEMORY_NAME_DLL = SYSTEM_MEMORY_NAME + DLL_POSTFIX;

        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME = "System.Runtime.CompilerServices.Unsafe";
        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL = SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME + DLL_POSTFIX;

        public const string SYSTEM_NUMERICS_VECTORS_NAME = "System.Numerics.Vectors";
        public const string SYSTEM_NUMERICS_VECTORS_NAME_DLL = SYSTEM_NUMERICS_VECTORS_NAME + DLL_POSTFIX;
        #endregion

        public static void LoadLibrary(string name, Func<bool> check) => LoadLibrary(name, check, true);

        private static byte[] ComputeHash(Stream stream)
        {
            using SHA256 sha = SHA256.Create();
            return sha.ComputeHash(stream);
        }

        private static byte[] GetResourceBytes(string resourceName)
        {
            Assembly assembly = DiscordSocialSDKPlugin.Instance.GetType().Assembly;
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new FileNotFoundException($"Embedded resource {resourceName} not found.");

            using MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }
        private static bool IsDllUpToDate(string path, string resourceName)
        {
            if (!File.Exists(path))
                return false;

            Assembly assembly = DiscordSocialSDKPlugin.Instance.GetType().Assembly;

            byte[] diskHash;
            using (FileStream fs = File.OpenRead(path))
                diskHash = ComputeHash(fs);

            byte[] resourceHash;
            using (Stream rs = assembly.GetManifestResourceStream(resourceName))
            {
                if (rs == null)
                    throw new FileNotFoundException($"Embedded resource {resourceName} not found.");
                resourceHash = ComputeHash(rs);
            }

            return diskHash.SequenceEqual(resourceHash);
        }

        private static void ExportResource(string resourceName, string dllPath)
        {
            byte[] bytes = GetResourceBytes(resourceName);
            File.WriteAllBytes(dllPath, bytes);
            Logger.LogInfo($"Exported {Path.GetFileName(dllPath)} to {dllPath}");
        }

        private static void LoadLibrary(string name, Func<bool> runtimeCheck, bool firstTime)
        {
            string pluginDir = Path.GetDirectoryName(DiscordSocialSDKPlugin.Instance.Info.Location);
            string dllPath = Path.Combine(pluginDir, name);
            string resourceName = PATH_PREFIX + name;

            if (File.Exists(dllPath) && !IsDllUpToDate(dllPath, resourceName))
            {
                Logger.LogWarning($"{name} on disk differs from embedded resource (outdated or corrupted). Re-exporting...");
                ExportResource(resourceName, dllPath);
            }
            else if (!File.Exists(dllPath))
            {
                if (!firstTime)
                    throw new FileNotFoundException($"Library {name} could not be loaded after export.");

                Logger.LogWarning($"{name} not found. Attempting to export from embedded resource...");
                ExportResource(resourceName, dllPath);
            }

            if (!runtimeCheck())
            {
                if (!firstTime)
                    throw new FileNotFoundException($"Library {name} could not be loaded.");

                LoadLibrary(name, runtimeCheck, false);
            }
            else
                Logger.LogInfo($"{name} is loaded and up to date.");
        }

        public static void CheckDiscordLibrary()
        {
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            string discordName = DISCORD_LIBRARY_NAME + ".dll";
            if (isLinux)
                discordName = $"lib{DISCORD_LIBRARY_NAME}.so";


            LoadLibrary(discordName, () =>
            {
                string path = Path.Combine(Path.GetDirectoryName(DiscordSocialSDKPlugin.Instance.Info.Location), discordName);
                return File.Exists(path);
            });
        }

        public static void CheckSystemMemoryLibrary()
        {
            LoadLibrary(SYSTEM_MEMORY_NAME_DLL, () =>
            {
                try { 
                    return Type.GetType("System.Span`1, System.Memory") != null; 
                }
                catch 
                { 
                    return false; 
                }
            });
        }

        public static void CheckUnsafeLibrary()
        {
            LoadLibrary(SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL, () =>
            {
                try 
                { 
                    return Type.GetType("System.Runtime.CompilerServices.Unsafe, System.Runtime.CompilerServices.Unsafe") != null; 
                }
                catch 
                {
                    return false; 
                }
            });
        }

        public static void CheckNumericsVectorsLibrary()
        {
            LoadLibrary(SYSTEM_NUMERICS_VECTORS_NAME_DLL, () =>
            {
                try 
                { 
                    return Type.GetType("System.Numerics.Vector3, System.Numerics.Vectors") != null; 
                }
                catch 
                { 
                    return false; 
                }
            });
        }
    }
}