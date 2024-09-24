namespace ShopDGHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductImage")]
    public partial class ProductImage
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }

        public bool? IsDefault { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreateDate { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
