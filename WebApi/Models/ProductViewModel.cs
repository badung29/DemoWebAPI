using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class ProductViewModel
    {
        public long ID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        public decimal? Price { get; set; }

        public int Quantity { get; set; }
    }
}