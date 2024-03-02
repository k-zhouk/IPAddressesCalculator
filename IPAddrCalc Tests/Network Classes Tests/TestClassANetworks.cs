using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IPAddrCalc_Tests.NetworkClasses
{
    public class TestClassANetworks
    {
        #region Tests for the Class A network mask
        /*
         * Subnet mask: 255.0.0.0
        */

        [Fact]
        public void ShouldPassIfMaskIsClassAMask()
        {
            // Arrange
            string testMask = "255.0.0.0";

            // Act
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);
            bool isClassAMask = IsClassAMask(mask);

            // Assert
            Assert.True(isClassAMask);
        }

        [Fact]
        public void ShouldBeFalseIfMaskIsNotClassAMask()
        {
            // Arrange
            string testMask = "248.0.0.0";

            // Act
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);
            bool isClassAMask = IsClassAMask(mask);

            // Assert
            Assert.False(isClassAMask);
        }
        #endregion

        #region General tests for the class A networks
        /* 
         * Range:           1.0.0.0 ~ 127.255.255.255
         * Subnet Mask:     255.0.0.0
        */

        [Fact]
        public void ShouldBeNotEqualIfAddressLessThanLowestClassAAddr()
        {
            // Arrange
            string testAddr = "0.255.225.255";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.NotEqual("A", networkClass);
        }

        [Fact]
        public void ShouldPassForLowestClassAAddr()
        {
            // Arrange
            string testAddr = "1.0.0.0";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("A", networkClass);
        }

        [Fact]
        public void ShouldPassForRandomClassAAddr()
        {
            // Arrange

            // Generation of a random A class network address
            Random rng = new();
            string firstByte = ((byte)rng.Next(1, 128)).ToString();
            string secondByte = ((byte)rng.Next(256)).ToString();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();

            string testAddr = firstByte + '.' + secondByte + '.' + thirdByte + '.' + fourthByte;

            IPv4Address? addrs = ParseInputIPAddress(testAddr);

            string testMask = "255.0.0.0";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addrs, mask);

            // Assert
            Assert.Equal("A", networkClass);
        }

        [Fact]
        public void ShouldPassForUpmostClassAAddr()
        {
            // Arrange
            string testAddr = "127.255.255.255";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.Equal("A", networkClass);
        }

        [Fact]
        public void ShouldBeNotEqualIfGreaterThanUpmostClassAAddr()
        {
            // Arrange
            string testAddr = "128.0.0.0";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            string networkClass = GetIPv4NetworkClass(addr, mask);

            // Assert
            Assert.NotEqual("A", networkClass);
        }
        #endregion
    }
}
