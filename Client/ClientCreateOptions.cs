using BepInEx.DiscordSocialSDK.Enums;
using BepInEx.DiscordSocialSDK.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms;

namespace BepInEx.DiscordSocialSDK.Client
{
    /// <summary>
    ///  Options for creating a new Client instance.
    /// </summary>
    /// <remarks>
    ///  This class may be used to set advanced initialization-time options on Client.
    ///
    /// </remarks>
    public class ClientCreateOptions : IDisposable
    {
        internal NativeMethods.ClientCreateOptions self;
        private int disposed_;

        internal ClientCreateOptions(NativeMethods.ClientCreateOptions self, int disposed)
        {
            this.self = self;
            this.disposed_ = disposed;
        }

        ~ClientCreateOptions() { Dispose(); }

        public ClientCreateOptions()
        {
            NativeMethods.__Init();
            unsafe
            {
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    NativeMethods.ClientCreateOptions.Init(self);
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
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    NativeMethods.ClientCreateOptions.Drop(self);
                }
            }
        }

        public ClientCreateOptions(ClientCreateOptions other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            if (other.disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(other));
            }
            unsafe
            {
                fixed (NativeMethods.ClientCreateOptions* otherPtr = &other.self)
                {
                    fixed (NativeMethods.ClientCreateOptions* selfPtr = &self)
                    {
                        NativeMethods.ClientCreateOptions.Clone(selfPtr, otherPtr);
                    }
                }
            }
        }
        internal unsafe ClientCreateOptions(NativeMethods.ClientCreateOptions* otherPtr)
        {
            unsafe
            {
                fixed (NativeMethods.ClientCreateOptions* selfPtr = &self)
                {
                    NativeMethods.ClientCreateOptions.Clone(selfPtr, otherPtr);
                }
            }
        }
        public string WebBase()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    NativeMethods.ClientCreateOptions.WebBase(self, &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free((void*)__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        public void SetWebBase(string value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                var __scratchAligned = stackalloc ulong[128];
                var __scratch = (byte*)__scratchAligned;
                var __scratchUsed = 0;
                NativeMethods.Discord_String __valueSpan;
                var __valueOwned =
                  NativeMethods.__InitStringLocal(__scratch, &__scratchUsed, 1024, &__valueSpan, value);
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    NativeMethods.ClientCreateOptions.SetWebBase(self, __valueSpan);
                }
                NativeMethods.__FreeLocalString(&__valueSpan, __valueOwned);
            }
        }
        public string ApiBase()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    NativeMethods.ClientCreateOptions.ApiBase(self, &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free((void*)__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        public void SetApiBase(string value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                var __scratchAligned = stackalloc ulong[128];
                var __scratch = (byte*)__scratchAligned;
                var __scratchUsed = 0;
                NativeMethods.Discord_String __valueSpan;
                var __valueOwned =
                  NativeMethods.__InitStringLocal(__scratch, &__scratchUsed, 1024, &__valueSpan, value);
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    NativeMethods.ClientCreateOptions.SetApiBase(self, __valueSpan);
                }
                NativeMethods.__FreeLocalString(&__valueSpan, __valueOwned);
            }
        }
        public AudioSystem ExperimentalAudioSystem()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                AudioSystem __returnValue;
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    __returnValue = NativeMethods.ClientCreateOptions.ExperimentalAudioSystem(self);
                }
                return __returnValue;
            }
        }
        public void SetExperimentalAudioSystem(AudioSystem value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    NativeMethods.ClientCreateOptions.SetExperimentalAudioSystem(self, value);
                }
            }
        }
        public bool ExperimentalAndroidPreventCommsForBluetooth()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                bool __returnValue;
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    __returnValue =
                      NativeMethods.ClientCreateOptions.ExperimentalAndroidPreventCommsForBluetooth(
                        self);
                }
                return __returnValue;
            }
        }
        public void SetExperimentalAndroidPreventCommsForBluetooth(bool value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    NativeMethods.ClientCreateOptions.SetExperimentalAndroidPreventCommsForBluetooth(
                      self, value);
                }
            }
        }
        public ulong? CpuAffinityMask()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                bool __returnIsNonNull;
                ulong __returnValue;
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    __returnIsNonNull =
                      NativeMethods.ClientCreateOptions.CpuAffinityMask(self, &__returnValue);
                }
                if (!__returnIsNonNull)
                {
                    return null;
                }
                return __returnValue;
            }
        }
        public void SetCpuAffinityMask(ulong? value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(ClientCreateOptions));
            }
            unsafe
            {
                var __valueLocal = value ?? default;
                fixed (NativeMethods.ClientCreateOptions* self = &this.self)
                {
                    NativeMethods.ClientCreateOptions.SetCpuAffinityMask(
                      self, (value != null ? &__valueLocal : null));
                }
            }
        }
    }
}
