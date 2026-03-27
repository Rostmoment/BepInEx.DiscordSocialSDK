using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.HelpTypes
{
    /// <summary>
    /// Wraps an array of IDisposable objects and disposes all items when disposed.
    /// </summary>
    internal struct DisposableArray<T> : IDisposable, IEnumerable<T> where T : IDisposable
    {
        /// <summary>
        /// Creates a wrapper around the provided array.
        /// </summary>
        public DisposableArray(T[] array) { this.underlying_ = array; }

        /// <summary>
        /// Disposes every item in the array.
        /// </summary>
        public void Dispose()
        {
            foreach (T item in underlying_)
                item.Dispose();
        }

        /// <summary>
        /// Returns an enumerator for iterating through the array.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T item in underlying_)
                yield return item;
        }

        /// <summary>
        /// Non-generic enumerator implementation
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 

        /// <summary>
        /// Returns the underlying array
        /// </summary>
        public T[] ToArray() => underlying_; 

        private readonly T[] underlying_;
    }

    /// <summary>
    /// Extension helpers for creating DisposableArray instances.
    /// </summary>
    internal static class DisposableArray
    {
        /// <summary>
        /// Converts an array of IDisposable objects into a DisposableArray wrapper
        /// </summary>
        public static DisposableArray<T> ToDisposable<T>(this T[] array) where T : IDisposable =>
            new DisposableArray<T>(array);
    }
}