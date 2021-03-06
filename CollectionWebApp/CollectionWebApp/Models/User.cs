using System.ComponentModel.DataAnnotations.Schema;

namespace CollectionWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;

        public virtual ICollection<UserCollection> UserCollections { get; set; } = null!;

        public virtual ICollection<Like> Likes { get; set; } = null!;

        public virtual ICollection<Commentary> Commentaries { get; set; } = null!;
    }
}
