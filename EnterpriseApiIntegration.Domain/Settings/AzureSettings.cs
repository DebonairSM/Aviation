namespace EnterpriseApiIntegration.Domain.Settings;

public class AzureSettings
{
    public string SubscriptionId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string ResourceGroupName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string KeyVaultName { get; set; } = string.Empty;
    public string SqlServerName { get; set; } = string.Empty;
    public string SqlDatabaseName { get; set; } = string.Empty;
    public string AppServiceName { get; set; } = string.Empty;
} 