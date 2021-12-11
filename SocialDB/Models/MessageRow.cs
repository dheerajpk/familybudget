using System;

namespace SocialDB.Models
{
    public class MessageRow<T> where T : class
    {
        public string PrimaryId { get; set; }

        public string RowContent { get; set; }

        public T Data { get; set; }

        public bool IsSuccessFetch { get; internal set; }

        public Error RowError { get; set; }
    }

    public enum ErrorCode
    {
        ParserError,
        EmptyString
    }

    public class Error
    {
        private ErrorCode parserError;

        public Error(ErrorCode parserError, string message)
        {
            this.parserError = parserError;
            Message = message;
        }

        public ErrorCode Code { get; internal set; }

        public string Message { get; internal set; }
    }

    public class NoDataException : Exception
    {
        public NoDataException(string message) : base(message)
        {

        }
    }
}
