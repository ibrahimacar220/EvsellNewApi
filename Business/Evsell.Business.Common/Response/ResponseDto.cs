namespace Evsell.Business.Common.Response
{
    public class ResponseDto<TDto>
    {
        public ResponseDto()
        {
            IsSuccess = false;
            HasException = false;
            Message = null;
            Dto = default(TDto);
        }

        public bool IsSuccess { get; set; }
        public bool HasException { get; set; }
        public string Message { get; set; }
        public long? ReturnedId { get; set; }
        public TDto Dto { get; set; }

        public ResponseDto<T> CopyWithoutDto<T>()
        {
            ResponseDto<T> response = new ResponseDto<T>();
            response.Dto = default(T);
            response.IsSuccess = IsSuccess;
            response.HasException = HasException;
            response.Message = Message;
            response.ReturnedId = ReturnedId;

            return response;
        }

        public ResponseDto<TDto> ToException(Exception ex)
        {
            IsSuccess = false;
            HasException = true;
            Message = ex.Message;
            return this;
        }

        public ResponseDto<TDto> Success(TDto dto, long? returnedId = null)
        {
            Dto = dto;
            IsSuccess = true;
            Message = null;
            ReturnedId = returnedId;
            return this;
        }

        public void Set(TDto dto, bool isSuccess, string message)
        {
            Dto = dto;
            IsSuccess = isSuccess;
            Message = message;
        }

        public ResponseDto<TDto> Failed(string message)
        {
            IsSuccess = false;
            HasException = true;
            Message = message;
            return this;
        }

        public ResponseDto<TDto> FailedWithException(Exception ex)
        {
            IsSuccess = false;
            HasException = true;
            Message = ex.Message;
            return this;
        }
    }

    /// <summary>
    /// Encapsulation class for returns of every method in controllers.
    /// It should also be used to encapsulate any return method among the webapi.
    /// 
    /// 'ReturnedId' will not be if the method is a save(insert/update) method and 'IsSuccess' is true.
    /// 'ReturnedId' will be new id of that table in the db or given id in update cases.
    /// </summary>
    public class ResponseDto
    {
        public ResponseDto()
        {
            IsSuccess = false;
            HasException = false;
            Message = null;
            ReturnedId = null;
        }

        public bool IsSuccess { get; set; }
        public bool HasException { get; set; }
        public string Message { get; set; }
        public long? ReturnedId { get; set; }

        public ResponseDto<T> ToResponse<T>()
        {
            return new ResponseDto<T>() { IsSuccess = this.IsSuccess, Message = this.Message, Dto = default(T) };
        }
        public ResponseDto<T> ToResponse<T>(T dto)
        {
            return new ResponseDto<T>() { IsSuccess = this.IsSuccess, Message = this.Message, Dto = dto };
        }

        public void Set(bool isSuccess, string message, long? returnedId)
        {
            IsSuccess = isSuccess;
            Message = message;
            ReturnedId = returnedId;
        }

        public ResponseDto Success(long? returnedId = null)
        {
            IsSuccess = true;
            Message = null;
            ReturnedId = returnedId;
            return this;
        }

        public ResponseDto Failed(string message)
        {
            IsSuccess = false;
            HasException = true;
            Message = message;
            return this;
        }

        public ResponseDto FailedWithException(Exception ex)
        {
            IsSuccess = false;
            HasException = true;
            Message = ex.Message;
            return this;
        }

        public ResponseDto FailedWithException(Exception ex, string message)
        {
            IsSuccess = false;
            HasException = true;
            Message = message;
            return this;
        }
    }
}
