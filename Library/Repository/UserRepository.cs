using Library.Interfaces;
using Library.Model;

namespace Library.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        private readonly ApplicationDbContext _context;

        public IQueryable<UserModel> GetUsers()
        {
            return _context.Users;
        }

        public bool AddUser(UserModel user)
        {
            _context.Users.Add(user);
            return SaveChanges();
        }

        public bool RemoveUser(UserModel user)
        {
            _context.Users.Remove(user);
            return SaveChanges();
        }

        public UserModel? GetUser(string login)
        {
            return _context.Users.FirstOrDefault(u => u.Login == login);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public UserModel? GetUser(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }
    }
}