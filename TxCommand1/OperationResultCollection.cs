using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TxCommand1
{
    /// <summary>
    /// Represents a collection of operation results with methods to calculate total duration
    /// and provide formatted string representations of the entire collection.
    /// Implements IEnumerable for easy iteration and LINQ support.
    /// </summary>
    public class OperationResultCollection : IEnumerable<OperationResult>
    {
        private readonly List<OperationResult> _operations;

        /// <summary>
        /// Gets the number of operation results in the collection.
        /// </summary>
        public int Count => _operations.Count;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets or sets the operation result at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the operation result to get or set.</param>
        /// <returns>The operation result at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
        public OperationResult this[int index]
        {
            get => _operations[index];
            set => _operations[index] = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultCollection"/> class.
        /// </summary>
        public OperationResultCollection()
        {
            _operations = new List<OperationResult>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultCollection"/> class
        /// with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial capacity of the collection.</param>
        public OperationResultCollection(int capacity)
        {
            _operations = new List<OperationResult>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultCollection"/> class
        /// with operation results from the specified collection.
        /// </summary>
        /// <param name="operations">The collection of operation results to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when operations is null.</exception>
        public OperationResultCollection(IEnumerable<OperationResult> operations)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));

            _operations = new List<OperationResult>(operations);
        }

        /// <summary>
        /// Adds an operation result to the collection.
        /// </summary>
        /// <param name="operationResult">The operation result to add.</param>
        public void Add(OperationResult operationResult)
        {
            _operations.Add(operationResult);
        }

        /// <summary>
        /// Adds an operation result to the collection using the operation name and duration.
        /// </summary>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="duration">The duration of the operation in seconds.</param>
        public void Add(string operationName, double duration)
        {
            _operations.Add(new OperationResult(operationName, duration));
        }

        /// <summary>
        /// Adds multiple operation results to the collection.
        /// </summary>
        /// <param name="operationResults">The operation results to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when operationResults is null.</exception>
        public void AddRange(IEnumerable<OperationResult> operationResults)
        {
            if (operationResults == null)
                throw new ArgumentNullException(nameof(operationResults));

            _operations.AddRange(operationResults);
        }

        /// <summary>
        /// Removes the first occurrence of the specified operation result from the collection.
        /// </summary>
        /// <param name="operationResult">The operation result to remove.</param>
        /// <returns>true if the operation result was successfully removed; otherwise, false.</returns>
        public bool Remove(OperationResult operationResult)
        {
            return _operations.Remove(operationResult);
        }

        /// <summary>
        /// Removes the operation result at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the operation result to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
        public void RemoveAt(int index)
        {
            _operations.RemoveAt(index);
        }

        /// <summary>
        /// Removes all operation results from the collection.
        /// </summary>
        public void Clear()
        {
            _operations.Clear();
        }

        /// <summary>
        /// Determines whether the collection contains the specified operation result.
        /// </summary>
        /// <param name="operationResult">The operation result to locate.</param>
        /// <returns>true if the operation result is found; otherwise, false.</returns>
        public bool Contains(OperationResult operationResult)
        {
            return _operations.Contains(operationResult);
        }

        /// <summary>
        /// Calculates the total duration of all operations in the collection.
        /// </summary>
        /// <returns>The sum of all operation durations in seconds.</returns>
        public double GetTotalDuration()
        {
            return _operations.Sum(op => op.Duration);
        }

        /// <summary>
        /// Gets the average duration of all operations in the collection.
        /// </summary>
        /// <returns>The average duration in seconds, or 0 if the collection is empty.</returns>
        public double GetAverageDuration()
        {
            return _operations.Count == 0 ? 0.0 : _operations.Average(op => op.Duration);
        }

        /// <summary>
        /// Gets the operation with the longest duration.
        /// </summary>
        /// <returns>The operation result with the maximum duration, or null if the collection is empty.</returns>
        public OperationResult? GetLongestOperation()
        {
            if (_operations.Count == 0)
                return null;

            return _operations.OrderByDescending(op => op.Duration).First();
        }

        /// <summary>
        /// Gets the operation with the shortest duration.
        /// </summary>
        /// <returns>The operation result with the minimum duration, or null if the collection is empty.</returns>
        public OperationResult? GetShortestOperation()
        {
            if (_operations.Count == 0)
                return null;

            return _operations.OrderBy(op => op.Duration).First();
        }

        /// <summary>
        /// Returns a string representation of the entire collection with summary information.
        /// </summary>
        /// <returns>A formatted string containing all operations and summary statistics.</returns>
        public override string ToString()
        {
            return ToString("F3", true);
        }

        /// <summary>
        /// Returns a string representation of the collection with custom formatting options.
        /// </summary>
        /// <param name="durationFormat">The format string for durations. Default is "F3".</param>
        /// <param name="includeSummary">Whether to include summary statistics. Default is true.</param>
        /// <returns>A formatted string representation of the collection.</returns>
        public string ToString(string durationFormat, bool includeSummary = true)
        {
            if (string.IsNullOrEmpty(durationFormat))
                durationFormat = "F3";

            var sb = new StringBuilder();

            if (_operations.Count == 0)
            {
                sb.AppendLine("Operation Results Collection (Empty)");
                return sb.ToString();
            }

            sb.AppendLine($"Operation Results Collection ({_operations.Count} operations):");
            sb.AppendLine(new string('-', 50));

            for (int i = 0; i < _operations.Count; i++)
            {
                sb.AppendLine($"{i + 1,3}. {_operations[i].ToString(durationFormat)}");
            }

            if (includeSummary)
            {
                sb.AppendLine(new string('-', 50));
                sb.AppendLine($"Total Duration: {GetTotalDuration().ToString(durationFormat, CultureInfo.InvariantCulture)}s");
                sb.AppendLine($"Average Duration: {GetAverageDuration().ToString(durationFormat, CultureInfo.InvariantCulture)}s");
                
                var longest = GetLongestOperation();
                var shortest = GetShortestOperation();
                
                if (longest.HasValue)
                    sb.AppendLine($"Longest: {longest.Value.ToString(durationFormat)}");
                
                if (shortest.HasValue)
                    sb.AppendLine($"Shortest: {shortest.Value.ToString(durationFormat)}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns a compact string representation showing only the summary.
        /// </summary>
        /// <param name="durationFormat">The format string for durations. Default is "F3".</param>
        /// <returns>A compact summary string.</returns>
        public string ToSummaryString(string durationFormat = "F3")
        {
            if (_operations.Count == 0)
                return "No operations";

            var totalDuration = GetTotalDuration();
            return $"{_operations.Count} operations, Total: {totalDuration.ToString(durationFormat, CultureInfo.InvariantCulture)}s";
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        public IEnumerator<OperationResult> GetEnumerator()
        {
            return _operations.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator for the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates a copy of the collection with all operation results.
        /// </summary>
        /// <returns>A new OperationResultCollection with the same operation results.</returns>
        public OperationResultCollection Clone()
        {
            return new OperationResultCollection(_operations);
        }

        /// <summary>
        /// Filters the collection to include only operations with durations within the specified range.
        /// </summary>
        /// <param name="minDuration">The minimum duration (inclusive).</param>
        /// <param name="maxDuration">The maximum duration (inclusive).</param>
        /// <returns>A new collection containing only operations within the duration range.</returns>
        public OperationResultCollection FilterByDuration(double minDuration, double maxDuration)
        {
            var filtered = _operations.Where(op => op.Duration >= minDuration && op.Duration <= maxDuration);
            return new OperationResultCollection(filtered);
        }

        /// <summary>
        /// Filters the collection to include only operations whose names contain the specified text.
        /// </summary>
        /// <param name="nameFilter">The text to search for in operation names (case-insensitive).</param>
        /// <returns>A new collection containing only operations matching the name filter.</returns>
        /// <exception cref="ArgumentNullException">Thrown when nameFilter is null.</exception>
        public OperationResultCollection FilterByName(string nameFilter)
        {
            if (nameFilter == null)
                throw new ArgumentNullException(nameof(nameFilter));

            var filtered = _operations.Where(op => 
                op.OperationName.IndexOf(nameFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            return new OperationResultCollection(filtered);
        }
    }
}
