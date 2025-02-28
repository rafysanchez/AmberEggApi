﻿using System;
using System.ComponentModel.DataAnnotations;
using Api.Common.Contracts.Entities;

namespace Api.Common.Repository.Entities
{
    public abstract class DomainEntity : IDomainEntity
    {
        [Required] public Guid AuditUserId { get; set; }

        [Key] public Guid Id { get; set; }

        [Required] public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public override string ToString()
        {
            return $"Type:{GetType().Name} - Id:{Id}";
        }
    }
}