using System.Drawing;
using System.Windows.Forms;

namespace Sakura.BetterControls.MessageBox
{
	internal class BetterMessageBoxForm<T> : Form
	{
		public T CustomResult { get; private set; } = default!;

		private readonly BetterMessageBoxData<T> _data;
		private readonly TextBox _textBox;
		private readonly Panel _buttonPanel;
		private readonly PictureBox? _pictureBox;

		public BetterMessageBoxForm(BetterMessageBoxData<T> data)
		{
			const int ButtonWidthPadding = 12;
			const int ButtonHeightPadding = 8;
			const int ButtonSpacing = 8;
			const int FormWidthPadding = 16;
			const int FormHeightPadding = 16;

			int imageWidth = data.Image?.Width ?? 0;
			int imageHeight = data.Image?.Height ?? 0;

			_data = data;
			DialogResult = DialogResult.None;

			Text = _data.Caption ?? string.Empty;
			MinimizeBox = false;
			MaximizeBox = false;
			ShowIcon = false;
			ShowInTaskbar = false;
			FormBorderStyle = FormBorderStyle.FixedSingle;

			Font font = new Font(_data.FontName ?? DefaultFont.FontFamily.Name, _data.FontSize);
			Font boldFont = new Font(_data.FontName ?? DefaultFont.FontFamily.Name, _data.FontSize, FontStyle.Bold);
			Size textSize = TextRenderer.MeasureText(_data.Message ?? string.Empty, font);

			_buttonPanel = new Panel();
			Controls.Add(_buttonPanel);

			if (data.Image != null && imageWidth > 0)
			{
				_pictureBox = new PictureBox();
				Controls.Add(_pictureBox);
				_pictureBox.Image = data.Image;
				_pictureBox.Size = data.Image.Size;
				_pictureBox.Location = new Point(FormWidthPadding, FormHeightPadding);
			}

			int textVerticalOffset = textSize.Height >= imageHeight
				? 0
				: (imageHeight - textSize.Height) / 2;

			_textBox = new TextBox();
			Controls.Add(_textBox);
			_textBox.BorderStyle = BorderStyle.None;
			_textBox.Multiline = true;
			_textBox.Location = new Point(FormWidthPadding + (imageWidth > 0 ? imageWidth + FormWidthPadding : 0), FormHeightPadding + textVerticalOffset);
			_textBox.Size = new Size(textSize.Width + FormWidthPadding * 2, textSize.Height + FormHeightPadding * 2);
			_textBox.WordWrap = true;
			_textBox.Text = _data.Message ?? string.Empty;
			_textBox.BackColor = BackColor;
			_textBox.Font = font;

			int buttonOffset = 0;
			int maxHeight = 0;
			foreach (BetterMessageBoxButton<T> buttonData in data.Buttons)
			{
				BetterMessageBoxButton<T> localButtonData = buttonData;

				Button button = new Button();
				_buttonPanel.Controls.Add(button);
				Font buttonFont = buttonData.Bold ? boldFont : font;
				Size buttonTextSize = TextRenderer.MeasureText(buttonData.Text ?? string.Empty, buttonFont);
				button.AutoSize = false;
				button.Location = new Point(buttonOffset, 0);
				button.Font = buttonFont;
				button.Size = new Size(buttonTextSize.Width + ButtonWidthPadding * 2,
					buttonTextSize.Height + ButtonHeightPadding * 2);
				button.Text = buttonData.Text;
				button.Click += (sender, e) =>
				{
					OnButtonClicked(localButtonData);
				};

				buttonOffset += buttonTextSize.Width + ButtonWidthPadding * 2 + ButtonSpacing;
				if (buttonTextSize.Height > maxHeight) maxHeight = buttonTextSize.Height;
			}
			foreach (Control control in _buttonPanel.Controls)
			{
				control.Height = maxHeight + ButtonHeightPadding * 2;
			}

			_buttonPanel.Size = new Size(buttonOffset - ButtonSpacing + FormWidthPadding,
				maxHeight + ButtonHeightPadding * 2 + FormHeightPadding);

			ClientSize = new Size(
				Math.Max(_buttonPanel.Width + FormWidthPadding, _textBox.Width + (imageWidth > 0 ? imageWidth + FormWidthPadding : 0)),
				Math.Max(imageHeight + FormHeightPadding * 2, _textBox.Height) + _buttonPanel.Height);

			_buttonPanel.Location = new Point(ClientSize.Width - _buttonPanel.Width, ClientSize.Height - _buttonPanel.Height);

			_buttonPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;

			StartPosition = _data.Owner != null ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Control? firstButton = _buttonPanel.Controls.Cast<Control>().FirstOrDefault();
			if (firstButton != null)
				firstButton.Focus();
		}

		protected void OnButtonClicked(BetterMessageBoxButton<T> button)
		{
			CustomResult = button.Value;
			DialogResult = DialogResult.OK;
			Close();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == Keys.Escape)
			{
				Close();
				return true;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (DialogResult == DialogResult.None)
			{
				DialogResult = DialogResult.Cancel;
				CustomResult = default!;
			}
		}
	}
}
