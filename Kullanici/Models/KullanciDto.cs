using System.ComponentModel.DataAnnotations;

namespace Kullanici.Models
{
    public class KullanciDto
    {
        [Required]
        public string ad { get; set; } = "";
        [Required]
        public string soyad { get; set; } = "";
        [Required]
        public string telefon { get; set; } = "";
        [Required]
        public string sehir { get; set; } = ""; 
    }
}
