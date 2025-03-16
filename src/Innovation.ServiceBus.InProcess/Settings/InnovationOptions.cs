namespace Innovation.ServiceBus.InProcess.Settings
{
    public class InnovationOptions
    {
        #region Properties

        // If false, the dispatcher will not perform validation on commands
        public bool IsValidationEnabled { get; set; }

        // Search locations can contain dll's which will be dynamically loaded and processed
        public string[] SearchLocations { get; set; }

        // if false th eruntime will not search for assemblies to dynamically load from seatch locations
        public bool DisableDynamicLoading { get; set; }

        #endregion Properties
    }
}
