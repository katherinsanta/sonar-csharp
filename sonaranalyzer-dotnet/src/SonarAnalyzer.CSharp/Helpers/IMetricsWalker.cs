using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using SonarAnalyzer.Common;

namespace SonarAnalyzer.Helpers
{
    public interface IMetricsWalker
    {
        /// <summary>
        /// Collects metrics for the provided node.
        /// </summary>
        void Visit(SyntaxNode node);

        /// <summary>
        /// Gets a bool specifying whether the walker visited the node successfully.
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Gets an int representing the metric value.
        /// </summary>
        int MetricValue { get; }

        /// <summary>
        /// Returns a collection with all locations that affected the metric.
        /// </summary>
        IEnumerable<SecondaryLocation> Locations { get; }
    }
}
