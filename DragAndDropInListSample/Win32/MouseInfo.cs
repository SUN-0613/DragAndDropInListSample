using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace DragAndDropInListSample.Win32
{

    /// <summary>Win32APIを使用したマウスカーソル位置情報</summary>
    /// <remarks>
    /// http://main.tinyjoker.net/Tech/CSharp/WPF/ListBox%A4%CE%A5%A2%A5%A4%A5%C6%A5%E0%A4%F2%C8%BE%C6%A9%CC%C0%A5%B4%A1%BC%A5%B9%A5%C8%A4%C4%A4%AD%A5%C9%A5%E9%A5%C3%A5%B0%A5%A2%A5%F3%A5%C9%A5%C9%A5%ED%A5%C3%A5%D7%A4%C7%CA%C2%A4%D9%C2%D8%A4%A8%A4%EB.html#subclass
    /// </remarks>
    public class MouseInfo
    {

        #region const

        /// <summary>user32.dllファイル名</summary>
        private const string _User32DllFileName = "user32.dll";

        #endregion

        #region struct

        /// <summary>座標</summary>
        private struct Win32Point
        {

            /// <summary>水平方向</summary>
            public uint X;

            /// <summary>垂直方向</summary>
            public uint Y;

        }

        #endregion

        #region Import DLL

        /// <summary>マウスカーソル位置座標取得</summary>
        /// <param name="point">マウスカーソル位置座標</param>
        [DllImport(_User32DllFileName)]
        private static extern void GetCursorPos(out Win32Point point);

        /// <summary>スクリーン上の点座標を指定したウィンドウを始点としたクライアント座標に変換</summary>
        /// <param name="hwnd">指定するウィンドウのハンドル</param>
        /// <param name="point">指定するウィンドウのスクリーン上の点座標</param>
        /// <returns>ウィンドウを始点としたクライアント座標</returns>
        [DllImport(_User32DllFileName)]
        private static extern int ScreenToClient(IntPtr handle, ref Win32Point point);

        #endregion

        /// <summary>指定したクライアントを始点としたマウスカーソル位置座標を取得</summary>
        /// <param name="client">指定するクライアント</param>
        /// <returns>マウスカーソル位置座標</returns>
        public static Point GetClientPosition(Visual client)
        {

            GetCursorPos(out var win32Point);

            if (HwndSource.FromVisual(client) is HwndSource source)
            {

                ScreenToClient(source.Handle, ref win32Point);

                return new Point(win32Point.X, win32Point.Y);

            }

            return new Point(-1, -1);

        }

        /// <summary>スクリーン上のマウスカーソル位置座標を取得</summary>
        /// <returns>マウスカーソル位置座標</returns>
        public static Point GetScreenPosition()
        {

            GetCursorPos(out var win32Point);

            return new Point(win32Point.X, win32Point.Y);

        }

    }

}
