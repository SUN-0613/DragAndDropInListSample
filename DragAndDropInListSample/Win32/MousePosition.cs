using System.Runtime.InteropServices;
using System.Windows;

namespace DragAndDropInListSample.Win32
{

    /// <summary>
    /// マウスカーソル位置
    /// </summary>
    public static class MousePosition
    {

        /// <summary>マウスカーソル位置座標</summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {

            /// <summary>
            /// 水平方向座標
            /// </summary>
            public int X;

            /// <summary>
            /// 垂直方向座標
            /// </summary>
            public int Y;

        }

        /// <summary>マウスカーソル位置取得</summary>
        /// <param name="pt">取得したカーソル位置座標</param>
        /// <returns>
        /// true :取得成功
        /// false:取得失敗
        /// </returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Win32Point pt);

        /// <summary>
        /// マウスカーソル位置取得
        /// </summary>
        /// <returns></returns>
        public static Point Get()
        {

            var w32Point = new Win32Point();
            GetCursorPos(ref w32Point);

            return new Point(w32Point.X, w32Point.Y);

        }

    }

}
