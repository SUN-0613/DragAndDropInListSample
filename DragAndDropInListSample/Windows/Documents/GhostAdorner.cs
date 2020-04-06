using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace DragAndDropInListSample.Windows.Documents
{

    /// <summary>
    /// ドラッグ操作でゴーストを表示
    /// Adorner派生クラス
    /// </summary>
    /// <remarks>
    /// http://yujiro15.net/blog/index.php?id=152
    /// </remarks>
    public class GhostAdorner : Adorner
    {

        #region 依存関係Property

        /// <summary>ゴースト表示位置依存プロパティ</summary>
        public static readonly DependencyProperty CurrentPointProperty
                                                    = DependencyProperty.Register(
                                                        nameof(CurrentPoint),
                                                        typeof(Point),
                                                        typeof(GhostAdorner),
                                                        new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>ゴースト表示位置</summary>
        public Point CurrentPoint
        {
            get { return (Point)GetValue(CurrentPointProperty); }
            set { SetValue(CurrentPointProperty, value); }
        }

        /// <summary>ゴースト表示位置のオフセット値依存プロパティ</summary>
        public static readonly DependencyProperty OffsetProperty
                                                    = DependencyProperty.Register(
                                                        nameof(Offset),
                                                        typeof(Point),
                                                        typeof(GhostAdorner),
                                                        new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>ゴースト表示位置のオフセット値</summary>
        public Point Offset
        {
            get { return (Point)GetValue(OffsetProperty); }
            set { SetValue(OffsetProperty, value); }
        }

        #endregion

        /// <summary>装飾層</summary>
        private AdornerLayer _Layer;

        /// <summary>アタッチされているか</summary>
        private bool _IsAttached;

        /// <summary>
        /// ドラッグ操作でゴーストを表示
        /// Adorner派生クラス
        /// </summary>
        /// <param name="visual">装飾する要素</param>
        /// <param name="adornedElement">装飾に用いる要素</param>
        /// <param name="point">
        /// adornedElementを表示する位置
        /// visualからの相対位置で指定
        /// </param>
        /// <param name="offset">adornedElementを表示する位置のオフセット</param>
        public GhostAdorner(Visual visual, UIElement adornedElement, Point point, Point offset) : base(adornedElement)
        {

            _Layer = AdornerLayer.GetAdornerLayer(visual);
            CurrentPoint = point;
            Offset = offset;

            Attach();

        }

        /// <summary>ゴースト表示</summary>
        public void Attach()
        {

            if (_Layer != null
                && !_IsAttached)
            {

                _Layer.Add(this);
                _IsAttached = true;

            }

        }

        /// <summary>ゴースト削除</summary>
        public void Detach()
        {

            if (_Layer != null
                && _IsAttached)
            {

                _Layer.Remove(this);
                _IsAttached = false;

            }

        }

        /// <summary>AdornedElementを新規に描画し、半透明で表示</summary>
        /// <param name="drawingContext">描画先のコンテキスト</param>
        protected override void OnRender(DrawingContext drawingContext)
        {

            var point = new Point(CurrentPoint.X + Offset.X, CurrentPoint.Y + Offset.Y);
            var rect = new Rect(point, AdornedElement.RenderSize);
            var brush = new VisualBrush(AdornedElement) { Opacity = 0.3 };

            drawingContext.DrawRectangle(brush, null, rect);

            //base.OnRender(drawingContext);

        }

    }

}
