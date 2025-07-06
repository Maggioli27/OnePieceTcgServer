using System.Net;
using System.Text.Json;
using OnePieceTcg.BLL.CustomExceptions;

namespace OnePieceTcg.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate m_Next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            m_Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await m_Next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            Console.WriteLine(exception);
            context.Response.ContentType = "application/json";

            int statusCode = 0;
            switch (exception)
            {
                case UserNameAlreadyUsedException:
                case EmailAlreadyUsedException:
                case InvalidLoginException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.StatusCode = statusCode;

            await SendResponse(context, exception.Message);
        }

        private async Task SendResponse(HttpContext context, string message)
        {
            // Create response body
            var response = new
            {
                message = context.Response.StatusCode == (int)HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred. Please try again later."
                    : message
            };

            // Serialize and return the response
            var responseText = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(responseText);
        }
    }
}

