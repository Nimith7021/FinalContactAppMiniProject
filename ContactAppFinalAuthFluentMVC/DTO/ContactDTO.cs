using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactAppFinalAuthFluentMVC.DTO
{
    public class ContactDTO
    {
        public int Id { get; set; }
        public string FName { get; set; }

        public string LName { get; set; }

        public bool IsActive { get; set; } = true;

        public List<ContactDetailDTO> contactDetailDTOs { get; set; }
    }
}