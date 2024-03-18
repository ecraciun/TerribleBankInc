using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TerribleBankInc.Models.ViewModels;

public class UserProfileViewModel
{
    public int ID { get; set; }

    [Required]
    public string Username { get; set; }
    public bool IsAdmin { get; set; }
}
