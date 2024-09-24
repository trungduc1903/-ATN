namespace ShopDGHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Promotion
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        public bool ApplyForAll { get; set; }

        public double? DiscountPercent { get; set; }

        public double? DiscountAmount { get; set; }

        public int? ProductId { get; set; }

        public int? BrandId { get; set; }

        [StringLength(200)]
        public string ProName { get; set; }

        public bool? Active { get; set; }
    }
}
