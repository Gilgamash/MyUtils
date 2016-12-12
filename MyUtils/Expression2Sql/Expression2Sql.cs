using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyUtils.Expression2Sql
{
    public class Expression2Sql
    {
    }

    public class QueryInfo<T> where T : class, new()
    {
        public List<T> Select(Expression<Func<T, object>> expression)
        {
            IExpression<T> factory = new ExpressFactory<T>().GetFactory(expression.Body);
            return factory.Select(expression);
        }
    }

    public class SqlFactory
    {
        private static List<string> LetterList = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        private Queue<string> _queueLetters = new Queue<string>(LetterList);
        public string Sql { get; set; }


    }

    public class ExpressFactory<T> where T : class, new()
    {
        public IExpression<T> GetFactory(Expression expression)
        {
            if (expression is UnaryExpression)
                return new UnaryExpressionFactory<T>();
            throw new NotImplementedException();
        }
    }

    public class UnaryExpressionFactory<T> : BaseExpressionFactory<UnaryExpression, T> where T : class, new()
    {
        public override List<T> Select(Expression expression)
        {
            return new List<T>();
        }
    }

    public abstract class BaseExpressionFactory<T, T1> : IExpression<T1> where T : Expression where T1 : class, new()
    {
        public abstract List<T1> Select(Expression expression);
    }

    public interface IExpression<T> where T : class, new()
    {
        List<T> Select(Expression expression);
    }
}
