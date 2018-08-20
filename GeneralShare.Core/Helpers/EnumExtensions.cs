﻿using System;
using System.Linq.Expressions;

namespace GeneralShare
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Determines whether the specified value has flags. Note this method is faster
        /// than the one that comes with .NET 4 as it avoids any explict boxing or unboxing. 
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="flag">The flag.</param>
        /// <returns>
        ///  <c>true</c> if the specified value has flags; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasFlags<TEnum>(this TEnum value, TEnum flag) where TEnum : Enum
        {
            return EnumExtensionsInternal<TEnum>.HasFlagsDelegate(value, flag);
        }

        /// <summary>
        /// Determines whether the specified value has multiple flags.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="flag">The flags.</param>
        /// <returns>
        ///  <c>true</c> if the specified value has any flag; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasFlags<TEnum>(this TEnum value, params TEnum[] flags) where TEnum : Enum
        {
            for (int i = 0; i < flags.Length; i++)
            {
                if (EnumExtensionsInternal<TEnum>.HasFlagsDelegate(value, flags[i]))
                    return true;
            }
            return false;
        }

        private static class EnumExtensionsInternal<TEnum> where TEnum : Enum
        {
            public static readonly Func<TEnum, TEnum, bool> HasFlagsDelegate = CreateHasFlagDelegate();
            
            private static Func<TEnum, TEnum, bool> CreateHasFlagDelegate()
            {
                ParameterExpression valueExpression = Expression.Parameter(typeof(TEnum));
                ParameterExpression flagExpression = Expression.Parameter(typeof(TEnum));
                ParameterExpression flagValueVariable = Expression.Variable(
                    Type.GetTypeCode(typeof(TEnum)) == TypeCode.UInt64 ? typeof(ulong) : typeof(long));

                Expression<Func<TEnum, TEnum, bool>> lambdaExpression = Expression.Lambda<Func<TEnum, TEnum, bool>>(
                  Expression.Block(
                    new[] { flagValueVariable },
                    Expression.Assign(
                      flagValueVariable,
                      Expression.Convert(
                        flagExpression,
                        flagValueVariable.Type
                      )
                    ),
                    Expression.Equal(
                      Expression.And(
                        Expression.Convert(
                          valueExpression,
                          flagValueVariable.Type
                        ),
                        flagValueVariable
                      ),
                      flagValueVariable
                    )
                  ),
                  valueExpression,
                  flagExpression
                );
                return lambdaExpression.Compile();
            }
        }
    }
}
