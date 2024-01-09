using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace labWork3.Models
{
    public class Contact
    {
        public int Id { set; get; } = 0!;
       
        public string FirstName { get; set; } = null!;
        
        public string LastName { get; set; } = null!;
        
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
