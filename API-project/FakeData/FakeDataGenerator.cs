using AppCore.Models;
using AppCore.Models.Enums;
using Bogus;
using Infrastructure.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FakeData
{
    public class FakeDataGenerator
    {
        private class userData
        {
            public string login { get; set; }
            public string email { get; set; }
        }
        public IEnumerable<UserEntity> RandomUser(int count = 1)
        {

            Randomizer.Seed = new Random();
            var faker = new Faker<userData>("pl")
                .RuleFor(e => e.email, f => f.Person.Email)
                .RuleFor(e => e.login, f => f.Internet.UserName());
            for (int i = 0; i < count; i++)
            {
                var user = faker.Generate();
                yield return new UserEntity()
                {
                    Email = user.email,
                    UserName = user.login
                };
            }
        }
        public IEnumerable<PublishTag> RandomTag(int count = 1)
        {
            Randomizer.Seed = new Random();
            var faker = new Faker<PublishTag>("pl")
                .RuleFor(e => e.Name, f => f.Commerce.Product());
            for (int i = 0; i < count; i++)
            {
                yield return faker.Generate();
            }
        }

        public IEnumerable<PublishAlbum> RandomAlbum(int count = 1)
        {
            Randomizer.Seed = new Random();
            var faker = new Faker<PublishAlbum>("pl")
                .RuleFor(e => e.Name, f => f.Hacker.IngVerb())
                .RuleFor(e => e.Status, f=> f.PickRandom<Status>());
            for (int i = 0; i < count; i++)
            {
                yield return faker.Generate();
            }
        }
        public IEnumerable<Publish> RandomPublish(int count = 1)
        {
            Randomizer.Seed = new Random();
            var faker = new Faker<Publish>("pl")
                .RuleFor(e => e.Status, f => f.PickRandom<Status>())
                .RuleFor(e => e.Camera, f => f.PickRandom<Cameras>())
                .RuleFor(e => e.Description, f => f.Random.Words(4))
                .RuleFor(e => e.ImageName,f => f.Random.Word());
            for (int i = 0; i < count; i++)
            {
                yield return faker.Generate();
            }
        }
        public Stream RandomImage()
        {
            Random random = new Random();
            var path = AppContext.BaseDirectory;
            path = path.Replace("WebApi\\bin\\Debug\\net7.0\\", "");
            path = path + $"FakeData\\SampleImages\\zdj{random.Next(1,9)}.jpg";
            return File.Open(path, FileMode.Open);
        }

        public IEnumerable<Comment> RandomComment(int count = 1)
        {
            Randomizer.Seed = new Random();
            var faker = new Faker<Comment>("pl")
                .RuleFor(e => e.Content, f => f.Random.Words(3));
               
            for (int i = 0; i < count; i++)
            {
                yield return faker.Generate();
            }
        }
    }
}