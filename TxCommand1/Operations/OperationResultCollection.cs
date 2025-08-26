using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TxCommand1.Operations
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
        public OperationResultCollection(int capacity)
        {
            _operations = new List<OperationResult>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResultCollection"/> class
        /// with operation results from the specified collection.
        /// </summary>
        public OperationResultCollection(IEnumerable<OperationResult> operations)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));

            _operations = new List<OperationResult>(operations);
        }

        /// <summary>
        /// Adds an operation result to the collection.
        /// </summary>
        public void Add(OperationResult operationResult)
        {
            _operations.Add(operationResult);
        }

        /// <summary>
        /// Adds an operation result to the collection using the operation name and duration.
        /// </summary>
        public void Add(string operationName, double duration)
        {
            _operations.Add(new OperationResult(operationName, duration));
        }

        /// <summary>
        /// Adds multiple operation results to the collection.
        /// </summary>
        public void AddRange(IEnumerable<OperationResult> operationResults)
        {
            if (operationResults == null)
                throw new ArgumentNullException(nameof(operationResults));

            _operations.AddRange(operationResults);
        }

        /// <summary>
        /// Removes the first occurrence of the specified operation result from the collection.
        /// </summary>
        public bool Remove(OperationResult operationResult)
        {
            return _operations.Remove(operationResult);
        }

        /// <summary>
        /// Removes the operation result at the specified index.
        /// </summary>
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
        public bool Contains(OperationResult operationResult)
        {
            return _operations.Contains(operationResult);
        }

        /// <summary>
        /// Calculates the total duration of all operations in the collection.
        /// </summary>
        public double GetTotalDuration()
        {
            return _operations.Sum(op => op.Duration);
        }

        /// <summary>
        /// Gets the average duration of all operations in the collection.
        /// </summary>
        public double GetAverageDuration()
        {
            return _operations.Count == 0 ? 0.0 : _operations.Average(op => op.Duration);
        }

        /// <summary>
        /// Gets the operation with the longest duration.
        /// </summary>
        public OperationResult? GetLongestOperation()
        {
            if (_operations.Count == 0)
                return null;

            return _operations.OrderByDescending(op => op.Duration).First();
        }

        /// <summary>
        /// Gets the operation with the shortest duration.
        /// </summary>
        public OperationResult? GetShortestOperation()
        {
            if (_operations.Count == 0)
                return null;

            return _operations.OrderBy(op => op.Duration).First();
        }

        /// <summary>
        /// Returns a string representation of the entire collection with summary information.
        /// </summary>
        public override string ToString()
        {
            return ToString("F3", true);
        }

        /// <summary>
        /// Returns a string representation of the collection with custom formatting options.
        /// </summary>
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
        public IEnumerator<OperationResult> GetEnumerator()
        {
            return _operations.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Creates a copy of the collection with all operation results.
        /// </summary>
        public OperationResultCollection Clone()
        {
            return new OperationResultCollection(_operations);
        }

        /// <summary>
        /// Filters the collection to include only operations with durations within the specified range.
        /// </summary>
        public OperationResultCollection FilterByDuration(double minDuration, double maxDuration)
        {
            var filtered = _operations.Where(op => op.Duration >= minDuration && op.Duration <= maxDuration);
            return new OperationResultCollection(filtered);
        }

        /// <summary>
        /// Filters the collection to include only operations whose names contain the specified text.
        /// </summary>
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


