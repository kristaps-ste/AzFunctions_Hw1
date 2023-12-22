using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AzureFunctionApplication.ContentApi.CustomActionResults
{
    internal class TextStreamActionResult : IActionResult
    {
        public TextStreamActionResult(Stream stream)
        {
            _stream = stream;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = ContentType;

            byte[] buffer = new byte[BufferSize];
            int bytesRead;
            while ((bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                await response.Body.WriteAsync(buffer, 0, bytesRead);
            }
        }

        #region Constants
        private const string ContentType = "text/plain";
        private const int BufferSize = 1024;
        #endregion
        
        #region Fields
        private readonly Stream _stream;
        #endregion
    }
}