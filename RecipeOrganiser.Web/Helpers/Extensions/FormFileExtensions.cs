using System.IO;
using Microsoft.AspNetCore.Http;

namespace RecipeOrganiser.Web.Helpers
{
	public static class FormFileExtensions
	{
		public static byte[] ToBytes(this IFormFile file)
		{
			using (var stream = new MemoryStream())
			{
				file.CopyTo(stream);

				return stream.ToArray();
			}
		}

		public static IFormFile ToFormFile(this byte[] bytes)
		{
			if (bytes == null)
				return null;

			using (var stream = new MemoryStream(bytes))
			{
				return new FormFile(stream, 0, bytes.Length, "name", "fileName");
			}
		}
	}
}
