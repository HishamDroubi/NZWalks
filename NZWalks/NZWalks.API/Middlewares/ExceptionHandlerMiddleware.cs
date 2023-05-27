using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger,RequestDelegate next) {
            this.logger = logger;
            this.next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // pass the request to continue and  if any exception haapens we wil catch it in the catch 
                await next(httpContext);
            }
            catch(Exception ex)
            {
                var errorId = Guid.NewGuid();

                logger.LogError(ex, $"{errorId}: {ex.Message}");

                //Retuen a Custome Error Response for any unhandled exception

                httpContext.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something Went Wrong Please Check Logs to see what is Wrong"
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
