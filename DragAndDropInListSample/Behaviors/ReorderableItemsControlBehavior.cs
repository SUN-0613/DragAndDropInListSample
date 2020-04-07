using DragAndDropInListSample.Behaviors.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DragAndDropInListSample.Behaviors
{

    /// <summary>
    /// Drag & Drop操作でListを並び替え
    /// 添付ビヘイビア
    /// </summary>
    /// <remarks>
    /// http://yujiro15.net/blog/index.php?id=151
    /// </remarks>
    public class ReorderableItemsControlBehavior
    {

        #region Callback

        /// <summary>Callback添付プロパティ</summary>
        public static readonly DependencyProperty CallbackProperty
                                                    = DependencyProperty.RegisterAttached(
                                                        "Callback",
                                                        typeof(Action<int>),
                                                        typeof(ReorderableItemsControlBehavior),
                                                        new PropertyMetadata(null, OnCallbackPropertyChanged));

        /// <summary>Callback添付プロパティの値取得</summary>
        /// <param name="target">対象Control</param>
        /// <returns>取得したCallback関数</returns>
        public static Action<int> GetCallback(DependencyObject target)
        {
            return (Action<int>)target.GetValue(CallbackProperty);
        }

        /// <summary>Callback添付プロパティの値設定</summary>
        /// <param name="target">対象Control</param>
        /// <param name="callback">コールバック関数</param>
        public static void SetCallback(DependencyObject target, Action<int> callback)
        {
            target.SetValue(CallbackProperty, callback);
        }

        /// <summary>Callback添付プロパティの値更新イベント</summary>
        /// <param name="dependencyObject">対象Control</param>
        /// <param name="e">値更新イベントデータ</param>
        private static void OnCallbackPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {

            if (dependencyObject is ItemsControl itemsControl)
            {

                if (GetCallback(itemsControl) != null)
                {

                    itemsControl.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                    itemsControl.PreviewMouseMove += OnPreviewMouseMove;
                    itemsControl.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
                    itemsControl.PreviewDragEnter += OnPreviewDragEnter;
                    itemsControl.PreviewDragLeave += OnPreviewDragLeave;
                    itemsControl.PreviewDrop += OnPreviewDrop;

                }
                else
                {

                    itemsControl.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
                    itemsControl.PreviewMouseMove -= OnPreviewMouseMove;
                    itemsControl.PreviewMouseLeftButtonUp -= OnPreviewMouseLeftButtonUp;
                    itemsControl.PreviewDragEnter -= OnPreviewDragEnter;
                    itemsControl.PreviewDragLeave -= OnPreviewDragLeave;
                    itemsControl.PreviewDrop -= OnPreviewDrop;

                }

            }

        }

        #endregion

        #region 対象Control向け実行イベント

        /// <summary>Drag中のデータ</summary>
        private static DragAndDropObject _TemporaryData;

        /// <summary>マウス左ボタン押下</summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is FrameworkElement control
                && e.OriginalSource is FrameworkElement source)
            {

                _TemporaryData = new DragAndDropObject()
                {
                    StartPoint = e.GetPosition(Window.GetWindow(control)),
                    DraggedItem = GetTemplatedRootElement(source)
                };

            }

        }

        /// <summary>指定したFrameworlElementのルート要素を取得</summary>
        /// <param name="element">指定するFrameworkElement</param>
        /// <returns>ルート要素</returns>
        private static FrameworkElement GetTemplatedRootElement(FrameworkElement element)
        {

            while (element.TemplatedParent != null)
            {
                element = element.TemplatedParent as FrameworkElement;
            }

            return element;

        }

        /// <summary>対象Control上でマウスカーソル移動</summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">マウスイベントデータ</param>
        private static void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (_TemporaryData != null
                && sender is FrameworkElement control
                && _TemporaryData.CheckStartDragging(e.GetPosition(Window.GetWindow(control))))
            {

                DragDrop.DoDragDrop(control, _TemporaryData.DraggedItem, DragDropEffects.Move);
                _TemporaryData = null;

            }

        }

        /// <summary>マウス左ボタン離上</summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private static void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (_TemporaryData != null)
            {
                _TemporaryData = null;
            }

        }

        /// <summary>Drag開始</summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">ドラッグイベントデータ</param>
        private static void OnPreviewDragEnter(object sender, DragEventArgs e)
        {

            if (_TemporaryData != null)
            {
                _TemporaryData.IsDroppable = true;
            }
            
        }

        /// <summary>Drag終了</summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">ドラッグイベントデータ</param>
        private static void OnPreviewDragLeave(object sender, DragEventArgs e)
        {

            if (_TemporaryData != null)
            {
                _TemporaryData.IsDroppable = false;
            }

        }

        /// <summary>Drop処理</summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">ドラッグイベントデータ</param>
        private static void OnPreviewDrop(object sender, DragEventArgs e)
        {

            if ((_TemporaryData?.IsDroppable ?? false)
                && sender is ItemsControl itemsControl
                && itemsControl.ItemContainerGenerator.IndexFromContainer(_TemporaryData.DraggedItem) >= 0
                && e.OriginalSource is FrameworkElement frameworkElement)
            {

                var targetContainer = GetTemplatedRootElement(frameworkElement);
                var index = itemsControl.ItemContainerGenerator.IndexFromContainer(targetContainer);

                if (index > -1)
                {
                    var callback = GetCallback(itemsControl);
                    callback(index);
                }

            }

        }

        #endregion

    }

}
