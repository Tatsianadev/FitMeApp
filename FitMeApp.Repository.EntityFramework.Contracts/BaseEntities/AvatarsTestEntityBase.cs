using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class AvatarsTestEntityBase
    {
        public int Id { get; set; }
        public byte[] Avatar { get; set; }
    }
}
