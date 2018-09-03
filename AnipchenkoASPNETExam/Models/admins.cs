namespace AnipchenkoASPNETExam.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class admins
    {
        public int id { get; set; }

        public int? userID { get; set; }

        public virtual users users { get; set; }
    }
}
