using System;

namespace TerribleBankInc.Models.OperationResults;

public class ForgotPasswordResult : OperationResult
{
    public string ForgotPasswordToken { get; set; }
    public DateTime ForgotPasswordExpiration { get; set; }
}
