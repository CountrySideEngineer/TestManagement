using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestManagement.Analyze.APP.Model.Converter
{
    internal interface IConverter<S, D> 
        where S : class 
        where D : class
    {
        IEnumerable<D> Convert(S input);
    }

    internal interface IConverter
    {
        IEnumerable<object> Convert(object input);
    }
}
