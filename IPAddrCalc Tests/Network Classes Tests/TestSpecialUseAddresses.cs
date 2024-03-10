using IP_Addresses_Calculator;
using static IP_Addresses_Calculator.IPAddrCalcLib;

namespace IPAddrCalc_Tests.NetworkClasses
{
    public class TestSpecialUseAddresses
    {
        #region Tests for the looback addresses
        /*
         * Range:       127.0.0.0 ~ 127.255.255.255 (10/8)
         * Mask:        255.x.x.x
         * Reference:   RFC6890
         * Note:        The first byte of the mask is 255, the other bytes could be any
         */

        [Fact]
        public void ShouldBeFalseIfLowerThanLowestLoopbackAddr()
        {
            // Arrange
            string testAddr = "126.255.255.255";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool testLoopback = IsLoopbackAddress(addr, mask);

            // Assert
            Assert.False(testLoopback);
        }

        [Fact]
        public void ShouldPassForLowestLoopbackAddr()
        {
            // Arrange
            string testAddr = "127.0.0.0";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool testLoopback = IsLoopbackAddress(addr, mask);

            // Assert
            Assert.True(testLoopback);
        }

        [Fact]
        public void ShouldPassForRandomLoopbackAddr()
        {
            // Arrange
            // Generation of a random A class network address
            Random rng = new Random();
            string secondByte = ((byte)rng.Next(256)).ToString();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();

            string testAddr = "127" + '.' + secondByte + '.' + thirdByte + '.' + fourthByte;

            IPv4Address? addr = ParseInputIPAddress(testAddr);

            string testMask = "255.0.0.0";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isLoopback = IsLoopbackAddress(addr, mask);

            // Assert
            Assert.True(isLoopback);
        }

        [Fact]
        public void ShouldPassForUpmostLoopbackAddr()
        {
            // Arrange
            string testAddr = "127.255.255.255";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool testLoopback = IsLoopbackAddress(addr, mask);

            // Assert
            Assert.True(testLoopback);
        }

        [Fact]
        public void ShouldBeNotEqualIfGreaterThanUpperLoopbackAddr()
        {
            // Arrange
            string testAddr = "128.0.0.0";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool testLoopback = IsLoopbackAddress(addr, mask);

            // Assert
            Assert.False(testLoopback);
        }
        #endregion

        #region Tests for the 24-bit block private networks
        /* 
         * Private range:   10.0.0.0 ~ 10.255.255.255 (10.0.0.0/8 prefix)
         * Reference:       RFC 1918
         * Note:            This range is a Class A network in pre-CIDR notation
        */

        [Fact]
        public void ShouldBeFalseIfLowerThanLowestClassAPrivateAddr()
        {
            // Arrange
            string testAddr = "9.255.255.255";
            string testMask = "255.0.0.0";
            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.False(isPrivate);
        }

        [Fact]
        public void ShouldPassForLowestClassAPrivateAddr()
        {
            // Arrange
            string testAddr = "10.0.0.0";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.True(isPrivate);
        }

        [Fact]
        public void ShouldPassForRandomeClassAPrivateNetworkAddr()
        {
            // Arrange
            // Generation of a random A class private network address
            Random rng = new Random();
            string secondByte = ((byte)rng.Next(256)).ToString();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();

            string testAddr = "10" + '.' + secondByte + '.' + thirdByte + '.' + fourthByte;
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            string testMask = "255.0.0.0";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPriavte = IsPrivateAddress(addr, mask);

            // Assert
            Assert.True(isPriavte);
        }

        [Fact]
        public void ShouldPassForUpmostClassAPrivateAddress()
        {
            // Arrange
            string testAddr = "10.255.255.255";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.True(isPrivate);
        }

        [Fact]
        public void ShouldBeFalseIfGreaterThanUpmostClassAPrivateAddress()
        {
            // Arrange
            string testAddr = "11.0.0.0";
            string testMask = "255.0.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.False(isPrivate);
        }
        #endregion

        #region Tests for the 20-bit block private networks
        /*
         * Range:       172.16.0.0 ~ 172.31.255.255 (172.16.0.0/12)
         * Reference:   RFC 1918
         * Note:        This range is a Class B network in pre-CIDR notation
        */

        [Fact]
        public void ShouldBeFalseIfLowerThanLowestBClassPrivateAddr()
        {
            // Arrange
            string testAddr = "172.15.255.255";
            string testMask = "255.240.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.False(isPrivate);
        }

        [Fact]
        public void ShouldPassForLowestBClassPrivateAddr()
        {
            // Arrange
            string testAddr = "172.16.0.0";
            string testMask = "255.240.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.True(isPrivate);
        }

        [Fact]
        public void ShouldPassForBClassPrivateNetworkRandomAddress()
        {
            // Arrange
            // Generation of a random A class private network address
            Random rng = new Random();
            string secondByte = ((byte)rng.Next(17, 32)).ToString();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();

            string testAddr = "172" + '.' + secondByte + '.' + thirdByte + '.' + fourthByte;
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            string testMask = "255.240.0.0";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPriavte = IsPrivateAddress(addr, mask);

            // Assert
            Assert.True(isPriavte);
        }

        [Fact]
        public void ShouldPassForUpmostBClassPrivateAddress()
        {
            // Arrange
            string testAddr = "172.31.255.255";
            string testMask = "255.240.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);
            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.True(isPrivate);
        }

        [Fact]
        public void ShouldBeFalseIfGreaterThanUpmostBClassBlockPrivateAddress()
        {
            // Arrange
            string testAddr = "172.32.0.0";
            string testMask = "255.240.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.False(isPrivate);
        }
        #endregion

        #region Tests for the 16-bit block private networks
        /*
         * Range:       192.168.0.0 ~ 192.168.255.255 (192.168/16 prefix)
         * Reference:   RFC 1918
         * Note:        This range is a Class C network in pre-CIDR notation
         */

        [Fact]
        public void ShouldBeFalseIfLowerThanLowestClassCPrivateAddr()
        {
            // Arrange
            string testAddr = "192.167.255.255";
            string testMask = "255.255.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.False(isPrivate);
        }

        [Fact]
        public void ShouldPassForLowestClassCPrivateAddr()
        {
            // Arrange
            string testAddr = "192.168.0.0";
            string testMask = "255.255.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.True(isPrivate);
        }

        [Fact]
        public void ShouldPassForClassCPrivateNetworkRandomAddr()
        {
            // Arrange
            // Generation of a random A class private network address
            Random rng = new Random();
            string thirdByte = ((byte)rng.Next(256)).ToString();
            string fourthByte = ((byte)rng.Next(256)).ToString();

            string testAddr = "192" + '.' + "168" + '.' + thirdByte + '.' + fourthByte;
            IPv4Address? addr = ParseInputIPAddress(testAddr);

            string testMask = "255.255.0.0";
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.True(isPrivate);
        }

        [Fact]
        public void ShouldPassForUpmostClassCPrivateAddr()
        {
            // Arrange
            string testAddr = "192.168.255.255";
            string testMask = "255.255.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.True(isPrivate);
        }

        [Fact]
        public void ShouldBeFalseIfGreaterThanUpmostClassCPrivateAddr()
        {
            // Arrange
            string testAddr = "172.32.0.0";
            string testMask = "255.255.0.0";

            IPv4Address? addr = ParseInputIPAddress(testAddr);
            IPv4SubnetMask? mask = ParseSubnetMaskString(testMask);

            // Act
            bool isPrivate = IsPrivateAddress(addr, mask);

            // Assert
            Assert.False(isPrivate);
        }
        #endregion
    }
}
