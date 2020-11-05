using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProduitApi.Models
{
    [Table("produit")]
    public class ProduitItem
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated
(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Column("Nom")]
        [Required]
        [StringLength(25)]
        public string Nom { get; set; }
        [Column("Stock")]
        [Required]
        public int Stock { get; set; }
        [Column("Prix")]
        [Required]
        public float Prix { get; set; }
    }
}


