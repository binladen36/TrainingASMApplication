//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TrainingStaff
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TrainingStaff()
        {
            this.TrainingStaff_Trainee = new HashSet<TrainingStaff_Trainee>();
        }
    
        public int TrainingStaffID { get; set; }
        public string TrainingStaffUserID { get; set; }
        public string TrainingStaffName { get; set; }
        public string TrainingStaffDes { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrainingStaff_Trainee> TrainingStaff_Trainee { get; set; }
    }
}
