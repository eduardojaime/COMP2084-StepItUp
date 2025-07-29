namespace StepItUp.Tests
{
    [TestClass]
    // Sealed classes cannot be inherited, which is useful for unit tests
    public sealed class Test1
    {
        [TestMethod]
        public void TwoPlusTwoEqualFour()
        {
            // Arrange > prepare test data
            var a = 2;
            var b = 2;
            var expected = 4;
            var actual = 0; // initialize the variable to hold the result

            // Act > perform the action to be tested or call the method to be tested
            actual = a + b;

            // Assert > verify the results
            // Use the Assert class
            Assert.AreEqual(expected, actual);
        }
    }
}
