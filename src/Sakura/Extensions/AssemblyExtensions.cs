using System.Drawing;

namespace Sakura.Extensions
{
	public static class AssemblyExtensions
	{
		public static byte[] GetEmbeddedResource(this Assembly assembly, string name)
		{
			using (Stream? stream = assembly.GetManifestResourceStream(@"Sakura." + name.Replace('/', '.').Replace('\\', '.')))
			{
				if (stream == null)
					throw new ArgumentException($"Embedded resource \"{name}\" not found.");
				using (MemoryStream memoryStream = new MemoryStream())
				{
					stream.CopyTo(memoryStream);
					return memoryStream.ToArray();
				}
			}
		}

		public static Image GetEmbeddedImage(this Assembly assembly, string name)
		{
			using (Stream? stream = assembly.GetManifestResourceStream(@"Sakura." + name.Replace('/', '.').Replace('\\', '.')))
			{
				if (stream == null)
					throw new ArgumentException($"Embedded resource \"{name}\" not found.");
				return Image.FromStream(stream);
			}
		}
	}
}
