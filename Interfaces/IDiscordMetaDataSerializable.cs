using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepInEx.DiscordSocialSDK.Interfaces
{
    public interface IDiscordMetaDataSerializable
    {
        public Dictionary<string, string> Serialize();

        public void Deserialize(Dictionary<string, string> data);
    }
}
