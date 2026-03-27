# BepInEx.DiscordSocialSDK

A wrapper around the official [Discord Social SDK](https://discord.com/developers/social-sdk) designed for easy use with [BepInEx 5](https://github.com/BepInEx/BepInEx).

This library provides a convenient interface for integrating Discord Social features into BepInEx plugins while keeping access to the full functionality of the original SDK.

---

# Features

- Full access to all methods from the **Discord Social SDK**
- Additional wrappers for easier and cleaner usage
- Designed specifically for **BepInEx 5 plugins**
- XML documentation included for IDE tooltips
- Extra explanations available in the project Wiki

---

# Installation

1. Download the latest release from the  
   **[Releases Page](https://github.com/Rostmoment/BepInEx.DiscordSocialSDK/releases)**

2. Install **[BepInEx 5](https://github.com/BepInEx/BepInEx)** if you don't have it already.

3. Extract the archive and move the **BepInEx** folder into your game's root directory.


---

# Usage

1. Add **BepInEx.DiscordSocialSDK** to your project dependencies.
2. Import the namespace in your plugin.
3. Use the provided wrappers or call original SDK methods directly.

All wrapper methods are documented in:

- the included **`.xml` documentation**
- the project **[Wiki](https://github.com/Rostmoment/BepInEx.DiscordSocialSDK/wiki) (WIP)**

For full SDK behavior and additional reference, see the official documentation:

- **[Discord Social SDK Documentation](https://discord.com/developers/docs/social-sdk/index.html)**

---

# Minimal Example

A minimal example that shows:

- initializing the client
- subscribing to incoming messages
- sending a message to a user

```csharp
using BepInEx;
using BepInEx.DiscordSocialSDK.Client;
using BepInEx.DiscordSocialSDK.Handles;
using UnityEngine;

[BepInPlugin("com.example.discordexample", "Discord Example", "1.0.0")]
public class DiscordExample : BaseUnityPlugin
{
    private ClientWrapper client;

    void Start()
    {
        // Replace with your Discord Application ID
        client = new ClientWrapper(YOUR_APPLICATION_ID);

        // Subscribe to messages
        client.onMessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(ulong messageId)
    {
        MessageHandle message = client.GetMessage(messageId);

        Logger.LogInfo($"Message received: {message.Content()} from {message.AuthorId()}");
    }


    public void SendMessage(ulong userId, string text)
    {
        client.SendMessageToUser(userId, text, null); 
    }
}