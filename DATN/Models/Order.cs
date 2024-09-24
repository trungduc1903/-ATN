namespace ShopDGHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }

        public int? UserId { get; set; }

        public int PaymentId { get; set; }

        [StringLength(200)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(13)]
        public string Phone { get; set; }

        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual User User { get; set; }
    }
}
