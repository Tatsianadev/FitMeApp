using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class ChatContactEntityBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string InterlocutorId { get; set; }
    }
}
