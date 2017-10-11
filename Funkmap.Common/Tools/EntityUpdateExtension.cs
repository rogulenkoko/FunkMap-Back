﻿using System;
using System.Collections;
using Microsoft.VisualBasic.CompilerServices;

namespace Funkmap.Tools
{
    public static class EntityUpdateExtension
    {
        public static T FillEntity<T>(this T entity, T newEntity)
        {
            var emptyInstance = Activator.CreateInstance(typeof(T));

            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                if (!CompareObjects(propertyInfo.GetValue(newEntity), propertyInfo.GetValue(emptyInstance)))
                {
                    propertyInfo.SetValue(entity, propertyInfo.GetValue(newEntity));
                }
            }

            return entity;
        }


        public static bool CompareObjects<T>(T expectInput, T actualInput)
        {
            if (actualInput == null && expectInput != null) return false;


            if (typeof(T).IsPrimitive)
            {
                if (expectInput.Equals(actualInput))
                {
                    return true;
                }

                return false;
            }

            if (expectInput is IEquatable<T>)
            {
                if (expectInput.Equals(actualInput))
                {
                    return true;
                }

                return false;
            }

            if (expectInput is IComparable)
            {
                if (((IComparable)expectInput).CompareTo(actualInput) == 0)
                {
                    return true;
                }

                return false;
            }
            
            if (expectInput is IEnumerable)
            {
                if (actualInput == null) return false;
                var expectEnumerator = ((IEnumerable)expectInput).GetEnumerator();
                var actualEnumerator = ((IEnumerable)actualInput).GetEnumerator();

                var canGetExpectMember = expectEnumerator.MoveNext();
                var canGetActualMember = actualEnumerator.MoveNext();

                while (canGetExpectMember && canGetActualMember && true)
                {
                    var currentType = expectEnumerator.Current.GetType();
                    object isEqual = typeof(Utils).GetMethod("CompareObjects").MakeGenericMethod(currentType).Invoke(null, new object[] { expectEnumerator.Current, actualEnumerator.Current });

                    if ((bool)isEqual == false)
                    {
                        return false;
                    }

                    canGetExpectMember = expectEnumerator.MoveNext();
                    canGetActualMember = actualEnumerator.MoveNext();
                }

                if (canGetExpectMember != canGetActualMember)
                {
                    return false;
                }

                return true;
            }
            
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var expectValue = typeof(T).GetProperty(property.Name).GetValue(expectInput);
                var actualValue = typeof(T).GetProperty(property.Name).GetValue(actualInput);

                if (expectValue == null || actualValue == null)
                {
                    if (expectValue == null && actualValue == null)
                    {
                        continue;
                    }

                    return false;
                }

                object isEqual = typeof(Utils).GetMethod("CompareObjects").MakeGenericMethod(property.PropertyType).Invoke(null, new object[] { expectValue, actualValue });

                if ((bool)isEqual == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
