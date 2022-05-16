namespace PopryzhenokApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Agent")]
    public partial class Agent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agent()
        {
            AgentPriorityHistory = new HashSet<AgentPriorityHistory>();
            ProductSale = new HashSet<ProductSale>();
            Shop = new HashSet<Shop>();
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "Введите название агента, пожалуйста!")]
        [StringLength(150, ErrorMessage = "В поле \"Название агента\" превышено макс. длины строки")]
        public string Title { get; set; }

        public int AgentTypeID { get; set; }

        [StringLength(300, ErrorMessage = "В поле \"Адрес\" превышено макс. длины строки")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Введите ИНН, пожалуйста!")]
        [StringLength(12, ErrorMessage = "Номер ИНН не должен превышать больше 12")]
        [MinLength(12, ErrorMessage = "Не соотвествует номеру ИНН")]
        public string INN { get; set; }

        [StringLength(9, ErrorMessage = "Номер КПП не должен превышать больше 9")]
        [MinLength(9, ErrorMessage = "Не соотвествует номеру КПП")]
        public string KPP { get; set; }

        [StringLength(100, ErrorMessage = "В поле \"Имя директора\" превышено макс. длины строки")]
        public string DirectorName { get; set; }

        [Required(ErrorMessage = "Введите номер телефона, пожалуйста!")]
        [StringLength(20, ErrorMessage = "В поле \"Номер телефона\" превышено макс. длины строки")]
        public string Phone { get; set; }

        [StringLength(255, ErrorMessage = "В поле \"Email\" превышено макс. длины строки")]
        public string Email { get; set; }

        [StringLength(100)]
        public string Logo { get; set; }

        public int Priority { get; set; }

        public virtual AgentType AgentType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AgentPriorityHistory> AgentPriorityHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductSale> ProductSale { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shop> Shop { get; set; }
    }
}
