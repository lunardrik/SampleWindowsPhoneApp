using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VOHRadio.Common;
using VOHRadio.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace VOHRadio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MediaFramePage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        List<VOHObject> lstPlaylist = new List<VOHObject>();
        private int _intPage = 0;

        public MediaFramePage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data.
            var item = (VOHObject)e.NavigationParameter;
            this.DefaultViewModel["Item"] = item;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            VOHObject item = (VOHObject)this.DefaultViewModel["Item"];

            _intPage++;
            switch (item.Type)
            {
                case "NEWCAT":
                    lstPlaylist.AddRange(Helper.GetNews(item.ID, _intPage));
                    break;
                case "SNDCAT":
                    lstPlaylist.AddRange(Helper.GetSound(item.ID, _intPage));
                    break;
                case "VIDCAT":
                    lstPlaylist.AddRange(Helper.GetVideo(item.ID, _intPage));
                    break;
                case "SCDCAT":
                    lstPlaylist.AddRange(Helper.GetScheduleDetail(item.ID));
                    break;
                default:
                    break;
            }

            lsvPlaylist.ItemsSource = lstPlaylist;
        }

        private void lsvPlaylist_ItemClick(object sender, ItemClickEventArgs e)
        {
            pfwPlayer.Stop();
            pfwPlayer.Source = null;
            pfwPlayer.Source = new Uri(((VOHObject)e.ClickedItem).URI, UriKind.Absolute);
            pfwPlayer.Play();
        }
        
    }
}
