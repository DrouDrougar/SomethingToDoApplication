using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SomethingToDoApplication.Models
{
    [Table("shoppingTaskModel")]
    public class ShoppingTaskModel
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("shopping_name")]
        public string ShoppingName { get; set; }

        [Column("description")]
        public string ShoppingDescription { get; set; }

        [Column("quantity")]
        public int ShoppingQuantity { get; set; }
    }
}
