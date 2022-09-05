namespace Sakura.BetterControls.MessageBox
{
	public class BetterMessageBoxButton<T>
	{
		public readonly string Text;
		public readonly T Value;
		public readonly bool Bold;

		public BetterMessageBoxButton(string text, T value, bool bold = false)
		{
			Text = text;
			Value = value;
			Bold = bold;
		}
	}
}
