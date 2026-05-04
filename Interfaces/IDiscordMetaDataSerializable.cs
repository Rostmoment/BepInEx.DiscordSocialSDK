using BepInEx.DiscordSocialSDK.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Interfaces
{
    /// <summary>
    /// Represents an object that can be serialized to and deserialized from
    /// Discord message metadata (a flat string key-value dictionary).
    /// <para>
    /// Implement this interface to attach structured data to Discord messages
    /// Keys must be unique across all metadata objects passed in a single call
    /// </para>
    /// </summary>
    public interface IDiscordMetaDataSerializable
    {
        /// <summary>
        /// Serializes the object into a flat string dictionary
        /// to be attached to a Discord message as metadata.
        /// </summary>
        /// <returns>A dictionary of key-value pairs representing the object's state.</returns>
        public Dictionary<string, string> Serialize();

        /// <summary>
        /// Restores the object's state from a metadata dictionary
        /// received from an incoming Discord message.
        /// </summary>
        /// <param name="data">The metadata dictionary from the message.</param>
        public void Deserialize(Dictionary<string, string> data);
    }
}
