using AYam.Common.MVVM;
using System.Collections.ObjectModel;

namespace DragAndDropInListSample.Forms.ViewModels
{

    /// <summary>
    /// 一覧表示Sample.ViewModel
    /// </summary>
    public class ListSample : ViewModelBase
    {

        #region Property

        /// <summary>
        /// List.ItemsSource
        /// </summary>
        public ObservableCollection<RowData> Items { get; set; } = new ObservableCollection<RowData>();

        /// <summary>
        /// List.SelectedItem
        /// </summary>
        public RowData SelectedItem { get; set; }

        #endregion

        /// <summary>
        /// 一覧表示Sample.ViewModel
        /// </summary>
        public ListSample()
        {

            for (var iLoop = 0; iLoop < 30; iLoop++)
            {
                Items.Add(new RowData(iLoop));
            }

        }

    }

}
