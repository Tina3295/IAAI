using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class Permission
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }



        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(50)]
        [Display(Name = "權限名稱")]
        public string Subject { get; set; }



        [Display(Name = "上層選單")]
        public int? RootId { get; set; }

        [ForeignKey("RootId")]
        public virtual Permission RootPermission { get; set; }



        [MaxLength(5)]
        [Display(Name = "權限代號")]
        public string Code { get; set; }


        [MaxLength(500)]
        [Display(Name = "URL")]
        public string URL { get; set; }


        public virtual ICollection<Permission> Permissions { get; set; }
    }
}