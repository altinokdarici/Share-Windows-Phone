using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ShareSource
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ShareType shareType;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            base.OnNavigatedTo(e);
        }

        private async void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (shareType == ShareType.Text)
            {
                args.Request.Data.Properties.Title = TextBoxTextShareTitle.Text;
                args.Request.Data.Properties.Description = TextBoxTextShareDescription.Text;
                args.Request.Data.SetText(TextBoxTextShareText.Text);
            }
            else
            {
                List<StorageFile> files = new List<StorageFile>();
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/mslogo.png"));
                files.Add(file);
                args.Request.Data.Properties.Title = TextBoxFileShareTitle.Text;
                args.Request.Data.Properties.Description = TextBoxFileShareDescription.Text;
                args.Request.Data.SetStorageItems(files);
                args.Request.Data.SetWebLink(new Uri("http://www.4sln.com/"));
                args.Request.GetDeferral().Complete();
            }
        }

        private void ButtonShareText_Click(object sender, RoutedEventArgs e)
        {
            shareType = ShareType.Text;
            DataTransferManager.ShowShareUI();
        }
        private void ButtonShareFile_Click(object sender, RoutedEventArgs e)
        {
            shareType = ShareType.WebLinkAndFile;
            DataTransferManager.ShowShareUI();
        }
    }
    public enum ShareType
    {
        Text,
        WebLinkAndFile
    }
}
