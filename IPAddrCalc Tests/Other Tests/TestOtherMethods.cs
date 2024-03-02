using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IPAddrCalc_Tests.Other_Tests
{
    public class TestOtherMethods
    {
        #region Tests for functions that checks whether the input string is valid for processing
        [Fact]
        public void ShouldBeFalseIfInputStringIsEmpty()
        {
            // Arrange
            string testString = string.Empty;

            // Act
            bool isValid = IsStringValidForProcessing(testString);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ShouldBeFalseIfInputStringIsNull()
        {
            // Arrange
            string? testString = null;

            // Act
            bool isValid = IsStringValidForProcessing(testString);

            // Assert
            Assert.False(isValid);
        }
        #endregion
    }
}
