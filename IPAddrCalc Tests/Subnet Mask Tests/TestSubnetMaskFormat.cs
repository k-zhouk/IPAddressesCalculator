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
        public void ShouldPassForRandomCIDRMask()
        {
            // Arrange
            Random rnd = new Random();

            // The mask is in the range from 1 to 31
            int randomCidr= rnd.Next(1, 32);
            string mask= randomCidr.ToString();

            // Act
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Check for null just in case
            if(parseResult is null)
            {
                throw new ArgumentNullException("The parse method returned null!");
            }
            else
            {
                // Assert
                Assert.Equal((uint)randomCidr, parseResult.CIDR);

                _output.WriteLine("CIDR mask to parse: {0}", mask);
                _output.WriteLine("CIDR of the object: {0}", parseResult.CIDR);
            }
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

        [Fact]
        // The last bit of a non-valid mask is 0
        public void ShouldBeFalseForNonValidkMask1()
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
        // The last bit of a non-valid mask is 1
        public void ShouldBeFalseForNonValidMask2()
        {
            // Arrange
            string testStringMask = "255.255.255.253";
            // BIN for this mask is 11111111.11111111.11111111.11111101

            // Manual setup of non-valid mas
            IPv4SubnetMask testMask = new();
            // Hex of the testMask is 0x64000000
            testMask.CIDR = 0xFFFFFFFD;

            // Act
            bool isValid= IsSubnetMaskValid(testMask.CIDR);

            // Assert
            Assert.False(isValid);
        }
        #endregion
    }
}
