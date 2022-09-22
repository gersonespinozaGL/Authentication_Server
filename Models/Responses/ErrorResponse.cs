using System.Collections.Generic;

namespace Models.Responses
{
    public class ErrorResponse
    {
        IEnumerable<string> _errorMessages { get; set; }

        public ErrorResponse(string errorMessage)
        {
            _errorMessages = new List<string>() { errorMessage };
        }

        public ErrorResponse(IEnumerable<string> errorMessages)
        {
            _errorMessages = errorMessages;
        }
    }
}