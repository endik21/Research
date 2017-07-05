namespace DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Domain;

    public class UserRepository : IUserRepository
    {
        public User FindById(int id)
        {
            User result;
            using (var ctx = new Context())
            {
                result = ctx.Users.FirstOrDefault(x => x.Id == id);
            }

            return result;
        }

        public bool Save(User user)
        {
            using (var ctx = new Context())
            {
                try
                {
                    ctx.Users.Add(user);
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public void Delete(IList<User> users)
        {
            throw new NotImplementedException();
        }

        public User FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }
    }
}
