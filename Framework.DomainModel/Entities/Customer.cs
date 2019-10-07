using Framework.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.DomainModel.Entities
{
    public class Customer : Entity
    {
        [LocalizeRequired(FieldName = "User name")]
        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime? Dob { get; set; }
        public string Description { get; set; }
        public int? Gender { get; set; }
        public bool? IsActive { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string ActiveCode { get; set; }
        public DateTime? ExpiredTime { get; set; }
        public string ForgotPassword { get; set; }
        public string UnsubscribeCode { get; set; }
        public bool? IsUnsubscribe { get; set; }


        public bool HasValidRefreshToken(string refreshToken)
        {
            return RefreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public void AddRefreshToken(string token, int customerId, double daysToExpire = 5)
        {
            RefreshTokens.Add(new RefreshToken
            {
                Token = token,
                Expires = DateTime.UtcNow.AddDays(daysToExpire),
                CustomerId = customerId
            });
        }

        public void RemoveRefreshToken(string refreshToken)
        {
            RefreshTokens.Remove(RefreshTokens.First(t => t.Token == refreshToken));
        }

        [LocalizeRequired]
        [LocalizeEmailAddress]
        public string Email { get; set; }
        public byte[] Avatar { get; set; }
        public bool? IsAccountFacebook { get; set; }
        public bool? IsAccountGoogle { get; set; }
        public int? LanguageId { get; set; }
        public string CategoryConfig { get; set; }
        public bool? IsCompleteSetupCategory { get; set; }
        public virtual ICollection<PlayList> PlayLists { get; set; } = new List<PlayList>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<LikeComment> LikeComments { get; set; } = new List<LikeComment>();
        public virtual ICollection<LikeVideo> LikeVideos { get; set; } = new List<LikeVideo>();
        public virtual Language Language { get; set; }
        public virtual ICollection<CustomerVideoWatched> CustomerVideoWatcheds { get; set; } = new List<CustomerVideoWatched>();
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}