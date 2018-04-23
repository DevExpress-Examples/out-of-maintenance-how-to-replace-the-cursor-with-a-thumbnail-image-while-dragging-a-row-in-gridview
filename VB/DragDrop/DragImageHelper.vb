Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.XtraGrid.Views.Grid.Drawing
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging

Namespace SwapRows
	Public Class DragImageHelper
		Inherits GridPainter
		Private ReadOnly _View As DevExpress.XtraGrid.Views.Grid.GridView
		Public Sub New(ByVal view As DevExpress.XtraGrid.Views.Grid.GridView)
			MyBase.New(view)
			_View = view
		End Sub

		Public Function GetDragCursor(ByVal rowHandle As Integer, ByVal e As Point) As Cursor
			Dim info As GridViewInfo = TryCast(_View.GetViewInfo(), GridViewInfo)
			Dim rowInfo As GridRowInfo = info.GetGridRowInfo(rowHandle)
			Dim imageBounds As New Rectangle(New Point(0, 0), rowInfo.TotalBounds.Size)
			Dim totalBounds As New Rectangle(New Point(0, 0), info.Bounds.Size)
			Dim bitmap As New Bitmap(totalBounds.Width, totalBounds.Height)
			Dim cache As New DevExpress.Utils.Drawing.GraphicsCache(Graphics.FromImage(bitmap))
			Dim args As New GridViewDrawArgs(cache, info, totalBounds)
			DrawRow(args, rowInfo)
			Dim result As New Bitmap(imageBounds.Width, imageBounds.Height)
			Dim resultGraphics As Graphics = Graphics.FromImage(result)
			Dim matrixItems()() As Single ={ New Single() {1, 0, 0, 0, 0}, New Single() {0, 1, 0, 0, 0}, New Single() {0, 0, 1, 0, 0}, New Single() {0, 0, 0, 0.7f, 0}, New Single() {0, 0, 0, 0, 1}}
			Dim colorMatrix As New ColorMatrix(matrixItems)
			Dim imageAttributes As New ImageAttributes()
			imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)
			resultGraphics.DrawImage(bitmap, imageBounds, rowInfo.TotalBounds.X, rowInfo.TotalBounds.Y, rowInfo.TotalBounds.Width, rowInfo.TotalBounds.Height, GraphicsUnit.Pixel, imageAttributes)
			Dim offset As New Point(e.X - rowInfo.TotalBounds.X, e.Y - rowInfo.TotalBounds.Y)
			Return CreateCursor(result, offset)
		End Function

		<DllImport("user32.dll")> _
		Public Shared Function GetIconInfo(ByVal hIcon As IntPtr, ByRef pIconInfo As IconInfo) As <MarshalAs(UnmanagedType.Bool)> Boolean
		End Function
		<DllImport("user32.dll")> _
		Public Shared Function CreateIconIndirect(ByRef icon As IconInfo) As IntPtr
		End Function

		Public Structure IconInfo
			Public fIcon As Boolean
			Public xHotspot As Integer
			Public yHotspot As Integer
			Public hbmMask As IntPtr
			Public hbmColor As IntPtr
		End Structure

		Public Shared Function CreateCursor(ByVal bmp As Bitmap, ByVal hotspot As Point) As Cursor
			If bmp Is Nothing Then
				Return Cursors.Default
			End If
			Dim ptr As IntPtr = bmp.GetHicon()
			Dim tmp As New IconInfo()
			GetIconInfo(ptr, tmp)
			tmp.fIcon = False
			tmp.xHotspot = hotspot.X
			tmp.yHotspot = hotspot.Y
			ptr = CreateIconIndirect(tmp)
			Return New Cursor(ptr)
		End Function


	End Class
End Namespace
