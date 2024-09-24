namespace ShopDGHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public string Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }

        public int? UserId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}
