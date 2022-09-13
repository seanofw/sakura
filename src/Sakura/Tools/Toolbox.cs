using System.Drawing;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sakura.Extensions;

namespace Sakura.Tools
{
	public class Toolbox : Control
	{
		private readonly Image _blackIcons;
		private readonly Image _whiteIcons;

		public MainWindow? MainWindow { get; internal set; }

		public ToolboxOrientation ToolboxOrientation
		{
			get => _toolboxOrientation;
			set
			{
				if (value != _toolboxOrientation)
				{
					_toolboxOrientation = value;
					PerformLayout();
					Invalidate();
				}
			}
		}
		private ToolboxOrientation _toolboxOrientation;

		public ToolboxSize ToolboxSize
		{
			get => _toolboxSize;
			set
			{
				if (value != _toolboxSize)
				{
					_toolboxSize = value;
					PerformLayout();
					Invalidate();
				}
			}
		}
		private ToolboxSize _toolboxSize;

		public int ToolboxPixelSize => ToolboxSize switch
		{
			ToolboxSize.XSmall => 20,
			ToolboxSize.Small => 24,
			ToolboxSize.Medium => 28,
			ToolboxSize.Large => 36,
			ToolboxSize.XLarge => 48,
			_ => 28,
		};

		public int ToolboxSeparatorSize => ToolboxPixelSize * 2 / 7;

		private int _currentGroup = 3;
		private int _currentToolGroup = 3;

		private int _clickToolIndex;

		private int _hoverToolIndex;
		private int _hoverGroupIndex;

		private struct ToolButtonInfo
		{
			public int Index;
			public ToolInfo ToolInfo;
			public Rectangle Rect;
			public Point DestImagePoint;
			public string Text;

			public ToolKind Kind => ToolInfo.Kind;
			public Point SrcImagePoint => new Point(ToolInfo.IconPosition.X * 16, ToolInfo.IconPosition.Y * 16);
			public int X => Rect.X;
			public int Y => Rect.Y;
			public int Width => Rect.Width;
			public int Height => Rect.Height;
		}

		public ToolboxConfig ToolboxConfig { get; }

		private List<ToolButtonInfo> _toolButtons = new List<ToolButtonInfo>();
		private List<ToolButtonInfo> _groupButtons = new List<ToolButtonInfo>();

		private ToolboxTip _toolboxTip;

		public Toolbox()
		{
			ToolboxConfig = null!;
			_blackIcons = _whiteIcons = null!;

			_toolboxTip = new ToolboxTip();

			string toolIconsFilename = Path.Combine(Program.ResourcePath, "ToolIcons.png");
			try
			{
				_blackIcons = Image.FromFile(toolIconsFilename);
				_whiteIcons = InvertImage(_blackIcons);
			}
			catch (Exception ex)
			{
				BetterControls.MessageBox.BetterMessageBox<bool>
					.Button("Uh oh", default, bold: true)
					.Caption(Program.Name)
					.StandardImage(BetterControls.MessageBox.StandardImageKind.Error)
					.Message($"Error: Cannot load '{toolIconsFilename}',\r\nwhich is critical for the main UI:\r\n{ex.Message}")
					.Show();
				Application.Exit();
				return;
			}

			string toolboxJsonFilename = Path.Combine(Program.ResourcePath, "Toolbox.json");
			try
			{
				string text = File.ReadAllText(toolboxJsonFilename, Encoding.UTF8);
				ToolboxConfig = JsonSerializer.Deserialize<ToolboxConfig>(text,
					new JsonSerializerOptions {
						PropertyNameCaseInsensitive = true,
						ReadCommentHandling = JsonCommentHandling.Skip,
						AllowTrailingCommas = true,
						Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
					})!;
			}
			catch (Exception ex)
			{
				ToolboxConfig = null!;
				BetterControls.MessageBox.BetterMessageBox<bool>
					.Button("Uh oh", default, bold: true)
					.Caption(Program.Name)
					.StandardImage(BetterControls.MessageBox.StandardImageKind.Error)
					.Message($"Error: Cannot load '{toolboxJsonFilename}',\r\nwhich is critical for the main UI:\r\n{ex.Message}")
					.Show();
				Application.Exit();
				return;
			}

			_clickToolIndex = -1;
			_hoverToolIndex = -1;
			_hoverGroupIndex = -1;

			DoubleBuffered = true;
		}

