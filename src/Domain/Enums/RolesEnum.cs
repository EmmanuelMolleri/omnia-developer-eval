using System.ComponentModel;

namespace Domain.Enums;

public enum RolesEnum
{
    [Description("Customer")]
    Customer,
    [Description("Manager")]
    Manager,
    [Description("Admin")]
    Admin
}
