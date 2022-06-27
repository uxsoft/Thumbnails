using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Thumbnails
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }
        private void Grid_DragLeave(object sender, DragEventArgs e)
        {
            grid.Background = new SolidColorBrush(Colors.White);
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            grid.Background = new SolidColorBrush(Colors.LightCyan);
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Link;
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            try
            {
                grid.Background = new SolidColorBrush(Colors.White);
                var files = await e.DataView.GetStorageItemsAsync();
                ThumbnailService.ProcessStorageItems(files);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
