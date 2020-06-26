using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace TQ.Authentication.UnitTests.Filters.ExceptionFilter
{
    public class ErrorResponseDto
    {
        [Fact]
        public void ReturnsErrorResponseDto()
        {
            var message = "message 1";

            var dto = new TQ.Authentication.Core.Dto.ErrorResponseDto
            {
                StatusCode = HttpStatusCode.OK,
                Messages = new List<string> { message }
            };

            Assert.Equal(HttpStatusCode.OK, dto.StatusCode);
            Assert.Equal(message, dto.Messages.FirstOrDefault());
        }
    }
}