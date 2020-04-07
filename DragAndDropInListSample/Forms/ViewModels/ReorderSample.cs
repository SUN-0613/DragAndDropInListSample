using AYam.Common.MVVM;
using System;
using System.Collections.ObjectModel;

namespace DragAndDropInListSample.Forms.ViewModels
{

    /// <summary>
    /// Drag & Drop操作でListを並び替え
    /// ViewModel
    /// </summary>
    /// <remarks>
    /// http://yujiro15.net/blog/index.php?id=151
    /// </remarks>
    public class ReorderSample : ViewModelBase
    {

        #region Property

        /// <summary>
        /// Listのデータ一覧
        /// </summary>
        public ObservableCollection<RowData> Items { get; set; } = new ObservableCollection<RowData>();

        /// <summary>
        /// Listで選択中のデータのIndex
        /// </summary>
        public int SelectedIndex { get; set; }

        /// <summary>Drop処理コールバック関数</summary>
        public Action<int> DropCallback { get { return OnDrop; } }

        #endregion

        /// <summary>
        /// Drag & Drop操作でListを並び替え
        /// ViewModel
        /// </summary>
        public ReorderSample()
        {

            for (var iLoop = 0; iLoop < 30; iLoop++)
            {
                Items.Add(new RowData(iLoop));
            }

        }

        /// <summary>List内でDropした箇所にデータを移動</summary>
        /// <param name="index">移動場所となる割り込みIndex</param>
        private void OnDrop(int index)
        {

            if (-1 < index && index < Items.Count)
            {
                Items.Move(SelectedIndex, index);
            }

        }

    }

}
