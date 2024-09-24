namespace ShopDGHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CtName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(13)]
        public string Phone { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        public string Content { get; set; }
    }
}
