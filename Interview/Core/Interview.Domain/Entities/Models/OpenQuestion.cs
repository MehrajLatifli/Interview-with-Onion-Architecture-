﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Interview.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Interview.Domain.Entities.Models;

[Table("OpenQuestion")]
public partial class OpenQuestion : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdOpenQuestion { get; set; }

    [Required]
    public string Question { get; set; }

    public bool? Result { get; set; }

    [InverseProperty("OpenQuestionIdForQuestionNavigation")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}