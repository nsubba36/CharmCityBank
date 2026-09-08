namespace CharmCityBank.Api.Domain.Audits;

public class AuditLog
{
    public long Id { get; set; }
    public string? IdentityUserId { get; set; }
    public AuditActionType ActionType { get; set; }
    public AuditEntityType EntityType { get; set; }
    public string? EntityId { get; set; }
    public string? Details { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}