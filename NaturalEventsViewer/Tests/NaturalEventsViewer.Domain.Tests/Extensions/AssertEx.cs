using NUnit.Framework;
using System;
using System.Collections;
using System.Reflection;

namespace NaturalEventsViewer.Domain.Tests.Extensions
{
    public static class AssertEx
    {
        /// <summary>
        /// Performs deep comparison between two objects
        /// </summary>
        /// <param name="actual">Actual object</param>
        /// <param name="expected">Expected object</param>
        /// <param name="propertyName">Property name</param>
        public static void PropertyValuesAreEquals(object actual, object expected, string propertyName = "")
        {
            if (Equals(actual, expected)) return;

            Type typeOfActual = actual.GetType();
            if (typeOfActual.IsPrimitive || actual is string)
            {
                Assert.Fail($"Property {propertyName} does not match. Expected: {expected} but was: {actual}");
            }
            else if (actual is IList && expected is IList)
            {
                AssertListsAreEquals((IList)actual, (IList)expected, propertyName);
            }
            else
            {
                PropertyInfo[] properties = expected.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    object actualValue = property.GetValue(actual, null);
                    object expectedValue = property.GetValue(expected, null);

                    PropertyValuesAreEquals(actualValue, expectedValue, $"{propertyName}.{property.Name}");
                }
            }
        }

        private static void AssertListsAreEquals(IList actualList, IList expectedList, string propertyName)
        {
            if (actualList.Count != expectedList.Count)
                Assert.Fail($"Property {propertyName} does not match. Expected IList containing {expectedList.Count} elements but was IList containing {actualList.Count} elements");

            for (int i = 0; i < actualList.Count; i++)
                PropertyValuesAreEquals(actualList[i], expectedList[i], $"{propertyName}[{i}]");
        }
    }
}
