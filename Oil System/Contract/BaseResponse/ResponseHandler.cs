using System.Net;

namespace Oil_System.Contract.BaseResponse
{
    public class ResponseHandler
    {
        public BaseResponse<T> Success<T>(T data)
        {
            return new BaseResponse<T>(data, true, HttpStatusCode.OK, "Operation completed successfully.");
        }

        public BaseResponse<T> Failure<T>(HttpStatusCode statusCode, string errorMessage)
        {
            return new BaseResponse<T>(false, statusCode, errorMessage);
        }

        public BaseResponse<T> Created<T>()
        {
            return new BaseResponse<T>(true, HttpStatusCode.Created, "Resource created successfully.");
        }

        public BaseResponse<T> Updated<T>()
        {
            return new BaseResponse<T>(true, HttpStatusCode.Accepted, "Resource updated successfully.");
        }

        public BaseResponse<T> Deleted<T>()
        {
            return new BaseResponse<T>(true, HttpStatusCode.Accepted, "Resource updated successfully.");
        }

        public BaseResponse<T> NotFound<T>()
        {
            return new BaseResponse<T>(false, HttpStatusCode.BadRequest, "Resource not Found.");
        }
    }
}
