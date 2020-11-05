using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageConsommer.Models
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
