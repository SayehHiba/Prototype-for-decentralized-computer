using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MessageConsommer.Models
{
    [Table("vente")]
    public class VenteItem
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
