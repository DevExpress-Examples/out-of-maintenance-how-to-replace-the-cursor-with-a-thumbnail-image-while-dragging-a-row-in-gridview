Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraGrid.Views.Grid.Drawing

Namespace SwapRows
	Public Class DragHelper
		Private downHitInfo As GridHitInfo = Nothing
		Private imageHelper As DragImageHelper
		Private _View As GridView
		Public Sub New(ByVal view As GridView)
			_View = view
			SubscribeEvents(view)
			imageHelper = New DragImageHelper(view)
		End Sub

		Private Sub SubscribeEvents(ByVal view As GridView)
			AddHandler view.MouseDown, AddressOf view_MouseDown
			AddHandler view.MouseMove, AddressOf view_MouseMove
			AddHandler view.GridControl.DragOver, AddressOf GridControl_DragOver
			AddHandler view.MouseUp, AddressOf view_MouseUp
			AddHandler view.GridControl.GiveFeedback, AddressOf GridControl_GiveFeedback
			AddHandler view.GridControl.Paint, AddressOf GridControl_Paint
		End Sub

		Private Sub GridControl_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)

			If downHitInfo Is Nothing Then
				Return
			End If
			Dim grid As GridControl = CType(sender, GridControl)
			Dim view As GridView = CType(grid.MainView, GridView)

			Dim isBottomLine As Boolean = DropTargetRowHandle = view.DataRowCount

			Dim viewInfo As GridViewInfo = TryCast(view.GetViewInfo(), GridViewInfo)
			Dim rowInfo As GridRowInfo
			If isBottomLine Then
				rowInfo = viewInfo.GetGridRowInfo(DropTargetRowHandle - 1)
			Else
				rowInfo = viewInfo.GetGridRowInfo(DropTargetRowHandle)
			End If

			If rowInfo Is Nothing Then
				Return
			End If

			Dim p1, p2 As Point
			If isBottomLine Then
				p1 = New Point(rowInfo.Bounds.Left, rowInfo.Bounds.Bottom - 1)
				p2 = New Point(rowInfo.Bounds.Right, rowInfo.Bounds.Bottom - 1)
			Else
				p1 = New Point(rowInfo.Bounds.Left, rowInfo.Bounds.Top - 1)
				p2 = New Point(rowInfo.Bounds.Right, rowInfo.Bounds.Top - 1)
			End If

			Dim pen As New Pen(Color.Blue, 3)
			e.Graphics.DrawLine(pen, p1, p2)
		End Sub

		Private Sub GridControl_GiveFeedback(ByVal sender As Object, ByVal e As GiveFeedbackEventArgs)
			If downHitInfo IsNot Nothing Then
				e.UseDefaultCursors = False
				Cursor.Current =_dragRowCursor
			End If
		End Sub

		Private Sub view_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
			downHitInfo = Nothing
		End Sub

		Private dropTargetRowHandle_Renamed As Integer
		Private Property DropTargetRowHandle() As Integer
			Get
				Return dropTargetRowHandle_Renamed
			End Get
			Set(ByVal value As Integer)
				dropTargetRowHandle_Renamed = value
				_View.Invalidate()
			End Set
		End Property
		Private Sub GridControl_DragOver(ByVal sender As Object, ByVal e As DragEventArgs)
			Dim grid As GridControl = CType(sender, GridControl)

			Dim pt As New Point(e.X, e.Y)
			pt = grid.PointToClient(pt)
			Dim view As GridView = TryCast(grid.GetViewAt(pt), GridView)
			If view Is Nothing Then
				Return
			End If

			Dim hitInfo As GridHitInfo = view.CalcHitInfo(pt)
			If hitInfo.HitTest = GridHitTest.EmptyRow Then
				DropTargetRowHandle = view.DataRowCount
			Else
				DropTargetRowHandle = hitInfo.RowHandle
			End If

			If DropTargetRowHandle >= 0 AndAlso e.Data.GetDataPresent(GetType(String)) Then
				e.Effect = DragDropEffects.Move
			Else
				e.Effect = DragDropEffects.None
			End If

		End Sub
		Private _dragRowCursor As Cursor
		Private Sub view_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim view As GridView = TryCast(sender, GridView)
			If e.Button = MouseButtons.Left AndAlso downHitInfo IsNot Nothing Then
				Dim dragSize As Size = SystemInformation.DragSize
				Dim dragRect As New Rectangle(New Point(downHitInfo.HitPoint.X - dragSize.Width / 2, downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize)

				If (Not dragRect.Contains(New Point(e.X, e.Y))) Then
					_dragRowCursor = imageHelper.GetDragCursor(downHitInfo.RowHandle, e.Location)
					view.GridControl.DoDragDrop(downHitInfo, DragDropEffects.All)
					downHitInfo = Nothing
				End If
			End If
		End Sub



		Private Sub view_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
			Dim view As GridView = TryCast(sender, GridView)
			downHitInfo = Nothing

			Dim hitInfo As GridHitInfo = view.CalcHitInfo(New Point(e.X, e.Y))
			If Control.ModifierKeys <> Keys.None Then
				Return
			End If
			If e.Button = MouseButtons.Left AndAlso hitInfo.InRow AndAlso hitInfo.RowHandle <> GridControl.NewItemRowHandle Then
				downHitInfo = hitInfo
			End If
		End Sub
	End Class
End Namespace