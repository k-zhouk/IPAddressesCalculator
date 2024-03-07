using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IPAddrCalc_Tests.NetworkClasses
{
    public class TestClassDNetworks
    {
        #region General tests for the class D networks
        /* 
         * Range:           224.0.0.0 (0xE0000) ~ 239.255.255.255 (0xEFFFFFFF)
         * Subnet Mask:     No
        */

        [Fact]
        public void ShouldBeNotEqualIfAddressLessThanLowestDClassAddr()
        {
            // Arrange
            string testAddr = "223.255.225.255";
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            // For the D class networks the mask is ignored, so it can be any
            string testMask = "255.255.255.255";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.NotEqual("D", networkClass);
        }

        [Fact]
        public void ShouldBeEqualForLowestClassDAddr()
        {
            // Arrange
            string testAddr = "224.0.0.0";
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            // For the D class networks the mask is ignored, so it can be any
            string testMask = "255.255.255.255";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("D", networkClass);
        }

        [Fact]
        public void ShouldBeEqualForRandomClassDAddr()
        {
            // Arrange

            // Generation of a random D class network address
            Random rng = new();
            string firstByte = ((byte)rng.Next(224, 240)).ToString();
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
            Assert.Equal("D", networkClass);
        }

        [Fact]
        public void ShouldBeEqualForUpmostClassDAddr()
        {
            // Arrange
            string testAddr = "239.255.255.255";
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            // For the D class networks the mask is ignored, so it can be any
            string testMask = "255.255.255.255";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("D", networkClass);
        }

        [Fact]
        public void ShouldBeNotEqualIfGreaterThanUpmostClassDAddr()
        {
            // Arrange
            string testAddr = "240.0.0.0";
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            // For the D class networks the mask is ignored, so it can be any
            string testMask = "255.255.255.255";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.NotEqual("D", networkClass);
        }
        #endregion
    }
}
