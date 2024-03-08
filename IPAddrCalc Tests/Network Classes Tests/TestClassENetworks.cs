using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IPAddrCalc_Tests.NetworkClasses
{
    public class TestClassENetworks
    {
        #region General tests for the class E networks
        /* 
         * Range:           240.0.0.0 (0xF0000) ~ 255.255.255.255 (0xFFFFFFFF)
         * Subnet Mask:     No
        */

        [Fact]
        public void ShouldBeNotEqualIfAddressLessThanLowestEClassAddr()
        {
            // Arrange
            string testAddr = "239.255.225.255";
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            // For the E class networks the mask is ignored, so it can be any
            string testMask = "255.255.255.255";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.NotEqual("E", networkClass);
        }

        [Fact]
        public void ShouldBeEqualForLowestClassEAddr()
        {
            // Arrange
            string testAddr = "240.0.0.0";
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            // For the E class networks the mask is ignored, so it can be any
            string testMask = "255.255.255.255";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("E", networkClass);
        }

        [Fact]
        public void ShouldBeEqualForRandomClassEAddr()
        {
            // Arrange

            // Generation of a random D class network address
            Random rng = new();
            string firstByte = ((byte)rng.Next(240, 254)).ToString();
            string secondByte = ((byte)rng.Next(256)).ToString();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();
            string testAddr = firstByte + '.' + secondByte + '.' + thirdByte + '.' + fourthByte;

            IPv4Address? addrs = ParseInputIPAddress(testAddr);

            // For the D class networks the mask is ignored, so it can be any
            string testMask = "255.255.255.255";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addrs, mask);

            // Assert
            Assert.Equal("E", networkClass);
        }

        [Fact]
        public void ShouldBeEqualForUpmostClassEAddr()
        {
            // Arrange
            string testAddr = "255.255.255.255";
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            // For the D class networks the mask is ignored, so it can be any
            string testMask = "255.255.255.255";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("E", networkClass);
        }

        #endregion
    }
}
