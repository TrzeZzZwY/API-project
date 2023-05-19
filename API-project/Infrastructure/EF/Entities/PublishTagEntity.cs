﻿namespace Infrastructure.EF.Entities
{
    public class PublishTagEntity
    {
        public PublishTagEntity()
        {
            
        }
        public PublishTagEntity(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public ISet<PublishEntity> Publishes { get; set; }
    }
}