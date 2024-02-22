using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IPAddrCalc_Tests.NetworkClasses
{
    public class TestClassBNetworks
    {
        #region Tests for the Class B network mask
        /*
         * Subnet mask: 255.255.0.0
        */

        [Fact]
        public void ShouldPassIfMaskIsClassBMask()
        {
            // Arrange
            string testSubnetMask = "255.255.0.0";

            // Act
            IPv4SubnetMask? subnetMask = ParseSubnetMaskString(testSubnetMask);
            bool isClassBMask = IsClassBMask(subnetMask);

            // Assert
            Assert.True(isClassBMask);
        }

        [Fact]
        public void ShouldBeFalseIfMaskIsNotClassBMask()
        {
            // Arrange
            string testSubnetMask = "255.254.0.0";

            // Act
            IPv4SubnetMask? subnetMask = ParseSubnetMaskString(testSubnetMask);
            bool isClassBMask = IsClassBMask(subnetMask);

            // Assert
            Assert.False(isClassBMask);
        }
        #endregion

        #region General tests for the class B networks
        /*
         * IP range:        128.0.0.0 ~ 191.255.255.255
         * Subnet mask:     255.255.0.0 (8 bits)
        */

        [Fact]
        public void ShouldBeNotEqualIfAddressLessThanLowestClassBAddr()
        {
            // Arrange
            string testAddr = "127.255.225.255";
            string testMask = "255.255.0.0";

            IPv4Address addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.NotEqual("B", networkClass);
        }

        [Fact]
        public void ShouldPassForLowestClassBAddr()
        {
            // Arrange
            string testAddr = "128.0.0.0";
            string testMask = "255.255.0.0";

            IPv4Address addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("B", networkClass);
        }

        [Fact]
        public void ShouldPassForRandomClassBAddr()
        {
            // Arrange

            // Generation of a random B class network address
            Random rng = new Random();
            string firstByte = ((byte)rng.Next(128, 192)).ToString();
            string secondByte = ((byte)rng.Next(256)).ToString();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();

            string testAddr = firstByte + '.' + secondByte + '.' + thirdByte + '.' + fourthByte;

            IPv4Address addrs = ParseInputIPAddress(testAddr);

            string testMask = "255.255.0.0";
            IPv4SubnetMask mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addrs, mask);

            // Assert
            Assert.Equal("B", networkClass);
        }

        [Fact]
        public void ShouldPassForUpmostClassBAddr()
        {
            // Arrange
            string testAddr = "191.255.255.255";
            string testMask = "255.255.0.0";

            IPv4Address addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("B", networkClass);
        }

        [Fact]
        public void ShouldBeNotEqualIfGreaterThanUpmostClassBAddr()
        {
            // Arrange
            string testAddr = "192.0.0.0";
            string testMask = "255.255.0.0";

            IPv4Address addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.NotEqual("B", networkClass);
        }
        #endregion
    }
}
