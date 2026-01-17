using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexisNexis.Common.Filters
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SearchWeightAttribute : Attribute
    {
        public double Weight { get; }

        public SearchWeightAttribute(double weight)
        {
            Weight = weight;
        }
    }

}
