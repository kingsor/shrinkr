﻿namespace Shrinkr.Infrastructure.EntityFramework.Configuration
{
    using DomainObjects;

    using Microsoft.Data.Objects;

    public class ReservedAliasConfiguration : EntityConfiguration<ReservedAlias>
    {
        public ReservedAliasConfiguration()
        {
            HasKey(r => r.Id);

            Property(r => r.Id).IsIdentity();
            Property(r => r.Name).IsUnicode().IsRequired().IsVariableLength().HasMaxLength(440);

            MapSingleType(r => new
                                   {
                                       r.Id,
                                       r.Name
                                   }).ToTable("ReservedAlias");
        }
    }
}