using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Extensions
{
    class EncodingExtensions
    {
        // Thanks to https://github.com/dartasen/Discord.SocialSdk-CSharp for this method
        public static unsafe int GetBytesUtf8(ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            fixed (char* charsPtr = &MemoryMarshal.GetReference(chars))
            fixed (byte* bytesPtr = &MemoryMarshal.GetReference(bytes))
            {
                return Encoding.UTF8.GetBytes(charsPtr, chars.Length, bytesPtr, bytes.Length);
            }
        }
    }
}
