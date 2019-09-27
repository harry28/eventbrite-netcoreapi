using System;

namespace eventbrite.CustomExceptions
{
    public class BaseCustomException : Exception
    {
        private int _code;
        private string _description;

        public int Code
        {
            get => _code;
        }
        public string Description
        {
            get => _description;
        }

        public BaseCustomException(string message, string description, int code) : base(message)
        {
            _code = code;
            _description = description;
        }
    }

    public class CustomErrorResponse
    {
        public string Message { get; set; }
        public string Description { get; set; }
    }
}
