using System;

using Hangfire;

using Microsoft.AspNetCore.Builder;

namespace RepositoryWithCaching.Infrastructure.Shared
{
    public static class PipelineRegistration
    {
        public static IApplicationBuilder AddSharedInfrastructureApplicationPipeline(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            // Hangfire Dashboard exposes sensitive information about your background jobs, including method
            // names and serialized arguments as well as gives you an opportunity to manage them by performing
            // different actions – retry, delete, trigger, etc. So it is really important to restrict access to the Dashboard.
            // By default, only local requests are allowed. That mean that when you run the app in a container you will no be
            // able to access the dashboard.
            // Read https://docs.hangfire.io/en/latest/configuration/using-dashboard.html#configuring-authorization for more.
            app.UseHangfireDashboard("/jobs");

            return app;
        }
    }
}