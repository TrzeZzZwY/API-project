﻿using AppCore.Interfaces.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Models
{
    public class Comment : IIdentity<Guid>
    {
        public Comment(Guid userId, Guid publishId, string content, bool? isEdited, Guid? id = null)
        {
            Id = id ?? Guid.Empty;
            UserId = userId;
            PublishId = publishId;
            Content = content;
            IsEdited = isEdited ?? false;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PublishId { get; set; }
        public string Content { get; set; }
        public bool IsEdited { get; set; }
    }
}
