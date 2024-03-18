using System;

namespace DefinitelyNotStealing.Models.Entities;

public class ExfiltratedData
{
    public int ID { get; set; }
    public DateTime Timestamp { get; set; }
    public string ClientIP { get; set; }
    public string DataType { get; set; }
    public string Data { get; set; }
    public Guid CorrelationId { get; set; }
}
