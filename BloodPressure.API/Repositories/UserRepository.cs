using BloodPressureApp.API.Models;

namespace BloodPressureApp.API.Repositories
{
    public class UserRepository : GenericRepository<Users>
    {
        private readonly BloodpressureContext context;

        public UserRepository(BloodpressureContext context)
            : base(context)
        {
            this.context = context;
        }

        public bool UserExists(string username)
        {
            return context.Users.Any(x => x.Username == username); 
        }

        public Users? GetUser(string username)
        {
            return context.Users.FirstOrDefault(x => x.Username == username);
        }
    }
}
