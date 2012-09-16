

namespace Kigg.EF.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    
    internal class DataLoadOptions
    {
        private static readonly string[] ValidTypes = new[] { "ComplexObject ", "StructuralObject", "EntityObject", "EntityCollection`1" };
        private readonly Dictionary<MetaPosition, MemberInfo> _includes;
        
        internal DataLoadOptions()
        {
            _includes = new Dictionary<MetaPosition, MemberInfo>();
        }

        internal MemberInfo[] GetPreloadedMembers<TEntity>()
        {
            var type = typeof (TEntity);
            var preloadedMembers = new List<MemberInfo>();

            foreach (var prop in _includes.Values.OfType<PropertyInfo>())
            {
                if (prop.DeclaringType == type)
                {
                    preloadedMembers.Add(prop);
                }
            }
            return preloadedMembers.ToArray();
        }
        
        internal bool IsPreloaded(MemberInfo member)
        {
            Check.Argument.IsNotNull(member,"member");
            
            return _includes.ContainsKey(new MetaPosition(member));
        }

        internal void LoadWith<T>(Expression<Func<T, object>> expression)
        {
            Check.Argument.IsNotNull(expression, "expression");

            MemberInfo loadWithMemberInfo = GetLoadWithMemberInfo(expression);
            if (IsPreloaded(loadWithMemberInfo))
            {
                throw new InvalidOperationException("Association is already added");
            }
            Preload(loadWithMemberInfo);
        }

        private void Preload(MemberInfo association)
        {
            Check.Argument.IsNotNull(association, "association");
            _includes.Add(new MetaPosition(association), association);
        }

        private static MemberInfo GetLoadWithMemberInfo(LambdaExpression lambda)
        {
            var body = lambda.Body;
            if ((body != null) && ((body.NodeType == ExpressionType.Convert) || (body.NodeType == ExpressionType.ConvertChecked)))
            {
                body = ((UnaryExpression)body).Operand;
            }
            var expression = body as MemberExpression;

            ValidateMemberExpression(expression);

            if (expression != null) return expression.Member;

            throw new InvalidOperationException("The expression specified must be of the form p.A, where p is the parameter and A is a property member.");
        }
        private static void ValidateMemberExpression(MemberExpression expression)
        {
            if ((expression == null) || (expression.Expression.NodeType != ExpressionType.Parameter) || expression.Member.MemberType != MemberTypes.Property)
            {
                throw new InvalidOperationException("The expression specified must be of the form p.A, where p is the parameter and A is a property member.");
            }
            var member = expression.Member as PropertyInfo;
            if(member != null)
            {
                //Member type must be EntityCollection<T> or StructuralObject
                var isEntityCollection = ValidTypes.Any(s => s == member.PropertyType.Name);
                var isEntity = member.PropertyType.BaseType != null &&
                               ValidTypes.Any(s => s == member.PropertyType.BaseType.Name);

                if(!isEntity && !isEntityCollection)
                {
                    var errorMsg =
                    String.Format(CultureInfo.InvariantCulture,
                        "Related end \"{0}\" must be of type that implements System.Data.Objects.DataClasses.EntityCollection<T> or System.Data.Objects.DataClasses.IEntityWithRelationships",
                        expression);
                    throw new InvalidOperationException(errorMsg);
                }
            }
            else
            {
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, 
                                                                "{0} must be property member", 
                                                                expression));
            }
        }

        private struct MetaPosition : IEqualityComparer<MetaPosition>, IEqualityComparer
        {
            private readonly int _metadataToken;

            private readonly Assembly _assembly;

            internal MetaPosition(MemberInfo mi)
                : this(mi.DeclaringType.Assembly, mi.MetadataToken)
            {
            }
            private MetaPosition(Assembly assembly, int metadataToken)
            {
                _assembly = assembly;
                _metadataToken = metadataToken;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (obj.GetType() != GetType())
                {
                    return false;
                }
                return AreEqual(this, (MetaPosition)obj);
            }
            public bool Equals(MetaPosition x, MetaPosition y)
            {
                return AreEqual(x, y);
            }
            public override int GetHashCode()
            {
                return _metadataToken;
            }
            public int GetHashCode(MetaPosition obj)
            {
                return obj._metadataToken;
            }

            public static bool operator ==(MetaPosition x, MetaPosition y)
            {
                return AreEqual(x, y);
            }
            public static bool operator !=(MetaPosition x, MetaPosition y)
            {
                return !AreEqual(x, y);
            }

            bool IEqualityComparer.Equals(object x, object y)
            {
                return Equals((MetaPosition)x, (MetaPosition)y);
            }
            int IEqualityComparer.GetHashCode(object obj)
            {
                return GetHashCode((MetaPosition)obj);
            }

            private static bool AreEqual(MetaPosition x, MetaPosition y)
            {
                return ((x._metadataToken == y._metadataToken) && (x._assembly == y._assembly));
            }

            //internal static bool AreSameMember(MemberInfo x, MemberInfo y)
            //{
            //    return ((x.MetadataToken == y.MetadataToken) && (x.DeclaringType.Assembly == y.DeclaringType.Assembly));
            //}
        }
    }
}