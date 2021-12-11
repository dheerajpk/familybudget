using System;
using SocialDB.Services.Facebook.Models;
using SocialDB.Services.Slack.Models;
using System.Collections.Generic;
using System.Linq;

namespace SocialDB.Models
{
    internal class MapModels
    {
        public static IEnumerable<MessageRow<T>> To<T>(IEnumerable<FeedMessage> messages) where T : class
        {
            return messages?.Select(ConvertTo<T>);
        }

        public static IEnumerable<MessageRow<T>> To<T>(IEnumerable<Message> messages) where T : class
        {
            return messages?.Select(ConvertTo<T>);
        }

        private static MessageRow<T> ConvertTo<T>(FeedMessage feedMessage) where T : class
        {
            var row = new MessageRow<T>
            {
                PrimaryId = feedMessage.Id,
                RowContent = feedMessage.Message
            };

            try
            {
                if (string.IsNullOrWhiteSpace(feedMessage.Message)) throw new NoDataException("Content can't be empty");

                row.Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(feedMessage.Message);

                row.IsSuccessFetch = true;
            }
            catch (NoDataException ex)
            {
                row.RowError = new Error(ErrorCode.EmptyString, ex.Message);
            }
            catch (System.Exception ex)
            {
                row.RowError = new Error(ErrorCode.ParserError, ex.Message);
            }

            return row;
        }

        private static MessageRow<T> ConvertTo<T>(Message message) where T : class
        {
            var row = new MessageRow<T>
            {
                PrimaryId = message.Ts,
                RowContent = message.Text
            };

            try
            {
                if (string.IsNullOrWhiteSpace(message.Text)) throw new NoDataException("Content can't be empty");

                row.Data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(message.Text);

                row.IsSuccessFetch = true;
            }
            catch (NoDataException ex)
            {
                row.RowError = new Error(ErrorCode.EmptyString, ex.Message);
            }
            catch (System.Exception ex)
            {
                row.RowError = new Error(ErrorCode.ParserError, ex.Message);
            }

            return row;
        }
    }
}
