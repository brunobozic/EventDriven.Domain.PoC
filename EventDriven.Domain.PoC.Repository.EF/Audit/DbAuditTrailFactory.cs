﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using EventDriven.Domain.PoC.Domain.DomainEntities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Repository.EF.Audit
{
    public class DbAuditTrailFactory
    {
        private readonly DbContext _context;

        public DbAuditTrailFactory(DbContext context)
        {
            _context = context;
        }

        public AuditTrail GetAudit(EntityEntry entry)
        {
            var audit = new AuditTrail
            {
                UserId = 1, // System.Web.HttpContext.Current.User.Identity.Name; //Change this line according to your needs
                TableName = GetTableName(entry),
                UpdatedAt = DateTime.Now,
                TableIdValue = GetKeyValue(entry)
            };

            switch (entry.State)
            {
                case EntityState.Added:
                {
                    var newValues = new StringBuilder();
                    SetAddedProperties(entry, newValues);
                    audit.NewData = newValues.ToString();
                    audit.Actions = AuditActions.I.ToString();
                    break;
                }
                case EntityState.Deleted:
                {
                    var oldValues = new StringBuilder();
                    SetDeletedProperties(entry, oldValues);
                    audit.OldData = oldValues.ToString();
                    audit.Actions = AuditActions.D.ToString();
                    break;
                }
                case EntityState.Modified:
                {
                    var oldValues = new StringBuilder();
                    var newValues = new StringBuilder();
                    SetModifiedProperties(entry, oldValues, newValues);
                    audit.OldData = oldValues.ToString();
                    audit.NewData = newValues.ToString();
                    audit.Actions = AuditActions.U.ToString();
                    break;
                }

                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
            }

            return audit;
        }

        private void SetAddedProperties(EntityEntry entry, StringBuilder newData)
        {
            var r = JsonConvert.SerializeObject(entry.Entity, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            foreach (var propertyName in entry.CurrentValues.Properties)
            {
                var newVal = entry.CurrentValues[propertyName];
                if (newVal != null) newData.AppendFormat("[{0}]=[{1}] || ", propertyName.Name, newVal);
            }

            if (newData.Length > 0)
                newData = newData.Remove(newData.Length - 3, 3);
        }

        private void SetDeletedProperties(EntityEntry entry, StringBuilder oldData)
        {
            var dbValues = entry.GetDatabaseValues();
            var r = JsonConvert.SerializeObject(entry.Entity, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            foreach (var propertyName in dbValues.Properties)
            {
                var oldVal = dbValues[propertyName];
                if (oldVal != null) oldData.AppendFormat("[{0}]=[{1}] || ", propertyName.Name, oldVal);
            }

            if (oldData.Length > 0)
                oldData = oldData.Remove(oldData.Length - 3, 3);
        }

        private void SetModifiedProperties(EntityEntry entry, StringBuilder oldData, StringBuilder newData)
        {
            var dbValues = entry.GetDatabaseValues();
            var r = JsonConvert.SerializeObject(entry.Entity, new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            foreach (var propertyName in entry.OriginalValues.Properties)
            {
                var oldVal = dbValues[propertyName];
                var newVal = entry.CurrentValues[propertyName];
                if (oldVal != null && newVal != null && !Equals(oldVal, newVal))
                {
                    newData.AppendFormat("[{0}]=[{1}] || ", propertyName.Name, newVal);
                    oldData.AppendFormat("[{0}]=[{1}] || ", propertyName.Name, oldVal);
                }
            }

            if (oldData.Length > 0)
                oldData = oldData.Remove(oldData.Length - 3, 3);
            if (newData.Length > 0)
                newData = newData.Remove(newData.Length - 3, 3);
        }

        public long? GetKeyValue(EntityEntry entry)
        {
            long id = 0;

            var t = entry.Entity.GetType();

            var propInfo = t.GetProperties().FirstOrDefault(o =>
                o.CustomAttributes.FirstOrDefault(oo => oo.AttributeType == typeof(KeyAttribute)) != null);

            if (propInfo == null) //Fall back to Id Name
                propInfo = t.GetProperty("Id");

            if (propInfo != null)
                id = (long) propInfo.GetValue(entry.Entity);

            return id;
        }

        private string GetTableName(EntityEntry dbEntry)
        {
            var tableAttr =
                dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false)
                    .SingleOrDefault() as TableAttribute;
            var tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;
            return tableName;
        }
    }
}