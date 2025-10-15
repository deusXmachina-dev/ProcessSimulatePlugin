using System;
using Tecnomatix.Engineering;

namespace DeusXMachinaCommand.Operations
{
	/// <summary>
	/// Wraps an optimized operation together with its estimated energy savings percentage.
	/// </summary>
	public sealed class EnergyOptimizationResult
	{
		/// <summary>
		/// Gets the optimized operation instance. Caller owns its lifecycle.
		/// </summary>
		public ITxOperation Operation { get; }

		/// <summary>
		/// Gets the estimated energy savings in percent (0-100).
		/// </summary>
		public double EnergySavingsPercent { get; }

		public EnergyOptimizationResult(ITxOperation operation, double energySavingsPercent)
		{
			if (operation == null)
				throw new ArgumentNullException(nameof(operation));

			if (double.IsNaN(energySavingsPercent) || double.IsInfinity(energySavingsPercent))
				throw new ArgumentOutOfRangeException(nameof(energySavingsPercent));

			Operation = operation;
			EnergySavingsPercent = energySavingsPercent;
		}
	}
}


