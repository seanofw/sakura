using System.Drawing;
using System.Windows.Forms;

namespace Sakura.BetterControls.MessageBox
{
	internal class BetterMessageBoxData<T>
	{
		public readonly IWin32Window? Owner;
		public readonly string? Message;
		public readonly string? Caption;
		public readonly ImmutableList<BetterMessageBoxButton<T>> Buttons;
		public readonly string? FontName;
		public readonly int FontSize;
		public readonly Image? Image;

		public BetterMessageBoxData(IWin32Window? owner, string? message, string? caption, ImmutableList<BetterMessageBoxButton<T>>? buttons, string? fontName, int fontSize, Image? image)
		{
			Owner = owner;
			Message = message;
			Caption = caption;
			Buttons = buttons ?? ImmutableList<BetterMessageBoxButton<T>>.Empty;
			FontName = fontName;
			FontSize = fontSize;
			Image = image;
		}

		public BetterMessageBoxData<T> WithMessage(string? message)
			=> new BetterMessageBoxData<T>(owner: Owner, message: message, caption: Caption, buttons: Buttons, fontName: FontName, fontSize: FontSize, image: Image);

		public BetterMessageBoxData<T> WithCaption(string? caption)
			=> new BetterMessageBoxData<T>(owner: Owner, message: Message, caption: caption, buttons: Buttons, fontName: FontName, fontSize: FontSize, image: Image);

		public BetterMessageBoxData<T> WithButtons(ImmutableList<BetterMessageBoxButton<T>>? buttons)
			=> new BetterMessageBoxData<T>(owner: Owner, message: Message, caption: Caption, buttons: buttons, fontName: FontName, fontSize: FontSize, image: Image);

		public BetterMessageBoxData<T> WithButtons(IEnumerable<BetterMessageBoxButton<T>> buttons)
			=> new BetterMessageBoxData<T>(owner: Owner, message: Message, caption: Caption, buttons: ImmutableList<BetterMessageBoxButton<T>>.Empty.AddRange(buttons), fontName: FontName, fontSize: FontSize, image: Image);

		public BetterMessageBoxData<T> WithOwner(IWin32Window? owner)
			=> new BetterMessageBoxData<T>(owner: owner, message: Message, caption: Caption, buttons: Buttons, fontName: FontName, fontSize: FontSize, image: Image);

		public BetterMessageBoxData<T> WithFont(string? fontName, int fontSize)
			=> new BetterMessageBoxData<T>(owner: Owner, message: Message, caption: Caption, buttons: Buttons, fontName: fontName, fontSize: fontSize, image: Image);

		public BetterMessageBoxData<T> AddButtons(IEnumerable<BetterMessageBoxButton<T>> buttons)
			=> new BetterMessageBoxData<T>(owner: Owner, message: Message, caption: Caption, buttons: Buttons.AddRange(buttons), fontName: FontName, fontSize: FontSize, image: Image);

		public BetterMessageBoxData<T> AddButtons(params BetterMessageBoxButton<T>[] buttons)
			=> new BetterMessageBoxData<T>(owner: Owner, message: Message, caption: Caption, buttons: Buttons.AddRange(buttons), fontName: FontName, fontSize: FontSize, image: Image);

		public BetterMessageBoxData<T> WithImage(Image? image)
			=> new BetterMessageBoxData<T>(owner: Owner, message: Message, caption: Caption, buttons: Buttons, fontName: FontName, fontSize: FontSize, image: image);
	}
}
