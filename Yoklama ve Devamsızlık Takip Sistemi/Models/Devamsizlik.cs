using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevamsizlikTakip.Models
{
    public class Devamsizlik
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Öğrenci/Çalışan seçimi zorunludur")]
        [Display(Name = "Öğrenci/Çalışan")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
        [Display(Name = "Başlangıç Tarihi")]
        [DataType(DataType.Date)]
        public DateTime BaslangicTarihi { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Bitiş tarihi zorunludur")]
        [Display(Name = "Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime BitisTarihi { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Devamsızlık türü seçimi zorunludur")]
        [Display(Name = "Devamsızlık Türü")]
        public DevamsizlikTuru DevamsizlikTipi { get; set; }

        [Display(Name = "Açıklama")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string? Aciklama { get; set; }

        [Display(Name = "Onay Durumu")]
        public bool Onaylandi { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }

    public enum DevamsizlikTuru
    {
        [Display(Name = "Hastalık")]
        Hastalik,
        
        [Display(Name = "İzin")]
        Izin,
        
        [Display(Name = "Rapor")]
        Rapor,
        
        [Display(Name = "Diğer")]
        Diger
    }
} 