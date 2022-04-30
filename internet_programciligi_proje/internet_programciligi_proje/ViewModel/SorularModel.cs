using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace internet_programciligi_proje.ViewModel
{
    public class SorularModel
    {
        public int soru_id { get; set; }
        public string soru { get; set; }
        public int uye_id { get; set; }
        public string uyeKullaniciAdi { get; set; }
        public int kategori_id { get; set; }
        public string kategoriAdi { get; set; }
        
        
    }
}