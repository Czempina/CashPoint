using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashPoint.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [Range(0, int.MaxValue)]
        public int AccountBalance { get; set; }
    }
}
