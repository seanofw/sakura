using System.Drawing;
using System.Windows.Forms;

namespace Sakura.BetterControls.MessageBox
{
	public class BetterMessageBox<T>
	{
		protected BetterMessageBox() { }

		public static BetterMessageBoxFluentInterface<T> Message(string message)
			=> new BetterMessageBoxFluentInterface<T>().Message(message);

		public static BetterMessageBoxFluentInterface<T> Caption(string caption)
			=> new BetterMessageBoxFluentInterface<T>().Caption(caption);

		public static BetterMessageBoxFluentInterface<T> Buttons(params BetterMessageBoxButton<T>[] buttons)
			=> new BetterMessageBoxFluentInterface<T>().Buttons(buttons);

		public static BetterMessageBoxFluentInterface<T> Button(string text, T value, bool bold = false)
			=> new BetterMessageBoxFluentInterface<T>().Button(text, value, bold);

		public static BetterMessageBoxFluentInterface<T> Owner(IWin32Window owner)
			=> new BetterMessageBoxFluentInterface<T>().Owner(owner);

		public static BetterMessageBoxFluentInterface<T> Font(string fontName, int fontSize)
			=> new BetterMessageBoxFluentInterface<T>().Font(fontName, fontSize);

		public static BetterMessageBoxFluentInterface<T> Image(Image image)
			=> new BetterMessageBoxFluentInterface<T>().Image(image);

		public static BetterMessageBoxFluentInterface<T> ResourceImage(Assembly assembly, string resourceName)
			=> new BetterMessageBoxFluentInterface<T>().ResourceImage(assembly, resourceName);

		public static BetterMessageBoxFluentInterface<T> StandardImage(StandardImageKind imageKind)
			=> new BetterMessageBoxFluentInterface<T>().StandardImage(imageKind);
	}

	public class BetterMessageBox : BetterMessageBox<DialogResult>
	{
	}
}
