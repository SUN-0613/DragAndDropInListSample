using DragAndDropInListSample.Win32;
using DragAndDropInListSample.Windows;
using DragAndDropInListSample.Windows.Documents;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DragAndDropInListSample.Behaviors
{

    /// <summary>
    /// ListBoxItem選択中にゴースト表示
    /// 添付ビヘイビア
    /// </summary>
    /// <remarks>
    /// http://main.tinyjoker.net/Tech/CSharp/WPF/ListBox%A4%CE%A5%A2%A5%A4%A5%C6%A5%E0%A4%F2%C8%BE%C6%A9%CC%C0%A5%B4%A1%BC%A5%B9%A5%C8%A4%C4%A4%AD%A5%C9%A5%E9%A5%C3%A5%B0%A5%A2%A5%F3%A5%C9%A5%C9%A5%ED%A5%C3%A5%D7%A4%C7%CA%C2%A4%D9%C2%D8%A4%A8%A4%EB.html#subclass
    /// </remarks>
    public class ListBoxItemBehavior
    {

        #region IsAttach

        /// <summary>添付ビヘイビアをアタッチしているか</summary>
        public static readonly DependencyProperty IsAttachProperty
            = DependencyProperty.RegisterAttached(
                "IsAttach",
                typeof(bool),
                typeof(ListBoxItemBehavior),
                new PropertyMetadata(false, OnIsAttachPropertyChanged));

        /// <summary>添付ビヘイビアをアタッチしているか値を取得</summary>
        /// <param name="target">ListBoxItem</param>
        /// <returns>
        /// true :アタッチしている
        /// false:アタッチしていない
        /// </returns>
        public static bool GetIsAttach(DependencyObject target)
        {
            return (bool)target.GetValue(IsAttachProperty);
        }

        /// <summary>添付ビヘイビアをアタッチするか値を設定</summary>
        /// <param name="target">ListBoxItem</param>
        /// <param name="isAttach">
        /// true :アタッチしている
        /// false:アタッチしていない
        /// </param>
        public static void SetCallback(DependencyObject target, bool isAttach)
        {
            target.SetValue(IsAttachProperty, isAttach);
        }

        /// <summary>IsAttach添付プロパティの値更新イベント</summary>
        /// <param name="dependencyObject">ListBoxItem</param>
        /// <param name="e">値更新イベントデータ</param>
        private static void OnIsAttachPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {

            if (dependencyObject is ListBoxItem listBoxItem)
            {

                if (GetIsAttach(listBoxItem))
                {

                    listBoxItem.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                    listBoxItem.PreviewMouseMove += OnPreviewMouseMove;
                    listBoxItem.QueryContinueDrag += OnQueryContinueDrag;

                }
                else
                {

                    listBoxItem.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                    listBoxItem.PreviewMouseMove -= OnPreviewMouseMove;
                    listBoxItem.QueryContinueDrag -= OnQueryContinueDrag;

                }

            }

        }

        #endregion

        #region Private

        /// <summary>
        /// 選択中のListBoxItem
        /// </summary>
        private static ListBoxItem _SelectedListBoxItem = null;

        /// <summary>
        /// ドラッグ操作開始位置
        /// </summary>
        private static Point _DragStartPosition;

        /// <summary>
        /// ListBoxItem.Ghost
        /// </summary>
        private static GhostListBoxAdorner _Ghost;

        #endregion

        /// <summary>マウス左ボタン押下イベント</summary>
        /// <param name="sender">ListBoxItem</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
        private static void OnPreviewMouseMove(object sender, MouseEventArgs e)
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
        private static void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

            if (sender is ListBoxItem listBoxItem
                && _Ghost != null)
            {

                var window = FrameworkElementInfo.FindAncestor<Window>(listBoxItem);

                _Ghost.Offset = MouseInfo.GetClientPosition(window);

            }

        }

    }

}
