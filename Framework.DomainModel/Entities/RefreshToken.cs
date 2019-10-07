using System;

namespace Framework.DomainModel.Entities
{
    public class RefreshToken : Entity
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public int CustomerId { get; set; }
        public bool Active => DateTime.UtcNow <= Expires;

    }
}