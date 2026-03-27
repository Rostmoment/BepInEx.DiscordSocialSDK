using BepInEx.DiscordSocialSDK.Channels;
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

namespace BepInEx.DiscordSocialSDK.Guilds
{
    /// <summary>
    ///  Represents a channel in a guild that the current user is a member of and may be able to be
    ///  linked to a lobby.
    /// </summary>
    public class GuildChannel : IDisposable
    {
        internal NativeMethods.GuildChannel self;
        private int disposed_;

        internal GuildChannel(NativeMethods.GuildChannel self, int disposed)
        {
            this.self = self;
            this.disposed_ = disposed;
        }

        ~GuildChannel() { Dispose(); }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref disposed_, 1) != 0)
            {
                return;
            }
            GC.SuppressFinalize(this);
            unsafe
            {
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.Drop(self);
                }
            }
        }

        public GuildChannel(GuildChannel other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            if (other.disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(other));
            }
            unsafe
            {
                fixed (NativeMethods.GuildChannel* otherPtr = &other.self)
                {
                    fixed (NativeMethods.GuildChannel* selfPtr = &self)
                    {
                        NativeMethods.GuildChannel.Clone(selfPtr, otherPtr);
                    }
                }
            }
        }
        internal unsafe GuildChannel(NativeMethods.GuildChannel* otherPtr)
        {
            unsafe
            {
                fixed (NativeMethods.GuildChannel* selfPtr = &self)
                {
                    NativeMethods.GuildChannel.Clone(selfPtr, otherPtr);
                }
            }
        }
        public ulong Id()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                ulong __returnValue;
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    __returnValue = NativeMethods.GuildChannel.Id(self);
                }
                return __returnValue;
            }
        }
        public void SetId(ulong value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.SetId(self, value);
                }
            }
        }
        public string Name()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                var __returnValue = new NativeMethods.Discord_String();
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.Name(self, &__returnValue);
                }
                string __returnValueSurface =
                  MarshalExtensions.PtrToStringUTF8((IntPtr)__returnValue.ptr, (int)__returnValue.size);
                NativeMethods.Discord_Free((void*)__returnValue.ptr);
                return __returnValueSurface;
            }
        }
        public void SetName(string value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                var __scratchAligned = stackalloc ulong[128];
                var __scratch = (byte*)__scratchAligned;
                var __scratchUsed = 0;
                NativeMethods.Discord_String __valueSpan;
                var __valueOwned =
                  NativeMethods.__InitStringLocal(__scratch, &__scratchUsed, 1024, &__valueSpan, value);
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.SetName(self, __valueSpan);
                }
                NativeMethods.__FreeLocalString(&__valueSpan, __valueOwned);
            }
        }
        public ChannelType Type()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                ChannelType __returnValue;
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    __returnValue = NativeMethods.GuildChannel.Type(self);
                }
                return __returnValue;
            }
        }
        public void SetType(ChannelType value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.SetType(self, value);
                }
            }
        }
        public int Position()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                int __returnValue;
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    __returnValue = NativeMethods.GuildChannel.Position(self);
                }
                return __returnValue;
            }
        }
        public void SetPosition(int value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.SetPosition(self, value);
                }
            }
        }
        public ulong? ParentId()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                bool __returnIsNonNull;
                ulong __returnValue;
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    __returnIsNonNull = NativeMethods.GuildChannel.ParentId(self, &__returnValue);
                }
                if (!__returnIsNonNull)
                {
                    return null;
                }
                return __returnValue;
            }
        }
        public void SetParentId(ulong? value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                var __valueLocal = value ?? default;
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.SetParentId(self,
                                                           (value != null ? &__valueLocal : null));
                }
            }
        }
        public bool IsLinkable()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                bool __returnValue;
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    __returnValue = NativeMethods.GuildChannel.IsLinkable(self);
                }
                return __returnValue;
            }
        }
        public void SetIsLinkable(bool value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.SetIsLinkable(self, value);
                }
            }
        }
        public bool IsViewableAndWriteableByAllMembers()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                bool __returnValue;
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    __returnValue = NativeMethods.GuildChannel.IsViewableAndWriteableByAllMembers(self);
                }
                return __returnValue;
            }
        }
        public void SetIsViewableAndWriteableByAllMembers(bool value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.SetIsViewableAndWriteableByAllMembers(self, value);
                }
            }
        }
        public LinkedLobby? LinkedLobby()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                bool __returnIsNonNull;
                var __returnValueNative = new NativeMethods.LinkedLobby();
                LinkedLobby? __returnValue = null;
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    __returnIsNonNull =
                      NativeMethods.GuildChannel.LinkedLobby(self, &__returnValueNative);
                }
                if (!__returnIsNonNull)
                {
                    return null;
                }
                __returnValue = new LinkedLobby(__returnValueNative, 0);
                return __returnValue;
            }
        }
        public void SetLinkedLobby(LinkedLobby? value)
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(GuildChannel));
            }
            unsafe
            {
                var __valueLocal = value?.self ?? default;
                fixed (NativeMethods.GuildChannel* self = &this.self)
                {
                    NativeMethods.GuildChannel.SetLinkedLobby(self,
                                                              (value != null ? &__valueLocal : null));
                }
                if (value != null)
                {
                    value.self = __valueLocal;
                }
            }
        }
    }
}
