using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IPAddrCalc_Tests.NetworkClasses
{
    public class TestClassCNetworks
    {
        #region Tests for the Class C network mask
        /*
         * Subnet mask: 255.255.255.0
        */

        [Fact]
        public void ShouldPassIfMaskIsClassCMask()
        {
            // Arrange
            string testSubnetMask = "255.255.255.0";

            // Act
            IPv4SubnetMask? subnetMask = ParseSubnetMaskString(testSubnetMask);
            bool isClassCMask = IsClassCMask(subnetMask);

            // Assert
            Assert.True(isClassCMask);
        }

        [Fact]
        public void ShouldBeFalseIfMaskIsNotClassCMask()
        {
            // Arrange
            string testSubnetMask = "255.255.254.0";

            // Act
            IPv4SubnetMask? subnetMask = ParseSubnetMaskString(testSubnetMask);
            bool isClassCMask = IsClassCMask(subnetMask);

            // Assert
            Assert.False(isClassCMask);
        }
        #endregion

        #region General tests for the class C networks
        /*
         * IP range:        192.0.0.0 ~ 223.255.255.255
         * Subnet mask:     255.255.255.0
        */

        [Fact]
        public void ShouldBeNotEqualIfAddressLessThanLowestClassCAddr()
        {
            // Arrange
            string testAddr = "191.255.225.255";
            string testMask = "255.255.255.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.NotEqual("C", networkClass);
        }

        [Fact]
        public void ShouldPassForLowestClassCAddr()
        {
            // Arrange
            string testAddr = "192.0.0.0";
            string testMask = "255.255.255.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("C", networkClass);
        }

        [Fact]
        public void ShouldPassForRandomClassCAddr()
        {
            // Arrange

            // Generation of a random B class network address
            Random rng = new Random();
            string firstByte = ((byte)rng.Next(192, 224)).ToString();
            string secondByte = ((byte)rng.Next(256)).ToString();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();

            string testAddr = firstByte + '.' + secondByte + '.' + thirdByte + '.' + fourthByte;

            IPv4Address? addrs = ParseInputIPAddress(testAddr);

            string testMask = "255.255.255.0";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addrs, mask);

            // Assert
            Assert.Equal("C", networkClass);
        }

        [Fact]
        public void ShouldPassForUpmostClassCAddr()
        {
            // Arrange
            string testAddr = "223.255.255.255";
            string testMask = "255.255.255.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("C", networkClass);
        }

        [Fact]
        public void ShouldBeNotEqualIfGreaterThanUpmostClassBAddr()
        {
            // Arrange
            string testAddr = "224.0.0.0";
            string testMask = "255.255.255.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.NotEqual("C", networkClass);
        }
        #endregion
    }
}
