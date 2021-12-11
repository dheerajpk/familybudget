using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using SocialDB.Http;
using SocialDB.Services.Slack.Models;

namespace SocialDB.Services.Slack
{
    internal class SlackService
    {
        private readonly string _defaultChannel = "DataRepo";

        private readonly string _token;

        private readonly HttpClientService _httpClientService = new HttpClientService();

        private Channel _familyChannel;

        public SlackService(string token)
        {
            _token = token;
        }

        public Channel FamChannel => _familyChannel;

        private async Task<string> CreateChannel(string channelName)
        {
            
            string requestUri = $"https://slack.com/api/channels.create?token={_token}&name={channelName}&pretty=1";

            string result = await _httpClientService.GetAysnc(requestUri);

            if (result != null)
            {
                var channel = JsonConvert.DeserializeObject<ChannelResponse>(result);

                return channel?.Channel.Id;
            }

            return string.Empty;
        }

        private async Task<List<Message>> GetChannelMessage(string channelId)
        {
           
            string requestUri = $"https://slack.com/api/channels.history?token={_token}&channel={channelId}&count=10000&pretty=1";

            string result = await _httpClientService.GetAysnc(requestUri);

            if (result != null)
            {
                var channels = JsonConvert.DeserializeObject<MessageList>(result);

                return channels?.Messages;
            }

            return null;
        }

        private async Task<List<Channel>> GetAllChannels()
        {

            string requestUri = $"https://slack.com/api/channels.list?token={_token}&pretty=1";

            string result = await _httpClientService.GetAysnc(requestUri).ConfigureAwait(false);

            if (result != null)
            {
                var channels = JsonConvert.DeserializeObject<ChannelList>(result);

                return channels?.Channels;
            }

            return null;
        }

        private async Task DeleteMessage(string messageId, string channelId)
        {

            string requestUri = $"https://slack.com/api/chat.delete?token={_token}&channel={channelId}&ts={messageId}&pretty=1";

            await _httpClientService.GetAysnc(requestUri);
        }

        private async Task UpdateMessage(string message, string messageId, string channelId)
        {

            string requestUri = $"https://slack.com/api/chat.update?token={_token}&channel={channelId}&text={message}&ts={messageId}&pretty=1";

            await _httpClientService.GetAysnc(requestUri);

        }

        private async Task<Message> PostMessage(string message, string channelId)
        {
            message = HttpUtility.UrlEncode(message);
            string requestUri = $"https://slack.com/api/chat.postMessage?token={_token}&channel={channelId}&text={message}&pretty=1";

            string result = await _httpClientService.GetAysnc(requestUri);

            if (result != null)
            {
                return JsonConvert.DeserializeObject<Message>(result);
            }

            return null;
        }

        public async Task<List<Message>> GetMessages()
        {
            if (_familyChannel == null)
                await Setup().ConfigureAwait(false);

            return await GetChannelMessage(_familyChannel.Id).ConfigureAwait(false);
        }

        public async Task<string> PostMessage(string message)
        {
            if (_familyChannel == null)
                await Setup().ConfigureAwait(false);

            var resultmessage = await PostMessage(message, _familyChannel.Id);

            return resultmessage.Ts;
        }

        public async Task<string> PostMessage<T1>(T1 message)
        {
            if (_familyChannel == null)
                await Setup().ConfigureAwait(false);

            var jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message, Formatting.None);

            var resultmessage = await PostMessage(jsonMessage, _familyChannel.Id).ConfigureAwait(false);

            return resultmessage.Ts;
        }

        public async Task<bool> UpdateMessage<T1>(T1 message, string postId)
        {
            if (_familyChannel == null)
                await Setup().ConfigureAwait(false);

            var jsonMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message, Formatting.None);

            await UpdateMessage(jsonMessage, postId, _familyChannel.Id);

            return true;
        }

        public async Task<bool> DeleteMessage(string postId)
        {
            if (_familyChannel == null)
                await Setup().ConfigureAwait(false);

            await DeleteMessage(postId, _familyChannel.Id);

            return true;
        }

        public async Task Setup()
        {
            await CheckFamilyChannel().ConfigureAwait(false);

            //Create Channel
            if (_familyChannel == null)
            {
                await CreateChannel(_defaultChannel).ConfigureAwait(false);
            }

            await CheckFamilyChannel().ConfigureAwait(false);
        }

        private async Task CheckFamilyChannel()
        {
            var allChannels = await GetAllChannels().ConfigureAwait(false);

            if (allChannels != null && allChannels.Any())
            {
                _familyChannel =
                    allChannels.FirstOrDefault(x => x.Name.Equals(_defaultChannel, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
