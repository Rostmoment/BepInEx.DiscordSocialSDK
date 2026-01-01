using BepInEx.DiscordSocialSDK.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.RPC
{
    /// <summary>
    /// \see Activity
    /// </summary>
    public class ActivityButton : IDisposable
    {

        internal NativeMethods.ActivityButton self;
        private int disposed_;

        internal ActivityButton(NativeMethods.ActivityButton self, int disposed)
        {
            this.self = self;
            disposed_ = disposed;
        }

        ~ActivityButton() { Dispose(); }

        public ActivityButton()
        {
            NativeMethods.__Init();
            unsafe
            {
                fixed (NativeMethods.ActivityButton* self = &this.self)
                {
                    NativeMethods.ActivityButton.Init(self);
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
                fixed (NativeMethods.ActivityButton* self = &this.self)
                {
                    NativeMethods.ActivityButton.Drop(self);
                }
            }
        }

        public ActivityButton(ActivityButton other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityButton));
            }
            if (other.disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(other));
            }
            unsafe
            {
                fixed (NativeMethods.ActivityButton* otherPtr = &other.self)
                {
                    fixed (NativeMethods.ActivityButton* selfPtr = &self)
                    {
                        NativeMethods.ActivityButton.Clone(selfPtr, otherPtr);
                    }
                }
            }
        }
        internal unsafe ActivityButton(NativeMethods.ActivityButton* otherPtr)
        {
            unsafe
            {
                fixed (NativeMethods.ActivityButton* selfPtr = &self)
                {
                    NativeMethods.ActivityButton.Clone(selfPtr, otherPtr);
                }
            }
        }
        public string Label()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityButton));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.ActivityButton* self = &this.self)
                {
                    NativeMethods.ActivityButton.Label(self, &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free(__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        public void SetLabel(string value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityButton));
            }
            unsafe
            {
                var __scratchAligned = stackalloc ulong[128];
                var __scratch = (byte*)__scratchAligned;
                var __scratchUsed = 0;
                NativeMethods.Discord_String __valueSpan;
                var __valueOwned =
                  NativeMethods.__InitStringLocal(__scratch, &__scratchUsed, 1024, &__valueSpan, value);
                fixed (NativeMethods.ActivityButton* self = &this.self)
                {
                    NativeMethods.ActivityButton.SetLabel(self, __valueSpan);
                }
                NativeMethods.__FreeLocalString(&__valueSpan, __valueOwned);
            }
        }
        public string Url()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityButton));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.ActivityButton* self = &this.self)
                {
                    NativeMethods.ActivityButton.Url(self, &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free(__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        public void SetUrl(string value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ActivityButton));
            }
            unsafe
            {
                var __scratchAligned = stackalloc ulong[128];
                var __scratch = (byte*)__scratchAligned;
                var __scratchUsed = 0;
                NativeMethods.Discord_String __valueSpan;
                var __valueOwned =
                  NativeMethods.__InitStringLocal(__scratch, &__scratchUsed, 1024, &__valueSpan, value);
                fixed (NativeMethods.ActivityButton* self = &this.self)
                {
                    NativeMethods.ActivityButton.SetUrl(self, __valueSpan);
                }
                NativeMethods.__FreeLocalString(&__valueSpan, __valueOwned);
            }
        }
    }
}
