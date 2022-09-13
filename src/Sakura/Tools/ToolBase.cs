using Sakura.MathLib;
using Sakura.Model;
using Sakura.Rendering;
using System.Windows.Forms;

namespace Sakura.Tools
{
	public abstract class ToolBase
	{
		protected MainWindow MainWindow { get; }

		protected DocumentWindow? DocumentWindow => MainWindow.ActiveDocument as DocumentWindow;

		protected VectorSurface? VectorSurface => DocumentWindow?.VectorSurface;

		protected Document Document
		{
			get => VectorSurface?.Document ?? Document.Empty;
			set {
				if (VectorSurface != null)
					VectorSurface.Document = value;
			}
		}

		protected Camera Camera
		{
			get => VectorSurface?.Camera ?? Camera.Default;
			set
			{
				if (VectorSurface != null)
					VectorSurface.Camera = value;
			}
		}

		protected ToolBase(MainWindow mainWindow)
		{
			MainWindow = mainWindow;
		}

		public Vector2d WorldToScreen(Vector2d worldPoint)
			=> VectorSurface != null ? VectorSurface.WorldToScreen(worldPoint) : worldPoint;

		public Vector2d ScreenToWorld(Vector2d screenPoint)
			=> VectorSurface != null ? VectorSurface.WorldToScreen(screenPoint) : screenPoint;

		public virtual void OnActivate(EventArgs e)
		{
		}

		public virtual void OnDeactivate(EventArgs e)
		{
		}

		public virtual void OnMouseClick(MouseEventArgs e)
		{
		}

		public virtual void OnMouseDoubleClick(MouseEventArgs e)
		{
		}

		public virtual void OnMouseDown(MouseEventArgs e)
		{
		}

		public virtual void OnMouseUp(MouseEventArgs e)
		{
		}

		public virtual void OnMouseMove(MouseEventArgs e)
		{
		}

		public virtual void OnMouseEnter(EventArgs e)
		{
		}

		public virtual void OnMouseLeave(EventArgs e)
		{
		}

		public virtual void OnMouseCaptureChanged(EventArgs e)
		{
		}

		public virtual void OnKeyDown(KeyEventArgs e)
		{
		}

		public virtual void OnKeyUp(KeyEventArgs e)
		{
		}

		public virtual void OnKeyPress(KeyPressEventArgs e)
		{
		}

		public virtual void OnGotFocus(EventArgs e)
		{
		}

		public virtual void OnLostFocus(EventArgs e)
		{
		}
	}
}