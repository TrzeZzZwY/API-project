using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models.Enums
{
    public enum Status
    {
        // jak będzie błąd to tu
        [Display(Name = "public")] public_publish = 0,
        [Display(Name = "private")] private_publish = 1
    }
}
