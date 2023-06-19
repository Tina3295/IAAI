using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int AdminId { get; set; }



        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(30)]
        [Display(Name = "帳號")]
        public string Account { get; set; }



        [Required(ErrorMessage = "{0}必填")]
        [StringLength(100, ErrorMessage = "{0}長度至少必須為 {2} 個字元。", MinimumLength = 8)]
        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(10)]
        [Display(Name = "暱稱")]
        public string UserName { get; set; }



        [Display(Name = "頭貼")]
        public string ProfilePicture { get; set; }



        [Display(Name = "帳號建立日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]  //只顯示年月日
        [DataType(DataType.DateTime)]
        public DateTime? InitDate { get; set; }



        [MaxLength(500)]
        [Display(Name = "權限")]
        public string Permission { get; set; }
    }
}