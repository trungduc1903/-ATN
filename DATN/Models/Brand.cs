namespace ShopDGHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Brand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string NameBrands { get; set; }

        [StringLength(500)]
        public string ImageBrands { get; set; }

        public bool? Active { get; set; }

        public string Describe { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public int? ChiNhanh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
