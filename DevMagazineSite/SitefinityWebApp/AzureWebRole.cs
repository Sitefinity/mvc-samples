using Telerik.Sitefinity.Cloud.WindowsAzure;

namespace SitefinityWebApp
{
    /// <summary>
    /// Sitefinity's Windows Azure role entry point.
    /// </summary>
    /// <remarks>
    /// Windows Azure looks for RoleEntryPoint inheritors only in the web role assembly and that is why this class is defined here.
    /// The functionality goes into the base class - <see cref="AzureWebRoleBase"/>.
    /// </remarks>
    public class AzureWebRole : AzureWebRoleBase
    {
    }
}
