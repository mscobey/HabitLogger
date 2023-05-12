using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabitLogger
{
    public class HabitModel
    {
        public int Id { get; set; } 
        
        public string HabitName {  get; set; }

        public int Quantity { get; set; }  

        public string Units { get; set; }   
    }
}
