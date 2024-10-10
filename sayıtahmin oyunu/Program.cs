using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sayıtahmin_oyunu
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int toplamTahmin = 0;
            int toplamYanlisTahmin = 0;
            int oyunSayisi = 0;
            int kazanilanOyunSayisi = 0;
            int enYuksekPuan = 0;
            double toplamPuan = 0;

            Random rastgele = new Random();
            bool oyunDevam = true;

            while (oyunDevam)
            {
                Console.WriteLine("Lütfen bir alt sınır girin:");
                int altSinir = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Lütfen bir üst sınır girin:");
                int ustSinir = Convert.ToInt32(Console.ReadLine());

                while (altSinir >= ustSinir)
                {
                    Console.WriteLine("Hata: Alt sınır, üst sınırdan küçük olmalıdır. Lütfen tekrar girin.");
                    Console.WriteLine("Lütfen bir alt sınır girin:");
                    altSinir = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Lütfen bir üst sınır girin:");
                    ustSinir = Convert.ToInt32(Console.ReadLine());
                }

                Console.WriteLine($"Rastgele sayı belirlendi.");
                int puan = 30;
                int rastgeleSayi = rastgele.Next(altSinir, ustSinir + 1);
                int geriSayimSuresi = 90;
                bool isTimeOut = false;
                int kalanSüre = geriSayimSuresi;

                Thread geriSayimThread = new Thread(() => GeriSayim(geriSayimSuresi, ref isTimeOut, ref kalanSüre));
                geriSayimThread.Start();

                bool kazandiMi = false;

                while (puan > 0 && !isTimeOut)
                {
                    Console.WriteLine("Tahmininizi giriniz: ");
                    int tahmin = Convert.ToInt32(Console.ReadLine());

                    if (tahmin < altSinir || tahmin > ustSinir)
                    {
                        Console.WriteLine($"Lütfen {altSinir} ile {ustSinir} arasında bir sayı girin.");
                        continue;
                    }

                    toplamTahmin++;

                    if (tahmin > rastgeleSayi)
                    {
                        puan -= 3;
                        toplamYanlisTahmin++;
                        Console.WriteLine("Tahmininiz yüksek. Kalan puan: " + puan);
                    }
                    else if (tahmin < rastgeleSayi)
                    {
                        puan -= 3;
                        toplamYanlisTahmin++;
                        Console.WriteLine("Tahmininiz düşük. Kalan puan: " + puan);
                    }
                    else
                    {
                        Console.WriteLine($"Tebrikler! Doğru tahmin ettiniz: {rastgeleSayi} Toplam puanınız: {puan}");
                        kazanilanOyunSayisi++;
                        kazandiMi = true;
                        break;
                    }

                    if (puan > 0 && !isTimeOut)
                    {
                        Console.WriteLine($"Geri sayım devam ediyor... Kalan süre: {kalanSüre} saniye");
                    }

                    if (puan <= 0)
                    {
                        Console.WriteLine("Puanınız bitti! Oyunu kaybettiniz. Doğru sayı: " + rastgeleSayi);
                    }
                }

                geriSayimThread.Join();

                oyunSayisi++;
                toplamPuan += puan;
                if (puan > enYuksekPuan)
                {
                    enYuksekPuan = puan;
                }

                Console.WriteLine($"\nOyun İstatistikleri:");
                Console.WriteLine($"Toplam Tahmin: {toplamTahmin}");
                Console.WriteLine($"Yanlış Tahmin: {toplamYanlisTahmin}");
                Console.WriteLine($"Oynanan Toplam Oyun Sayısı: {oyunSayisi}");
                Console.WriteLine($"Kazanılan Oyun Sayısı: {kazanilanOyunSayisi}");
                Console.WriteLine($"En Yüksek Puan: {enYuksekPuan}");
                Console.WriteLine($"Ortalama Puan: {(oyunSayisi > 0 ? toplamPuan / oyunSayisi : 0):F2}\n");

                Console.WriteLine("Tekrar oynamak ister misiniz? (Evet/Hayır)");

                string cevap = Console.ReadLine();

                if (cevap?.ToLower() != "evet")
                {
                    oyunDevam = false;
                }

                //if (kazandiMi)
                //{
                //    Console.WriteLine("Tekrar oynamak ister misiniz? (Evet/Hayır)");
                //    string cevap = Console.ReadLine();
                //    if (cevap?.ToLower() != "evet")
                //    {
                //        oyunDevam = true;
                //    }
                //    return;
                //}
                //else
                //{
                //    Console.WriteLine("Tekrar oynamak ister misiniz? (Evet/Hayır)");
                //    string cevap = Console.ReadLine();
                //    if (cevap?.ToLower() != "hayır")
                //    {
                //        oyunDevam = false;
                //    }
                //}

            }

            Console.WriteLine($"\nOyun sona erdi. İstatistikler:");
            Console.WriteLine($"Oynadığınız toplam oyun sayısı: {oyunSayisi}");
            Console.WriteLine($"Kazanılan oyun sayısı: {kazanilanOyunSayisi}");
            Console.WriteLine($"En yüksek puan: {enYuksekPuan}");
            Console.WriteLine($"Ortalama puan: {(oyunSayisi > 0 ? toplamPuan / oyunSayisi : 0):F2}");
        }

        static void GeriSayim(int sure, ref bool isTimeOut, ref int kalanSüre)
        {
            for (int i = sure; i > 0; i--)
            {
                Thread.Sleep(1000);
                kalanSüre--;
            }
            isTimeOut = true;
        }
    }
}
