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
        private RowData _SelectedItem = null;

        /// <summary>
        /// List.SelectedItem
        /// </summary>
        public RowData SelectedItem 
        { 
            get { return _SelectedItem; }
            set
            {

                // 選択データがあればPopupを表示
                IsOpenPopup = (value != null);
                CallPropertyChanged(nameof(IsOpenPopup));

                _SelectedItem = value;
                CallPropertyChanged();

            }
        }

        /// <summary>
        /// Popup.Show
        /// </summary>
        public bool IsOpenPopup { get; set; } = false;

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
