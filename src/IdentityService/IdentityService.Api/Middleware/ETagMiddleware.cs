using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Api.Middleware;

/// <summary>
/// </summary>
public class ETagMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// </summary>
    /// <param name="next"></param>
    public ETagMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var body = string.Empty;

        context.Response.OnStarting(state =>
        {
            var httpContext = (HttpContext)state;

            if (context.Response.StatusCode == (int)HttpStatusCode.OK &&
                context.Request.Method == "GET")
            {
                var key = GetKey(context.Request);
                Debug.WriteLine($"Key: {key}");
                Debug.WriteLine($"Body: {body}");

                var combinedKey = key + body;
                Debug.WriteLine($"CombinedKey: {combinedKey}");

                var combinedBytes = Encoding.UTF8.GetBytes(combinedKey);

                // generate ETag
                var ETAG = GenerateETag(combinedBytes);
                Debug.WriteLine($"ETag: {ETAG}");

                if (context.Request.Headers.Keys.Contains("If-None-Match") &&
                    context.Request.Headers["If-None-Match"].ToString() == ETAG)
                    // not modified
                    context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                else
                    context.Response.Headers.Add("ETag", new[] { ETAG });
            }

            return Task.FromResult(0);
        }, context);

        using var buffer = new MemoryStream();
        // replace the context response with our buffer
        var stream = context.Response.Body;
        context.Response.Body = buffer;

        // invoke the rest of the pipeline
        await _next.Invoke(context);

        // reset the buffer and read out the contents
        buffer.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(buffer);
        using (var bufferReader = new StreamReader(buffer))
        {
            body = await bufferReader.ReadToEndAsync();

            //reset to start of stream
            buffer.Seek(0, SeekOrigin.Begin);

            //copy our content to the original stream and put it back
            await buffer.CopyToAsync(stream);
            context.Response.Body = stream;

            Debug.WriteLine($"Response: {body}");
        }
    }

    private string GenerateETag(byte[] data)
    {
        var ret = string.Empty;

        using (var md5 = MD5.Create())
        {
            var hash = md5.ComputeHash(data);
            var hex = BitConverter.ToString(hash);
            ret = hex.Replace("-", "");
        }

        return ret;
    }

    private static string GetKey(HttpRequest request)
    {
        return request.GetDisplayUrl();
    }
}