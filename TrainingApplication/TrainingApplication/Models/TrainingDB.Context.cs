﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TrainingDBEntities : DbContext
    {
        public TrainingDBEntities()
            : base("name=TrainingDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Course_CourseCate> Course_CourseCate { get; set; }
        public virtual DbSet<CourseCate> CourseCates { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Trainee> Trainees { get; set; }
        public virtual DbSet<Trainee_Course> Trainee_Course { get; set; }
        public virtual DbSet<Trainer> Trainers { get; set; }
        public virtual DbSet<Trainer_Course_Topic> Trainer_Course_Topic { get; set; }
        public virtual DbSet<TrainingStaff> TrainingStaffs { get; set; }
        public virtual DbSet<TrainingStaff_Trainee> TrainingStaff_Trainee { get; set; }
    }
}
