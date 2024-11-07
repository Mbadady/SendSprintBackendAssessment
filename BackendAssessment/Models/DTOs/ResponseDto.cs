using Newtonsoft.Json;

namespace BackendAssessment.Models.DTOs
{
    public class ResponseDto
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ExceptionError { get; set; }

        public ResponseDto()
        {
        }
        internal ResponseDto(bool succeeded, object entity = default)
        {
            IsSuccess = succeeded;
            Result = entity;
        }
        internal ResponseDto(bool succeeded, string message, object result = default, string exception = null)
        {
            IsSuccess = succeeded;
            Message = message;
            ExceptionError = exception;
            Result = result;
        }
        // Success method
        public static ResponseDto Success(object entity, string message)
        {
            return new ResponseDto
            {
                IsSuccess = true,
                Message = message,
                Result = entity,

            };
        }
        public static ResponseDto Success(string message)
        {
            return new ResponseDto(true, message);
        }
        // Failure method (by Type)
        public static ResponseDto Failure(Type request, string error)
        {
            string requestString = JsonConvert.SerializeObject(request);
            Log.Error("{Error} {EnvironmentNewLine} {RequestString}", error, Environment.NewLine, requestString);
            return new ResponseDto
            {
                IsSuccess = false,
                Message = error
            };
        }

        // Failure method (by generic type)
        public static ResponseDto Failure<T>(T request, string error)
        {
            string requestString = JsonConvert.SerializeObject(request);
            Log.Error("{Error} {EnvironmentNewLine} {RequestString} {DateTime}", error, Environment.NewLine, requestString, DateTime.Now);
            return new ResponseDto
            {
                IsSuccess = false,
                Message = error
            };
        }

        public static ResponseDto Failure(string error)
        {
            Log.Error(error);
            return new ResponseDto(false, message: error);
        }
        public static ResponseDto Failure(string prefixMessage, Exception ex)
        {
            Log.Error(ex, "Error: {ExceptionMessage} {EnvironmentNewLine} {ExceptionInnerMessage} {DateTime}", prefixMessage, Environment.NewLine, ex?.InnerException?.Message, DateTime.Now);
            return new ResponseDto(false, $"{prefixMessage}");
        }

        // Failure method with exception and prefix message
        public static ResponseDto Failure<T>(T request, string prefixMessage, Exception ex)
        {
            string requestString = JsonConvert.SerializeObject(request);
            Log.Error(ex, "Error: {ExceptionMessage} {EnvironmentNewLine} {ExceptionInnerMessage} {EnvironmentNextNewLine} {RequestString} {DateTime}", prefixMessage, Environment.NewLine, ex?.InnerException?.Message, Environment.NewLine, requestString, DateTime.Now);
            return new ResponseDto
            {
                IsSuccess = false,
                Message = $"{prefixMessage} Error: {ex?.Message}",
                ExceptionError = ex.InnerException?.Message // Capture the inner exception message
            };
        }
    }
}
