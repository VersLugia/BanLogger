using System.Collections.Generic;
using System.Linq;
using BanLogger.Structs;
using Exiled.API.Features;
using MEC;
using UnityEngine.Networking;
using Utf8Json;

namespace BanLogger.Utils
{
    public class DiscordHandler
    {
        public static Dictionary<string, List<Embed>> Queue = new Dictionary<string, List<Embed>>();
        public static CoroutineHandle CoroutineHandle;

        private static IEnumerator<float> SendMessage(Message message, string url)
        {   
            UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(JsonSerializer.Serialize(message));
            uploadHandler.contentType = "application/json";
            webRequest.uploadHandler = uploadHandler;

            yield return Timing.WaitUntilDone(webRequest.SendWebRequest());

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Log.Error($"Error sending the message: {webRequest.responseCode} - {webRequest.error}");
            }
        }
        
        public static IEnumerator<float> HandleQueue()
        {
            for (;;)
            {
                foreach (string webhook in Queue.Keys.ToList())
                {
                    if (Queue[webhook].Count == 0)
                        Queue.Remove(webhook);
                    
                    Message message = new Message();
                    foreach (Embed embed in Queue[webhook].ToList())
                    {
                        if (message.embeds.Count < 10)
                        {   
                            message.embeds.Add(embed);
                            Queue[webhook].Remove(embed);
                        }
                    }
                    yield return Timing.WaitUntilDone(SendMessage(message, webhook));
                    yield return Timing.WaitForSeconds(5);
                }
                
                if (Queue.Keys.Count == 0)
                    yield break;
            }
        }
    }
}