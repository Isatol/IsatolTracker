using System;
using System.Net.Http;
using System.Text;
using System.Web;

namespace TestTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            Isatol.Tracker.Track track = new Isatol.Tracker.Track(httpClient);
            //var estafeta = track.Estafeta("");
            //var ups = track.UPS("", Isatol.Tracker.Track.Locale.es_MX);
            //var dhl = track.DHL("HKAXGBG2007082185125", Isatol.Tracker.Track.Locale.en_US);
            var key = Encoding.UTF8.GetBytes("brwZ7HaGcscE@J4hQR%zQjB^yr%2x6DNbCo2ksxR8c@par9*La#$fN4yjUMFWrpWJzrNYbni57ysf743yk$r%@9imT#zuoF5N%tVAN@KpXpZ8STx3En!kY3mXWPLNxko");
            var data = Encrypt.AES.Encrypt("Colima");
            var dataDecrypted = Encrypt.AES.Decrypt(data);

            byte[] _key = { 1, 22, 19, 111, 24, 26,
           85, 45, 114, 184, 27, 111, 37, 112, 100, 200, 241,
           24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218,
           131, 236, 53, 209 };
          var key2 =  Encoding.UTF8.GetString(_key);
            var key3 =HttpUtility.HtmlDecode(key2);
        }
    }
}
