using DragAndDropInListSample.Forms.Views;
using System.Windows;

namespace DragAndDropInListSample
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// 処理開始
        /// </summary>
        /// <param name="e">コマンドライン引数イベントデータ</param>
        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);

            new AdornerSample().ShowDialog();

        }

    }

}
