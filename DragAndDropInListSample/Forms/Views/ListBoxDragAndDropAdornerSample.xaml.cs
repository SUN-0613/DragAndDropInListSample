using DragAndDropInListSample.Win32;
using DragAndDropInListSample.Windows;
using DragAndDropInListSample.Windows.Documents;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace DragAndDropInListSample.Forms.Views
{
    /// <summary>
    /// ListBoxDragAndDropAdornerSample.xaml の相互作用ロジック
    /// </summary>
    public partial class ListBoxDragAndDropAdornerSample : Window
    {

        /// <summary>
        /// 選択中のListBoxItem
        /// </summary>
        private ListBoxItem _SelectedListBoxItem;

        /// <summary>
        /// ドラッグ操作開始位置
        /// </summary>
        private Point _DragStartPosition;

        /// <summary>
        /// ListBoxItem.Ghost
        /// </summary>
        private GhostListBoxAdorner _Ghost;

        /// <summary>
        /// ListBoxでListBoxItemのDrag&Drop中ゴーストを表示するサンプル.View
        /// </summary>
        /// <remarks>
        /// http://main.tinyjoker.net/Tech/CSharp/WPF/ListBox%A4%CE%A5%A2%A5%A4%A5%C6%A5%E0%A4%F2%C8%BE%C6%A9%CC%C0%A5%B4%A1%BC%A5%B9%A5%C8%A4%C4%A4%AD%A5%C9%A5%E9%A5%C3%A5%B0%A5%A2%A5%F3%A5%C9%A5%C9%A5%ED%A5%C3%A5%D7%A4%C7%CA%C2%A4%D9%C2%D8%A4%A8%A4%EB.html#subclass
        /// </remarks>
        public ListBoxDragAndDropAdornerSample()
        {

            InitializeComponent();

        }

        /// <summary>マウス左ボタン押下イベント</summary>
        /// <param name="sender">ListBoxItem</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListBoxItem listBoxItem)
            {

                // ListBoxItem、マウス座標を取得
                _SelectedListBoxItem = listBoxItem;
                _DragStartPosition = e.GetPosition(listBoxItem);

            }

        }

        /// <summary>マウス操作イベント</summary>
        /// <param name="sender">ListBoxItem</param>
        /// <param name="e">マウスイベントデータ</param>
        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (sender is ListBoxItem listBoxItem
                && listBoxItem == _SelectedListBoxItem
                && e.LeftButton == MouseButtonState.Pressed
                && _Ghost == null)
            {

                var position = e.GetPosition(listBoxItem);
                
                if (Math.Abs(position.X - _DragStartPosition.X) > SystemParameters.MinimumHorizontalDragDistance
                    || Math.Abs(position.Y - _DragStartPosition.Y) > SystemParameters.MinimumVerticalDragDistance)
                {

                    // ListBoxItemを持つListBoxを取得
                    var listBox = FrameworkElementInfo.FindAncestor<ListBox>(listBoxItem);

                    if (listBox != null)
                    {

                        listBox.AllowDrop = true;

                        // ゴースト生成
                        _Ghost = new GhostListBoxAdorner(listBox, listBoxItem, 1.0, _DragStartPosition);

                        // レイヤにゴースト追加
                        var layer = AdornerLayer.GetAdornerLayer(listBox);
                        layer.Add(_Ghost);

                        // Drag&Dropを開始してゴーストを表示
                        // ここからドラッグ操作が終了するまでOnQueryContinueDrag()が実行される
                        DragDrop.DoDragDrop(listBoxItem, listBoxItem, DragDropEffects.Move);

                        // レイヤからゴーストを削除して初期化
                        layer.Remove(_Ghost);

                        _Ghost = null;
                        _SelectedListBoxItem = null;

                        listBox.AllowDrop = false;

                    }

                }

            }

        }

        /// <summary>ドラッグ操作継続イベント</summary>
        /// <param name="sender">ListBoxItem</param>
        /// <param name="e">ドラッグ操作継続イベントデータ</param>
        private void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

            if (sender is ListBoxItem listBoxItem
                && _Ghost != null)
            {

                _Ghost.Offset = MouseInfo.GetClientPosition(this);

            }

        }

    }

}
