using System;
using DevExpress.XtraGrid.Views.Grid.Drawing;
using DevExpress.XtraGrid.Views.Grid;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace SwapRows
{
    public class DragImageHelper : GridPainter
    {
        private readonly DevExpress.XtraGrid.Views.Grid.GridView _View;
        public DragImageHelper(DevExpress.XtraGrid.Views.Grid.GridView view)
            : base(view)
        {
            _View = view;
        }

        public Cursor GetDragCursor(int rowHandle, Point e)
        {
            GridViewInfo info = _View.GetViewInfo() as GridViewInfo;
            GridRowInfo rowInfo = info.GetGridRowInfo(rowHandle);
            Rectangle imageBounds = new Rectangle(new Point(0, 0), rowInfo.TotalBounds.Size);
            Rectangle totalBounds = new Rectangle(new Point(0, 0), info.Bounds.Size);
            Bitmap bitmap = new Bitmap(totalBounds.Width, totalBounds.Height);
            DevExpress.Utils.Drawing.GraphicsCache cache = new DevExpress.Utils.Drawing.GraphicsCache(Graphics.FromImage(bitmap));
            GridViewDrawArgs args = new GridViewDrawArgs(cache, info, totalBounds);
            DrawRow(args, rowInfo);
            Bitmap result = new Bitmap(imageBounds.Width, imageBounds.Height);
            Graphics resultGraphics = Graphics.FromImage(result);
            float[][] matrixItems ={ 
                               new float[] {1, 0, 0, 0, 0},
                               new float[] {0, 1, 0, 0, 0},
                               new float[] {0, 0, 1, 0, 0},
                               new float[] {0, 0, 0, 0.7f, 0}, 
                               new float[] {0, 0, 0, 0, 1}};
            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            resultGraphics.DrawImage(bitmap, imageBounds, rowInfo.TotalBounds.X, rowInfo.TotalBounds.Y, rowInfo.TotalBounds.Width, rowInfo.TotalBounds.Height, GraphicsUnit.Pixel, imageAttributes);
            Point offset = new Point(e.X - rowInfo.TotalBounds.X, e.Y - rowInfo.TotalBounds.Y);
            return CreateCursor(result, offset);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        public static Cursor CreateCursor(Bitmap bmp, Point hotspot)
        {
            if (bmp == null) return Cursors.Default;
            IntPtr ptr = bmp.GetHicon();
            IconInfo tmp = new IconInfo();
            GetIconInfo(ptr, ref tmp);
            tmp.fIcon = false;
            tmp.xHotspot = hotspot.X;
            tmp.yHotspot = hotspot.Y;
            ptr = CreateIconIndirect(ref tmp);
            return new Cursor(ptr);
        }


    }
}
