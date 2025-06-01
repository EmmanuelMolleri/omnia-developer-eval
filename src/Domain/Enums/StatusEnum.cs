using System.ComponentModel;

namespace Domain.Enums;

public enum StatusEnum
{
    [Description("Active")]
    Active,
    [Description("Inactive")]
    Inactive,
    [Description("Suspended")]
    Suspended
}
