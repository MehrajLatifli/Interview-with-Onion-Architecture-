﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Interview.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Interview.Domain.Entities.IdentityAuth;

[Table("RoleClaims")]
public  class RoleClaim : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string ClaimType { get; set; }

    public string ClaimValue { get; set; }

    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("RoleClaim")]
    public virtual Role Role { get; set; }
}