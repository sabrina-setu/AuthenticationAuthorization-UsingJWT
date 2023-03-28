namespace JWT.Core.Entities.Common;

public abstract class Auditable
{
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAtUTC { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAtUTC { get; set; }

}
