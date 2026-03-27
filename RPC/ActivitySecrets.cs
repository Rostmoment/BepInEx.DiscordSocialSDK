using BepInEx.DiscordSocialSDK.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.RPC
{
    /// <summary>
    /// Represents the secret codes associated with a Discord Rich Presence activity.
    /// The primary secret is the "Join" code, shared with users accepted into the party,
    /// allowing them to join the activity session. The Join secret must be a string
    /// between 2 and 128 characters, e.g., a Discord lobby ID or internal game server ID.
    /// </summary>
    public class ActivitySecrets : IDisposable
    {
        internal NativeMethods.ActivitySecrets self;
        private int disposed_;

        internal ActivitySecrets(NativeMethods.ActivitySecrets self, int disposed)
        {
            this.self = self;
            this.disposed_ = disposed;
        }

        ~ActivitySecrets() { Dispose(); }

        public ActivitySecrets()
        {
            NativeMethods.__Init();
            unsafe
            {
                fixed (NativeMethods.ActivitySecrets* self = &this.self)
                {
                    NativeMethods.ActivitySecrets.Init(self);
                }
            }
            NativeMethods.__OnPostConstruct(this);
        }
        public void Dispose()
        {
            if (Interlocked.Exchange(ref disposed_, 1) != 0)
            {
                return;
            }
            GC.SuppressFinalize(this);
            unsafe
            {
                fixed (NativeMethods.ActivitySecrets* self = &this.self)
                {
                    NativeMethods.ActivitySecrets.Drop(self);
                }
            }
        }

        public ActivitySecrets(ActivitySecrets other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivitySecrets));
            }
            if (other.disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(other));
            }
            unsafe
            {
                fixed (NativeMethods.ActivitySecrets* otherPtr = &other.self)
                {
                    fixed (NativeMethods.ActivitySecrets* selfPtr = &self)
                    {
                        NativeMethods.ActivitySecrets.Clone(selfPtr, otherPtr);
                    }
                }
            }
        }
        internal unsafe ActivitySecrets(NativeMethods.ActivitySecrets* otherPtr)
        {
            unsafe
            {
                fixed (NativeMethods.ActivitySecrets* selfPtr = &self)
                {
                    NativeMethods.ActivitySecrets.Clone(selfPtr, otherPtr);
                }
            }
        }
        public string Join()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivitySecrets));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.ActivitySecrets* self = &this.self)
                {
                    NativeMethods.ActivitySecrets.Join(self, &__returnValue);
                }
                string __returnValueSurface = MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free((void*)__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        public void SetJoin(string value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivitySecrets));
            }
            unsafe
            {
                var __scratchAligned = stackalloc ulong[128];
                var __scratch = (byte*)__scratchAligned;
                var __scratchUsed = 0;
                NativeMethods.Discord_String __valueSpan;
                var __valueOwned =
                  NativeMethods.__InitStringLocal(__scratch, &__scratchUsed, 1024, &__valueSpan, value);
                fixed (NativeMethods.ActivitySecrets* self = &this.self)
                {
                    NativeMethods.ActivitySecrets.SetJoin(self, __valueSpan);
                }
                NativeMethods.__FreeLocalString(&__valueSpan, __valueOwned);
            }
        }
    }
}
