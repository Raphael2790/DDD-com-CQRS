using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RssStore.Core.DomainObjects.Validations
{
    public static class AssertionConcern
    {
        public static void ValidateIsEqual(object a, object b, string message)
        {
            if (a.Equals(b))
                throw new DomainException(message);
            
        }

        public static void ValidateIsDifferent(object a, object b, string message)
        {
            if (!a.Equals(b))
                throw new DomainException(message);
        }

        public static void ValidateCaracteres(string value, int max, string message)
        {
            var length = value.Trim().Length;
            if (length > max)
                throw new DomainException(message);
        }

        public static void ValidateCaracteres(string value, int min, int max, string message)
        {
            var length = value.Trim().Length;
            if (length < min || length > max)
                throw new DomainException(message);
        }

        public static void ValidateExpression(string pattern, string value, string message)
        {
            var regex = new Regex(pattern);
            if (regex.IsMatch(value))
                throw new DomainException(message);
        }

        public static void ValidateIsEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
                throw new DomainException(message);
        }
        
        public static void ValidateIsNull(object object1, string message)
        {
            if (object1 is null)
                throw new DomainException(message);
        }

        public static void ValidateMinMax(double value, double min, double max, string message)
        {
            if (value < min || value > max)
                throw new DomainException(message);
        }

        public static void ValidateMinMax(float value, float min, float max, string message)
        {
            if (value < min || value > max)
                throw new DomainException(message);
        }

        public static void ValidateMinMax(decimal value, decimal min, decimal max, string message)
        {
            if (value < min || value > max)
                throw new DomainException(message);
        }

        public static void ValidateMinMax(int value, int min, int max, string message)
        {
            if (value < min || value > max)
                throw new DomainException(message);
        }

        public static void ValidateMinMax(long value, long min, long max, string message)
        {
            if (value < min || value > max)
                throw new DomainException(message);
        }

        public static void ValidateIsLessThen(long value, long min, string message)
        {
            if (value < min)
                throw new DomainException(message);
        }

        public static void ValidateIsLessThen(int value, int min, string message)
        {
            if (value < min)
                throw new DomainException(message);
        }

        public static void ValidateIsLessThen(double value, double min, string message)
        {
            if (value < min)
                throw new DomainException(message);
        }

        public static void ValidateIsLessThen(float value, float min, string message)
        {
            if (value < min)
                throw new DomainException(message);
        }

        public static void ValidateIsLessThen(decimal value, decimal min, string message)
        {
            if (value < min)
                throw new DomainException(message);
        }

        public static void ValidateIsFalse(bool boolValue, string message)
        {
            if (!boolValue)
                throw new DomainException(message);
        }

        public static void ValidateIsTrue(bool boolValue, string message)
        {
            if (boolValue)
                throw new DomainException(message);
        }
    }
}
