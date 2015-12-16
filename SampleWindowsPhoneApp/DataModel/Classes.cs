using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace VOHRadio.Data
{
    public class VOHSettings
    {
        public string Text { get; set; }
        public Symbol Icon { get; set; }
    }

    public class VOHObject
    {
        private string _strImageUri;

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ImageURI
        {
            get
            {
                return _strImageUri;
            }
            set
            {
                _strImageUri = value;
                if (_strImageUri != "" && _strImageUri != null)
                    ImageURIP = new Uri(value, UriKind.Absolute);
                else
                    ImageURIP = new Uri("ms-appx:///Assets/NoImage.png", UriKind.Absolute);
            }
        }
        public Uri ImageURIP { get; set; }
        public string URI { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
    }

    public class VOHRadio
    {
        public string RadioID { get; set; }
        public string RadioName { get; set; }
        public string RadioUrl { get; set; }
        public string RadioThumbnail { get; set; }
        public string RadioContent { get; set; }
    }

    public class VOHComment
    {
        public string CommentID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Content { get; set; }
        public string PostDay { get; set; }
    }
}
