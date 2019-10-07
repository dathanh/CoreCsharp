using Framework.DomainModel;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Mapping;
using Framework.DomainModel.ValueObject;
using Framework.Exceptions;
using Framework.Service.Diagnostics;
using Framework.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Database.Persistance.Tenants
{
    public class TenantDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRoleFunction> UserRoleFunctions { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<GridConfig> GridConfigs { get; set; }
        public DbSet<SecurityOperation> SecurityOperations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDiagnosticService _diagnosticService;

        public TenantDataContext(DbContextOptions<TenantDataContext> options, IHttpContextAccessor httpContextAccessor,
            IDiagnosticService diagnosticService)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _diagnosticService = diagnosticService;
        }

        /// <summary>
        /// Get current user and store created user, modified user in each record
        /// </summary>
        /// <returns></returns>
        public User GetCurrentUser()
        {
            User currentUser = null;
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.User == null || _httpContextAccessor.HttpContext.User.Claims == null)
            {
                return null;
            }
            var userDataClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimsDeclaration.UserDataClaim);

            if (userDataClaim != null)
            {
                var userDataJson = userDataClaim.Value;
                var userDto = JsonConvert.DeserializeObject<UserDto>(userDataJson);
                if (userDto != null)
                {
                    currentUser = Users.SingleOrDefault(x => x.Id == userDto.Id);
                }
            }

            return currentUser;
        }

        /// <summary>
        /// Save change with unit of work in data context
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            var currentUser = GetCurrentUser();

            var validationResults = new List<ValidationResult>();

            // Re-calculate the user created, user modified, created on, modified on
            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                if (entry.Entity.IsDeleted)
                {
                    entry.State = EntityState.Deleted;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.SetCreatedOn(DateTime.UtcNow);

                        entry.Entity.SetCreatedBy(currentUser);
                        entry.Entity.SetLastUser(currentUser);
                        entry.Entity.SetLastModified(DateTime.UtcNow);
                        break;
                    case EntityState.Modified:
                        entry.Entity.SetLastModified(DateTime.UtcNow);
                        entry.Entity.SetLastUser(currentUser);
                        entry.OriginalValues["LastModified"] = entry.Entity.LastModified;
                        break;
                    default:
                        if (entry.State == EntityState.Deleted || entry.Entity.IsDeleted)
                        {
                            entry.State = EntityState.Deleted;
                            //entry.OriginalValues["LastModified"] = entry.Entity.LastModified;
                        }
                        break;
                }

                // Calculate the validate object and write log in case we have validate error for entity when save
                if (!Validator.TryValidateObject(entry.Entity, new ValidationContext(entry.Entity), validationResults))
                {
                    // throw new ValidationException() or do whatever you want
                }
            }

            int result;

            try
            {
                result = base.SaveChanges();
            }
            catch (Exception ex) when (validationResults.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var failure in validationResults)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.MemberNames);
                }
                if (sb.Length != 0)
                {
                    _diagnosticService?.Error(sb.ToString());
                }
                throw new UserVisibleException("GeneralExceptionMessageText", ex);
            }
            catch (Exception ex)
            {
                var exceptionString = ex.ToString();
                var exceptionNameArray = exceptionString.Substring(0, exceptionString.IndexOf(':')).Split(new[] { '.' });
                var actualName =
                    $"Database.Persistance.ExceptionHandler.{exceptionNameArray[exceptionNameArray.Length - 1]}Handler";
                var exceptionType = Type.GetType(actualName);
                var exceptionParams = new object[] { "GeneralExceptionMessageText", ex };

                if (exceptionType != null)
                {
                    exceptionParams = (object[])exceptionType.GetMethod("Process").Invoke(null, new object[] { ex });
                }

                throw new UserVisibleException(exceptionParams[0].ToString(), (Exception)exceptionParams[1]);
            }

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
            modelBuilder.ApplyConfiguration(new UserRoleFunctionMap());
            modelBuilder.ApplyConfiguration(new DocumentTypeMap());
            modelBuilder.ApplyConfiguration(new GridConfigMap());
            modelBuilder.ApplyConfiguration(new SecurityOperationMap());
            modelBuilder.ApplyConfiguration(new LanguageMap());
            modelBuilder.ApplyConfiguration(new ConfigMap());
            modelBuilder.ApplyConfiguration(new RefreshTokenMap());
        }
    }
}