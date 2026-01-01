using BepInEx.DiscordSocialSDK.Enums;
using BepInEx.DiscordSocialSDK.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.RPC
{
    // I don't know what discord means by this summary comment, but here it is
    /// <summary>
    /// \see Activity
    /// </summary>
    public class ActivityParty : IDisposable
    {
        internal NativeMethods.ActivityParty self;
        private int disposed_;

        internal ActivityParty(NativeMethods.ActivityParty self, int disposed)
        {
            this.self = self;
            disposed_ = disposed;
        }

        ~ActivityParty() { Dispose(); }

        public ActivityParty()
        {
            NativeMethods.__Init();
            unsafe
            {
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    NativeMethods.ActivityParty.Init(self);
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
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    NativeMethods.ActivityParty.Drop(self);
                }
            }
        }

        public ActivityParty(ActivityParty other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityParty));
            }
            if (other.disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(other));
            }
            unsafe
            {
                fixed (NativeMethods.ActivityParty* otherPtr = &other.self)
                {
                    fixed (NativeMethods.ActivityParty* selfPtr = &self)
                    {
                        NativeMethods.ActivityParty.Clone(selfPtr, otherPtr);
                    }
                }
            }
        }
        internal unsafe ActivityParty(NativeMethods.ActivityParty* otherPtr)
        {
            unsafe
            {
                fixed (NativeMethods.ActivityParty* selfPtr = &self)
                {
                    NativeMethods.ActivityParty.Clone(selfPtr, otherPtr);
                }
            }
        }
        public string Id()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityParty));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    NativeMethods.ActivityParty.Id(self, &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free(__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        public void SetId(string value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityParty));
            }
            unsafe
            {
                var __scratchAligned = stackalloc ulong[128];
                var __scratch = (byte*)__scratchAligned;
                var __scratchUsed = 0;
                NativeMethods.Discord_String __valueSpan;
                var __valueOwned =
                  NativeMethods.__InitStringLocal(__scratch, &__scratchUsed, 1024, &__valueSpan, value);
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    NativeMethods.ActivityParty.SetId(self, __valueSpan);
                }
                NativeMethods.__FreeLocalString(&__valueSpan, __valueOwned);
            }
        }
        public int CurrentSize()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityParty));
            }
            unsafe
            {
                int __returnValue;
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    __returnValue = NativeMethods.ActivityParty.CurrentSize(self);
                }
                return __returnValue;
            }
        }
        public void SetCurrentSize(int value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityParty));
            }
            unsafe
            {
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    NativeMethods.ActivityParty.SetCurrentSize(self, value);
                }
            }
        }
        public int MaxSize()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityParty));
            }
            unsafe
            {
                int __returnValue;
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    __returnValue = NativeMethods.ActivityParty.MaxSize(self);
                }
                return __returnValue;
            }
        }
        public void SetMaxSize(int value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityParty));
            }
            unsafe
            {
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    NativeMethods.ActivityParty.SetMaxSize(self, value);
                }
            }
        }
        public ActivityPartyPrivacy Privacy()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityParty));
            }
            unsafe
            {
                ActivityPartyPrivacy __returnValue;
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    __returnValue = NativeMethods.ActivityParty.Privacy(self);
                }
                return __returnValue;
            }
        }
        public void SetPrivacy(ActivityPartyPrivacy value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityParty));
            }
            unsafe
            {
                fixed (NativeMethods.ActivityParty* self = &this.self)
                {
                    NativeMethods.ActivityParty.SetPrivacy(self, value);
                }
            }
        }
    }
}
