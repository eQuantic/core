using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;
using eQuantic.Core.Linq.Extensions;

namespace eQuantic.Core.Tests
{
    public enum EnumExample
    {
        Option1,
        Option2,
        Option3
    }

    public class SimpleObject
    {
        public string SimplePropString { get; set; }
        public int SimplePropInt { get; set; }
    }

    public class ComplexObject
    {
        public string PropString { get; set; }
        public int PropInt { get; set; }
        public DateTime PropDateTime { get; set; }
        public EnumExample PropEnum { get; set; }
        public decimal PropDecimal { get; set; }

        public SimpleObject SimpleObject { get; set; }

        public IEnumerable<SimpleObject> SimpleObjectList { get; set; }
    }

    public class ExpressionMapperTest
    {
        [Fact]
        public void Extension_ToExpressionMapper()
        {
            Expression<Func<ComplexObject, bool>> expression = o =>
                o.PropString == "teste" && o.PropInt > 10 && (o.PropDecimal > 0 || o.PropDecimal < 10);

            expression.ToExpressionMapper();
        }
    }
}
