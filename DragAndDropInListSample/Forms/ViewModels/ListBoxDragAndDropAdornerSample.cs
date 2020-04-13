using AYam.Common.MVVM;
using System.Collections.ObjectModel;

namespace DragAndDropInListSample.Forms.ViewModels
{

    /// <summary>
    /// ListBoxでListBoxItemのDrag&Drop中ゴーストを表示するサンプル.ViewModel
    /// </summary>
    /// <remarks>
    /// http://main.tinyjoker.net/Tech/CSharp/WPF/ListBox%A4%CE%A5%A2%A5%A4%A5%C6%A5%E0%A4%F2%C8%BE%C6%A9%CC%C0%A5%B4%A1%BC%A5%B9%A5%C8%A4%C4%A4%AD%A5%C9%A5%E9%A5%C3%A5%B0%A5%A2%A5%F3%A5%C9%A5%C9%A5%ED%A5%C3%A5%D7%A4%C7%CA%C2%A4%D9%C2%D8%A4%A8%A4%EB.html#subclass
    /// </remarks>
    public class ListBoxDragAndDropAdornerSample : ViewModelBase
    {

        #region Property

        /// <summary>
        /// ListBox.データ一覧
        /// </summary>
        public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();

        #endregion

        /// <summary>
        /// ListBoxでListBoxItemのDrag&Drop中ゴーストを表示するサンプル.ViewModel
        /// </summary>
        public ListBoxDragAndDropAdornerSample()
        {

            for (var iLoop = 1; iLoop <= 10; iLoop++)
            {

                Items.Add(iLoop.ToString());

            }

        }

    }

}
