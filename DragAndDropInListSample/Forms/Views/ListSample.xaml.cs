using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DragAndDropInListSample.Forms.Views
{
    /// <summary>
    /// ListSample.xaml の相互作用ロジック
    /// </summary>
    public partial class ListSample : Window
    {

        /// <summary>
        /// マウスポインタの座標
        /// </summary>
        private Point _MousePoint;

        /// <summary>
        /// 座標初期値
        /// </summary>
        private readonly Point _InitializePoint = new Point(-1, -1);

        /// <summary>
        /// 一覧表示Sample.View
        /// </summary>
        public ListSample()
        {

            InitializeComponent();

        }

        /// <summary>
        /// ListBoxでデータ選択
        /// </summary>
        /// <param name="sender">ListBox</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private void ListBox_PreviewMouseLeftButtonEvent(object sender, MouseButtonEventArgs e)
        {

            _MousePoint = e.GetPosition(this);

        }

        /// <summary>
        /// Popup上でマウス左ボタンを操作
        /// </summary>
        /// <param name="sender">Popup</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private void Popup_MouseLeftButtonEvent(object sender, MouseButtonEventArgs e)
        {

            //_MousePoint = e.GetPosition(this);
            //Debug.WriteLine("Set Position");

        }

        /// <summary>
        /// Popup上でマウスを移動
        /// </summary>
        /// <param name="sender">Popup</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private void Popup_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {

                if (_MousePoint.Equals(_InitializePoint))
                {
                    _MousePoint = e.GetPosition(this);
                    Debug.WriteLine("No data : Set Position");
                }

                var point = _MousePoint - e.GetPosition(this);

                if (sender is Popup popup)
                {

                    popup.HorizontalOffset = -10d - point.X;
                    popup.VerticalOffset = -10d - point.Y;

                }

            }
            else
            {
                _MousePoint = _InitializePoint;
                Debug.WriteLine("Initialize");
            }

        }

    }

}
