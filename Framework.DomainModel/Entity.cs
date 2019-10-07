using Framework.DomainModel.Entities;
using Framework.DomainModel.Interfaces;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.DomainModel
{

    /// <summary>
    ///     The Entity class represents a "thing" in the system's domain model.
    /// </summary>
    /// <remarks>
    ///     The term Entity is borrowed from the practices documented in the book
    ///     Domain Driven Design by Eric Evans. All core business data within the
    ///     system is defined using the DDD approach.
    ///     All entities in the system will require the standard audit information,
    ///     Last Modified By, Last Modified On, Created By and Created On. This
    ///     information should be managed as part of the transaction implementation
    ///     for all objects.
    /// </remarks>
    [Serializable]
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; }

        public int? LastUserId { get; set; }
        public int CreatedById { get; set; }

        public virtual User CreatedBy { get; set; }

        public virtual User LastUser { get; set; }

        [JsonIgnore]
        public DateTime? CreatedOn { get; protected set; }

        [JsonIgnore]
        public DateTime? LastTime { get; protected set; }

        [Timestamp]
        public virtual Byte[] LastModified { get; set; }

        public void SetCreatedOn(DateTime createdOn)
        {
            CreatedOn = createdOn;
        }

        public void SetCreatedBy(User createdBy)
        {
            CreatedBy = createdBy;
            if (createdBy != null)
            {
                CreatedById = createdBy.Id;
            }
            else
            {
                CreatedById = 0;
            }
        }

        public void SetLastModified(DateTime lastTime)
        {
            LastTime = lastTime;
        }

        public void SetLastUser(User lastUser)
        {
            LastUser = lastUser;
            if (lastUser != null)
            {
                LastUserId = lastUser.Id;
            }
            else
            {
                LastUserId = 0;
            }
        }
    }
}
