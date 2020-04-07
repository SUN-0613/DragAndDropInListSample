using System.Windows;

namespace DragAndDropInListSample.Behaviors.Data
{

    /// <summary>Drag & Dropしているデータ</summary>
    internal class DragAndDropObject
    {

        #region Property

        /// <summary>Drag開始位置座標</summary>
        public Point StartPoint { get; set; }

        /// <summary>Drag対象Control</summary>
        public FrameworkElement DraggedItem { get; set; }

        /// <summary>Drop可能か</summary>
        public bool IsDroppable { get; set; }

        #endregion

        /// <summary>
        /// Drag開始に必要となる最短距離
        /// </summary>
        private static readonly Vector MinimumDragPoint = new Vector(SystemParameters.MinimumHorizontalDragDistance, SystemParameters.MinimumVerticalDragDistance);

        /// <summary>DragがOKか確認</summary>
        /// <param name="current">マウス現在位置座標</param>
        /// <returns>マウス位置が十分に移動していればOKとしtrueを戻す</returns>
        public bool CheckStartDragging(Point current)
        {
            return (current - StartPoint).Length - MinimumDragPoint.Length > 0;
        }

    }

}
