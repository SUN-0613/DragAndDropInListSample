using System.Windows;
using System.Windows.Media;

namespace DragAndDropInListSample.Windows
{

    /// <summary>FrameworkElement関連クラス情報</summary>
    public static class FrameworkElementInfo
    {

        /// <summary>elementから遡り、指定した型の親要素を探す</summary>
        /// <typeparam name="T">指定した型</typeparam>
        /// <param name="element">探索対象のelement</param>
        /// <returns>指定した型に一致する親要素</returns>
        public static T FindAncestor<T>(FrameworkElement element) where T : FrameworkElement
        {

            while (element != null)
            {

                // 親要素を取得
                element = VisualTreeHelper.GetParent(element) as FrameworkElement;

                // 親要素が指定した型であるか
                if (element is T ancestor)
                {
                    return ancestor;
                }

            }

            return null;

        }

    }

}
