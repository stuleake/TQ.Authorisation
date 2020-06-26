using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TQ.Geocoding.Service
{
    /// <summary>
    /// ExpressionParameterReplacer
    /// </summary>
    public class ExpressionParameterReplacer : ExpressionVisitor
    {
        public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
        {
            if (!fromParameters?.Any() ?? false)
            {
                throw new ArgumentNullException($"{nameof(fromParameters)} is null or empty");
            }

            if (!toParameters?.Any() ?? false)
            {
                throw new ArgumentNullException($"{nameof(toParameters)} is null or empty");
            }

            if (fromParameters.Count() != toParameters.Count())
            {
                throw new ArgumentException($"{nameof(fromParameters)} and {nameof(toParameters)} have different lengths");
            }

            ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
            for (int index = 0; index != fromParameters.Count && index != toParameters.Count; index++)
            {
                ParameterReplacements.Add(fromParameters[index], toParameters[index]);
            }
        }
        private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }
        
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (ParameterReplacements.TryGetValue(node, out ParameterExpression replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }
    }
}