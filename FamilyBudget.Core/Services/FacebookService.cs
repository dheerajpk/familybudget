using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using FamilyBudget.Core.Facebook.Models;
using Newtonsoft.Json;

namespace FamilyBudget.Core.Services
{
    public class FacebookService
    {
        private const string AccessToken = "EAACHKSOAqz8BAO0ZACSlGa8ZAtsfm1rq5IDQZCTDIK0ZCePz4cVaEN8ZCaxbHKC9SZBKgZCgKIznwgjxWcqjqOswuKtWeDWp3pKclxOpq3PuYeBuglgUH22NuUfx2mC98jjjhSRHoQN3BL5EmHgMRljxqbgJDZCnJIgZD";

        //public async Task GetFBMessage()
        //{
        //    FacebookClient client = new FacebookClient(AccessToken);

        //    var result = await client.GetTaskAsync("me/feed").ConfigureAwait(false);

        //    if (result != null)
        //    {
        //        var dic = new Dictionary<string, string>();
        //        dic.Add("value", "SELF");
        //        await client.PostTaskAsync("me/feed", new { message = result.ToString(), privacy = dic });
        //    }

        //}

        public async Task<List<FeedMessage>> GetMessages()
        {
            FacebookClient client = new FacebookClient(AccessToken);

            var jsonValue = await client.GetTaskAsync("me/feed?limit=10000").ConfigureAwait(false);

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Feeds>(jsonValue.ToString());
            return result?.Messages;
        }

        public async Task<string> PostMessage(string message)
        {
            FacebookClient client = new FacebookClient(AccessToken);

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
            FacebookClient client = new FacebookClient(AccessToken);

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
            FacebookClient client = new FacebookClient(AccessToken);

            await client.DeleteTaskAsync(postId);

            return true;
        }
    }

    public interface IService<T>
    {
        Task<List<T>> GetMessages();

        Task<string> PostMessage(string message);

        Task<string> PostMessage<T1>(T1 message);

        Task<bool> UpdateMessage<T1>(T1 message, string postId);

        Task<bool> DeleteMessage(string postId);
    }
}
