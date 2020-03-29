using System.Linq.Expressions;

namespace eQuantic.Core.Linq
{
    public class ExpressionMapper
    {
        private ExpressionMapper()
        {
            
        }
        public ExpressionMapper(Expression expression)
        {
            switch (expression)
            {
                case LambdaExpression lambdaExpression:
                    switch (lambdaExpression.Body)
                    {
                        case BinaryExpression binaryExpression:

                            this.SetFromBinaryExpression(this, binaryExpression);
                            
                            break;
                    }
                    break;
            }
        }
        public string PropertyName { get; set; }
        public ExpressionMapper Left { get; set; }
        public ExpressionType Type { get; set; }
        public ExpressionMapper Right { get; set; }
        public object Value { get; set; }
        
        private void SetFromBinaryExpression(ExpressionMapper mapper, BinaryExpression binaryExpression)
        {
            switch (binaryExpression.Left)
            {
                case BinaryExpression leftBinaryExpression:
                    
                    mapper.Left = new ExpressionMapper();
                    SetFromBinaryExpression(mapper.Left,  leftBinaryExpression);
                    
                    break;
                case MemberExpression memberExpression:
                    mapper.PropertyName = memberExpression.Member.Name;
                    break;
            }

            mapper.Type = binaryExpression.NodeType;

            switch (binaryExpression.Right)
            {
                case BinaryExpression rightBinaryExpression:

                    mapper.Right = new ExpressionMapper();
                    SetFromBinaryExpression(mapper.Right,  rightBinaryExpression);
                    
                    break;
                case ConstantExpression constantExpression:
                    mapper.Value = constantExpression.Value;
                    break;
            }
        }
    }
}