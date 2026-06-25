using FlightScan.Core.Exceptions;
using FluentValidation;
using System.Text.Json;

namespace FlightScan.Api.Middlewares
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                await WriteResponse(context, 422, "422", string.Join(" ", errors));
            }
            catch (UnauthorizedException ex)
            {
                await WriteResponse(context, 401, ex.Code, ex.Message);
            }
            catch (NotFoundException ex)
            {
                await WriteResponse(context, 404, ex.Code, ex.Message);
            }
            catch (BadRequestException ex)
            {
                await WriteResponse(context, 400, ex.Code, ex.Message);
            }
            catch (GlobalException ex)
            {
                await WriteResponse(context, 500, ex.Code, ex.Message);
            }
            catch (Exception ex)
            {
                await WriteResponse(context, 500, "500", ex.Message);
            }
        }

        private static async Task WriteResponse(HttpContext context, int statusCode, string errorCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                error = message,
                errorCode
            });

            await context.Response.WriteAsync(result);
        }
    }
}
