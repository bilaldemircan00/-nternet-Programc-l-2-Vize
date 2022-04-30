using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace internet_programciligi_proje.ViewModel
{
    public class CevaplarModel
    {
        public int cevap_id { get; set; }
        public string cevap { get; set; }
        public int uye_id { get; set; }
        public string kullaniciAdi { get; set; }
        public int soru_id { get; set; }
        public string soruİcerik { get; set; }
    }
}