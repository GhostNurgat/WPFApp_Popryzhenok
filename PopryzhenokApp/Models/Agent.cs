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

        [Required(ErrorMessage = "������� �������� ������, ����������!")]
        [StringLength(150, ErrorMessage = "� ���� \"�������� ������\" ��������� ����. ����� ������")]
        public string Title { get; set; }

        public int AgentTypeID { get; set; }

        [StringLength(300, ErrorMessage = "� ���� \"�����\" ��������� ����. ����� ������")]
        public string Address { get; set; }

        [Required(ErrorMessage = "������� ���, ����������!")]
        [StringLength(12, ErrorMessage = "����� ��� �� ������ ��������� ������ 12")]
        [MinLength(12, ErrorMessage = "�� ������������ ������ ���")]
        public string INN { get; set; }

        [StringLength(9, ErrorMessage = "����� ��� �� ������ ��������� ������ 9")]
        [MinLength(9, ErrorMessage = "�� ������������ ������ ���")]
        public string KPP { get; set; }

        [StringLength(100, ErrorMessage = "� ���� \"��� ���������\" ��������� ����. ����� ������")]
        public string DirectorName { get; set; }

        [Required(ErrorMessage = "������� ����� ��������, ����������!")]
        [StringLength(20, ErrorMessage = "� ���� \"����� ��������\" ��������� ����. ����� ������")]
        public string Phone { get; set; }

        [StringLength(255, ErrorMessage = "� ���� \"Email\" ��������� ����. ����� ������")]
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
