using System;
using SocialDB.Models;
using SocialDB.Services;
using SocialDB.Services.Facebook;
using SocialDB.Services.Slack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialDB.Query
{
    public class DataQuery
    {
        private readonly ServiceConnection _serviceConnection;

        private readonly FacebookService _facebookService;

        private readonly SlackService _slackService;

        public DataQuery(string token, ServiceConnection serviceConnection)
        {
            _serviceConnection = serviceConnection;

            if (serviceConnection == ServiceConnection.Facebook) _facebookService = new FacebookService(token);

            if (serviceConnection == ServiceConnection.Slack) _slackService = new SlackService(token);
        }

        public Task<IEnumerable<MessageRow<T>>> Select<T>() where T : class
        {
            if (_serviceConnection == ServiceConnection.Facebook) return GetFacebookMessages<T>();

            if (_serviceConnection == ServiceConnection.Slack) return GetSlackMessages<T>();

            return null;
        }

        public async Task<IEnumerable<MessageRow<T>>> Select<T>(Func<MessageRow<T>, bool> predicate) where T : class
        {
            if (_serviceConnection == ServiceConnection.Facebook)
            {
                var messages = await GetFacebookMessages<T>().ConfigureAwait(false);

                return messages.Where(predicate);
            }

            if (_serviceConnection == ServiceConnection.Slack)
            {
                var messages = await GetSlackMessages<T>().ConfigureAwait(false);

                return messages.Where(predicate);
            }

            return null;
        }

        public Task<bool> Update<T>(T value, string primaryId) where T : class
        {
            if (_serviceConnection == ServiceConnection.Facebook) return _facebookService.UpdateMessage(value, primaryId);

            if (_serviceConnection == ServiceConnection.Slack) return _slackService.UpdateMessage(value, primaryId);

            return Task.FromResult(false);
        }

        public Task<bool> Delete<T>(MessageRow<T> message) where T : class
        {
            return Delete(message.PrimaryId);
        }

        public Task<bool> Delete(string primaryId)
        {
            if (_serviceConnection == ServiceConnection.Facebook) return _facebookService.DeleteMessage(primaryId);

            if (_serviceConnection == ServiceConnection.Slack) return _slackService.DeleteMessage(primaryId);

            return Task.FromResult(false);
        }

        public async Task<MessageRow<T>> Insert<T>(T value) where T : class
        {
            if (_serviceConnection == ServiceConnection.Facebook)
            {
                var id = await _facebookService.PostMessage<T>(value).ConfigureAwait(false);

                return new MessageRow<T>()
                {
                    Data = value,
                    PrimaryId = id
                };

            }

            if (_serviceConnection == ServiceConnection.Slack)
            {
                var id = await _slackService.PostMessage<T>(value).ConfigureAwait(false);

                return new MessageRow<T>()
                {
                    Data = value,
                    PrimaryId = id
                };
            }

            return null;
        }

        private async Task<IEnumerable<MessageRow<T>>> GetFacebookMessages<T>() where T : class
        {
            var messages = await _facebookService.GetMessages().ConfigureAwait(false);

            return MapModels.To<T>(messages);
        }

        private async Task<IEnumerable<MessageRow<T>>> GetSlackMessages<T>() where T : class
        {
            var messages = await _slackService.GetMessages().ConfigureAwait(false);

            return MapModels.To<T>(messages);
        }
    }
}
