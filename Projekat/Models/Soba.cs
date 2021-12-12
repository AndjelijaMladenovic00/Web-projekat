using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Sobe")]
    public class Soba
    {
        [Key]
        public int SobaID { get; set; }

        [Required]
        public int BrojSobe { get; set; }

        [Required]
        [Range(1,4)]
        public int BrojKreveta { get; set; }

        [Required]
        [Range(1,3)]
        public int Kategorija { get; set; }

        [Required]
        public bool Izdata { get; set; }

        [JsonIgnore]
        public Hotel Hotel { get; set; }

        [JsonIgnore]
        public Recepcioner Recepcioner { get; set; }

        [JsonIgnore]
        public Gost Gost { get; set; }


    }
}