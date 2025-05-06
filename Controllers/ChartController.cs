using ChartJsVeriGorsellestirme.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChartJsVeriGorsellestirme.Controllers
{
    public class ChartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // View sayfasýný döner
        public IActionResult Index()
        {
            return View();
        }

        // En çok çýkan sayýlarý JSON olarak döner
        [HttpGet]
        public IActionResult EnCokCikanSayilar()
        {
            var frekans = new Dictionary<int, int>();

            var tumCekilisler = _context.SuperLotoSonuclari.ToList();

            foreach (var cekilis in tumCekilisler)
            {
                var sayilar = cekilis.Sayilar
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s.Trim()));

                foreach (var sayi in sayilar)
                {
                    if (!frekans.ContainsKey(sayi))
                        frekans[sayi] = 1;
                    else
                        frekans[sayi]++;
                }
            }

            // En çok çýkanlara göre sýrala
            var sonuc = frekans.OrderByDescending(x => x.Value)
                               .ToDictionary(x => x.Key, x => x.Value);

            return Json(sonuc);
        }

        [HttpGet]
        public IActionResult SayiAralikDagilimi()
        {
            var araliklar = new Dictionary<string, int>
                            {
                                { "1–10", 0 },
                                { "11–20", 0 },
                                { "21–30", 0 },
                                { "31–40", 0 },
                                { "41–50", 0 },
                                { "51–60", 0 }
                            };

            var tumCekilisler = _context.SuperLotoSonuclari.ToList();

            foreach (var cekilis in tumCekilisler)
            {
                var sayilar = cekilis.Sayilar
                    .Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s.Trim()));

                foreach (var sayi in sayilar)
                {
                    if (sayi >= 1 && sayi <= 10) araliklar["1–10"]++;
                    else if (sayi <= 20) araliklar["11–20"]++;
                    else if (sayi <= 30) araliklar["21–30"]++;
                    else if (sayi <= 40) araliklar["31–40"]++;
                    else if (sayi <= 50) araliklar["41–50"]++;
                    else if (sayi <= 60) araliklar["51–60"]++;
                }
            }

            return Json(araliklar);
        }
        [HttpGet]
        public IActionResult TekCiftDagilimi()
        {
            int tekSayilar = 0;
            int ciftSayilar = 0;

            var tumCekilisler = _context.SuperLotoSonuclari.ToList();

            foreach (var cekilis in tumCekilisler)
            {
                var sayilar = cekilis.Sayilar
                    .Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s.Trim()));

                foreach (var sayi in sayilar)
                {
                    if (sayi % 2 == 0)
                        ciftSayilar++;
                    else
                        tekSayilar++;
                }
            }

            var sonuc = new Dictionary<string, int>
                        {
                            { "Tek", tekSayilar },
                            { "Çift", ciftSayilar }
                        };

            return Json(sonuc);
        }
        [HttpGet]
        public IActionResult YillikDagilim()
        {
            var yillikToplam = new Dictionary<int, int>();

            var tumCekilisler = _context.SuperLotoSonuclari.ToList();

            foreach (var cekilis in tumCekilisler)
            {
                int yil = cekilis.Tarih.Year;

                var sayilar = cekilis.Sayilar
                    .Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s.Trim()));

                int sayiAdedi = sayilar.Count();

                if (!yillikToplam.ContainsKey(yil))
                    yillikToplam[yil] = sayiAdedi;
                else
                    yillikToplam[yil] += sayiAdedi;
            }

            var sirali = yillikToplam.OrderBy(x => x.Key)
                                     .ToDictionary(x => x.Key.ToString(), x => x.Value);

            return Json(sirali);
        }
        [HttpGet]
        public IActionResult KombinasyonDagilimi()
        {
            var kombinasyonFrekans = new Dictionary<string, int>();

            var tumCekilisler = _context.SuperLotoSonuclari.ToList();

            foreach (var cekilis in tumCekilisler)
            {
                var sayilar = cekilis.Sayilar
                    .Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.Parse(s.Trim()))
                    .OrderBy(n => n)
                    .ToList();

                for (int i = 0; i < sayilar.Count; i++)
                {
                    for (int j = i + 1; j < sayilar.Count; j++)
                    {
                        string key = $"{sayilar[i]}-{sayilar[j]}";

                        if (kombinasyonFrekans.ContainsKey(key))
                            kombinasyonFrekans[key]++;
                        else
                            kombinasyonFrekans[key] = 1;
                    }
                }
            }

            // En çok tekrar eden ilk 10 kombinasyon
            var enCokCikan = kombinasyonFrekans
                .OrderByDescending(x => x.Value)
                .Take(10)
                .ToDictionary(x => x.Key, x => x.Value);

            return Json(enCokCikan);
        }


    }
}
