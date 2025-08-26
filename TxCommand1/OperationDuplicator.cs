using System;
using System.Collections;
using Tecnomatix.Engineering;           // API types

// (if you use clipboard helpers)

namespace TxCommand1
{
    /// <summary>
    /// Provides functionality to duplicate operations within Tecnomatix Engineering environments.
    /// </summary>
    public class OperationDuplicator
    {
        /// <summary>
        /// Duplicate an operation by pasting it into the specified target collection.
        /// Returns the copied operation (or null if not found).
        /// </summary>
        /// <param name="sourceOp">The source operation to duplicate.</param>
        /// <returns>The copied operation as ITxObject, or null if the operation could not be found or duplicated.</returns>
        /// <exception cref="ArgumentNullException">Thrown when sourceOp or targetCollection is null.</exception>
        public static ITxOperation DuplicateOperation(ITxOperation sourceOp)
        {
            if (sourceOp == null) throw new ArgumentNullException(nameof(sourceOp));
            TxObjectList origins = new TxObjectList { sourceOp };
            Hashtable originToCopied;
            TxApplication.ActiveDocument.OperationRoot.Paste(origins, out originToCopied);
            return originToCopied[sourceOp] as ITxOperation;
        }
    }
}