		private static Image InvertImage(Image image)
		{
			Img.Image img = ((Bitmap)image).ToImage();

			img.RemapMatrix(new MathLib.Matrix4x4f(
				-1,  0,  0,  1,
				 0, -1,  0,  1,
				 0,  0, -1,  1,
				 0,  0,  0,  1
			));

			return img.ToBitmap();
		}

		public void SetCurrentToolGroup(ToolKind toolKind)
		{
			List<ToolKind> currentTools = ToolboxConfig.Groups[_currentGroup].Tools;
			int newToolGroup = _currentGroup;
			if (!currentTools.Contains(toolKind))
			{
				for (int i = 0; i < ToolboxConfig.Groups.Count; i++)
				{
					ToolboxGroup group = ToolboxConfig.Groups[i];
					if (group.Tools.Contains(toolKind))
					{
						newToolGroup = i;
						break;
					}
				}
			}

			if (_currentToolGroup != newToolGroup)
			{
				_currentToolGroup = newToolGroup;
				Invalidate();
			}
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			const int IconSize = 16;

			if (ToolboxConfig == null || MainWindow == null)
				return;

			int w = ToolboxPixelSize;
			int h = w;
			int separatorSize = ToolboxSeparatorSize;

			int x, y;

			(int dx, int dy, int dsx, int dsy, int sw, int sh, int ox, int oy) = ToolboxOrientation switch
			{
				ToolboxOrientation.Top    => (w, 0, separatorSize, 0, separatorSize, h,  0, +1),
				ToolboxOrientation.Bottom => (w, 0, separatorSize, 0, separatorSize, h,  0, -1),
				ToolboxOrientation.Right  => (0, h, 0, separatorSize, w, separatorSize, -1,  0),
				_                         => (0, h, 0, separatorSize, w, separatorSize, +1,  0),
			};

			(x, y) = ToolboxOrientation switch
			{
				ToolboxOrientation.Right  => (Width - w, 0),
				ToolboxOrientation.Top    => (0, 0),
				ToolboxOrientation.Bottom => (0, Height - h),
				_                         => (0, 0),
			};
			_groupButtons.Clear();
			for (int i = 0; i < ToolboxConfig.Groups.Count; i++)
			{
				ToolboxGroup group = ToolboxConfig.Groups[i];
				ToolInfo toolInfo = MainWindow.ToolTable.Get(group.Icon)!;

				_groupButtons.Add(new ToolButtonInfo
				{
					Index = i,
					ToolInfo = toolInfo,
					Rect = new Rectangle(x, y, w, h),
					DestImagePoint = new Point(x + (w - IconSize) / 2 + ox, y + (h - IconSize) / 2 + oy),
					Text = group.Title,
				});

				y += dy;
				x += dx;
			}

			List<ToolKind> currentTools = ToolboxConfig.Groups[_currentGroup].Tools;

			(x, y) = ToolboxOrientation switch
			{
				ToolboxOrientation.Right  => (Width - w - 1 - w, 0),
				ToolboxOrientation.Top    => (0, h + 1),
				ToolboxOrientation.Bottom => (0, Height - h - 1 - h),
				_                         => (w + 1, 0),
			};
			_toolButtons.Clear();
			for (int i = 0; i < currentTools.Count; i++)
			{
				ToolInfo toolInfo = MainWindow.ToolTable.Get(currentTools[i])!;

				if (toolInfo.Kind == ToolKind.Separator)
				{
					_toolButtons.Add(new ToolButtonInfo
					{
						Index = i,
						ToolInfo = toolInfo,
						Rect = new Rectangle(x, y, sw, sh),
						DestImagePoint = new Point(0, 0),
						Text = toolInfo.Title,
					});
					x += dsx;
					y += dsy;
				}
				else
				{
					_toolButtons.Add(new ToolButtonInfo
					{
						Index = i,
						ToolInfo = toolInfo,
						Rect = new Rectangle(x, y, w, h),
						DestImagePoint = new Point(x + (w - IconSize) / 2, y + (h - IconSize) / 2),
						Text = toolInfo.Title,
					});
					y += dy;
					x += dx;
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			Color baseColor = SystemColors.Control;

			Color slightlyLighter = Color.FromArgb(
				Math.Min(baseColor.R * 17 / 16, 255),
				Math.Min(baseColor.G * 17 / 16, 255),
				Math.Min(baseColor.B * 17 / 16, 255));
			Color slightlyDarker = Color.FromArgb(
				baseColor.R * 15 / 16,
				baseColor.G * 15 / 16,
				baseColor.B * 15 / 16);
			Color darker = Color.FromArgb(
				baseColor.R * 14 / 16,
				baseColor.G * 14 / 16,
				baseColor.B * 14 / 16);

			int w = ToolboxPixelSize;
			int h = w;
			int halfSep = ToolboxSeparatorSize / 2;

			(Rectangle groupRect, Rectangle groupSep) = ToolboxOrientation switch
			{
				ToolboxOrientation.Right  => (new Rectangle(Width - w, 0, w, Height), new Rectangle(Width - w - 1, 0, 1, Height)),
				ToolboxOrientation.Top    => (new Rectangle(0, 0, Width, h),          new Rectangle(0, h, Width, 1)),
				ToolboxOrientation.Bottom => (new Rectangle(0, Height - h, Width, h), new Rectangle(0, Height - h - 1, Width, 1)),
				_ =>                         (new Rectangle(0, 0, w, Height),         new Rectangle(w, 0, 1, Height)),
			};
			e.Graphics.FillRectangle(new SolidBrush(baseColor), 0, 0, Width, Height);
			e.Graphics.FillRectangle(new SolidBrush(slightlyDarker), groupRect);
			e.Graphics.FillRectangle(new SolidBrush(darker), groupSep);

			if (ToolboxConfig == null)
				return;

			foreach (ToolButtonInfo button in _groupButtons)
			{
				if (_currentGroup == button.Index)
				{
					if (ToolboxOrientation == ToolboxOrientation.Top || ToolboxOrientation == ToolboxOrientation.Bottom)
					{
						e.Graphics.FillRectangle(new SolidBrush(darker),
							new Rectangle(button.X - 1, button.Y, 1, button.Height));
						e.Graphics.FillRectangle(new SolidBrush(baseColor),
							new Rectangle(button.X, button.Y - 1, button.Width, button.Height + 2));
						e.Graphics.FillRectangle(new SolidBrush(darker),
							new Rectangle(button.X + button.Width, button.Y, 1, button.Height));
					}
					else
					{
						e.Graphics.FillRectangle(new SolidBrush(darker),
							new Rectangle(button.X, button.Y - 1, button.Width, 1));
						e.Graphics.FillRectangle(new SolidBrush(baseColor),
							new Rectangle(button.X - 1, button.Y, button.Width + 2, button.Height));
						e.Graphics.FillRectangle(new SolidBrush(darker),
							new Rectangle(button.X, button.Y + button.Height, button.Width, 1));
					}
				}
				else if (button.Index == _hoverGroupIndex)
				{
					e.Graphics.FillRectangle(new SolidBrush(baseColor),
						new Rectangle(button.Rect.X + 1, button.Rect.Y + 1, button.Rect.Width - 2, button.Rect.Height - 2));
				}

				if (button.Index == _currentToolGroup)
				{
					const int thickness = 2;
					Rectangle rectangle = ToolboxOrientation switch
					{
						ToolboxOrientation.Top => new Rectangle(button.Rect.X, button.Rect.Y, button.Rect.Width, thickness),
						ToolboxOrientation.Bottom => new Rectangle(button.Rect.X, button.Rect.Y + button.Rect.Height - thickness, button.Rect.Width, thickness),
						ToolboxOrientation.Right => new Rectangle(button.Rect.X + button.Rect.Width - thickness, button.Rect.Y, thickness, button.Rect.Height),
						_ => new Rectangle(button.Rect.X, button.Rect.Y, thickness, button.Rect.Height),
					};
					e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight), rectangle);
				}

				e.Graphics.DrawImage(_blackIcons, new Rectangle(button.DestImagePoint, new Size(16, 16)),
					new Rectangle(button.SrcImagePoint, new Size(16, 16)), GraphicsUnit.Pixel);
			}

			ToolKind currentTool = MainWindow != null ? MainWindow.CurrentTool : ToolKind.Object_Select;

			foreach (ToolButtonInfo button in _toolButtons)
			{
				if (button.Kind == ToolKind.Separator)
				{
					if (ToolboxOrientation == ToolboxOrientation.Top || ToolboxOrientation == ToolboxOrientation.Bottom)
					{
						e.Graphics.FillRectangle(new SolidBrush(darker),
							new Rectangle(button.X + button.Width / 2, button.Y + halfSep, 1, button.Height - (halfSep * 2)));
					}
					else
					{
						e.Graphics.FillRectangle(new SolidBrush(darker),
							new Rectangle(button.X + halfSep, button.Y + button.Height / 2, button.Width - (halfSep * 2), 1));
					}
					continue;
				}

				Image icons = _blackIcons;
				Color fillColor = SystemColors.Control;

				if (button.Index == _clickToolIndex
					|| (button.Kind == currentTool && _clickToolIndex < 0)
					|| (MainWindow != null && MainWindow.ActiveTools.Contains(button.Kind)))
					(fillColor, icons) = (SystemColors.Highlight, _whiteIcons);
				else if (button.Index == _hoverToolIndex || button.Kind == currentTool)
					fillColor = slightlyLighter;

				e.Graphics.FillRectangle(new SolidBrush(fillColor), button.Rect);

				e.Graphics.DrawImage(icons, new Rectangle(button.DestImagePoint, new Size(16, 16)),
					new Rectangle(button.SrcImagePoint, new Size(16, 16)), GraphicsUnit.Pixel);
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			if (ToolboxConfig == null)
				return;

			if (_hoverGroupIndex != -1)
				Invalidate(_groupButtons[_hoverGroupIndex].Rect);
			if (_hoverToolIndex != -1)
				Invalidate(_toolButtons[_hoverToolIndex].Rect);

			_hoverGroupIndex = -1;
			_hoverToolIndex = -1;
			_clickToolIndex = -1;

			_toolboxTip.Hide();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (ToolboxConfig == null)
				return;

			int newHoverGroupIndex = -1;
			foreach (ToolButtonInfo button in _groupButtons)
			{
				if (button.Rect.Contains(e.Location))
				{
					newHoverGroupIndex = button.Index;
					break;
				}
			}
			if (newHoverGroupIndex != _hoverGroupIndex)
			{
				if (_hoverGroupIndex != -1)
					Invalidate(_groupButtons[_hoverGroupIndex].Rect);
				_hoverGroupIndex = newHoverGroupIndex;
				if (_hoverGroupIndex != -1)
					Invalidate(_groupButtons[_hoverGroupIndex].Rect);

				if (_hoverGroupIndex >= 0)
				{
					_toolboxTip.Text = _groupButtons[_hoverGroupIndex].Text;
					_toolboxTip.ShowFor(this, _groupButtons[_hoverGroupIndex].Rect, ToolboxOrientation);
				}
				else _toolboxTip.Hide();
			}

			int newHoverToolIndex = -1;
			foreach (ToolButtonInfo button in _toolButtons)
			{
				if (button.Rect.Contains(e.Location))
				{
					newHoverToolIndex = button.Index;
					_toolboxTip.Text = button.Text;
					break;
				}
			}
			if (newHoverToolIndex != _hoverToolIndex)
			{
				if (_clickToolIndex >= 0)
				{
					Invalidate();
					_clickToolIndex = -1;
				}

				if (_hoverToolIndex != -1)
					Invalidate(_toolButtons[_hoverToolIndex].Rect);
				_hoverToolIndex = newHoverToolIndex;
				if (_hoverToolIndex != -1)
					Invalidate(_toolButtons[_hoverToolIndex].Rect);

				if (_hoverToolIndex >= 0 && _toolButtons[_hoverToolIndex].Kind != ToolKind.Separator)
				{
					_toolboxTip.Text = _toolButtons[_hoverToolIndex].Text;
					_toolboxTip.ShowFor(this, _toolButtons[_hoverToolIndex].Rect, ToolboxOrientation);
				}
				else _toolboxTip.Hide();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (_hoverGroupIndex >= 0 && _hoverGroupIndex < _groupButtons.Count)
			{
				_currentGroup = _hoverGroupIndex;
				PerformLayout();
				Invalidate();
				return;
			}

			_clickToolIndex = _hoverToolIndex;
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (ToolboxConfig == null)
				return;

			List<ToolKind> currentTools = ToolboxConfig.Groups[_currentGroup].Tools;

			if (_clickToolIndex >= 0 && _clickToolIndex < currentTools.Count)
			{
				if (MainWindow != null)
					MainWindow.ActivateTool(currentTools[_clickToolIndex]);

				Invalidate();
			}
			_clickToolIndex = -1;
		}
	}
}
