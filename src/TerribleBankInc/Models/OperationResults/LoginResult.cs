using TerribleBankInc.Models.Dtos;

namespace TerribleBankInc.Models.OperationResults;

public class LoginResult : OperationResult
{
    public ClientUser ClientUser { get; set; }
}
