using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAI.Models
{
    public class EnumList
    {
        public enum GenderType
        {
            男,
            女
        }

        public enum ApplicationType
        {
            正式會員,
            準會員,
            個人贊助會員,
            學生會員
        }
    }
}