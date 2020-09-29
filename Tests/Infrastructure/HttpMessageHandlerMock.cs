using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Infrastructure
{
    class HttpMessageHandlerMock : HttpMessageHandler
    {
        HttpStatusCode _statusCode;
        HttpContent _content;
        public HttpMessageHandlerMock(StringContent content, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _statusCode = statusCode;
            _content = content;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return  new  HttpResponseMessage
            {
                StatusCode = _statusCode,
                Content = _content
            };
        }
    }
}
