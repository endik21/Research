namespace DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Domain;

    public interface IUserRepository
    {
        User FindById(int id);

        bool Save(User user);

        void Delete(IList<User> users);

        User FindByLastName(string lastName);
    }
}