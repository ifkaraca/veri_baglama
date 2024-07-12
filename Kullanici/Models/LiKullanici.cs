using System.ComponentModel.DataAnnotations;

namespace Kullanici.Models
{
    public class LiKullanici
    {
      
        public int id { get; set; }
       
        public string ad { get; set; } 
       
        public string soyad { get; set; } 
      
        public string telefon { get; set; } 
     
        public string sehir { get; set; } 
    }
}
