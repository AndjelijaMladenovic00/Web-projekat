using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Recepcioneri")]
    public class Recepcioner
    {
        [Key]
        public int RecepcionerID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Prezime { get; set; }

        [Required]
        [MaxLength(5)]
        [MinLength(5)]
        public string ID_kartica { get; set; }

        [JsonIgnore]
        public Hotel Hotel { get; set; }

        public List<Soba> IzdateSobe { get; set; }
    }
}