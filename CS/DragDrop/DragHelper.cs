using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.Drawing;

namespace SwapRows
{
    public class DragHelper
    {
        GridHitInfo downHitInfo = null;
        DragImageHelper imageHelper;
        GridView _View;
        public DragHelper(GridView view)
        {
            _View = view;
            SubscribeEvents(view);
            imageHelper = new DragImageHelper(view);
        }

        private void SubscribeEvents(GridView view)
        {
            view.MouseDown += new MouseEventHandler(view_MouseDown);
            view.MouseMove += new MouseEventHandler(view_MouseMove);
            view.GridControl.DragOver += new DragEventHandler(GridControl_DragOver);
            view.MouseUp += new MouseEventHandler(view_MouseUp);
            view.GridControl.GiveFeedback += new GiveFeedbackEventHandler(GridControl_GiveFeedback);
            view.GridControl.Paint += new PaintEventHandler(GridControl_Paint);
        }

        void GridControl_Paint(object sender, PaintEventArgs e)
        {

            if (downHitInfo == null)
                return;
            GridControl grid = (GridControl)sender;
            GridView view = (GridView)grid.MainView;

            bool isBottomLine = DropTargetRowHandle == view.DataRowCount;

            GridViewInfo viewInfo = view.GetViewInfo() as GridViewInfo;
            GridRowInfo rowInfo = viewInfo.GetGridRowInfo(isBottomLine ? DropTargetRowHandle - 1 : DropTargetRowHandle);

            if (rowInfo == null) return;

            Point p1, p2;
            if (isBottomLine)
            {
                p1 = new Point(rowInfo.Bounds.Left, rowInfo.Bounds.Bottom - 1);
                p2 = new Point(rowInfo.Bounds.Right, rowInfo.Bounds.Bottom - 1);
            }
            else
            {
                p1 = new Point(rowInfo.Bounds.Left, rowInfo.Bounds.Top - 1);
                p2 = new Point(rowInfo.Bounds.Right, rowInfo.Bounds.Top - 1);
            }

            Pen pen = new Pen(Color.Blue, 3);
            e.Graphics.DrawLine(pen, p1, p2);
        }

        void GridControl_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (downHitInfo != null)
            {
                e.UseDefaultCursors = false;
                Cursor.Current =_dragRowCursor;
            }
        }

        void view_MouseUp(object sender, MouseEventArgs e)
        {
            downHitInfo = null;
        }

        int dropTargetRowHandle;
        int DropTargetRowHandle
        {
            get { return dropTargetRowHandle; }
            set
            {
                dropTargetRowHandle = value;
                _View.Invalidate();
            }
        }
        void GridControl_DragOver(object sender, DragEventArgs e)
        {
            GridControl grid = (GridControl)sender;

            Point pt = new Point(e.X, e.Y);
            pt = grid.PointToClient(pt);
            GridView view = grid.GetViewAt(pt) as GridView;
            if (view == null) return;

            GridHitInfo hitInfo = view.CalcHitInfo(pt);
            if (hitInfo.HitTest == GridHitTest.EmptyRow)
                DropTargetRowHandle = view.DataRowCount;
            else
                DropTargetRowHandle = hitInfo.RowHandle;

            if (DropTargetRowHandle >= 0 && e.Data.GetDataPresent(typeof(string)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;

        }
        Cursor _dragRowCursor;
        void view_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.Button == MouseButtons.Left && downHitInfo != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    _dragRowCursor = imageHelper.GetDragCursor(downHitInfo.RowHandle, e.Location);
                    view.GridControl.DoDragDrop(downHitInfo, DragDropEffects.All);
                    downHitInfo = null;
                }
            }
        }



        void view_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            downHitInfo = null;

            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));
            if (Control.ModifierKeys != Keys.None)
                return;
            if (e.Button == MouseButtons.Left && hitInfo.InRow && hitInfo.RowHandle != GridControl.NewItemRowHandle)
                downHitInfo = hitInfo;
        }
    }
}