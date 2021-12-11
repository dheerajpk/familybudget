using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using Newtonsoft.Json;
using SocialDB.Services.Facebook.Models;

namespace SocialDB.Services.Facebook
{
    internal class FacebookService
    {
        //private const string AccessToken = "EAACHKSOAqz8BAO0ZACSlGa8ZAtsfm1rq5IDQZCTDIK0ZCePz4cVaEN8ZCaxbHKC9SZBKgZCgKIznwgjxWcqjqOswuKtWeDWp3pKclxOpq3PuYeBuglgUH22NuUfx2mC98jjjhSRHoQN3BL5EmHgMRljxqbgJDZCnJIgZD";

        private readonly string _accessToken;

        public FacebookService(string accessToken)
        {
            _accessToken = accessToken;
        }

        public async Task<List<FeedMessage>> GetMessages()
        {
            FacebookClient client = new FacebookClient(_accessToken);

            var jsonValue = await client.GetTaskAsync("me/feed?limit=10000").ConfigureAwait(false);

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Feeds>(jsonValue.ToString());
            return result?.Messages;
        }

        public async Task<string> PostMessage(string message)
        {
            FacebookClient client = new FacebookClient(_accessToken);

            var dic = new Dictionary<string, string>
            {
                { "value", "SELF" }
            };

            var result = await client.PostTaskAsync("me/feed", new { message = message, privacy = dic });

            FeedMessage postMessage = null;

            if (result != null)
            {
                postMessage = JsonConvert.DeserializeObject<FeedMessage>(result.ToString());
            }

            return postMessage?.Id;
        }

        public async Task<string> PostMessage<T>(T message) where T : class
        {
            var jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message, Formatting.None);

            return await PostMessage(jsonMessage);
        }

        public async Task<bool> UpdateMessage<T>(T message, string postId) where T : class
        {
            FacebookClient client = new FacebookClient(_accessToken);

            var dic = new Dictionary<string, string>
            {
                { "value", "SELF" }
            };

            var jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message, Formatting.None);

            var result = await client.PostTaskAsync($"{postId}", new { message = jsonMessage, privacy = dic });

            var success = result?.ToString().Contains("true");

            return success.HasValue && success.Value;
        }

        public async Task<bool> DeleteMessage(string postId)
        {
            FacebookClient client = new FacebookClient(_accessToken);

            await client.DeleteTaskAsync(postId);

            return true;
        }
    }
}
