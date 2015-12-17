using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VOHRadio.Data;

namespace VOHRadio.Common
{
    class Helper
    {
        private const int ITEM_PER_PAGE = 20;

        private const string NEWS_CAT_URL = "http://www.voh.com.vn/WebService/GetNewsCate";
        private const string NEWS_DET_URL = "http://www.voh.com.vn/WebService/GetNews?CateID={0}&item={1}&page={2}";
        private const string SUND_CAT_URL = "http://www.voh.com.vn/WebService/SoundCate";
        private const string SUND_DET_URL = "http://www.voh.com.vn/WebService/Sounds?CateID={0}&item={1}&page={2}";
        private const string VIDE_CAT_URL = "http://www.voh.com.vn/WebService/VideoCate";
        private const string VIDE_DET_URL = "http://www.voh.com.vn/WebService/Videos?CateID={0}&item={1}&page={2}";
        private const string COMM_GET_URL = "http://www.voh.com.vn/WebService/GetComment?item={0}&page={1}";
        private const string COMM_POT_URL = "http://www.voh.com.vn/WebService/AddComment";
        private const string SCHD_CAT_URL = "http://www.voh.com.vn/WebService/LichPhatSong";
        private const string SCHD_DET_URL = "http://www.voh.com.vn/WebService/ChiTietLichPhatSong?LPSID={0}";
        private const string RADI_GET_URL = "http://www.voh.com.vn/WebService/GetRadioOnline";


        public static List<VOHObject> GetNewsCategories()
        {
            List<VOHObject> lstNewsCat = new List<VOHObject>();
            string json = GetJsonData(NEWS_CAT_URL);
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListCate)
            {
                lstNewsCat.Add(new VOHObject
                {
                    ID = item.CateID,
                    Title = item.CateName,
                    ImageURI = item.CateThumbnail,
                    Type = "NEWCAT"
                });
            }
            return lstNewsCat;
        }

        public static List<VOHObject> GetNews(string id, int page)
        {
            List<VOHObject> lstNews = new List<VOHObject>();
            string json = GetJsonData(string.Format(NEWS_DET_URL, id, ITEM_PER_PAGE, page));
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListNewMode)
            {
                lstNews.Add(new VOHObject
                {
                    ID = item.NewID,
                    Title = item.Title,
                    SubTitle = item.CateName,
                    ImageURI = item.Thumbnail,
                    URI = item.Url,
                    Type = "NEWDET"
                });
            }
            return lstNews;
        }

        public static List<VOHObject> GetSoundCategories()
        {
            List<VOHObject> lstSoundCat = new List<VOHObject>();
            string json = GetJsonData(SUND_CAT_URL);
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListCate)
            {
                lstSoundCat.Add(new VOHObject
                {
                    ID = item.CateID,
                    Title = item.CateName,
                    ImageURI = item.CateThumbnail,
                    Type = "SNDCAT"
                });
            }
            return lstSoundCat;
        }

        public static List<VOHObject> GetSound(string id, int page)
        {
            List<VOHObject> lstSound = new List<VOHObject>();
            string json = GetJsonData(string.Format(SUND_DET_URL, id, ITEM_PER_PAGE, page));
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListSound)
            {
                lstSound.Add(new VOHObject
                {
                    ID = item.SoundID,
                    Title = item.SoundName,
                    ImageURI = item.Thumbnail,
                    URI = item.SoundUrl,
                    Type = "SNDDET"
                });
            }
            return lstSound;
        }

        public static List<VOHObject> GetVideoCategories()
        {
            List<VOHObject> lstVideoCat = new List<VOHObject>();
            string json = GetJsonData(VIDE_CAT_URL);
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListCate)
            {
                lstVideoCat.Add(new VOHObject
                {
                    ID = item.CateID,
                    Title = item.CateName,
                    ImageURI = item.CateThumbnail,
                    Type = "VIDCAT"
                });
            }
            return lstVideoCat;
        }

        public static List<VOHObject> GetVideo(string id, int page)
        {
            List<VOHObject> lstVideo = new List<VOHObject>();
            string json = GetJsonData(string.Format(VIDE_DET_URL, id, ITEM_PER_PAGE, page));
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListVideo)
            {
                lstVideo.Add(new VOHObject
                {
                    ID = item.VideoID,
                    Title = item.VideoName,
                    ImageURI = item.VideoThumbnail,
                    URI = item.VideoUrl,
                    Type = "VIDDET"
                });
            }
            return lstVideo;
        }

        public static List<VOHComment> GetComment(int page)
        {
            List<VOHComment> lstComment = new List<VOHComment>();
            string json = GetJsonData(string.Format(COMM_GET_URL, ITEM_PER_PAGE, page));
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListCommen)
            {
                lstComment.Add(new VOHComment
                {
                    CommentID = item.CommenID,
                    FullName = item.FullName,
                    Phone = item.Phone,
                    Email = item.Email,
                    Content = item.Content,
                    PostDay = item.PostDay
                });
            }
            return lstComment;
        }

        public static List<VOHObject> GetSchedules()
        {
            List<VOHObject> lstSchedule = new List<VOHObject>();
            string json = GetJsonData(SCHD_CAT_URL);
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListLPS)
            {
                lstSchedule.Add(new VOHObject
                {
                    ID = item.LPSID,
                    Title = item.LPSName,
                    SubTitle = item.LPSType,
                    Type = "SCDCAT"
                });
            }
            return lstSchedule;
        }

        public static List<VOHObject> GetScheduleDetail(string id)
        {
            List<VOHObject> lstScheduleDetail = new List<VOHObject>();
            string json = GetJsonData(string.Format(SCHD_DET_URL, id));
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListChiTietLPS)
            {
                lstScheduleDetail.Add(new VOHObject
                {
                    ID = item.ID,
                    Title = item.NoiDung,
                    SubTitle = item.ThoiGian,
                    ImageURI = item.Thumbnail,
                    Type = "SCDDET"
                });
            }
            return lstScheduleDetail;
        }

        public static List<VOHObject> GetRadioList()
        {
            List<VOHObject> lstRadio = new List<VOHObject>();
            string json = GetJsonData(RADI_GET_URL);
            dynamic lstCat = JsonConvert.DeserializeObject<dynamic>(json);

            foreach (dynamic item in lstCat.ListRadio)
            {
                lstRadio.Add(new VOHObject
                {
                    ID = item.RadioID,
                    Title = item.RadioName,
                    URI = item.RadioUrl,
                    ImageURI = item.RadioThumbnail,
                    SubTitle = item.RadioContent,
                    Type = "RDOGET"
                });
            }
            return lstRadio;
        }

        public static string GetJsonData(string url)
        {
            var promise = GetWebStringAsync(url);
            // do something else if needed
            var jsonData = promise.Result;
            return jsonData;
        }

        public static async Task<string> GetWebStringAsync(string uri)
        {
            using (var client = new HttpClient())
            {
                var stringTask = await client.GetStringAsync(uri).ConfigureAwait(continueOnCapturedContext: false);
                return stringTask;
            }
        }
    }
}
