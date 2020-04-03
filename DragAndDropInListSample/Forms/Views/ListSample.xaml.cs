using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace DragAndDropInListSample.Forms.Views
{
    /// <summary>
    /// ListSample.xaml の相互作用ロジック
    /// </summary>
    public partial class ListSample : Window
    {

        /// <summary>
        /// マウスポインタの座標
        /// </summary>
        private Point _MousePoint;

        /// <summary>
        /// 座標初期値
        /// </summary>
        private readonly Point _InitializePoint = new Point(int.MinValue, int.MinValue);

        /// <summary>
        /// 一覧表示Sample.View
        /// </summary>
        public ListSample()
        {

            InitializeComponent();

        }

        /// <summary>
        /// ListBoxでデータ選択
        /// </summary>
        /// <param name="sender">ListBox</summary>
        /// <param name="e">マウスボタンイベントデータ</param>
        private void OnPreviewMouseLeftButtonEvent(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _MousePoint = e.GetPosition(this);
            }

        }

        /// <summary>
        /// ListBoxの選択データ変更
        /// </summary>
        /// <param name="sender">ListBox</param>
        /// <param name="e">変更データイベント</param>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MakePopup();
        }

        /// <summary>
        /// Popupコントロールの作成
        /// </summary>
        private void MakePopup()
        {

            if (this.DataContext is ViewModels.ListSample viewModel)
            {

                #region 子要素の作成

                var textBlock1_1 = new TextBlock() { Text = "Index = ", };
                var textBlock1_2 = new TextBlock();
                var binding1_2 = new Binding(nameof(viewModel.SelectedItem.Index)) { Source = viewModel.SelectedItem };

                textBlock1_2.SetBinding(TextBlock.TextProperty, binding1_2);

                var horizontalStackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal };
                horizontalStackPanel1.Children.Add(textBlock1_1);
                horizontalStackPanel1.Children.Add(textBlock1_2);

                var textBlock2_1 = new TextBlock() { Text = "Value = ", };
                var textBlock2_2 = new TextBlock();
                var binding2_2 = new Binding(nameof(viewModel.SelectedItem.Value)) { Source = viewModel.SelectedItem };

                textBlock2_2.SetBinding(TextBlock.TextProperty, binding2_2);

                var horizontalStackPanel2 = new StackPanel() { Orientation = Orientation.Horizontal };
                horizontalStackPanel1.Children.Add(textBlock2_1);
                horizontalStackPanel1.Children.Add(textBlock2_2);

                var verticalStackPanel = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5, 0, 5, 0),
                    Background = Brushes.White,
                };

                verticalStackPanel.Children.Add(horizontalStackPanel1);
                verticalStackPanel.Children.Add(horizontalStackPanel2);

                #endregion

                // _MousePoint = e.GetPosition(this);
                var popup = new Popup()
                {
                    IsOpen = true,
                    PopupAnimation = PopupAnimation.Fade,
                    AllowsTransparency = true,
                    HorizontalOffset = -10d,
                    VerticalOffset = -10d,
                    Placement = PlacementMode.MousePoint,
                    Child = verticalStackPanel,
                };

                popup.MouseMove += OnMouseMove;
                popup.MouseLeave += OnMouseLeave;

                if (this.Content is Grid grid)
                {
                    grid.Children.Add(popup);
                }

            }

        }

        /// <summary>
        /// Popup上でマウスを移動
        /// </summary>
        /// <param name="sender">Popup</param>
        /// <param name="e">マウスイベントデータ</param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {

                var position = e.GetPosition(this);

                if (_MousePoint.Equals(_InitializePoint))
                {
                    _MousePoint = position;
                }

                var point = _MousePoint - position;

                if (sender is Popup popup)
                {
                    popup.HorizontalOffset = -10d - point.X;
                    popup.VerticalOffset = -10d - point.Y;
                }

            }

        }

        /// <summary>
        /// Popup上からマウス離脱
        /// </summary>
        /// <param name="sender">Popup</param>
        /// <param name="e">マウスイベントデータ</param>
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {

            if (sender is Popup popup)
            {

                popup.IsOpen = false;

                popup.MouseMove -= OnMouseMove;
                popup.MouseLeave -= OnMouseLeave;

                if (this.Content is Grid grid)
                {

                    var index = grid.Children.IndexOf(popup);
                   
                    if (index > -1)
                    {
                        grid.Children.RemoveAt(index);
                    }

                }

            }

        }

    }

}
