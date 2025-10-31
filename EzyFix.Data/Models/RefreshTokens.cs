using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzyFix.DAL.Models
{
    public partial class RefreshTokens
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; }
        
        [Column(TypeName = "timestamp with time zone")]
        public DateTime CreateAt { get; set; }
        
        [Column(TypeName = "timestamp with time zone")]
        public DateTime ExpiresAt { get; set; }

        public virtual User User { get; set; }
    }
}