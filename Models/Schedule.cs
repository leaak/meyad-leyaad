//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeyadLeyaad1.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    public partial class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Schedule { get; set; }
		[Required]
        public string Day { get; set; }
        [Required]
        public string Start_Time { get; set; }
        [Required]
        public string End_Time { get; set; }
        public int Id_User { get; set; }
        public Nullable<bool> User_Type { get; set; }
    }
}
