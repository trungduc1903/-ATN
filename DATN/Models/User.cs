namespace ShopDGHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Comments = new HashSet<Comment>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(200)]
        public string Fullname { get; set; }

        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Note { get; set; }

        public bool? Active { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
