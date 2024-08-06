using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.SymbolStore;
using System.Threading;

namespace vtislemleri
{

    class vtislemleri
    {   //veritabanı bağlantı nesnesi
        OleDbConnection bag = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source=DataBase3.mdb");
        //kullanılcak tablonun adı
        string tabloAdı = "personel";


        public char menu()
        {
            Console.Clear();
            Console.WriteLine("KAYIT İŞLEMLERİ\n------------------------\n");
            Console.WriteLine("1-Kayıtları Listele");
            Console.WriteLine("2-Kayıt Ara");
            Console.WriteLine("3-Kayıt Ekle");
            Console.WriteLine("4-Kayıt Güncelle");
            Console.WriteLine("5-Kayıt Sil");
            Console.WriteLine("6-ÇIKIŞ");
            char secenek = Console.ReadKey().KeyChar;
            return secenek;
        }
        public void kayitListele(string dep)
        {
            //veritabanı komutu oluşturma
            OleDbCommand komut = new OleDbCommand(" SELECT * FROM " + tabloAdı + " WHERE departman LIKE @dep ", bag);
            komut.Parameters.AddWithValue("@dep", dep);
            OleDbDataAdapter adp = new OleDbDataAdapter(komut);
            DataTable tablo = new DataTable();
            adp.Fill(tablo);
            ekrandaGoster(tablo);
        }
       public
        void ekrandaGoster(DataTable tablo)
        {
            Console.Clear(); // Ekranı temizler
            Console.WriteLine(" KAYIT LİSTESİ");
            Console.WriteLine(new string('>', Console.BufferWidth)); // Ekranın genişliği kadar '>' karakteri basar

            int ag = Console.BufferWidth / tablo.Columns.Count; // Her sütun için genişlik ayarla

            // Sütun başlıklarını yazdır
            for (int i = 0; i < tablo.Columns.Count; i++)
            {
                Console.Write(tablo.Columns[i].ColumnName.PadRight(ag));
            }
            Console.WriteLine("\n" + new string('-', Console.BufferWidth)); // Ekranın genişliği kadar '-' karakteri basar

            // Satırları yazdır
            foreach (DataRow row in tablo.Rows)
            {
                for (int i = 0; i < tablo.Columns.Count; i++)
                {
                    Console.Write(row[i].ToString().PadRight(ag));
                }
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', Console.BufferWidth)); // Ekranın genişliği kadar '-' karakteri basar
            Console.ReadLine(); // Kullanıcı bir tuşa basana kadar bekler
        }
        public void kayitAra()
        {
            Console.Clear();
            Console.WriteLine(" KAYIT ARAMA");
            Console.WriteLine(new string('-', Console.BufferWidth));
            Console.WriteLine("Aradığınz kaydın departmanını giriniz: ");
            string dep = Console.ReadLine();
            kayitListele(dep);
        }
        public void kayitEkle()
        {
            Console.Clear();
            Console.WriteLine(" YENİ KAYIT EKLEME ");
            Console.WriteLine(new string('-', Console.BufferWidth));
            Console.Write("Kaç Kayıt Ekleyeceksiniz: ");
            int ks = int.Parse(Console.ReadLine());
             for (int i = 0; i < ks; i++)
             {
                Console.Clear();
                Console.WriteLine(" YENİ KAYIT EKLEME");
                Console.WriteLine(new string('-', Console.BufferWidth)); // Ekranın genişliği kadar '-' karakteri basar

                // Yeni kayıt bilgilerini al
                Console.Write("KİMLİK: ");
                int kim = int.Parse(Console.ReadLine().ToUpper());
                Console.Write("AD: ");
                string ad = Console.ReadLine().ToUpper();
                Console.Write("SOYAD: ");
                string soyad = Console.ReadLine().ToUpper();
                Console.Write("ŞEHİR: ");
                string seh = Console.ReadLine().ToUpper();
                Console.Write("DEPARTMAN: ");
                string dep = Console.ReadLine().ToUpper();

                // veritabanı komutu oluşturma
                OleDbCommand komut = new OleDbCommand();
                komut.CommandText = "INSERT INTO " + tabloAdı + " VALUES(@kim,@a,@soy,@şeh,@dep)";//ekleme komutu
                komut.Connection = bag;
                komut.Parameters.AddWithValue("@kim", kim);
                komut.Parameters.AddWithValue("@a", ad);
                komut.Parameters.AddWithValue("@soy", soyad);
                komut.Parameters.AddWithValue("@şeh", seh);
                komut.Parameters.AddWithValue("@dep", dep);
                bag.Open(); // Veritabanı bağlantısını aç
                komut.ExecuteNonQuery(); // Komutu çalıştır (ekleme işlemi)
                bag.Close(); // Veritabanı bağlantısını kapat
                Console.WriteLine("Kayıt Eklendi! Personel Listesine Yönlendiriliyorsunuz ....");
                Thread.Sleep(1500); 
                kayitListele("%"); 
                Console.ReadKey();
             } 
        }
        public void kayitGuncelle()
        {
            Console.Clear(); // Ekranı temizler
            Console.Write("Güncellemek istediğiniz personelin kimliğini giriniz :");
            int kim = int.Parse(Console.ReadLine()); // Güncellenecek personel ID'sini al

            Console.WriteLine(new string('-', Console.BufferWidth)); 
            Console.Write("AD: ");
            string ad = Console.ReadLine().ToUpper();
            Console.Write("SOYAD: ");
            string soyad = Console.ReadLine().ToUpper();
            Console.Write("ŞEHİR: ");
            string seh = Console.ReadLine().ToUpper();
            Console.Write("DEPERTMAN: ");
            string dep = Console.ReadLine().ToUpper();

            OleDbCommand komut = new OleDbCommand();
            komut.CommandText = "UPDATE " + tabloAdı + " SET ad=@ad, soyad=@soy, şehir=@şeh, departman=@dep WHERE kimlik=@kim";
            komut.Connection = bag;
            komut.Parameters.AddWithValue("@kim", kim);
            komut.Parameters.AddWithValue("@a", ad);
            komut.Parameters.AddWithValue("@soy", soyad);
            komut.Parameters.AddWithValue("@şeh", seh);
            komut.Parameters.AddWithValue("@dep", dep);
            try
            {
                bag.Open(); // Veritabanı bağlantısını aç
                int rowsAffected = komut.ExecuteNonQuery(); // Komutu çalıştır (güncelleme işlemi) ve etkilenen satır sayısını al

                if (rowsAffected > 0) // Güncellenen satır sayısı 0'dan büyükse
                    Console.WriteLine("Güncelleme başarılı!  Personel Listesine Yönlendiriliyorsunuz ....");
                else
                    Console.WriteLine("Güncellenmek istenen personel bulunamadı."); // Güncellenen satır yoksa
            }
            catch (Exception ex) // Hata oluşursa
            {
                Console.WriteLine("Hata: " + ex.Message);    // Hata mesajını yazdır
            }
            finally
            {
                bag.Close(); // Veritabanı bağlantısını kapat
                komut.Dispose(); // OleDbCommand nesnesini serbest bırak
            }
            Thread.Sleep(2000); // 2 saniye bekler
            kayitListele("%"); // Tüm kayıtları listele
        }
        public void kayitSil()
        {
            Console.Clear(); // Ekranı temizler
            Console.Write("Silmek istediginiz Kişinin Kimliğini giriniz :");
            int kim = int.Parse(Console.ReadLine()); // Silinecek personel ID'sini al

            // Veritabanı komutu oluştur ve parametreleri ekle
            using (OleDbCommand komut = new OleDbCommand())
            {
                komut.Connection = bag; // Komutun bağlantısını belirler
                komut.CommandText = "DELETE FROM Personel WHERE Kimlik = @kim"; // Silme sorgusunu ayarla
                komut.Parameters.AddWithValue("@kim", kim); // Personel ID parametresi ekle

                try
                {
                    bag.Open(); // Veritabanı bağlantısını aç
                    komut.ExecuteNonQuery(); // Komutu çalıştır (silme işlemi)
                    Console.WriteLine("Personel Kayıt Silindi Personel Listesine Yönlendiriliyorsunuz ....");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                }
                finally
                {
                    bag.Close(); // Veritabanı bağlantısını kapat
                }
            }
            Thread.Sleep(2000);
            kayitListele("%");
        }

        internal class Program
        {
            static void Main(string[] args)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                char s;
               vtislemleri islem = new vtislemleri(); // Islemler sınıfından bir nesne oluşturur
                do
                {
                    s = islem.menu();
                    switch (s)
                    {
                        case '1': islem.kayitListele("%"); break; // Tüm kayıtları listele
                        case '2': islem.kayitAra(); break; // Kayıt ara
                        case '3': islem.kayitEkle(); break; // Kayıt ekle
                        case '4': islem.kayitGuncelle(); break; // Kayıt güncelle
                        case '5': islem.kayitSil(); break; // Kayıt sil
                    }
                } while (s != '6'); // Çıkış seçeneği seçilene kadar devam et
            }
        }
    }
}