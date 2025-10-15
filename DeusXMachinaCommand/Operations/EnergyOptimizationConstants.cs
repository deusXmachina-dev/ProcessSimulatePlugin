namespace DeusXMachinaCommand.Operations
{
    /// <summary>
    /// Constants for estimating energy demand and savings in robotic motions.
    /// </summary>
    public static class EnergyOptimizationConstants
    {
        /// <summary>
        /// Minimum speed ratio observed for large sweeping motions.
        /// </summary>
        public const double MinSpeedRatio = 0.72;
        
        /// <summary>
        /// Maximum speed ratio observed for minimal motions (e.g., moving between close weld-points).
        /// </summary>
        public const double MaxSpeedRatio = 1.0;
        
        /// <summary>
        /// Maximum energy savings based on measurements on real robots.
        /// </summary>
        public const double MaxSavings = 0.4;
        
        /// <summary>
        /// Minimum energy savings.
        /// </summary>
        public const double MinSavings = 0.0;
        
        /// <summary>
        /// Minimum energy consumption (relative units).
        /// Example: a robot with consumption of 0.5kW for very slow motions would draw around 4kW for fast sweeping motions.
        /// </summary>
        public const double MinEnergyConsumption = 1;
        
        /// <summary>
        /// Maximum energy consumption (relative units).
        /// Example: a robot with consumption of 0.5kW for very slow motions would draw around 4kW for fast sweeping motions.
        /// </summary>
        public const double MaxEnergyConsumption = 8;
    }
}

