using System.Collections.Generic;
using TerribleBankInc.Models.Entities;

namespace TerribleBankInc.Models.ViewModels;

public class ClientBankAccountsViewModel
{
    public List<BankAccount> ActiveAccounts { get; set; }
    public List<BankAccount> DisabledAccounts { get; set; }
    public List<BankAccount> PendingAccounts { get; set; }
    public List<BankAccount> RejectedAccounts { get; set; }
}
