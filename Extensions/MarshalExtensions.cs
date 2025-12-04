using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Extensions
{
    class MarshalExtensions
    {
        // Netstandard2.0 does not have Marshal.StringToCoTaskMemUTF8 or Marshal.PtrToStringUTF8
        // Thanks to https://github.com/dartasen/Discord.SocialSdk-CSharp for these methods
        public static unsafe IntPtr StringToCoTaskMemUTF8(string value)
        {
            if (value == null)
                return IntPtr.Zero;
            

            byte[] utf8Bytes = Encoding.UTF8.GetBytes(value);
            IntPtr ptr = Marshal.AllocCoTaskMem(utf8Bytes.Length + 1);
            Marshal.Copy(utf8Bytes, 0, ptr, utf8Bytes.Length);
            Marshal.WriteByte(ptr, utf8Bytes.Length, 0);

            return ptr;
        }
        public static unsafe string PtrToStringUTF8(IntPtr ptr, int byteLen)
        {
            if (IsNullOrWin32Atom(ptr))
                throw new ArgumentNullException(nameof(ptr));

            if (byteLen < 0)
                throw new ArgumentOutOfRangeException(nameof(byteLen), "byteLen must be non-negative");
            

            return CreateStringFromEncoding((byte*)ptr, byteLen, Encoding.UTF8);
        }

        private static bool IsNullOrWin32Atom(IntPtr ptr)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                const long HIWORDMASK = unchecked((long)0xffffffffffff0000L);
                long lPtr = (long)ptr;

                return 0 == (lPtr & HIWORDMASK);
            }

            return ptr == IntPtr.Zero;
        }
        private static unsafe string CreateStringFromEncoding(byte* bytes, int byteLength, Encoding encoding)
        {

            int stringLength = encoding.GetCharCount(bytes, byteLength);

            // They gave us an empty string if they needed one
            // 0 bytelength might be possible if there's something in an encoder
            if (stringLength == 0)
                return string.Empty;
            

            string s = new string(' ', stringLength);
            fixed (char* pTempChars = s)
            {
                int doubleCheck = encoding.GetChars(bytes, byteLength, pTempChars, stringLength);
            }

            return s;
        }
    }
}
