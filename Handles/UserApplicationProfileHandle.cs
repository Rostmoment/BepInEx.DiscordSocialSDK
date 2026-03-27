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

namespace BepInEx.DiscordSocialSDK.Handles
{
    /// <summary>
    ///  A UserApplicationProfileHandle represents a profile from an external identity provider, such as
    ///  Steam or Epic Online Services.
    /// </summary>
    /// <remarks>
    ///  Handle objects in the SDK hold a reference both to the underlying data, and to the SDK
    ///  instance. Changes to the underlying data will generally be available on existing handles
    ///  objects without having to re-create them. If the SDK instance is destroyed, but you still have
    ///  a reference to a handle object, note that it will return the default value for all method calls
    ///  (ie an empty string for methods that return a string).
    ///
    /// </remarks>
    public class UserApplicationProfileHandle : IDisposable
    {
        internal NativeMethods.UserApplicationProfileHandle self;
        private int disposed_;

        internal UserApplicationProfileHandle(NativeMethods.UserApplicationProfileHandle self,
                                              int disposed)
        {
            this.self = self;
            this.disposed_ = disposed;
        }

        ~UserApplicationProfileHandle() { Dispose(); }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref disposed_, 1) != 0)
            {
                return;
            }
            GC.SuppressFinalize(this);
            unsafe
            {
                fixed (NativeMethods.UserApplicationProfileHandle* self = &this.self)
                {
                    NativeMethods.UserApplicationProfileHandle.Drop(self);
                }
            }
        }

        public UserApplicationProfileHandle(UserApplicationProfileHandle other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserApplicationProfileHandle));
            }
            if (other.disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(other));
            }
            unsafe
            {
                fixed (NativeMethods.UserApplicationProfileHandle* otherPtr = &other.self)
                {
                    fixed (NativeMethods.UserApplicationProfileHandle* selfPtr = &self)
                    {
                        NativeMethods.UserApplicationProfileHandle.Clone(selfPtr, otherPtr);
                    }
                }
            }
        }
        internal unsafe
        UserApplicationProfileHandle(NativeMethods.UserApplicationProfileHandle* otherPtr)
        {
            unsafe
            {
                fixed (NativeMethods.UserApplicationProfileHandle* selfPtr = &self)
                {
                    NativeMethods.UserApplicationProfileHandle.Clone(selfPtr, otherPtr);
                }
            }
        }
        /// <summary>
        ///  Returns the user's in-game avatar hash.
        /// </summary>
        public string AvatarHash()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserApplicationProfileHandle));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.UserApplicationProfileHandle* self = &this.self)
                {
                    NativeMethods.UserApplicationProfileHandle.AvatarHash(self, &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free((void*)__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        /// <summary>
        ///  Returns any metadata set by the developer.
        /// </summary>
        public string Metadata()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserApplicationProfileHandle));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.UserApplicationProfileHandle* self = &this.self)
                {
                    NativeMethods.UserApplicationProfileHandle.Metadata(self, &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free((void*)__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        /// <summary>
        ///  Returns the user's external identity provider ID if it exists.
        /// </summary>
        public string? ProviderId()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserApplicationProfileHandle));
            }
            unsafe
            {
                bool __returnIsNonNull;
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.UserApplicationProfileHandle* self = &this.self)
                {
                    __returnIsNonNull =
                      NativeMethods.UserApplicationProfileHandle.ProviderId(self, &__returnValue);
                }
                if (!__returnIsNonNull)
                {
                    return null;
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free((void*)__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        /// <summary>
        ///  Returns the user's external identity provider issued user ID.
        /// </summary>
        public string ProviderIssuedUserId()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserApplicationProfileHandle));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.UserApplicationProfileHandle* self = &this.self)
                {
                    NativeMethods.UserApplicationProfileHandle.ProviderIssuedUserId(self,
                                                                                    &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free((void*)__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        /// <summary>
        ///  Returns the type of the external identity provider.
        /// </summary>
        public ExternalIdentityProviderType ProviderType()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserApplicationProfileHandle));
            }
            unsafe
            {
                ExternalIdentityProviderType __returnValue;
                fixed (NativeMethods.UserApplicationProfileHandle* self = &this.self)
                {
                    __returnValue = NativeMethods.UserApplicationProfileHandle.ProviderType(self);
                }
                return __returnValue;
            }
        }
        /// <summary>
        ///  Returns the user's in-game username.
        /// </summary>
        public string Username()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserApplicationProfileHandle));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.UserApplicationProfileHandle* self = &this.self)
                {
                    NativeMethods.UserApplicationProfileHandle.Username(self, &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free((void*)__returnValue.ptr);
                return __returnValueSurface;
            }
        }
    }
}
