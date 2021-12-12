using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models
{

    [Table("Hoteli")]
    public class Hotel
    {
        [Key]
        public int HotelID { get; set; }

        [MaxLength(70)]
        [Required]
        public string Naziv { get; set; }

        [MaxLength(200)]
        public string Lokacija { get; set; }

        [Range(1, 40)]
        [Required]
        public int BrojSpratova { get; set; }

        [Range(1, 20)]
        [Required]
        public int BrojSobaPoSpratu { get; set; }

        [Required]
        public int Cena_I_kat { get; set; }

        [Required]
        public int Cena_II_kat { get; set; }

        [Required]
        public int Cena_III_kat { get; set; }

        public List<Soba> Sobe { get; set; }

        [JsonIgnore]
        public List<Gost> Gosti { get; set;}

        public List<Recepcioner> Recepcioneri { get; set; }

    }
}