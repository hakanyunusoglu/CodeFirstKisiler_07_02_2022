using CodeFirstKisiler.Models;
using CodeFirstKisiler.Models.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeFirstKisiler.Controllers
{
    public class AdresController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        public ActionResult YeniAdres()
        {
            List<SelectListItem> kisilerList = db.Kisiler.Select(x => new SelectListItem
            {
                Text = string.Concat(x.Ad + " " + x.Soyad),
                Value = x.ID.ToString()
            }).ToList();
            TempData["Kisiler"] = kisilerList;
            ViewBag.kisiler = kisilerList;
            return View();
        }
        [HttpPost]
        public ActionResult YeniAdres(Adres adres)
        {
            Kisiler kisi = db.Kisiler.FirstOrDefault(x => x.ID == adres.KisiId);
            if (kisi != null)
            {
                adres.Kisiler = kisi;
                db.Adresler.Add(adres);
                
                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                {
                    ViewBag.Result = "Adres başarıyla eklendi";
                    ViewBag.Status = "success";

                }
                else
                {
                    ViewBag.Result = "Adres eklenememiştir";
                    ViewBag.Status = "danger";
                }
                ViewBag.kisiler = TempData["Kisiler"];
            }
            return View();
        }
        public ActionResult Duzenle(int? adresid)
        {
            Adres adres = db.Adresler.FirstOrDefault(x=>x.ID == adresid);
            Kisiler kisi = db.Kisiler.FirstOrDefault(x=>x.ID == adres.Kisiler.ID);
            List<SelectListItem> kisilerList = db.Kisiler.Select(x => new SelectListItem
            {
                Text = string.Concat(x.Ad + " " + x.Soyad),
                Value = x.ID.ToString(),
                Selected = false
            }).ToList();
           
            foreach(var item in kisilerList)
            {

                if (item.Value == kisi.ID.ToString())
                {
                    item.Selected = true;

                }
            }
            TempData["Kisiler"] = kisilerList;
            ViewBag.kisiler = kisilerList;
            return View(adres);
        }
        [HttpPost]
        public ActionResult Duzenle(Adres model,int? adresid)
        {
            Adres adres = db.Adresler.FirstOrDefault(x => x.ID == adresid);
            if (adres != null)
            {
                adres.AdresTanim = model.AdresTanim;
                adres.KisiId = model.KisiId;
                adres.Kisiler = model.Kisiler;
                int sonuc = db.SaveChanges();
                if (sonuc > 0)
                {
                    ViewBag.Result = "Güncelleme işlemi başarıyla tamamlandı!";
                    ViewBag.Status = "success";
                }
                else
                {
                    ViewBag.Result = "Güncelleme işlemi başarısız oldu!";
                    ViewBag.Status = "danger";
                }
            }
            return View();
        }
        public ActionResult Sil(int? adresid)
        {
            Adres adres = new Adres();
            if (adresid != null)
            {
                adres = db.Adresler.FirstOrDefault(x => x.ID == adresid);
            }
            return View(adres);
        }
        [HttpPost, ActionName("Sil")]
        public ActionResult SilOk(int? adresid)
        {
            Adres adres = new Adres();
            if (adresid != null)
            {
                adres = db.Adresler.FirstOrDefault(x => x.ID == adresid);
                db.Adresler.Remove(adres);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}