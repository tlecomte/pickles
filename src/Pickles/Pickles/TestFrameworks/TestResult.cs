﻿#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace PicklesDoc.Pickles.TestFrameworks
{
    public struct TestResult
    {
        private readonly bool wasExecuted;

        private readonly bool wasSuccessful;

        private readonly bool wasNotFound;

        private static readonly TestResult passed = new TestResult(wasExecuted: true, wasSuccessful: true, wasNotFound: false);

        private static readonly TestResult failed = new TestResult(wasExecuted: true, wasSuccessful: false, wasNotFound: false);

        private static readonly TestResult inconclusive = new TestResult(wasExecuted: false, wasSuccessful: false, wasNotFound: false);

        private static readonly TestResult notFound = new TestResult (wasExecuted: false, wasSuccessful: false, wasNotFound: true);

        private TestResult(bool wasExecuted, bool wasSuccessful, bool wasNotFound)
        {
            this.wasExecuted = wasExecuted;
            this.wasSuccessful = wasSuccessful;
            this.wasNotFound = wasNotFound;
        }

        public bool WasExecuted
        {
            get { return wasExecuted; }
        }

        public bool WasSuccessful
        {
            get { return wasSuccessful; }
        }

        public bool WasNotFound
        {
            get { return wasNotFound; }
        }

        public static TestResult Passed()
        {
            return passed;
        }

        public static TestResult Failed()
        {
            return failed;
        }

        public static TestResult Inconclusive()
        {
            return inconclusive;
        }

        public static TestResult NotFound()
        {
            return notFound;
        }

        public bool Equals(TestResult other)
        {
            return this.WasExecuted.Equals(other.WasExecuted) && this.WasSuccessful.Equals(other.WasSuccessful) && this.WasNotFound.Equals(other.WasNotFound);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TestResult && Equals((TestResult)obj);
        }

        public override int GetHashCode()
        {
            int hashCode = this.WasExecuted.GetHashCode();
            hashCode = (hashCode) ^ this.WasSuccessful.GetHashCode();
            hashCode = (hashCode) ^ this.WasNotFound.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("WasExecuted: {0}, WasSuccessful: {1}, WasNotFound: {2}", this.WasExecuted, this.WasSuccessful, this.WasNotFound);
        }

        public static bool operator ==(TestResult left, TestResult right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TestResult left, TestResult right)
        {
            return !(left == right);
        }
    }

    public static class TestResultExtensions
    {
        public static TestResult Merge(this IEnumerable<TestResult> testResults)
        {
            if (testResults == null)
            {
                throw new ArgumentNullException("testResults");
            }

            TestResult[] items = testResults.ToArray();

            if (!items.Any())
            {
                return new TestResult();
            }

            if (items.Count() == 1)
            {
                return items.Single();
            }

            if (items.Any(i => i == TestResult.Failed()))
            {
                return TestResult.Failed();
            }

            if (items.Any(i => i == TestResult.Inconclusive() || i == TestResult.NotFound()))
            {
                return TestResult.Inconclusive();
            }

            return TestResult.Passed();
        }
    }
}