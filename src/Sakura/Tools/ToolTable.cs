using Sakura.MathLib;

namespace Sakura.Tools
{
	public class ToolTable
	{
		public MainWindow MainWindow { get; }

		public IReadOnlyDictionary<ToolKind, ToolInfo> Tools { get; }

		public ToolTable(MainWindow mainWindow)
		{
			MainWindow = mainWindow;
			Tools = MakeTools();
		}

		public ToolInfo? Get(ToolKind kind)
			=> Tools.TryGetValue(kind, out ToolInfo? toolInfo) ? toolInfo : null;

		private IReadOnlyDictionary<ToolKind, ToolInfo> MakeTools()
			=> new Dictionary<ToolKind, ToolInfo>
			{
				{
					ToolKind.Diamond,
					new ToolInfo(
						kind: ToolKind.Diamond,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(0, 0),
						title: "Regular Tools"
					)
				},
				{
					ToolKind.Separator,
					new ToolInfo(
						kind: ToolKind.Separator,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(0, 1),
						title: "-"
					)
				},

				{
					ToolKind.File_New,
					new ToolInfo(
						kind: ToolKind.File_New,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(1, 0),
						title: "New Document"
					)
				},
				{
					ToolKind.File_Open,
					new ToolInfo(
						kind: ToolKind.File_Open,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(1, 1),
						title: "Open..."
					)
				},
				{
					ToolKind.File_Save,
					new ToolInfo(
						kind: ToolKind.File_Save,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(1, 2),
						title: "Save"
					)
				},
				{
					ToolKind.File_Print,
					new ToolInfo(
						kind: ToolKind.File_Print,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(1, 3),
						title: "Print"
					)
				},
				{
					ToolKind.File_Export,
					new ToolInfo(
						kind: ToolKind.File_Export,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(1, 4),
						title: "Export..."
					)
				},
				{
					ToolKind.File_Import,
					new ToolInfo(
						kind: ToolKind.File_Import,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(1, 5),
						title: "Import..."
					)
				},

				{
					ToolKind.Camera_Zoom,
					new ToolInfo(
						kind: ToolKind.Camera_Zoom,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(2, 0),
						title: "Zoom Tool"
					)
				},
				{
					ToolKind.Camera_ZoomIn,
					new ToolInfo(
						kind: ToolKind.Camera_ZoomIn,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(2, 1),
						title: "Zoom In"
					)
				},
				{
					ToolKind.Camera_ZoomOut,
					new ToolInfo(
						kind: ToolKind.Camera_ZoomOut,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(2, 2),
						title: "Zoom Out"
					)
				},
				{
					ToolKind.Camera_Zoom100,
					new ToolInfo(
						kind: ToolKind.Camera_Zoom100,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(2, 3),
						title: "Zoom to 100%"
					)
				},
				{
					ToolKind.Camera_ZoomFit,
					new ToolInfo(
						kind: ToolKind.Camera_ZoomFit,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(2, 4),
						title: "Zoom to Fit"
					)
				},
				{
					ToolKind.Camera_Pan,
					new ToolInfo(
						kind: ToolKind.Camera_Pan,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(2, 5),
						title: "Pan Tool"
					)
				},
				{
					ToolKind.Camera_Rotate,
					new ToolInfo(
						kind: ToolKind.Camera_Rotate,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(2, 6),
						title: "Rotate Page"
					)
				},
				{
					ToolKind.Camera_ToggleRuler,
					new ToolInfo(
						kind: ToolKind.Camera_ToggleRuler,
						mode: ToolMode.Toggle,
						iconPosition: new Vector2i(2, 7),
						title: "Show Rulers"
					)
				},
				{
					ToolKind.Camera_ToggleScrollbar,
					new ToolInfo(
						kind: ToolKind.Camera_ToggleScrollbar,
						mode: ToolMode.Toggle,
						iconPosition: new Vector2i(2, 8),
						title: "Show Scrollbars"
					)
				},
				{
					ToolKind.Camera_ToggleGrid,
					new ToolInfo(
						kind: ToolKind.Camera_ToggleGrid,
						mode: ToolMode.Toggle,
						iconPosition: new Vector2i(2, 9),
						title: "Show Grid"
					)
				},
				{
					ToolKind.Camera_ToggleGridSnap,
					new ToolInfo(
						kind: ToolKind.Camera_ToggleGridSnap,
						mode: ToolMode.Toggle,
						iconPosition: new Vector2i(2, 10),
						title: "Snap to Grid"
					)
				},
				{
					ToolKind.Camera_RenderWireframe,
					new ToolInfo(
						kind: ToolKind.Camera_RenderWireframe,
						mode: ToolMode.Toggle,
						iconPosition: new Vector2i(2, 11),
						title: "Render Wireframe"
					)
				},
				{
					ToolKind.Camera_RenderRough,
					new ToolInfo(
						kind: ToolKind.Camera_RenderRough,
						mode: ToolMode.Toggle,
						iconPosition: new Vector2i(2, 12),
						title: "Render Rough"
					)
				},
				{
					ToolKind.Camera_RenderFinal,
					new ToolInfo(
						kind: ToolKind.Camera_RenderFinal,
						mode: ToolMode.Toggle,
						iconPosition: new Vector2i(2, 13),
						title: "Render Final"
					)
				},

				{
					ToolKind.Edit_Undo,
					new ToolInfo(
						kind: ToolKind.Edit_Undo,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 2),
						title: "Undo"
					)
				},
				{
					ToolKind.Edit_Redo,
					new ToolInfo(
						kind: ToolKind.Edit_Redo,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 3),
						title: "Redo"
					)
				},
				{
					ToolKind.Edit_Cut,
					new ToolInfo(
						kind: ToolKind.Edit_Cut,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 4),
						title: "Cut"
					)
				},
				{
					ToolKind.Edit_Copy,
					new ToolInfo(
						kind: ToolKind.Edit_Copy,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 5),
						title: "Copy"
					)
				},
				{
					ToolKind.Edit_Paste,
					new ToolInfo(
						kind: ToolKind.Edit_Paste,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 6),
						title: "Paste"
					)
				},
				{
					ToolKind.Edit_Delete,
					new ToolInfo(
						kind: ToolKind.Edit_Delete,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 7),
						title: "Delete"
					)
				},

				{
					ToolKind.Object_Select,
					new ToolInfo(
						kind: ToolKind.Object_Select,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(3, 0),
						title: "Select Objects"
					)
				},
				{
					ToolKind.Object_Lasso,
					new ToolInfo(
						kind: ToolKind.Object_Lasso,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(3, 1),
						title: "Lasso Objects"
					)
				},
				{
					ToolKind.Object_Duplicate,
					new ToolInfo(
						kind: ToolKind.Object_Duplicate,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(3, 8),
						title: "Duplicate Objects"
					)
				},
				{
					ToolKind.Object_Warp,
					new ToolInfo(
						kind: ToolKind.Object_Warp,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(3, 9),
						title: "Warp Objects"
					)
				},
				{
					ToolKind.Object_Forward,
					new ToolInfo(
						kind: ToolKind.Object_Forward,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 10),
						title: "Move Object Forward One"
					)
				},
				{
					ToolKind.Object_Back,
					new ToolInfo(
						kind: ToolKind.Object_Back,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 11),
						title: "Move Object Back One"
					)
				},
				{
					ToolKind.Object_InFrontOf,
					new ToolInfo(
						kind: ToolKind.Object_InFrontOf,
						mode: ToolMode.UntilClick,
						iconPosition: new Vector2i(3, 12),
						title: "Move Object in Front of..."
					)
				},
				{
					ToolKind.Object_Behind,
					new ToolInfo(
						kind: ToolKind.Object_Behind,
						mode: ToolMode.UntilClick,
						iconPosition: new Vector2i(3, 13),
						title: "Move Object Behind..."
					)
				},
				{
					ToolKind.Object_ToFront,
					new ToolInfo(
						kind: ToolKind.Object_ToFront,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 14),
						title: "Move Object to Front"
					)
				},
				{
					ToolKind.Object_ToBack,
					new ToolInfo(
						kind: ToolKind.Object_ToBack,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 15),
						title: "Move Object to Back"
					)
				},
				{
					ToolKind.Object_Group,
					new ToolInfo(
						kind: ToolKind.Object_Group,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 16),
						title: "Group Objects Together"
					)
				},
				{
					ToolKind.Object_Ungroup,
					new ToolInfo(
						kind: ToolKind.Object_Ungroup,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 17),
						title: "Ungroup Objects"
					)
				},
				{
					ToolKind.Object_Clip,
					new ToolInfo(
						kind: ToolKind.Object_Clip,
						mode: ToolMode.UntilClick,
						iconPosition: new Vector2i(3, 18),
						title: "Clip Objects"
					)
				},
				{
					ToolKind.Object_Unclip,
					new ToolInfo(
						kind: ToolKind.Object_Unclip,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 19),
						title: "Unclip Contents"
					)
				},
				{
					ToolKind.Object_CreateClipper,
					new ToolInfo(
						kind: ToolKind.Object_CreateClipper,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 20),
						title: "Create Empty Clipper"
					)
				},
				{
					ToolKind.Object_DeleteClipper,
					new ToolInfo(
						kind: ToolKind.Object_DeleteClipper,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 21),
						title: "Delete Clipper"
					)
				},
				{
					ToolKind.Object_AlignLeft,
					new ToolInfo(
						kind: ToolKind.Object_AlignLeft,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 22),
						title: "Align Left"
					)
				},
				{
					ToolKind.Object_AlignHCenter,
					new ToolInfo(
						kind: ToolKind.Object_AlignHCenter,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 23),
						title: "Align to Center (horz)"
					)
				},
				{
					ToolKind.Object_AlignRight,
					new ToolInfo(
						kind: ToolKind.Object_AlignRight,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 24),
						title: "Align Right"
					)
				},
				{
					ToolKind.Object_AlignTop,
					new ToolInfo(
						kind: ToolKind.Object_AlignTop,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 25),
						title: "Align to Top"
					)
				},
				{
					ToolKind.Object_AlignVCenter,
					new ToolInfo(
						kind: ToolKind.Object_AlignVCenter,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 26),
						title: "Align to Center (vert)"
					)
				},
				{
					ToolKind.Object_AlignBottom,
					new ToolInfo(
						kind: ToolKind.Object_AlignBottom,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(3, 27),
						title: "Align to Bottom"
					)
				},

				{
					ToolKind.Node_Select,
					new ToolInfo(
						kind: ToolKind.Node_Select,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(4, 0),
						title: "Select Nodes"
					)
				},
				{
					ToolKind.Node_Eraser,
					new ToolInfo(
						kind: ToolKind.Node_Eraser,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(4, 1),  "Eraser"
					)
				},
				{
					ToolKind.Node_Crop,
					new ToolInfo(
						kind: ToolKind.Node_Crop,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(4, 2),
						title: "Crop Tool"
					)
				},
				{
					ToolKind.Node_Knife,
					new ToolInfo(
						kind: ToolKind.Node_Knife,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(4, 3),
						title: "Knife Tool"
					)
				},
				{
					ToolKind.Node_ContinueCurve,
					new ToolInfo(
						kind: ToolKind.Node_ContinueCurve,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 4),
						title: "Continue Curves"
					)
				},
				{
					ToolKind.Node_MergeNodes,
					new ToolInfo(
						kind: ToolKind.Node_MergeNodes,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 5),
						title: "Merge Nodes"
					)
				},
				{
					ToolKind.Node_DiceCurve,
					new ToolInfo(
						kind: ToolKind.Node_DiceCurve,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 6),
						title: "Dice Curve"
					)
				},
				{
					ToolKind.Node_SliceCurve,
					new ToolInfo(
						kind: ToolKind.Node_SliceCurve,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 7),
						title: "Slice Curve"
					)
				},
				{
					ToolKind.Node_SliceCurveAndTrim,
					new ToolInfo(
						kind: ToolKind.Node_SliceCurveAndTrim,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 8),
						title: "Slice Curve & Trim"
					)
				},
				{
					ToolKind.Node_Cusp,
					new ToolInfo(
						kind: ToolKind.Node_Cusp,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 9),
						title: "Cusp Node"
					)
				},
				{
					ToolKind.Node_Smooth,
					new ToolInfo(
						kind: ToolKind.Node_Smooth,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 10),
						title: "Smooth Node"
					)
				},
				{
					ToolKind.Node_Symmetric,
					new ToolInfo(
						kind: ToolKind.Node_Symmetric,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 11),
						title: "Symmetric Node"
					)
				},
				{
					ToolKind.Node_ToCurve,
					new ToolInfo(
						kind: ToolKind.Node_ToCurve,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 12),
						title: "To Curve"
					)
				},
				{
					ToolKind.Node_ToLine,
					new ToolInfo(
						kind: ToolKind.Node_ToLine,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 13),
						title: "To Line"
					)
				},
				{
					ToolKind.Node_AllCusp,
					new ToolInfo(
						kind: ToolKind.Node_AllCusp,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 14),
						title: "All Cusp Nodes"
					)
				},
				{
					ToolKind.Node_AllSmooth,
					new ToolInfo(
						kind: ToolKind.Node_AllSmooth,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 15),
						title: "All Smooth Nodes"
					)
				},
				{
					ToolKind.Node_AllSymmetric,
					new ToolInfo(
						kind: ToolKind.Node_AllSymmetric,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 16),
						title: "All Symmetric Nodes"
					)
				},
				{
					ToolKind.Node_AllToCurve,
					new ToolInfo(
						kind: ToolKind.Node_AllToCurve,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 17),
						title: "All to Curves"
					)
				},
				{
					ToolKind.Node_AllToLine,
					new ToolInfo(
						kind: ToolKind.Node_AllToLine,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 18),
						title: "All to Lines"
					)
				},
				{
					ToolKind.Node_Combine,
					new ToolInfo(
						kind: ToolKind.Node_Combine,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 19),
						title: "Combine Objects"
					)
				},
				{
					ToolKind.Node_Separate,
					new ToolInfo(
						kind: ToolKind.Node_Combine,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 20),
						title: "Separate Objects"
					)
				},
				{
					ToolKind.Node_ObjectToCurves,
					new ToolInfo(
						kind: ToolKind.Node_ObjectToCurves,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 21),
						title: "Convert Objects to Curves"
					)
				},
				{
					ToolKind.Node_Union,
					new ToolInfo(
						kind: ToolKind.Node_Union,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 22),
						title: "Union"
					)
				},
				{
					ToolKind.Node_Intersect,
					new ToolInfo(
						kind: ToolKind.Node_Intersect,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 23),
						title: "Intersect"
					)
				},
				{
					ToolKind.Node_SubtractFront,
					new ToolInfo(
						kind: ToolKind.Node_SubtractFront,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 24),
						title: "Subtract Front"
					)
				},
				{
					ToolKind.Node_SubtractBack,
					new ToolInfo(
						kind: ToolKind.Node_SubtractBack,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 25),
						title: "Subtract Back"
					)
				},
				{
					ToolKind.Node_Xor,
					new ToolInfo(
						kind: ToolKind.Node_Xor,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 26),
						title: "Xor"
					)
				},
				{
					ToolKind.Node_Boundary,
					new ToolInfo(
						kind: ToolKind.Node_Boundary,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(4, 27),
						title: "Boundary"
					)
				},

				{
					ToolKind.Draw_Freehand,
					new ToolInfo(
						kind: ToolKind.Draw_Freehand,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(5, 0),
						title: "Draw Freehand"
					)
				},
				{
					ToolKind.Draw_Brush,
					new ToolInfo(
						kind: ToolKind.Draw_Brush,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(5, 1),
						title: "Paintbrush"
					)
				},
				{
					ToolKind.Draw_Curve,
					new ToolInfo(
						kind: ToolKind.Draw_Curve,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(5, 2),
						title: "Draw Curves"
					)
				},
				{
					ToolKind.Draw_Rectangle,
					new ToolInfo(
						kind: ToolKind.Draw_Rectangle,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(5, 3),
						title: "Draw Rectangles"
					)
				},
				{
					ToolKind.Draw_Ellipse,
					new ToolInfo(
						kind: ToolKind.Draw_Ellipse,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(5, 4),
						title: "Draw Ellipses"
					)
				},
				{
					ToolKind.Draw_Polygon,
					new ToolInfo(
						kind: ToolKind.Draw_Polygon,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(5, 5),
						title: "Draw Polygons"
					)
				},
				{
					ToolKind.Draw_Star,
					new ToolInfo(
						kind: ToolKind.Draw_Star,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(5, 6),
						title: "Draw Stars"
					)
				},

				{
					ToolKind.Style_Fill,
					new ToolInfo(
						kind: ToolKind.Style_Fill,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(6, 0),
						title: "Fill Tool"
					)
				},
				{
					ToolKind.Style_Stroke,
					new ToolInfo(
						kind: ToolKind.Style_Stroke,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(6, 1),
						title: "Stroke Tool"
					)
				},
				{
					ToolKind.Style_Dropper,
					new ToolInfo(
						kind: ToolKind.Style_Dropper,
						mode: ToolMode.UntilClick,
						iconPosition: new Vector2i(6, 2),
						title: "Eyedropper"
					)
				},
				{
					ToolKind.Style_Transparency,
					new ToolInfo(
						kind: ToolKind.Style_Transparency,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(6, 3),
						title: "Transparency Tool"
					)
				},
				{
					ToolKind.Style_StartArrow,
					new ToolInfo(
						kind: ToolKind.Style_StartArrow,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(6, 4),
						title: "Add Start Arrow"
					)
				},
				{
					ToolKind.Style_EndArrow,
					new ToolInfo(
						kind: ToolKind.Style_EndArrow,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(6, 5),
						title: "Add End Arrow"
					)
				},
				{
					ToolKind.Style_Weight0,
					new ToolInfo(
						kind: ToolKind.Style_Weight0,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(6, 6),
						title: "Stroke Weight 0"
					)
				},
				{
					ToolKind.Style_Weight1,
					new ToolInfo(
						kind: ToolKind.Style_Weight1,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(6, 7),
						title: "Stroke Weight 1"
					)
				},
				{
					ToolKind.Style_Weight2,
					new ToolInfo(
						kind: ToolKind.Style_Weight2,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(6, 8),
						title: "Stroke Weight 2"
					)
				},
				{
					ToolKind.Style_Weight4,
					new ToolInfo(
						kind: ToolKind.Style_Weight4,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(6, 9),
						title: "Stroke Weight 4"
					)
				},
				{
					ToolKind.Style_Weight8,
					new ToolInfo(
						kind: ToolKind.Style_Weight8,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(6, 10),
						title: "Stroke Weight 8"
					)
				},
				{
					ToolKind.Style_Weight16,
					new ToolInfo(
						kind: ToolKind.Style_Weight16,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(6, 11),
						title: "Stroke Weight 16"
					)
				},

				{
					ToolKind.Text_Tool,
					new ToolInfo(
						kind: ToolKind.Text_Tool,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(7, 0),
						title: "Text Tool"
					)
				},
				{
					ToolKind.Text_Paragraph,
					new ToolInfo(
						kind: ToolKind.Text_Paragraph,
						mode: ToolMode.Normal,
						iconPosition: new Vector2i(7, 1),
						title: "Paragraph Tool"
					)
				},
				{
					ToolKind.Text_LeftAlign,
					new ToolInfo(
						kind: ToolKind.Text_LeftAlign,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 2),
						title: "Align Left"
					)
				},
				{
					ToolKind.Text_RightAlign,
					new ToolInfo(
						kind: ToolKind.Text_RightAlign,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 3),
						title: "Align Right"
					)
				},
				{
					ToolKind.Text_Center,
					new ToolInfo(
						kind: ToolKind.Text_Center,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 4),
						title: "Align Center"
					)
				},
				{
					ToolKind.Text_Justify,
					new ToolInfo(
						kind: ToolKind.Text_Justify,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 5),
						title: "Justify"
					)
				},
				{
					ToolKind.Text_JustifyAll,
					new ToolInfo(
						kind: ToolKind.Text_JustifyAll,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 6),
						title: "Justify All"
					)
				},
				{
					ToolKind.Text_Bold,
					new ToolInfo(
						kind: ToolKind.Text_Bold,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 7),
						title: "Bold"
					)
				},
				{
					ToolKind.Text_Italic,
					new ToolInfo(
						kind: ToolKind.Text_Italic,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 8),
						title: "Italic"
					)
				},
				{
					ToolKind.Text_Underline,
					new ToolInfo(
						kind: ToolKind.Text_Underline,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 9),
						title: "Underline"
					)
				},
				{
					ToolKind.Text_Strike,
					new ToolInfo(
						kind: ToolKind.Text_Strike,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 10),
						title: "Strikeout"
					)
				},
				{
					ToolKind.Text_Superscript,
					new ToolInfo(
						kind: ToolKind.Text_Superscript,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 11),
						title: "Superscript"
					)
				},
				{
					ToolKind.Text_Subscript,
					new ToolInfo(
						kind: ToolKind.Text_Subscript,
						mode: ToolMode.OneShot,
						iconPosition: new Vector2i(7, 12),
						title: "Subscript"
					)
				},
			};
	}
}
