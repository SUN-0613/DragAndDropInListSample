using DragAndDropInListSample.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DragAndDropInListSample.Behaviors
{

    /// <summary>
    /// ドラッグ操作でゴーストを表示
    /// 添付ビヘイビア
    /// </summary>
    /// <remarks>
    /// http://yujiro15.net/blog/index.php?id=152
    /// </remarks>
    public class AdornerBehavior
    {

        #region IsEnabled

        /// <summary>対象ControlのIsEnabled添付プロパティ</summary>
        public static readonly DependencyProperty IsEnabledProperty
                                                    = DependencyProperty.RegisterAttached(
                                                        "IsEnabled",
                                                        typeof(bool),
                                                        typeof(AdornerBehavior),
                                                        new PropertyMetadata(false, OnIsEnabledPropertyChanged));

        /// <summary>対象ControlのIsEnabled添付プロパティの値を取得</summary>
        /// <param name="target">対象Control</param>
        /// <returns>対象ControlのIsEnabledの値</returns>
        public static bool GetIsEnabled(DependencyObject target)
        {
            return (bool)target.GetValue(IsEnabledProperty);
        }

        /// <summary>対象ControlのIsEnabled添付プロパティの値を更新</summary>
        /// <param name="target">対象Control</param>
        /// <param name="value">更新値</param>
        public static void SetIsEnabled(DependencyObject target, bool value)
        {
            target.SetValue(IsEnabledProperty, value);
        }

        /// <summary>IsEnabled添付プロパティの値変更イベント</summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">変更内容データ</param>
        private static void OnIsEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            if (sender is UIElement element
                && GetIsEnabled(element))
            {

                element.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                element.PreviewMouseLeftButtonUp += OnPreviewMouseLeftButtonUp;
                element.PreviewMouseMove += OnPreviewMouseMove;

            }

        }

        #endregion

        #region 対象Control向け実行イベント

        /// <summary>ゴースト表示</summary>
        private static GhostAdorner _Adorner;

        /// <summary>
        /// マウス左ボタン押下イベント
        /// ゴーストを新規作成し表示
        /// </summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.OriginalSource is FrameworkElement element)
            {

                var parent = FindAncestor<Panel>(element);

                if (parent != null
                    && sender is FrameworkElement adornedElement)
                {

                    var point = e.GetPosition(adornedElement);
                    var offset = new Point((-1) * point.X, (-1) * point.Y);

                    _Adorner = new GhostAdorner(parent, adornedElement, point, offset);

                    adornedElement.CaptureMouse();

                }

            }

        }

        /// <summary>elementから遡り、指定した型の親要素を探す</summary>
        /// <typeparam name="T">指定した型</typeparam>
        /// <param name="element">探索対象のelement</param>
        /// <returns>指定した型に一致する親要素</returns>
        private static T FindAncestor<T>(FrameworkElement element) where T : FrameworkElement
        { 

            while (element != null)
            {

                element = VisualTreeHelper.GetParent(element) as FrameworkElement;

                if (element is T ancestor)
                {
                    return ancestor;
                }

            }

            return null;

        }

        /// <summary>
        /// マウス左ボタンイベント離上イベント
        /// ゴースト削除
        /// </summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">マウスボタンイベントデータ</param>
        private static void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (_Adorner != null)
            {

                _Adorner.AdornedElement.ReleaseMouseCapture();
                _Adorner.Detach();
                _Adorner = null;

            }

        }

        /// <summary>
        /// マウス移動イベント
        /// ゴーストをマウスカーソルに追従させる
        /// </summary>
        /// <param name="sender">対象Control</param>
        /// <param name="e">マウスイベントデータ</param>
        private static void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {

            if (_Adorner != null
                && _Adorner.AdornedElement.IsMouseCaptured
                && e.LeftButton.Equals(MouseButtonState.Pressed))
            {

                _Adorner.CurrentPoint = e.GetPosition(_Adorner.AdornedElement);

            }

        }

        #endregion

    }

}
