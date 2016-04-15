using System;

using CrossPlatformLibrary.Geolocation;
using CrossPlatformLibrary.Utils;

namespace MapsSample.Model
{
    public class Restaurant : IEquatable<Restaurant>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public int ZipCode { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Phone { get; set; }

        public string WebAddress { get; set; }

        public bool AcceptLunchCheck { get; set; }

        public bool AcceptCard { get; set; }

        public bool AcceptGiftCard { get; set; }

        public Position Location { get; set; }
        
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2} {3}", this.Name, this.Street, this.ZipCode, this.City);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((Restaurant)obj);
        }

        public bool Equals(Restaurant obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return string.Equals(this.Name, obj.Name) && string.Equals(this.Street, obj.Street) && this.ZipCode == obj.ZipCode && string.Equals(this.City, obj.City);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(this.Name, this.Street, this.ZipCode, this.City);
        }
    }
}