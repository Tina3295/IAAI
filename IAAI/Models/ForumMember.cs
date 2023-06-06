using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class ForumMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int ForumMemberId { get; set; }


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
        [Display(Name = "姓名")]
        public string Name { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "性別")]
        public EnumList.GenderType Gender { get; set; }


        [Display(Name = "生日")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]  //只顯示年月日
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "申請類別")]
        public EnumList.ApplicationType ApplicationType { get; set; }


        [MaxLength(20)]
        [Display(Name = "連絡電話(公)")]
        public string BusinessPhone { get; set; }


        [MaxLength(20)]
        [Display(Name = "手機")]
        public string CellPhone { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(100)]
        [Display(Name = "通訊處")]
        public string Address { get; set; }


        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "Email格式不符")]
        [MaxLength(100)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "國際會籍")]
        public string Membership { get; set; }


        [MaxLength(50)]
        [Display(Name = "現職單位")]
        public string NowUnit { get; set; }


        [MaxLength(30)]
        [Display(Name = "職稱")]
        public string JobTitle { get; set; }


        [MaxLength(30)]
        [Display(Name = "最高學歷")]
        public string HighestEducation { get; set; }


        [Display(Name = "帳號建立日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]  //只顯示年月日
        [DataType(DataType.DateTime)]
        public DateTime? InitDate { get; set; }




        [MaxLength(50)]
        [Display(Name = "服務單位1")]
        public string HistoryUnit1 { get; set; }


        [MaxLength(30)]
        [Display(Name = "職稱1")]
        public string HistoryJobTitle1 { get; set; }


        [RegularExpression(@"^(19|20)\d{2}$", ErrorMessage = "年份須介於1900年~2099年之間")]
        [Display(Name = "起年1")]
        public int? StartYear1 { get; set; }


        [Range(1, 12, ErrorMessage = "月份須介於1~12之間")]
        [Display(Name = "起月1")]
        public int? StartMonth1 { get; set; }


        [RegularExpression(@"^(19|20)\d{2}$", ErrorMessage = "年份須介於1900年~2099年之間")]
        [Display(Name = "迄年1")]
        public int? EndYear1 { get; set; }


        [Range(1, 12, ErrorMessage = "月份須介於1~12之間")]
        [Display(Name = "迄月1")]
        public int? EndMonth1 { get; set; }




        [MaxLength(50)]
        [Display(Name = "服務單位2")]
        public string HistoryUnit2 { get; set; }


        [MaxLength(30)]
        [Display(Name = "職稱2")]
        public string HistoryJobTitle2 { get; set; }


        [RegularExpression(@"^(19|20)\d{2}$", ErrorMessage = "年份須介於1900年~2099年之間")]
        [Display(Name = "起年2")]
        public int? StartYear2 { get; set; }


        [Range(1, 12, ErrorMessage = "月份須介於1~12之間")]
        [Display(Name = "起月2")]
        public int? StartMonth2 { get; set; }


        [RegularExpression(@"^(19|20)\d{2}$", ErrorMessage = "年份須介於1900年~2099年之間")]
        [Display(Name = "迄年2")]
        public int? EndYear2 { get; set; }


        [Range(1, 12, ErrorMessage = "月份須介於1~12之間")]
        [Display(Name = "迄月2")]
        public int? EndMonth2 { get; set; }




        [MaxLength(50)]
        [Display(Name = "服務單位3")]
        public string HistoryUnit3 { get; set; }


        [MaxLength(30)]
        [Display(Name = "職稱3")]
        public string HistoryJobTitle3 { get; set; }


        [RegularExpression(@"^(19|20)\d{2}$", ErrorMessage = "年份須介於1900年~2099年之間")]
        [Display(Name = "起年3")]
        public int? StartYear3 { get; set; }


        [Range(1, 12, ErrorMessage = "月份須介於1~12之間")]
        [Display(Name = "起月3")]
        public int? StartMonth3 { get; set; }


        [RegularExpression(@"^(19|20)\d{2}$", ErrorMessage = "年份須介於1900年~2099年之間")]
        [Display(Name = "迄年3")]
        public int? EndYear3 { get; set; }


        [Range(1, 12, ErrorMessage = "月份須介於1~12之間")]
        [Display(Name = "迄月3")]
        public int? EndMonth3 { get; set; }


        public virtual ICollection<Forums> Forums { get; set; }
    }
}