using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Gosti")]
    public class Gost
    {
        [Key]
        public int GostID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Prezime { get; set; }

        [Required]
        [MaxLength(9)]
        [MinLength(9)]
        public string BrojLicneKarte { get; set; } //string jer moze da pocinje nulom

        public Hotel Hotel { get; set; }
        public List<Soba> Soba { get; set; } //moze da iznajmi vise soba
    }
}