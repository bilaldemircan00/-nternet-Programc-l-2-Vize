using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace internet_programciligi_proje.ViewModel
{
    public class UyelerModel
    {
        public int uye_id { get; set; }
        public string uye_ad_soyad { get; set; }
        public string uye_mail { get; set; }
        public string uye_cinsiyet { get; set; }
        public string uye_dogum_tarihi { get; set; }
        public string uye_kullanici_adi { get; set; }
        public string uye_parola { get; set; }
        public int uye_yetki { get; set; }
    }
}