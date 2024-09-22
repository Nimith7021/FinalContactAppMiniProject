using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactAppFinalAuthFluentMVC.DTO;
using ContactAppFinalAuthFluentMVC.Models;

namespace ContactAppFinalAuthFluentMVC
{
    public class Mapper
    {
        public static ContactDTO ToDTo(Contact contact)
        {
            return new ContactDTO
            {
                Id = contact.Id,
                FName = contact.FName,
                LName = contact.LName,
                IsActive = contact.IsActive,
                contactDetailDTOs = contact.ContactDetails.Select(cd => new ContactDetailDTO
                {
                    Id = cd.Id,
                    Type = cd.Type,
                    Value = cd.Value
                }).ToList()


            };
        }

        public static ContactDetailDTO ToDetailDTO(ContactDetail contactDetail)
        {
            return new ContactDetailDTO
            {
                Id = contactDetail.Id,
                Type = contactDetail.Type,
                Value = contactDetail.Value
            };

        }

        
    }
}