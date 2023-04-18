using System.Text.Json.Serialization;

namespace Tradify.Identity.Domain.Entities
{
    public class RefreshSession : BaseEntity
    {
        public int UserId { get; set; }
        public Guid RefreshToken { get; set; }

        [JsonIgnore] 
        public User? User { get; set; }
    }
}
