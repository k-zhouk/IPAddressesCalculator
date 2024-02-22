using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;
using Xunit.Abstractions;

namespace IPAddrCalc_Tests.Subnet_Mask_Tests
{
    public class TestSubnetMaskFormat
    {
        // Helper property and method for output
        private readonly ITestOutputHelper _output;
        public TestSubnetMaskFormat(ITestOutputHelper output)
        {
            _output = output;
        }

        #region Tests for the CIDR format of the subnet mask
        [Fact]
        public void ShouldBeNullForNegativeCIDRMask()
        {
            // Arrange
            string mask = "-1";

            // Act
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Assert
            Assert.Null(parseResult);
        }

        [Fact]
        public void ShouldBe0For0CIDRMask()
        {
            // Arrange
            string mask = "0";

            // Act
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Assert
            Assert.Equal((uint)0, parseResult.CIDR);

            _output.WriteLine("CIDR mask to parse: {0}", mask);
            _output.WriteLine("CIDR of the object: {0}", parseResult.CIDR);
        }

        [Fact]
        public void ShouldPassForRandomeCIDRMask()
        {
            // Arrange
            Random rnd = new Random();
            int randomCidr= rnd.Next(32);
            string mask= randomCidr.ToString();

            // Act
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Assert
            Assert.Equal((uint)randomCidr, parseResult.CIDR);

            _output.WriteLine("CIDR mask to parse: {0}", mask);
            _output.WriteLine("CIDR of the object: {0}", parseResult.CIDR);
        }

        [Fact]
        public void ShouldBe32For32CIDRMask()
        {
            // Arrange
            string mask = "32";

            // Act
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Assert
            Assert.Equal((uint)32, parseResult.CIDR);

            _output.WriteLine("CIDR mask to parse: {0}", mask);
            _output.WriteLine("CIDR of the object: {0}", parseResult.CIDR);
        }

        [Fact]
        public void ShouldBeNullForCidrMaskGreaterThan32()
        {
            // Arrange
            Random rnd = new Random();
            int randomCidr = rnd.Next(33, 256);
            string mask = randomCidr.ToString();

            // Act
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Assert
            Assert.Null(parseResult);
        }
        #endregion

        #region Test other methods to process a subnet mask
        
        [Fact]
        public void ShouldBeFalseForNonValidNetworkMask()
        {
            // Arrange
            string testStringMask = "100.0.0.0";
            // BIN for of this mask is 01100100.00000000.00000000.00000000

            // Manual setup of the non-valid mask
            IPv4SubnetMask testMask = new();
            // Hex of the testMask is 0x64000000
            testMask.CIDR = 0x64000000;

            // Act
            bool isValid = IsSubnetMaskValid(testMask.CIDR);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ShouldBeTrueForAClassMask()
        {
            // Arrange
            // BIN for of this mask is 11111111.00000000.00000000.00000000
            string mask = "255.0.0.0";
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Act
            bool isValid = IsSubnetMaskValid(parseResult.SubnetMask);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ShouldBeTrueForBClassMask()
        {
            // Arrange
            // BIN for of this mask is 11111111.11111111.00000000.00000000
            string mask = "255.255.0.0";
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Act
            bool isValid = IsSubnetMaskValid(parseResult.SubnetMask);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ShouldBeTrueForCClassMask()
        {
            // Arrange
            // BIN for of this mask is 11111111.11111111.11111111.00000000
            string mask = "255.255.255.0";
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Act
            bool isValid = IsSubnetMaskValid(parseResult.SubnetMask);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ShouldBeTrueForValidCIDRMask()
        {
            // Arrange
            // BIN for of this mask is 11111111.11111000.00000000.00000000
            string mask = "255.248.0.0";
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Act
            bool isValid = IsSubnetMaskValid(parseResult.SubnetMask);

            // Assert
            Assert.True(isValid);
        }
        #endregion
    }
}
