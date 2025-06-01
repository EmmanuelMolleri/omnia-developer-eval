using System.ComponentModel;

namespace Domain.Enums;

public enum OrderEnums
{
    [Description("id")]
    id,
    [Description("desc")]
    desc,
    [Description("userId")]
    userId,
    [Description("asc")]
    asc,
    [Description("title")]
    title,
    [Description("CartId")]
    CartId,
    [Description("UserId")]
    User
}
