using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;
using Microsoft.FeatureManagement;
namespace Flagcraft;
public class LaunchDarklyFeatureDefinitionProvider(LdClient client) : IFeatureDefinitionProvider
{
    public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
    {
        yield break;
    }

    public async Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
    {
        var context = Context.Builder("default-user").Build();
        var isEnabled = client.BoolVariation(featureName, context, false);
       
        return new FeatureDefinition
        {
            Name = featureName,
            EnabledFor = isEnabled ? new List<FeatureFilterConfiguration> { } : new List<FeatureFilterConfiguration>()
        };
    }
}