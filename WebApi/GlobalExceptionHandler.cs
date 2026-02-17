using Application.Common.Models;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
        {
            _logger.LogError(exception, "Ошибка при обработке запроса: {Message}", exception.Message);

            // Явно указываем тип кортежа, чтобы избежать CS8131/CS8130
            (int StatusCode, string Message) result = exception switch
            {
                ValidationException ex => (
                    StatusCodes.Status400BadRequest,
                    string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))
                ),

                // Обязательно добавь using Domain.Exceptions в начало файла
                DomainException ex => (
                    StatusCodes.Status422UnprocessableEntity,
                    ex.Message
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Произошла непредвиденная ошибка"
                )
            };

            context.Response.StatusCode = result.StatusCode;

            // Возвращаем стандартизированный ответ, который Orval превратит в интерфейс
            await context.Response.WriteAsJsonAsync(
                Result<object>.Failure(result.Message),
                ct
            );

            return true;
        }
    }
}
