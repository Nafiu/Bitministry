using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BitministryBank.Models
{
    public class tranferamount
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Username")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Field")]
        public string To { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Currency")]
        [Range(1,100000,ErrorMessage ="Please Enter The Valid Currency")]
        [DataType(DataType.Currency)]
        public int Amount { get; set; }
    }
}