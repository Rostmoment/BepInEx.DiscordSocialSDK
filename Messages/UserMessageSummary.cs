using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Messages
{
    /// <summary>
    ///  Represents a summary of a DM conversation with a user.
    /// </summary>
    public class UserMessageSummary : IDisposable
    {
        internal NativeMethods.UserMessageSummary self;
        private int disposed_;

        internal UserMessageSummary(NativeMethods.UserMessageSummary self, int disposed)
        {
            this.self = self;
            this.disposed_ = disposed;
        }

        ~UserMessageSummary() { Dispose(); }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref disposed_, 1) != 0)
            {
                return;
            }
            GC.SuppressFinalize(this);
            unsafe
            {
                fixed (NativeMethods.UserMessageSummary* self = &this.self)
                {
                    NativeMethods.UserMessageSummary.Drop(self);
                }
            }
        }

        public UserMessageSummary(UserMessageSummary other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserMessageSummary));
            }
            if (other.disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(other));
            }
            unsafe
            {
                fixed (NativeMethods.UserMessageSummary* otherPtr = &other.self)
                {
                    fixed (NativeMethods.UserMessageSummary* selfPtr = &self)
                    {
                        NativeMethods.UserMessageSummary.Clone(selfPtr, otherPtr);
                    }
                }
            }
        }
        internal unsafe UserMessageSummary(NativeMethods.UserMessageSummary* otherPtr)
        {
            unsafe
            {
                fixed (NativeMethods.UserMessageSummary* selfPtr = &self)
                {
                    NativeMethods.UserMessageSummary.Clone(selfPtr, otherPtr);
                }
            }
        }
        /// <summary>
        ///  Returns the ID of the last message sent in the DM conversation.
        /// </summary>
        public ulong LastMessageId()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserMessageSummary));
            }
            unsafe
            {
                ulong __returnValue;
                fixed (NativeMethods.UserMessageSummary* self = &this.self)
                {
                    __returnValue = NativeMethods.UserMessageSummary.LastMessageId(self);
                }
                return __returnValue;
            }
        }
        /// <summary>
        ///  Returns the ID of the other user in the DM conversation.
        /// </summary>
        public ulong UserId()
        {
            if (disposed_ != 0)
            {
                throw new ObjectDisposedException(nameof(UserMessageSummary));
            }
            unsafe
            {
                ulong __returnValue;
                fixed (NativeMethods.UserMessageSummary* self = &this.self)
                {
                    __returnValue = NativeMethods.UserMessageSummary.UserId(self);
                }
                return __returnValue;
            }
        }
    }
}
