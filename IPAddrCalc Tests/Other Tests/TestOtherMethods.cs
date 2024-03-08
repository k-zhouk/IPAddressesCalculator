using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IPAddrCalc_Tests.Other_Tests
{
    public class TestOtherMethods
    {
        // Tests for functions that checks whether the input string is valid for processing
        [Fact]
        public void ShouldBeNotValidForProcessingIfInputStringIsEmpty()
        {
            // Arrange
            string testString = string.Empty;

            // Act
            bool isValid = IsStringValidForProcessing(testString);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ShouldBeNotValidForProcessingIfInputStringIsNull()
        {
            // Arrange
            string? testString = null;

            // Act
            bool isValid = IsStringValidForProcessing(testString);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ShouldBeValidForProcessingForNonEmptyOrNonNullString()
        {
            // Arrange
            string testString = "abcd";

            // Act
            bool isValid = IsStringValidForProcessing(testString);

            // Assert
            Assert.True(isValid);
        }


        /* 
         * Tests to compare 2 networks
         * Example 1:
         * 10.10.1.32/27 and 10.10.1.44/27--> same network
         * 
         * Example 2:
         * 10.10.1.44/27 and 10.10.1.90/27--> different networks
         */

        [Fact]
        public void NetworkPartShouldBeEqualForSameNetworks()
        {
            // Arrange
            string firstAddressSring = "10.10.1.32";
            string firstMaskString = "27";

            // Null-forgiving operator is used, as we are sure that null is not possible
            IPv4Address firstAddr = ParseInputIPAddress(firstAddressSring)!;
            IPv4SubnetMask firstMask= ParseSubnetMaskString(firstMaskString)!;

            string secondAddressString = "10.10.1.44";
            string secondMaskString = "27";
            IPv4Address secondAddr = ParseInputIPAddress(secondAddressString)!;
            IPv4SubnetMask secondMask = ParseSubnetMaskString(secondMaskString)!;

            // Act
            uint firstNetworkPart = GetNetworkPart(firstAddr, firstMask);
            uint secondNetworkPart = GetNetworkPart(secondAddr, secondMask);

            // Assert
            Assert.Equal(firstNetworkPart, secondNetworkPart);
        }

        [Fact]
        public void NetworkPartsShouldNotBeEqualForDifferentNetworks()
        {
            // Arrange
            string firstAddressSring = "10.10.1.32";
            string firstMaskString = "27";

            // Null-forgiving operator is used, as we are sure that null is not possible
            IPv4Address firstAddr = ParseInputIPAddress(firstAddressSring)!;
            IPv4SubnetMask firstMask = ParseSubnetMaskString(firstMaskString)!;

            string secondAddressString = "10.10.1.90";
            string secondMaskString = "27";
            IPv4Address secondAddr = ParseInputIPAddress(secondAddressString)!;
            IPv4SubnetMask secondMask = ParseSubnetMaskString(secondMaskString)!;

            // Act
            uint firstNetworkPart = GetNetworkPart(firstAddr, firstMask);
            uint secondNetworkPart = GetNetworkPart(secondAddr, secondMask);

            // Assert
            Assert.NotEqual(firstNetworkPart, secondNetworkPart);
        }
    }
}
