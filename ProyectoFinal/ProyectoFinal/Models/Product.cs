using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
