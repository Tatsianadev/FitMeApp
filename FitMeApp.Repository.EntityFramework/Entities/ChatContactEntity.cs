using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("ChatContacts")]
    public class ChatContactEntity: ChatContactEntityBase
    {
        public virtual User Users { get; set; }
    }
}
