using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace BepInEx.DiscordSocialSDK
{
    internal class Libraries
    {
        #region constants
        private const string PATH_PREFIX = "BepInEx.DiscordSocialSDK.libs.";
        private const string DLL_POSTFIX = ".dll";

        public const string DISCORD_LIBRARY_NAME = "discord_partner_sdk";
        public const string DISCORD_LIBRARY_NAME_DLL = DISCORD_LIBRARY_NAME + DLL_POSTFIX;
        public const string DISCORD_LIBRARY_RESOURCE = PATH_PREFIX + DISCORD_LIBRARY_NAME_DLL;

        public const string SYSTEM_MEMORY_NAME = "System.Memory";
        public const string SYSTEM_MEMORY_NAME_DLL = SYSTEM_MEMORY_NAME + DLL_POSTFIX;
        public const string SYSTEM_MEMORY_RESOURCE = PATH_PREFIX + SYSTEM_MEMORY_NAME_DLL;

        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME = "System.Runtime.CompilerServices.Unsafe";
        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL = SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME + DLL_POSTFIX;
        public const string SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_RESOURCE = PATH_PREFIX + SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME_DLL;

        public const string SYSTEM_NUMERICS_VECTORS_NAME = "System.Numerics.Vectors";
        public const string SYSTEM_NUMERICS_VECTORS_NAME_DLL = SYSTEM_NUMERICS_VECTORS_NAME + DLL_POSTFIX;
        public const string SYSTEM_NUMERICS_VECTORS_RESOURCE = PATH_PREFIX + SYSTEM_NUMERICS_VECTORS_NAME_DLL;
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
        private static bool IsDllUpToDate(string dllPath, string resourceName)
        {
            if (!File.Exists(dllPath))
                return false;

            Assembly assembly = DiscordSocialSDKPlugin.Instance.GetType().Assembly;

            byte[] diskHash;
            using (FileStream fs = File.OpenRead(dllPath))
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
            string dllName = name + DLL_POSTFIX;
            string pluginDir = Path.GetDirectoryName(DiscordSocialSDKPlugin.Instance.Info.Location);
            string dllPath = Path.Combine(pluginDir, dllName);
            string resourceName = PATH_PREFIX + dllName;

            if (File.Exists(dllPath) && !IsDllUpToDate(dllPath, resourceName))
            {
                Logger.LogWarning($"{dllName} on disk differs from embedded resource (outdated or corrupted). Re-exporting...");
                ExportResource(resourceName, dllPath);
            }
            else if (!File.Exists(dllPath))
            {
                if (!firstTime)
                    throw new FileNotFoundException($"Library {dllName} could not be loaded after export.");

                Logger.LogWarning($"{dllName} not found. Attempting to export from embedded resource...");
                ExportResource(resourceName, dllPath);
            }

            if (!runtimeCheck())
            {
                if (!firstTime)
                    throw new FileNotFoundException($"Library {dllName} could not be loaded.");

                LoadLibrary(name, runtimeCheck, false);
            }
            else
            {
                Logger.LogInfo($"{dllName} is loaded and up to date.");
            }
        }

        public static void CheckDiscordLibrary() =>
            LoadLibrary(DISCORD_LIBRARY_NAME, () =>
            {
                string path = Path.Combine(
                    Path.GetDirectoryName(DiscordSocialSDKPlugin.Instance.Info.Location),
                    DISCORD_LIBRARY_NAME_DLL);
                return File.Exists(path);
            });

        public static void CheckSystemMemoryLibrary() =>
            LoadLibrary(SYSTEM_MEMORY_NAME, () =>
            {
                try { return Type.GetType("System.Span`1, System.Memory") != null; }
                catch { return false; }
            });

        public static void CheckUnsafeLibrary() =>
            LoadLibrary(SYSTEM_RUNTIME_COMPILER_SERVICES_UNSAFE_NAME, () =>
            {
                try { return Type.GetType("System.Runtime.CompilerServices.Unsafe, System.Runtime.CompilerServices.Unsafe") != null; }
                catch { return false; }
            });

        public static void CheckNumericsVectorsLibrary() =>
            LoadLibrary(SYSTEM_NUMERICS_VECTORS_NAME, () =>
            {
                try { return Type.GetType("System.Numerics.Vector3, System.Numerics.Vectors") != null; }
                catch { return false; }
            });
    }
}