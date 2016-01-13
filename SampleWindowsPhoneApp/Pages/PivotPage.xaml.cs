using VOHRadio.Common;
using VOHRadio.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.Store;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Email;
using Windows.UI.Popups;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace VOHRadio
{
    public sealed partial class PivotPage : Page
    {
        private const string FirstGroupName = "FirstGroup";
        private const string SecondGroupName = "SecondGroup";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

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

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Adds an item to the list when the app bar button is clicked.
        /// </summary>
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            // var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;

            var item = (VOHObject)e.ClickedItem;

            pfwRadioPlayer.Stop();

            if (item.Type == "SNDCAT")
            {
                if (!Frame.Navigate(typeof(MediaFramePage), e.ClickedItem))
                {
                    throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
                }
            }
            else if (!Frame.Navigate(typeof(ItemPage), e.ClickedItem))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Loads the content for the second pivot item when it is scrolled into view.
        /// </summary>
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
            this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
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
            DataTransferManager dtManager = DataTransferManager.GetForCurrentView();
            dtManager.DataRequested += dtManager_DataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private async void dtManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.Properties.Title = "VOH Radio";
            e.Request.Data.Properties.Description = "Voice of Ho Chi Minh city people radio";
            e.Request.Data.SetWebLink(new Uri(string.Format("ms-windows-store:navigate?appid={0}", CurrentApp.AppId)));
        }

        #endregion

        #region Data List

        #region Settings
        public List<VOHSettings> lstSettingItems = new List<VOHSettings>() {
            new VOHSettings {
                Text = "Thay đổi màu sắc",
                Icon = Symbol.Document,
                AsyncAction = async(Frame) => {
                    //if (!Frame.Navigate(typeof(ThemeSettingPage)))
                    //{
                    //    return false;
                    //}
                    MessageDialog msgbox = new MessageDialog("Comming Soon...");
                    await msgbox.ShowAsync();
                    //return true;
                }
            },
            new VOHSettings {
                Text = "Giới thiệu",
                Icon = Symbol.ViewAll,
                Action = (Frame) => {
                    if (!Frame.Navigate(typeof(AboutPage)))
                    {
                        return false;
                    }
                    return true;
                }
            },
            new VOHSettings {
                Text = "Đánh giá",
                Icon = Symbol.SolidStar,
                AsyncAction = async(Frame) => {
                    var uri = new Uri(string.Format("ms-windows-store:navigate?appid={0}", CurrentApp.AppId));
                    await Windows.System.Launcher.LaunchUriAsync(uri);
                }
            },
            new VOHSettings {
                Text = "Chia sẻ",
                Icon = Symbol.ReShare,
                Action = (Frame) => {
                    Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
                    return true;
                }
            },
            new VOHSettings {
                Text = "Góp ý",
                Icon = Symbol.Comment,
                AsyncAction = async(Frame) => {
                    EmailRecipient sendTo = new EmailRecipient()
                    {
                        Address = "vohradio@gmail.com"
                    };

                    //generate mail object
                    EmailMessage mail = new EmailMessage();
                    mail.Subject = "Góp ý ứng dụng VOH Radio (Windows Phone)";


                    //add recipients to the mail object
                    mail.To.Add(sendTo);
                    //mail.Bcc.Add(sendTo);
                    //mail.CC.Add(sendTo);

                    //open the share contract with Mail only:
                    await EmailManager.ShowComposeNewEmailAsync(mail);
                }
            },
            new VOHSettings {
                Text = "Hướng dẫn sử dụng",
                Icon = Symbol.Library
            },
            new VOHSettings {
                Text = "Liên hệ hỗ trợ",
                Icon = Symbol.CellPhone,
                Action = (Frame) => {
                    if (!Frame.Navigate(typeof(ContactPage)))
                    {
                        return false;
                    }
                    return true;
                }
            }
        };
        #endregion

        #region News
        public List<VOHObject> lstNews;
        #endregion

        #region Audio
        public List<VOHObject> lstAudio;
        #endregion

        #region Favourite
        public List<VOHObject> lstFavourite;
        #endregion

        #region Video
        public List<VOHObject> lstVideo;
        #endregion

        #region Comment
        public List<VOHComment> lstComment;
        private int _intCommentPage = 0;
        #endregion

        #region Schedule
        public List<VOHObject> lstSchedule;
        #endregion

        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            myIndeterminateProbar.Visibility = Visibility.Collapsed;

            // Assign Resources
            lsvSettings.ItemsSource = lstSettingItems;

            lstNews = Helper.GetNewsCategories();
            lsvNewsCat.ItemsSource = lstNews;

            lstAudio = Helper.GetSoundCategories();
            lsvAudioCat.ItemsSource = lstAudio;

            lstVideo = Helper.GetVideoCategories();
            lsvVideoCat.ItemsSource = lstVideo;

            lstFavourite = new List<VOHObject>();
            lsvFavourite.ItemsSource = lstFavourite;

            _intCommentPage++;
            lstComment = Helper.GetComment(_intCommentPage);
            lsvComment.ItemsSource = lstComment;

            lstSchedule = Helper.GetSchedules();
            lvsSchedule.ItemsSource = lstSchedule;
        }

        private void Radio_Click(object sender, RoutedEventArgs e)
        {
            var lstRadio = Helper.GetRadioList();
            var btnRadio = (AppBarButton)sender;
            BitmapImage bitmapImage = new BitmapImage();

            var strRadioId = btnRadio.Tag.ToString();
            VOHObject obj = null;

            foreach (var radio in lstRadio)
            {
                if (radio.ID == strRadioId)
                {
                    obj = radio; break;
                }
            }

            if (obj != null)
            {
                pfwRadioPlayer.Opacity = 0;
                bitmapImage.UriSource = obj.ImageURIP;
                imgBackground.Source = bitmapImage;

                txbOnAirLabel.Visibility = Visibility.Visible;
                txbOnAirText.Visibility = Visibility.Visible;

                txbVOHNameEN.Visibility = Visibility.Visible;
                txbVOHNameVN.Visibility = Visibility.Visible;

                txbOnAirText.Text = obj.SubTitle;

                pviRadio.Header = obj.Title;

                pfwRadioPlayer.Stop();
                pfwRadioPlayer.Source = null;
                pfwRadioPlayer.Source = new Uri(obj.URI, UriKind.Absolute);
                pfwRadioPlayer.Play();
            } else
            {
                pfwRadioPlayer.Opacity = 100;

                txbOnAirLabel.Visibility = Visibility.Collapsed;
                txbOnAirText.Visibility = Visibility.Collapsed;

                txbVOHNameEN.Visibility = Visibility.Collapsed;
                txbVOHNameVN.Visibility = Visibility.Collapsed;

                pviRadio.Header = "VOH Radio";

                pfwRadioPlayer.Stop();
                pfwRadioPlayer.Source = null;
                pfwRadioPlayer.Source = new Uri("http://221.132.38.110:1935/live/channel4/playlist.m3u8", UriKind.Absolute);
                pfwRadioPlayer.Play();
            }
        }

        private void lsvSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            var setting = (VOHSettings)e.ClickedItem;

            if (setting.Action != null && !setting.Action(Frame))
            {
                // Do-nothing
            }
            if (setting.Action == null && setting.AsyncAction != null)
            {
                setting.AsyncAction(Frame);
            }
        }
    }
}
