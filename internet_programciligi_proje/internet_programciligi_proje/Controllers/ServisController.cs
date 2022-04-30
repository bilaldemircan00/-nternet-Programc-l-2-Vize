using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using internet_programciligi_proje.Models;
using internet_programciligi_proje.ViewModel;


namespace internet_programciligi_proje.Controllers
{
    public class ServisController : ApiController
    {
        DB01Entities2 db = new DB01Entities2();
        SonucModel sonuc = new SonucModel();


        #region Üye
        [HttpGet]
        [Route("api/uyeliste")]
        public List<UyelerModel> UyeListe()
        {
            List<UyelerModel> liste = db.uyeler.Select(x => new UyelerModel()
            {
                uye_id = x.uye_id,
                uye_ad_soyad = x.uye_ad_soyad,
                uye_mail = x.uye_mail,
                uye_kullanici_adi = x.uye_kullanici_adi,
                uye_parola = x.uye_parola,
                uye_cinsiyet = x.uye_cinsiyet,
                uye_dogum_tarihi = x.uye_dogum_tarihi,
                uye_yetki = x.uye_yetki
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/uyebyid/{uye_id}")]
        public UyelerModel UyeById(int uye_id)
        {
            UyelerModel kayit = db.uyeler.Where(s => s.uye_id == uye_id).Select(x => new UyelerModel()
            {
                uye_id = x.uye_id,
                uye_ad_soyad = x.uye_ad_soyad,
                uye_mail = x.uye_mail,
                uye_kullanici_adi = x.uye_kullanici_adi,
                uye_parola = x.uye_parola,
                uye_cinsiyet = x.uye_cinsiyet,
                uye_dogum_tarihi = x.uye_dogum_tarihi,
                uye_yetki = x.uye_yetki
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/uyeekle")]
        public SonucModel UyeEkle(UyelerModel model)
        {
            if (db.uyeler.Count(s => s.uye_kullanici_adi == model.uye_kullanici_adi || s.uye_mail == model.uye_mail) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kullanıcı Adı veya E-Posta Adresi Kayıtlıdır!";
                return sonuc;
            }

            uyeler yeni = new uyeler();
            yeni.uye_ad_soyad = model.uye_ad_soyad;
            yeni.uye_mail = model.uye_mail;
            yeni.uye_kullanici_adi = model.uye_kullanici_adi;
            yeni.uye_parola = model.uye_parola;
            yeni.uye_dogum_tarihi = model.uye_dogum_tarihi;
            yeni.uye_cinsiyet = model.uye_cinsiyet;
            yeni.uye_yetki = model.uye_yetki;


            db.uyeler.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/uyeduzenle")]
        public SonucModel UyeDuzenle(UyelerModel model)
        {
            uyeler kayit = db.uyeler.Where(s => s.uye_id == model.uye_id).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }
            kayit.uye_ad_soyad = model.uye_ad_soyad;
            kayit.uye_mail = model.uye_mail;
            kayit.uye_kullanici_adi = model.uye_kullanici_adi;
            kayit.uye_cinsiyet = model.uye_cinsiyet;
            kayit.uye_parola = model.uye_parola;
            kayit.uye_yetki = model.uye_yetki;
            kayit.uye_dogum_tarihi = model.uye_dogum_tarihi;


            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Düzenlendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/uyesil/{uye_id}")]
        public SonucModel UyeSil(int uye_id)
        {
            uyeler kayit = db.uyeler.Where(s => s.uye_id == uye_id).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }

            if (db.sorular.Count(s => s.uye_id == uye_id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Makale Kaydı Olan Üye Silinemez!";
                return sonuc;
            }

            if (db.cevaplar.Count(s => s.uye_id == uye_id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Yorum Kaydı Olan Üye Silinemez!";
                return sonuc;
            }

            db.uyeler.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Üye Silindi";
            return sonuc;
        }
        #endregion

        #region kategori


        [HttpGet]
        [Route("api/kategoriliste")]

        public List<KategoriModel> KategoriListe()
        {
            List<KategoriModel> liste = db.kategoriler.Select(x => new KategoriModel()
            {
                kategori_id = x.kategori_id,
                karegori_adi = x.karegori_adi,
                KategoriSoruSayisi = x.sorular.Count
            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/kategoribyid/{kategori_id}")]
        public KategoriModel KategoriById(int kategori_id)
        {
            KategoriModel kayit = db.kategoriler.Where(s => s.kategori_id == kategori_id).Select(x => new KategoriModel()
            {
                kategori_id = x.kategori_id,
                karegori_adi = x.karegori_adi,
                KategoriSoruSayisi = x.sorular.Count
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel KategoriEkle(KategoriModel model)
        {
            if (db.kategoriler.Count(s => s.karegori_adi == model.karegori_adi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kategori Adı Kayıtlıdır!";
                return sonuc;
            }

            kategoriler yeni = new kategoriler();
            yeni.karegori_adi = model.karegori_adi;
            db.kategoriler.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kategoriduzenle")]
        public SonucModel KategoriDuzenle(KategoriModel model)
        {
            kategoriler kayit = db.kategoriler.Where(s => s.kategori_id == model.kategori_id).FirstOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.karegori_adi = model.karegori_adi;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategorisil/{kategori_id}")]
        public SonucModel KategoriSil(int kategori_id)
        {
            kategoriler kayit = db.kategoriler.Where(s => s.kategori_id == kategori_id).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.kategoriler.Count(s => s.kategori_id == kategori_id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Makale Kayıtlı Kategori Silinemez!";
                return sonuc;
            }

            db.kategoriler.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";
            return sonuc;
        }


        #endregion

        #region soru

        [HttpGet]
        [Route("api/soruliste")]
        public List<SorularModel> MakaleListe()
        {
            List<SorularModel> liste = db.sorular.Select(x => new SorularModel()
            {
                soru_id = x.soru_id,
                soru = x.soru,
                kategori_id = x.kategori_id,
                kategoriAdi = x.kategoriler.karegori_adi,
                uye_id = x.uye_id,
                uyeKullaniciAdi = x.uyeler.uye_kullanici_adi

            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/sorulistesoneklenenler/{s}")]
        public List<SorularModel> SoruListeSonEklenenler(int s)
        {
            List<SorularModel> liste = db.sorular.OrderByDescending(o => o.soru_id).Take(s).Select(x => new SorularModel()
            {
                soru_id = x.soru_id,
                soru = x.soru,
                kategori_id = x.kategori_id,
                kategoriAdi = x.kategoriler.karegori_adi,
                uye_id = x.uye_id,
                uyeKullaniciAdi = x.uyeler.uye_kullanici_adi


            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/sorulistebykatid/{kategori_id}")]
        public List<SorularModel> SoruListeBykategori_id(int kategori_id)
        {
            List<SorularModel> liste = db.sorular.Where(s => s.kategori_id == kategori_id).Select(x => new SorularModel()
            {
                soru_id = x.soru_id,
                soru = x.soru,
                kategori_id = x.kategori_id,
                kategoriAdi = x.kategoriler.karegori_adi,
                uye_id = x.uye_id,
                uyeKullaniciAdi = x.uyeler.uye_kullanici_adi

            }).ToList();

            return liste;
        }
        [HttpGet]
        [Route("api/sorulistebyuyeid/{uye_id}")]
        public List<SorularModel> SoruListeByUyeId(int uye_id)
        {
            List<SorularModel> liste = db.sorular.Where(s => s.uye_id == uye_id).Select(x => new SorularModel()
            {
                soru_id = x.soru_id,
                soru = x.soru,
                kategori_id = x.kategori_id,
                kategoriAdi = x.kategoriler.karegori_adi,
                uye_id = x.uye_id,
                uyeKullaniciAdi = x.uyeler.uye_kullanici_adi

            }).ToList();

            return liste;
        }

        [HttpGet]
        [Route("api/sorubyid/{soru_id}")]
        public SorularModel SoruById(int soru_id)
        {
            SorularModel kayit = db.sorular.Where(s => s.soru_id == soru_id).Select(x => new SorularModel()
            {
                soru_id = x.soru_id,
                soru = x.soru,
                kategori_id = x.kategori_id,
                kategoriAdi = x.kategoriler.karegori_adi,
                uye_id = x.uye_id,
                uyeKullaniciAdi = x.uyeler.uye_kullanici_adi
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/soruekle")]
        public SonucModel MakaleEkle(SorularModel model)
        {
            if (db.sorular.Count(s => s.soru_id == model.soru_id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Makale Başlığı Kayıtlıdır!";
                return sonuc;
            }

            sorular yeni = new sorular();
            yeni.soru = model.soru;
            yeni.kategori_id = model.kategori_id;
            yeni.uye_id = model.uye_id;
            db.sorular.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Soru Eklendi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/soruduzenle")]
        public SonucModel MakaleDuzenle(SorularModel model)
        {
            sorular kayit = db.sorular.Where(s => s.soru_id == model.soru_id).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.soru = model.soru;
            kayit.kategori_id = model.kategori_id;
            kayit.uye_id = model.uye_id;
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Soru Düzenlendi";
            return sonuc;

        }

        [HttpDelete]
        [Route("api/soruesil/{soru_id}")]
        public SonucModel MakaleSil(int soru_id)
        {
            sorular kayit = db.sorular.Where(s => s.soru_id == soru_id).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.cevaplar.Count(s => s.soru_id == soru_id) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Cevap Kayıtlı Soru Silinemez!";
                return sonuc;
            }

            db.sorular.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Soru Silindi";
            return sonuc;
        }


        #endregion

        #region cevap

        [HttpGet]
        [Route("api/cevapliste")]
        public List<CevaplarModel> YorumListe()
        {
            List<CevaplarModel> liste = db.cevaplar.Select(x => new CevaplarModel()
            {
                cevap_id = x.cevap_id,
                cevap = x.cevap,
                soru_id = x.soru_id,
                uye_id = x.uye_id,
                kullaniciAdi = x.uyeler.uye_kullanici_adi,
                soruİcerik = x.sorular.soru,

            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/sorulistebyuyeid/{uye_id}")]
        public List<CevaplarModel> CevapListeByUyeId(int uye_id)
        {
            List<CevaplarModel> liste = db.cevaplar.Where(s => s.uye_id == uye_id).Select(x => new CevaplarModel()
            {
                cevap_id = x.cevap_id,
                cevap = x.cevap,
                soru_id = x.soru_id,
                uye_id = x.uye_id,
                kullaniciAdi = x.uyeler.uye_kullanici_adi,
                soruİcerik = x.sorular.soru,

            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/cevaplistebysoruid/{soru_id}")]
        public List<CevaplarModel> CevapListeBymakaleId(int soru_id)
        {
            List<CevaplarModel> liste = db.cevaplar.Where(s => s.soru_id == soru_id).Select(x => new CevaplarModel()
            {
                cevap_id = x.cevap_id,
                cevap = x.cevap,
                soru_id = x.soru_id,
                uye_id = x.uye_id,
                kullaniciAdi = x.uyeler.uye_kullanici_adi,
                soruİcerik = x.sorular.soru,

            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/cevaplistesoneklenenler/{s}")]
        public List<CevaplarModel> CevapListeSonEklenenler(int s)
        {
            List<CevaplarModel> liste = db.cevaplar.OrderByDescending(o => o.soru_id).Take(s).Select(x => new CevaplarModel()
            {
                cevap_id = x.cevap_id,
                cevap = x.cevap,
                soru_id = x.soru_id,
                uye_id = x.uye_id,
                kullaniciAdi = x.uyeler.uye_kullanici_adi,
                soruİcerik = x.sorular.soru,

            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/cevapbyid/{cevap_id}")]

        public CevaplarModel YorumById(int cevap_id)
        {
            CevaplarModel kayit = db.cevaplar.Where(s => s.cevap_id == cevap_id).Select(x => new CevaplarModel()
            {
                cevap_id = x.cevap_id,
                cevap = x.cevap,
                soru_id = x.soru_id,
                uye_id = x.uye_id,
                kullaniciAdi = x.uyeler.uye_kullanici_adi,
                soruİcerik = x.sorular.soru,
            }).SingleOrDefault();

            return kayit;
        }

        [HttpPost]
        [Route("api/cevapekle")]
        public SonucModel YorumEkle(CevaplarModel model)
        {
            if (db.cevaplar.Count(s => s.uye_id == model.uye_id && s.soru_id == model.soru_id && s.cevap == model.cevap) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Aynı Kişi, Aynı Soruya Aynı Cevabı Yapamaz!";
                return sonuc;
            }

            cevaplar yeni = new cevaplar();
            yeni.cevap_id = model.cevap_id;
            yeni.cevap = model.cevap;
            yeni.soru_id = model.soru_id;
            yeni.uye_id = model.uye_id;
            db.cevaplar.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Cevap Eklendi";

            return sonuc;
        }
        [HttpPut]
        [Route("api/cevapduzenle")]
        public SonucModel CevapDuzenle(CevaplarModel model)
        {

            cevaplar kayit = db.cevaplar.Where(s => s.cevap_id == model.cevap_id).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.cevap_id = model.cevap_id;
            kayit.cevap = model.cevap;
            kayit.soru_id = model.soru_id;
            kayit.uye_id = model.uye_id;

            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Cevap Düzenlendi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/cevapsil/{cevap_id}")]
        public SonucModel YorumSil(int cevap_id)
        {
            cevaplar kayit = db.cevaplar.Where(s => s.cevap_id == cevap_id).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            db.cevaplar.Remove(kayit);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Cevap Silindi";

            return sonuc;
        }


        #endregion
    }
}
