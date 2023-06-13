using AppCore.Models.Enums;
using Infrastructure.EF;
using Infrastructure.EF.Entities;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Microsoft.OpenApi.Writers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using WebApi.Dto.Input;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Json.Nodes;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json;
using WebApi.Dto.Output;
using Microsoft.EntityFrameworkCore.Query.Internal;
using AppCore.Models;

namespace IntegrationTest
{
    public class ImageAppTest : IClassFixture<ImageAppTestFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly ImageAppTestFactory<Program> _app;
        private readonly AppDbContext _context;
        private readonly UserLogin _adminLoginData = new UserLogin("Admin", "1qazXSW@");
        private readonly UserLogin _userLoginData = new UserLogin("User", "1qazXSW@");
        private const string uri = "https://localhost:7068/api/v1/";

        public ImageAppTest(ImageAppTestFactory<Program> app)
        {
            _app = app;
            _client = app.CreateClient();
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<UserEntity>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<UserRoleEntity>>();

                _context = scope.ServiceProvider.GetService<AppDbContext>();


                InitUsers(userManager, roleManager);
                InitData(userManager);
            }
        }
        private void InitUsers(UserManager<UserEntity> userManager, RoleManager<UserRoleEntity> roleManager)
        {
            UserRoleEntity AdminRole = new UserRoleEntity() { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" };
            UserRoleEntity UserRole = new UserRoleEntity() { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" };

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.CreateAsync(AdminRole);
                roleManager.CreateAsync(UserRole);
                _context.SaveChanges();
            }

            UserEntity UserEntity = new UserEntity() { Email = "user@app.com", UserName = "User" };
            UserEntity AdminEntity = new UserEntity() { Email = "admin@app.com", UserName = "Admin" };
            if(userManager.Users.Count() == 0)
            {
                userManager.CreateAsync(UserEntity, "1qazXSW@");
                userManager.CreateAsync(AdminEntity, "1qazXSW@");
                userManager.AddToRoleAsync(UserEntity, "USER");
                userManager.AddToRoleAsync(AdminEntity, "ADMIN");
            }

        }
        private void InitData(UserManager<UserEntity> userManager)
        {
            var admin = userManager.Users.FirstOrDefault(e => e.UserName == "Admin");
            var user = userManager.Users.FirstOrDefault(e => e.UserName == "User");

            //Albumy
            PublishAlbumEntity userPublicAlbum = new PublishAlbumEntity()
            {
                Id = Guid.NewGuid(),
                Name = "userPublicAlbum",
                Status = Status.Visible,
                Publishes = new HashSet<PublishEntity>(),
                User = user
            };
            PublishAlbumEntity userPrivateAlbum = new PublishAlbumEntity()
            {
                Id = Guid.NewGuid(),
                Name = "userPrivateAlbum",
                Status = Status.Hidden,
                Publishes = new HashSet<PublishEntity>(),
                User = user
            };
            PublishAlbumEntity adminPublicAlbum = new PublishAlbumEntity()
            {
                Id = Guid.NewGuid(),
                Name = "adminPublicAlbum",
                Status = Status.Visible,
                Publishes = new HashSet<PublishEntity>(),
                User = admin
            };
            PublishAlbumEntity adminPrivateAlbum = new PublishAlbumEntity()
            {
                Id = Guid.NewGuid(),
                Name = "adminPrivateAlbum",
                Status = Status.Hidden,
                Publishes = new HashSet<PublishEntity>(),
                User = admin
            };
            PublishAlbumEntity adminPrivateAlbumToUpdate = new PublishAlbumEntity()
            {
                Id = Guid.NewGuid(),
                Name = "adminUpdate",
                Status = Status.Hidden,
                Publishes = new HashSet<PublishEntity>(),
                User = admin
            };

            if (_context.Albums.Count() == 0)
            {
                userPublicAlbum = _context.Albums.Add(userPublicAlbum).Entity;
                userPrivateAlbum = _context.Albums.Add(userPrivateAlbum).Entity;
                adminPublicAlbum = _context.Albums.Add(adminPublicAlbum).Entity;
                adminPrivateAlbum = _context.Albums.Add(adminPrivateAlbum).Entity;
                adminPrivateAlbumToUpdate = _context.Albums.Add(adminPrivateAlbumToUpdate).Entity;
                _context.SaveChanges();
            }

            //Tagi
            PublishTagEntity tag1 = new PublishTagEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Animal",
                Publishes = new HashSet<PublishEntity>()
            };
            PublishTagEntity tag2 = new PublishTagEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Food",
                Publishes = new HashSet<PublishEntity>()
            };
            PublishTagEntity tag3 = new PublishTagEntity()
            {
                Id = Guid.NewGuid(),
                Name = "Funny",
                Publishes = new HashSet<PublishEntity>()
            };

            if(_context.Tags.Count() == 0)
            {
                tag1 = _context.Tags.Add(tag1).Entity;
                tag2 = _context.Tags.Add(tag2).Entity;
                tag3 = _context.Tags.Add(tag3).Entity;
                _context.SaveChanges();
            }
            //Publikacje
            PublishEntity userPublicPublish = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "userPublicPublish",
                Description = "userPublicPublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Visible,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>()
                {
                    tag1,tag2
                },
                UserLikes = new HashSet<UserEntity>(),
                User = user,
                Album = userPublicAlbum
            };
            PublishEntity userPrivatePublish = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "userPrivatePublish",
                Description = "userPrivatePublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>(),
                UserLikes = new HashSet<UserEntity>(),
                User = user,
                Album = userPrivateAlbum
            };
            PublishEntity userPrivatePublish2 = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "userPublicPublish2",
                Description = "userPrivatePublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>(),
                UserLikes = new HashSet<UserEntity>(),
                User = user,
                Album = userPublicAlbum
            };
            PublishEntity adminPublicPublish = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "adminPublicPublish",
                Description = "adminPublicPublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Visible,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag3 },
                UserLikes = new HashSet<UserEntity>(),
                User = admin,
                Album = adminPublicAlbum
            };
            PublishEntity adminPrivatePublish = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "adminPrivatePublish",
                Description = "adminPrivatePublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag2},
                UserLikes = new HashSet<UserEntity>(),
                User = admin,
                Album = adminPrivateAlbum
            };
            PublishEntity adminPrivatePublish2 = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "adminPrivatePublish2",
                Description = "adminPublicPublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag2 },
                UserLikes = new HashSet<UserEntity>(),
                User = admin,
                Album = adminPublicAlbum
            };
            PublishEntity userPublicPublishNoAlbum = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "userPublicPublish",
                Description = "userPublicPublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Visible,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag1 },
                UserLikes = new HashSet<UserEntity>(),
                User = user
            };
            PublishEntity userPrivatePublishNoAlbum = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "userPrivatePublish",
                Description = "userPrivatePublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag3 },
                UserLikes = new HashSet<UserEntity>(),
                User = user
            };
            PublishEntity adminPublicPublishNoAlbum = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "adminPublicPublish",
                Description = "adminPublicPublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Visible,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>(),
                UserLikes = new HashSet<UserEntity>(),
                User = admin
            };
            PublishEntity adminPrivatePublishNoAlbum = new PublishEntity()
            {
                Id = Guid.NewGuid(),
                ImageName = "adminPrivatePublish",
                Description = "adminPrivatePublishDescription",
                FileName = Guid.NewGuid().ToString(),
                UploadDate = DateTime.Now,
                Camera = Cameras.Canon,
                Status = Status.Hidden,
                Comments = new HashSet<CommentEntity>(),
                PublishTags = new HashSet<PublishTagEntity>() { tag1,tag3 },
                UserLikes = new HashSet<UserEntity>(),
                User = admin
            };

            if(_context.Publishes.Count() == 0)
            {
                userPublicPublish = _context.Publishes.Add(userPublicPublish).Entity;
                userPrivatePublish = _context.Publishes.Add(userPrivatePublish).Entity;
                userPrivatePublish2 = _context.Publishes.Add(userPrivatePublish2).Entity;
                adminPublicPublish = _context.Publishes.Add(adminPublicPublish).Entity;
                adminPrivatePublish = _context.Publishes.Add(adminPrivatePublish).Entity;
                adminPrivatePublish2 = _context.Publishes.Add(adminPrivatePublish2).Entity;
                userPublicPublishNoAlbum = _context.Publishes.Add(userPublicPublishNoAlbum).Entity;
                userPrivatePublishNoAlbum = _context.Publishes.Add(userPrivatePublishNoAlbum).Entity;
                adminPublicPublishNoAlbum = _context.Publishes.Add(adminPublicPublishNoAlbum).Entity;
                adminPrivatePublishNoAlbum = _context.Publishes.Add(adminPrivatePublishNoAlbum).Entity;
                _context.SaveChanges();
            }

            //Komentarze
            CommentEntity UserComment1 = new CommentEntity()
            {
                Id = Guid.NewGuid(),
                CommentContent = "Nice photo!",
                IsEdited = false,
                User = user,
                Publish = adminPublicPublish
            };
            CommentEntity UserComment2 = new CommentEntity()
            {
                Id = Guid.NewGuid(),
                CommentContent = "Haha",
                IsEdited = false,
                User = user,
                Publish = adminPublicPublishNoAlbum
            };
            CommentEntity AdminComment3 = new CommentEntity()
            {
                Id = Guid.NewGuid(),
                CommentContent = "XDD",
                IsEdited = false,
                User = admin,
                Publish = userPublicPublishNoAlbum
            };
            CommentEntity AdminComment4 = new CommentEntity()
            {
                Id = Guid.NewGuid(),
                CommentContent = "Awesome",
                IsEdited = false,
                User = admin,
                Publish = userPublicPublish
            };

            if(_context.Comments.Count() == 0)
            {
                UserComment1 = _context.Comments.Add(UserComment1).Entity;
                UserComment2 = _context.Comments.Add(UserComment2).Entity;
                AdminComment3 = _context.Comments.Add(AdminComment3).Entity;
                AdminComment4 = _context.Comments.Add(AdminComment4).Entity;
                _context.SaveChanges();
            }
        }

        [Fact]
        public async void LoginTest()
        {
            var json = JsonConvert.SerializeObject(_adminLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Login/Login"),
                Method = HttpMethod.Post,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Theory]
        [InlineData("",1,10,5)]
        [InlineData("Admin",1,10,3)]
        [InlineData("User",1,10,2)]
        public async void GetManyAlbumsAsAdminTest(string? user,int page,int take,int expectedCount)
        {
            string token = await GetToken(_adminLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Albums?userLogin={user}&page={page}&take={take}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }           
            };
            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<PublishAlbumOutputDto>>(json);


            Assert.NotNull(list);
            Assert.Equal(expectedCount, list.Count);
        }
        [Theory]
        [InlineData("", 1, 10, 3)]
        [InlineData("Admin", 1, 10, 1)]
        [InlineData("User", 1, 10, 2)]
        public async void GetManyAlbumsAsUserTest(string? user, int page, int take, int expectedCount)
        {
            string token = await GetToken(_userLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Albums?userLogin={user}&page={page}&take={take}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<PublishAlbumOutputDto>>(json);

            Assert.NotNull(list);
            Assert.Equal(expectedCount, list.Count);
        }
        [Theory]
        [InlineData("Admin", "adminPublicAlbum", true)]
        [InlineData("Admin", "adminPrivateAlbum", true)]
        [InlineData("User", "userPublicAlbum", true)]
        [InlineData("User", "userPrivateAlbum", true)]
        public async void GetOneAlbumAsAdmin(string userName,string albumName,bool acces)
        {
            string token = await GetToken(_adminLoginData);          
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Albums/{userName}/{albumName}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            if (acces)
            {
                var json = await response.Content.ReadAsStringAsync();
                var album = JsonConvert.DeserializeObject<PublishAlbumOutputDto>(json);
                Assert.NotNull(album);
            }
            else
            {
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }
        }
        [Theory]
        [InlineData("Admin", "adminPublicAlbum", true)]
        [InlineData("Admin", "adminPrivateAlbum", false)]
        [InlineData("User", "userPublicAlbum", true)]
        [InlineData("User", "userPrivateAlbum", true)]
        public async void GetOneAlbumAsUserTest(string userName, string albumName, bool acces)
        {
            string token = await GetToken(_userLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Albums/{userName}/{albumName}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            if (acces)
            {
                var json = await response.Content.ReadAsStringAsync();
                var album = JsonConvert.DeserializeObject<PublishAlbumOutputDto>(json);
                Assert.NotNull(album);
            }
            else
            {
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }
        }
        [Fact]
        public async void CreateAlbumTest()
        {
            string token = await GetToken(_adminLoginData);
            PublishAlbumInputDto input = new PublishAlbumInputDto()
            {
                Name = "CreateTest",
                Status = Status.Visible
            };
            var json = JsonConvert.SerializeObject(input);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Albums"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
                
            };
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            HttpRequestMessage request2 = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Albums/CreateTest"),
                Method = HttpMethod.Delete,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            await _client.SendAsync(request2);
        }
        [Fact]
        public async void UpdateAlbumTest()
        {
            string token = await GetToken(_adminLoginData);
            PublishAlbumInputDto input = new PublishAlbumInputDto()
            {
                Name = "adminUpdate",
                Status = Status.Visible
            };
            var json = JsonConvert.SerializeObject(input);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Albums/adminUpdate"),
                Method = HttpMethod.Patch,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
            };
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async void DeleteAlbumTest()
        {
            string token = await GetToken(_adminLoginData);
            PublishAlbumInputDto input = new PublishAlbumInputDto()
            {
                Name = "DeleteTest",
                Status = Status.Visible
            };
            var json = JsonConvert.SerializeObject(input);
            HttpRequestMessage request2 = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Albums"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json"),

            };
            await _client.SendAsync(request2);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Albums/DeleteTest"),
                Method = HttpMethod.Delete,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(null,null, 10)]
        [InlineData("Admin", "adminPublicAlbum", 2)]
        [InlineData("User", null, 2)]
        public async void GetManyPublishesAsAdminTest(string? user,string? albumName, int expectedCount)
        {
            string token = await GetToken(_adminLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Publishes/?userLogin={user}&albumName={albumName}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<PublishOutputDto>>(json);


            Assert.NotNull(list);
            Assert.Equal(expectedCount, list.Count);
        }
        [Theory]
        [InlineData(null, null, 7)]
        [InlineData("Admin",null, 1)]
        [InlineData("User", "userPublicAlbum", 2)]
        public async void GetManyPublishesAsUserTest(string? user, string? albumName, int expectedCount)
        {
            string token = await GetToken(_userLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Publishes/?userLogin={user}&albumName={albumName}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<PublishOutputDto>>(json);

            Assert.NotNull(list);
            Assert.Equal(expectedCount, list.Count);
        }
        [Theory]
        [InlineData("Admin", "adminPublicAlbum", "adminPublicPublish", true)]
        [InlineData("Admin", "adminPrivateAlbum", "adminPrivatePublish", true)]
        [InlineData("User", "userPublicAlbum", "userPublicPublish", true)]
        [InlineData("User", "userPrivateAlbum", "userPrivatePublish", true)]
        public async void GetOnePublishAsAdmin(string userName, string? albumName,string imageName, bool acces)
        {
            string token = await GetToken(_adminLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Publishes/{userName}/{imageName}?albumName={albumName}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            if (acces)
            {
                var json = await response.Content.ReadAsStringAsync();
                var publish = JsonConvert.DeserializeObject<PublishOutputDto>(json);
                Assert.NotNull(publish);
            }
            else
            {
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }
        }
        [Theory]
        [InlineData("Admin", "adminPublicAlbum", "adminPublicPublish", true)]
        [InlineData("Admin", "adminPrivateAlbum", "adminPrivatePublish", false)]
        [InlineData("User", "userPublicAlbum", "userPublicPublish", true)]
        [InlineData("User", "userPrivateAlbum", "userPrivatePublish", true)]
        public async void GetOnePublishAsUser(string userName, string? albumName, string imageName, bool acces)
        {
            string token = await GetToken(_userLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Publishes/{userName}/{imageName}?albumName={albumName}"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            if (acces)
            {
                var json = await response.Content.ReadAsStringAsync();
                var publish = JsonConvert.DeserializeObject<PublishOutputDto>(json);
                Assert.NotNull(publish);
            }
            else
            {
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }
        }

        [Fact]
        public async void GetManyTagsTest()
        {
            string token = await GetToken(_adminLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Tags"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<PublishTagOutputDto>>(json);


            Assert.NotNull(list);
            Assert.Equal(3, list.Count);
        }
        [Fact]
        public async void GetOneTag()
        {
            string token = await GetToken(_adminLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Tags/Food"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var tag = JsonConvert.DeserializeObject<PublishTagOutputDto>(json);


            Assert.NotNull(tag);
            Assert.Equal(tag.Name,"Food");
        }
        [Fact]
        public async void CreateTagTest()
        {
            string token = await GetToken(_adminLoginData);
            PublishTagInputDto input = new PublishTagInputDto()
            {
                TagName = "CreateTest",
            };
            var json = JsonConvert.SerializeObject(input);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Tags"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json"),

            };
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            HttpRequestMessage request2 = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Tags/CreateTest"),
                Method = HttpMethod.Delete,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            await _client.SendAsync(request2);
        }
        [Fact]
        public async void UpdateTagTest()
        {
            string token = await GetToken(_adminLoginData);
            PublishTagInputDto input = new PublishTagInputDto()
            {
                TagName = "Dog",
            };
            var json = JsonConvert.SerializeObject(input);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Tags/Animal"),
                Method = HttpMethod.Patch,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
            };
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async void DeleteTagTest()
        {
            string token = await GetToken(_adminLoginData);
            PublishTagInputDto input = new PublishTagInputDto()
            {
                TagName = "DeleteTest",
            };
            var json = JsonConvert.SerializeObject(input);
            HttpRequestMessage request2 = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Tags"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json"),

            };
            await _client.SendAsync(request2);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Tags/DeleteTest"),
                Method = HttpMethod.Delete,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async void GetManyCommentsTest()
        {
            string token = await GetToken(_adminLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Comments"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<CommentOutputDto>>(json);


            Assert.NotNull(list);
        }
        [Fact]
        public async void GetManyCommentsForPublishTest()
        {
            string token = await GetToken(_adminLoginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Comments/User/userPublicPublish"),
                Method = HttpMethod.Get,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                }
            };
            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<CommentOutputDto>>(json);


            Assert.NotNull(list);
            Assert.Equal(1, list.Count);
            Assert.Equal("XDD", list[0].CommentContent);
        }
        [Fact]
        public async void CreateCommentTest()
        {
            string token = await GetToken(_adminLoginData);
            CommentInputDto input = new CommentInputDto()
            {
                AlbumName = "userPublicAlbum",
                CommentContent = "Test",
                PublishName = "userPublicPublish",
                UserName = "User"
            };
            var json = JsonConvert.SerializeObject(input);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@$"{uri}Comments"),
                Method = HttpMethod.Post,
                Headers =
                {
                    {HttpRequestHeader.Authorization.ToString(), $"Bearer {token}"}
                },
                Content = new StringContent(json, Encoding.UTF8, "application/json"),

            };
            var response = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }


        private async Task<string> GetToken(UserLogin loginData)
        {
            var json = JsonConvert.SerializeObject(loginData);
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(@"https://localhost:7068/api/v1/Login/Login"),
                Method = HttpMethod.Post,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }       
}
