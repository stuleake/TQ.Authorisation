using System.Collections;
using System.Collections.Generic;

namespace TQ.Geocoding.API.Test
{
    public class AddressByPostcode : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "B91 2PJ", "3", 1, "3 SILVERBIRCH ROAD, SOLIHULL, B91 2PJ", 52.41314, -1.76195526, 416288, 279485 };
            yield return new object[] { "B91", "8 SILVER*", 1, "8 SILVERBIRCH ROAD, SOLIHULL, B91 2PJ", 52.41351, -1.76223266, 416269, 279526 };
            yield return new object[] { "BZ1", "", 0, "0", 0, 0, 0, 0 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class AddressByText : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "3 B91 2PJ", 1, "3 SILVERBIRCH ROAD, SOLIHULL, B91 2PJ", 52.41314, -1.76195526, 416288, 279485 };
            yield return new object[] { "8 SILVER* B91", 1, "8 SILVERBIRCH ROAD, SOLIHULL, B91 2PJ", 52.41351, -1.76223266, 416269, 279526 };
            yield return new object[] { "BZ1", 0, "", 0, 0, 0, 0 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class AddressByUprn : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 100071010823, 1, "3 SILVERBIRCH ROAD, SOLIHULL, B91 2PJ", 52.41314, -1.76195526, 416288, 279485 };
            yield return new object[] { 100071010828, 1, "8 SILVERBIRCH ROAD, SOLIHULL, B91 2PJ", 52.41351, -1.76223266, 416269, 279526 };
            yield return new object[] { 0, 0, "", 0, 0, 0, 0 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class AddressByCoords : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 52.41314, -1.76195526, 416288, 279485, 1, "3 SILVERBIRCH ROAD, SOLIHULL, B91 2PJ" };
            yield return new object[] { 52.41351, -1.76223266, 416269, 279526, 1, "8 SILVERBIRCH ROAD, SOLIHULL, B91 2PJ" };
            yield return new object[] { 99.99, 99.99, 0, 0, 0, "" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class AddressCountByPostcode : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "B91 2PJ", "3", 1 };
            yield return new object[] { "B91", "8 SILVER*", 1 };
            yield return new object[] { "B91", "SILVERBIRCH", 31 };
            yield return new object[] { "BZ1", "", 0 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
