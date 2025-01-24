using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevamsizlikTakip.Models
{
    public class User
    {
        public User()
        {
            Devamsizliklar = new List<Devamsizlik>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "TC Kimlik No zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 karakter olmalıdır")]
        [Display(Name = "TC Kimlik No")]
        public string TCKimlik { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        [Display(Name = "Telefon")]
        public string Telefon { get; set; }

        [Required(ErrorMessage = "Kullanıcı tipi seçilmelidir")]
        [Display(Name = "Kullanıcı Tipi")]
        public UserType KullaniciTipi { get; set; }

        [Display(Name = "Sınıf")]
        public string? Sinif { get; set; }

        [Display(Name = "Departman")]
        public string? Departman { get; set; }

        public virtual ICollection<Devamsizlik>? Devamsizliklar { get; set; }
    }

    public enum UserType
    {
        [Display(Name = "Öğrenci")]
        Ogrenci,
        
        [Display(Name = "Çalışan")]
        Calisan,
        
        [Display(Name = "Yönetici")]
        Admin
    }
}
