using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DragAndDropInListSample.Windows.Documents
{

    /// <summary>
    /// ListBoxItemのドラッグ操作でゴースト表示
    /// Adorner派生クラス
    /// </summary>
    /// <remarks>
    /// http://main.tinyjoker.net/Tech/CSharp/WPF/ListBox%A4%CE%A5%A2%A5%A4%A5%C6%A5%E0%A4%F2%C8%BE%C6%A9%CC%C0%A5%B4%A1%BC%A5%B9%A5%C8%A4%C4%A4%AD%A5%C9%A5%E9%A5%C3%A5%B0%A5%A2%A5%F3%A5%C9%A5%C9%A5%ED%A5%C3%A5%D7%A4%C7%CA%C2%A4%D9%C2%D8%A4%A8%A4%EB.html#subclass
    /// </remarks>
    internal class GhostListBoxAdorner : Adorner
    {

        #region property

        /// <summary>表示位置オフセット値</summary>
        private Point _Offset;

        /// <summary>表示位置オフセット値</summary>
        public Point Offset
        {
            get { return _Offset; }
            set
            {

                _Offset.X = value.X - _StartPosition.X;
                _Offset.Y = value.Y - _StartPosition.Y;
                UpdatePosition();

            }
        }

        #endregion

        #region private

        /// <summary>ListBoxItemの内容</summary>
        private UIElement _Child;

        /// <summary>ドラッグ操作開始時点のマウスカーソル位置</summary>
        private readonly Point _StartPosition;

        #endregion

        /// <summary>
        /// ListBoxItemのドラッグ操作でゴースト表示
        /// Adorner派生クラス
        /// </summary>
        /// <param name="listBox">ListBox</param>
        /// <param name="listBoxItem">ListBoxItem</param>
        /// <param name="opacity">透明度</param>
        /// <param name="startPosition">ドラッグ操作開始時点のマウスカーソル位置</param>
        public GhostListBoxAdorner(UIElement listBox, UIElement listBoxItem, double opacity, Point startPosition) : base(listBox)
        {

            // ListBoxItemのVisualを半透明にしてコピーし、ゴーストを作成
            var visual = new VisualBrush(listBoxItem)
            {
                Opacity = opacity,
            };

            // ListBoxItemのレイアウトを取得
            var bounds = VisualTreeHelper.GetDescendantBounds(listBoxItem);

            // Rectangleを生成してListBoxItemのゴーストを作成
            _Child = new Rectangle()
            {
                Width = bounds.Width,
                Height = bounds.Height,
                Fill = visual,
            };

            // マウスカーソルの開始位置を記憶
            _StartPosition = startPosition;

        }

        /// <summary>
        /// ゴースト表示位置の更新
        /// </summary>
        private void UpdatePosition()
        {

            if (Parent is AdornerLayer adorner)
            {
                adorner.Update(AdornedElement);
            }

        }

        #region override

        /// <summary>ListBoxItemのゴーストを取得</summary>
        /// <param name="index">未使用</param>
        /// <returns>ゴースト</returns>
        protected override Visual GetVisualChild(int index)
        {
            return _Child;
        }

        /// <summary>1つのゴーストを表示するので、カウントは1固定で返す</summary>
        protected override int VisualChildrenCount { get { return 1; } }

        /// <summary>作ろうとしているコントール(ゴースト)の最大サイズを取得</summary>
        /// <param name="constraint">最大サイズの制限値</param>
        /// <returns>ゴーストの最大サイズ</returns>
        protected override Size MeasureOverride(Size constraint)
        {

            _Child.Measure(constraint);
            return _Child.DesiredSize;

        }

        /// <summary>作ろうとしているコントロール(ゴースト)の実際に必要なサイズを取得</summary>
        /// <param name="finalSize">親コントロールのサイズ</param>
        /// <returns>ゴーストのサイズ</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {

            _Child.Arrange(new Rect(_Child.DesiredSize));
            return _Child.RenderSize;

        }

        /// <summary>コントロールの変換方法の取得</summary>
        /// <param name="transform">現在適用している変換方法</param>
        /// <returns>コントロールの変換方法</returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {

            var group = new GeneralTransformGroup();

            group.Children.Add(base.GetDesiredTransform(transform));
            group.Children.Add(new TranslateTransform(Offset.X, Offset.Y));

            return group;

        }

        #endregion

    }

}
