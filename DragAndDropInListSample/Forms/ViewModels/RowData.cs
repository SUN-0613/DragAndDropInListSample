using AYam.Common.MVVM;
using System;

namespace DragAndDropInListSample.Forms.ViewModels
{

    /// <summary>
    /// 行データ.ViewModel
    /// </summary>
    public class RowData : ViewModelBase
    {

        #region Property

        /// <summary>
        /// List.Index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 値
        /// </summary>
        public int Value { get; set; }

        #endregion

        /// <summary>
        /// 行データ.ViewModel
        /// </summary>
        /// <param name="index">List.Index</param>
        public RowData(int index)
        {

            Index = index;

            // 値はRandomで生成
            var seed = index
                        + (int.TryParse(DateTime.Now.Ticks.ToString(), out var num)
                        ? num
                        : DateTime.Now.Millisecond);

            var rnd = new Random(seed);

            Value = rnd.Next(1000);

        }

    }

}
