﻿using System;
using System.Linq.Expressions;

namespace GeneralShare
{
    public static class EnumConverter<TEnum> where TEnum : Enum
    {
        private static readonly Func<long, TEnum> _convertTo = GenerateToConverter();
        private static readonly Func<TEnum, long> _convertFrom = GenerateFromConverter();

        public static TEnum Convert(long value)
        {
            return _convertTo.Invoke(value);
        }

        public static TEnum Convert(ulong value)
        {
            return Convert((long)value);
        }

        public static long Convert(TEnum value)
        {
            return _convertFrom.Invoke(value);
        }

        private static Func<long, TEnum> GenerateToConverter()
        {
            var parameter = Expression.Parameter(typeof(long));
            var conversion = Expression.Convert(parameter, typeof(TEnum));
            var method = Expression.Lambda<Func<long, TEnum>>(conversion, parameter);
            return method.Compile();
        }

        private static Func<TEnum, long> GenerateFromConverter()
        {
            var parameter = Expression.Parameter(typeof(TEnum));
            var conversion = Expression.Convert(parameter, typeof(long));
            var method = Expression.Lambda<Func<TEnum, long>>(conversion, parameter);
            return method.Compile();
        }
    }
}
