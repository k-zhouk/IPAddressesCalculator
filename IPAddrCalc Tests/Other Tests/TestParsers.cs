using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IPAddrCalc_Tests.Subnet_Mask_Tests
{
    public class TestParsers
    {
        #region Tests for IP address parser
        [Fact]
        public void ShouldBeNotNullForValidLowestIPAddress()
        {
            // Arrange
            string addressString = "0.0.0.0";

            // Act
            IPv4Address? address= ParseInputIPAddress(addressString);

            // Assert
            Assert.NotNull(address);
        }

        [Fact]
        public void ShouldBeNotNullForValidRandomIPAddress()
        {
            // Arrange
            // Generation of a random IPv4 network address
            Random rng = new();
            string firstByte = ((byte)rng.Next(256)).ToString();
            string secondByte = ((byte)rng.Next(256)).ToString();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();

            string addressString = firstByte + '.' + secondByte + '.' + thirdByte + '.' + fourthByte;

            // Act
            IPv4Address? address = ParseInputIPAddress(addressString);

            // Assert
            Assert.NotNull(address);
        }

        [Fact]
        public void ShouldBeNotNullForHighestIPAddress()
        {
            // Arrange
            string addressString = "255.255.255.255";

            // Act
            IPv4Address? address = ParseInputIPAddress(addressString);

            // Assert
            Assert.NotNull(address);
        }

        [Fact]
        public void ShouldBeNullForNonValidAddress()
        {
            // Arrange
            string addressString = "127.0.0";

            // Act
            IPv4Address? address = ParseInputIPAddress(addressString);

            // Assert
            Assert.Null(address);
        }
        #endregion

        #region Tests for Subnet mask parser
        [Fact]
        public void ShouldBeNotNullForLowestMask()
        {
            // Arrange
            string maskString = "0.0.0.0";

            // Act
            IPv4SubnetMask? mask = ParseSubnetMaskString(maskString);

            // Assert
            Assert.NotNull(mask);
        }

        [Fact]
        public void ShouldBeNotNullForRandomeMask()
        {
            // Arrange
            // Generation of a random mask 
            Random rng = new();
            string firstByte = ((byte)rng.Next(256)).ToString();
            string secondByte = ((byte)rng.Next(256)).ToString();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();

            string maskString = firstByte + '.' + secondByte + '.' + thirdByte + '.' + fourthByte;
            // Act
            IPv4SubnetMask? mask = ParseSubnetMaskString(maskString);

            // Assert
            Assert.NotNull(mask);
        }

        [Fact]
        public void ShouldBeNotNullForHighestMask()
        {
            // Arrange
            string maskString = "255.255.255.255";

            // Act
            IPv4SubnetMask? mask = ParseSubnetMaskString(maskString);

            // Assert
            Assert.NotNull(mask);
        }

        [Fact]
        public void ShouldBeNullForNonValidStringMask()
        {
            // Arrange
            string maskString = "127.0.0";

            // Act
            IPv4Address? mask = ParseInputIPAddress(maskString);

            // Assert
            Assert.Null(mask);
        }

        [Fact]
        public void ShouldBeNullForCidrMaskGreaterThan32()
        {
            // Arrange
            Random rnd = new();
            int randomCidr = rnd.Next(33, 256);
            string mask = randomCidr.ToString();

            // Act
            IPv4SubnetMask? parseResult = ParseSubnetMaskString(mask);

            // Assert
            Assert.Null(parseResult);
        }

        #endregion
    }
}
