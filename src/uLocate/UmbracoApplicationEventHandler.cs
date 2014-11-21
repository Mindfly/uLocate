namespace uLocate
{
    using Umbraco.Core;

    /// <summary>
    /// Responsible for handling the Umbraco Application Events
    /// </summary>
    public class UmbracoApplicationEventHandler : ApplicationEventHandler
    {
        /// <summary>
        /// Handles Umbraco application started.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The umbraco application.
        /// </param>
        /// <param name="applicationContext">
        /// The application context.
        /// </param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            AutomapperMappings.CreateMappings();
        }
    }
}