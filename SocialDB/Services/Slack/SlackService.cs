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
            /*
             * https://slack.com/api/channels.create?token=xoxp-316896464659-317845698950-316344801840-697ae5995e361fe3241eed85961350f5&name=TestChannel3&pretty=1
             */

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
            /*
             * https://slack.com/api/channels.history?token=xoxp-316896464659-317845698950-316344801840-697ae5995e361fe3241eed85961350f5&channel=C9BQVLMD4&count=10000&pretty=1
             */

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
            //https://slack.com/api/channels.list?token=xoxp-316896464659-317845698950-316344801840-697ae5995e361fe3241eed85961350f5&pretty=1

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
            //https://slack.com/api/chat.delete?token=xoxp-316896464659-317845698950-316344801840-697ae5995e361fe3241eed85961350f5&channel=C9AD5LYSD&ts=1519495737.000028&pretty=1

            string requestUri = $"https://slack.com/api/chat.delete?token={_token}&channel={channelId}&ts={messageId}&pretty=1";

            await _httpClientService.GetAysnc(requestUri);
        }

        private async Task UpdateMessage(string message, string messageId, string channelId)
        {
            //https://slack.com/api/chat.update?token=xoxp-316896464659-317845698950-316344801840-697ae5995e361fe3241eed85961350f5&channel=C9AD5LYSD&text=Hello%20again%202...&ts=1519495969.000100&pretty=1

            string requestUri = $"https://slack.com/api/chat.update?token={_token}&channel={channelId}&text={message}&ts={messageId}&pretty=1";

            await _httpClientService.GetAysnc(requestUri);

        }

        private async Task<Message> PostMessage(string message, string channelId)
        {
            //https://slack.com/api/chat.postMessage?token=xoxp-316896464659-317845698950-316344801840-697ae5995e361fe3241eed85961350f5&channel=C9AD5LYSD&text=This%20Is%20Posty&pretty=1
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
