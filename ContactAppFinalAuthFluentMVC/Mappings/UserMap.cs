using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinalAuthFluentMVC.Models;
using FluentNHibernate.Mapping;

namespace ContactAppFinalAuthFluentMVC.Mappings
{
    public class UserMap:ClassMap<User>
    {
        public UserMap() {

            Table("Users");
            Id(u => u.Id).GeneratedBy.GuidComb();
            Map(u => u.FName);
            Map(u => u.LName);
            Map(u => u.IsAdmin);
            Map(u => u.IsActive);
            Map(u => u.Password);
            HasMany(c => c.Contacts).Inverse().Cascade.All();
            HasOne(u=>u.Role).PropertyRef(u=>u.User).Cascade.All().Constrained();
        }    
    }
}