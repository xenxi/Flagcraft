using Microsoft.FeatureManagement;

namespace Flagcraft;

public static class FeatureToggleEndpoints
{
    public static void RegisterFeatureToggleEndpoints(this WebApplication app)
    {
        app.MapGet("/check-feature/{featureName}", async (string featureName, IFeatureManager featureManager) =>
        {
            var isEnabled = await featureManager.IsEnabledAsync(featureName);
            return Results.Ok(new { feature = featureName, enabled = isEnabled });
        });
    }
}