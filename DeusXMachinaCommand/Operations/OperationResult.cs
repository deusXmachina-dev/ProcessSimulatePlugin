using System;
using System.Globalization;

namespace DeusXMachinaCommand.Operations
{
    /// <summary>
    /// Represents an operation result containing the operation name and duration.
    /// Follows .NET best practices with immutable design and proper ToString implementation.
    /// </summary>
    public readonly struct OperationResult : IEquatable<OperationResult>
    {
        /// <summary>
        /// Gets the name of the operation.
        /// </summary>
        public string OperationName { get; }

        /// <summary>
        /// Gets the duration of the operation in seconds.
        /// </summary>
        public double Duration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> struct.
        /// </summary>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="duration">The duration of the operation in seconds.</param>
        /// <exception cref="ArgumentNullException">Thrown when operationName is null.</exception>
        /// <exception cref="ArgumentException">Thrown when operationName is empty or whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when duration is negative or not a valid number.</exception>
        public OperationResult(string operationName, double duration)
        {
            if (operationName == null)
                throw new ArgumentNullException(nameof(operationName));
            
            if (string.IsNullOrWhiteSpace(operationName))
                throw new ArgumentException("Operation name cannot be empty or whitespace.", nameof(operationName));
            
            if (duration < 0 || double.IsNaN(duration) || double.IsInfinity(duration))
                throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be a non-negative finite number.");

            OperationName = operationName;
            Duration = duration;
        }

        /// <summary>
        /// Returns a string representation of the operation result.
        /// </summary>
        /// <returns>A formatted string containing the operation name and duration.</returns>
        public override string ToString()
        {
            return $"{OperationName}: {Duration:F3}s";
        }

        /// <summary>
        /// Returns a string representation of the operation result with custom formatting.
        /// </summary>
        /// <param name="durationFormat">The format string for the duration. Default is "F3" (3 decimal places).</param>
        /// <returns>A formatted string containing the operation name and duration.</returns>
        public string ToString(string durationFormat)
        {
            if (string.IsNullOrEmpty(durationFormat))
                durationFormat = "F3";
            
            return $"{OperationName}: {Duration.ToString(durationFormat, CultureInfo.InvariantCulture)}s";
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current OperationResult.
        /// </summary>
        /// <param name="obj">The object to compare with the current OperationResult.</param>
        /// <returns>true if the specified object is equal to the current OperationResult; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is OperationResult other && Equals(other);
        }

        /// <summary>
        /// Determines whether the specified OperationResult is equal to the current OperationResult.
        /// </summary>
        /// <param name="other">The OperationResult to compare with the current OperationResult.</param>
        /// <returns>true if the specified OperationResult is equal to the current OperationResult; otherwise, false.</returns>
        public bool Equals(OperationResult other)
        {
            return string.Equals(OperationName, other.OperationName, StringComparison.Ordinal) &&
                   Math.Abs(Duration - other.Duration) < double.Epsilon;
        }

        /// <summary>
        /// Returns the hash code for this OperationResult.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((OperationName?.GetHashCode() ?? 0) * 397) ^ Duration.GetHashCode();
            }
        }

        /// <summary>
        /// Determines whether two OperationResult instances are equal.
        /// </summary>
        public static bool operator ==(OperationResult left, OperationResult right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two OperationResult instances are not equal.
        /// </summary>
        public static bool operator !=(OperationResult left, OperationResult right)
        {
            return !left.Equals(right);
        }
    }
}


