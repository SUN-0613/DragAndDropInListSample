using System;
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
        Point _MousePoint;

        /// <summary>
        /// 一覧表示Sample.View
        /// </summary>
        public ListSample()
        {

            InitializeComponent();

        }

        /// <summary>
        /// ListBox.マウス左ボタン押下
        /// </summary>
        /// <param name="sender">ListBox</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            _MousePoint = e.GetPosition(this);

        }

        /// <summary>
        /// Popup.Opend
        /// </summary>
        /// <param name="sender">Popup</param>
        /// <param name="e">イベントデータ</param>
        private void Popup_Opened(object sender, EventArgs e)
        {

            if (sender is Popup popup)
            {

                popup.HorizontalOffset = _MousePoint.X - 10d;
                popup.VerticalOffset = _MousePoint.Y - 10d;

            }

        }

    }

}
