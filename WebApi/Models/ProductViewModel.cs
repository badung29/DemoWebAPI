using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ProductViewModel
    {
        public long ID { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = "Code is required!")]
        public string Code { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "Title is required!")]
        public string MetaTitle { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Quantity is required!")]
        public int? Quantity { get; set; }
      
    }
}