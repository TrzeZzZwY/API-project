using FakeItEasy;
using Infrastructure.EF.Entities;
using Infrastructure.EF.Services.Authorized;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Dto.Input;
using WebApi.Dto.Mappers;

namespace UnitTest
{
    public class TestApiPublishController
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly EfPublishServiceAuthorized _publishService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TestApiPublishController()
        {
            _userManager = A.Fake<UserManager<UserEntity>>();
            _publishService = A.Fake<EfPublishServiceAuthorized>();
            _hostEnvironment = A.Fake<IWebHostEnvironment>();
        }

    }
}
