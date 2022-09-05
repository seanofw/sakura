using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Sakura.BetterControls.MessageBox
{
	public class BetterMessageBoxFluentInterface<T>
	{
		internal BetterMessageBoxData<T> BetterMessageBoxData { get; private set; }
			= new BetterMessageBoxData<T>(null, null, null, null, "Verdana", 9, null);

		internal BetterMessageBoxFluentInterface() { }

		public BetterMessageBoxFluentInterface<T> Owner(IWin32Window owner)
		{
			BetterMessageBoxData = BetterMessageBoxData.WithOwner(owner);
			return this;
		}

		public BetterMessageBoxFluentInterface<T> Message(string message)
		{
			BetterMessageBoxData = BetterMessageBoxData.WithMessage(message);
			return this;
		}

		public BetterMessageBoxFluentInterface<T> Caption(string caption)
		{
			BetterMessageBoxData = BetterMessageBoxData.WithCaption(caption);
			return this;
		}

		public BetterMessageBoxFluentInterface<T> Buttons(params BetterMessageBoxButton<T>[] buttons)
		{
			BetterMessageBoxData = BetterMessageBoxData.AddButtons(buttons);
			return this;
		}

		public BetterMessageBoxFluentInterface<T> Button(string text, T value, bool bold = false)
		{
			BetterMessageBoxData = BetterMessageBoxData.AddButtons(new BetterMessageBoxButton<T>(text, value, bold));
			return this;
		}

		public BetterMessageBoxFluentInterface<T> Font(string fontName, int fontSize)
		{
			BetterMessageBoxData = BetterMessageBoxData.WithFont(fontName, fontSize);
			return this;
		}

		public BetterMessageBoxFluentInterface<T> Image(Image image)
		{
			BetterMessageBoxData = BetterMessageBoxData.WithImage(image);
			return this;
		}

		public BetterMessageBoxFluentInterface<T> ResourceImage(Assembly assembly, string resourceName)
		{
			Stream? stream = assembly.GetManifestResourceStream(resourceName.Replace('/', '.').Replace('\\', '.'));
			Image? image = stream != null ? System.Drawing.Image.FromStream(stream) : null;
			if (image != null)
				BetterMessageBoxData = BetterMessageBoxData.WithImage(image);
			return this;
		}

		public BetterMessageBoxFluentInterface<T> StandardImage(StandardImageKind imageKind)
		{
			byte[] imageData;
			switch (imageKind)
			{
				case StandardImageKind.Error:
					imageData = BetterMessageBoxStandardIcons.Error48;
					break;
				case StandardImageKind.Warning:
					imageData = BetterMessageBoxStandardIcons.Warning48;
					break;
				case StandardImageKind.Question:
					imageData = BetterMessageBoxStandardIcons.Question48;
					break;
				case StandardImageKind.Success:
					imageData = BetterMessageBoxStandardIcons.Success48;
					break;
				default:
					throw new ArgumentException("imageKind");
			}
			using (MemoryStream stream = new MemoryStream(imageData))
			{
				Image image = System.Drawing.Image.FromStream(stream);
				BetterMessageBoxData = BetterMessageBoxData.WithImage(image);
			}
			return this;
		}

		public T Show()
		{
			BetterMessageBoxForm<T> form = new BetterMessageBoxForm<T>(BetterMessageBoxData);

			if (BetterMessageBoxData.Owner != null)
				form.ShowDialog(BetterMessageBoxData.Owner);
			else
				form.ShowDialog();

			return form.CustomResult;
		}
	}
}
