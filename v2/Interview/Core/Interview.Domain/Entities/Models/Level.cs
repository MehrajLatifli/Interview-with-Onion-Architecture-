﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Interview.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Interview.Domain.Entities.Models;


[Table("Levels")]
public  class Level : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal Coefficient { get; set; }

    [InverseProperty("Level")]
    public virtual ICollection<Question> Question { get; set; } = new List<Question>();
}