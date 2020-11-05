using System;
using System.Collections.Generic;
using System.Text;

namespace TraitementMessageConsommer.Models
{
    [Table("vente")]
    class VenteItem
    {
        [Column("Id")]
        [Key]
        [DatabaseGenerated
(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Column("idProduit")]
        [Required]
        public int IdProduit { get; set; }
        [Column("quantite")]
        [Required]
        public int Quantite { get; set; }

    }
}
