using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RevStackCore.IO.Pdf
{
	public static class PdfResponse
	{
		public static void Write(HttpResponse response, string fileName, byte[] buffer)
		{
			response.ContentType = "application/pdf";
			response.Headers.Add("content-disposition", new string[] { "inline;filename=" + fileName });
			response.Body.Write(buffer, 0, buffer.Length);
		}

		public static Task WriteAsync(HttpResponse response, string fileName, byte[] buffer)
		{
			return Task.Run(() => Write(response, fileName, buffer));
		}
	}
}
