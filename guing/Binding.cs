using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using Spring.Core;
using Spring.Util;
using System.Linq;

namespace guing
{
    public class Binding<T>
    {
        private readonly IList<Action<T>> updates = new List<Action<T>>();
        public Binding<T> Property<R>(Expression<Func<T, R>> property, R newValue)
        {
            var body = property.Body;
            var memberExpression = body as MemberExpression;
            AssertUtils.ArgumentNotNull(memberExpression, "memberExpression", "unhandled Expression " + property.Body.GetType());
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null || propertyInfo.GetSetMethod() == null || propertyInfo.CanWrite == false || propertyInfo.GetSetMethod().IsPublic == false)
            {
                var name = memberExpression.Member.Name;
                throw new NotWritablePropertyException(memberExpression.Expression.Type + "." + name);
            }
            updates.Add((item) => propertyInfo.SetValue(item, newValue, null));            
            return this;
        }

        public Binding<T> Property(Expression<Action<T>> property)
        {
            var body = property.Body;
            var invocationExpression = body as MethodCallExpression;
            AssertUtils.ArgumentNotNull(invocationExpression, "invocationExpression", "unhandled Expression " + property.Body.GetType());                               
            Console.WriteLine("Method:" + invocationExpression.Method);
            var arguments = invocationExpression.Arguments;
            
            foreach (Expression argument in arguments)
            {
                Console.WriteLine("arg:" + argument);
                Console.WriteLine("type:" + argument.GetType());

                if (argument is MemberExpression)
                {
                    
                    var me = argument as MemberExpression;
                    Console.WriteLine("me.Expression.GetType():" + me.Expression.GetType());

                    var ce = me.Expression as ConstantExpression;
                    Console.WriteLine("member:" + me.Member);
                    var mfinfo = me.Member as FieldInfo;
                    if (ce != null)
                    {
                        var value = ce.Value;
                        Console.WriteLine("value is: " + value);
                    }
                    
                    if (mfinfo != null)
                    {                        
                        Console.WriteLine("field value:" + mfinfo.GetValue(ce.Value));
                    }

                }
            }
                    


            return this;
        }

        private void OnStart(Expression<Action<T>> func)
        {
            throw new NotImplementedException();
        }

        public T Bind()
        {
            var bound = (T)typeof(T).GetConstructor(new Type[]{}).Invoke(new object[]{});
            updates.ToList().ForEach(u => u(bound));
            return bound;
        }
    }
}