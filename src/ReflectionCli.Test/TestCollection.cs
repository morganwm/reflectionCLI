using System;
using Xunit;

namespace ReflectionCli.Test
{
    public class TestCollection
    {
        [CollectionDefinition("ReflectionCLi")]
        public class AxialTestCollection : ICollectionFixture<TestFixture>
        {
        }
    }
}
