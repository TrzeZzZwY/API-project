﻿using AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Interfaces.Services
{
    public interface IPublishService
    {
        public IEnumerable<Publish> GetAll();
        public IEnumerable<Publish> GetAllFor(Guid userId); 
        public IEnumerable<Publish> GetAllFor(string userLogin); //TODO-Jasiek: wszystkie zdjęcia w danym folderze
        
        //wariant 1 - prymitywne, mniej dokładniejsze:
        //var service =  publishService
        //wariant 2:
        //var service2 = publishServiceForUser
        //
        //getAllFor(token,user){
        //  if(admin lub właściciel)
        //  {
        //      return service.getLLFOR(User);
        //  }
        //  else
        //  {
        //      wariant 1: return service.getAllFor(user).Select(public)
        //      wariant 2: return service2.GetAllFor(user)
        //  }
        //}

        public Publish GetByOne(Guid publishId);
        public Publish GetByOne(Guid albumId, string imageName);
        public Publish GetByOne(Guid userID, string? albumName, string imageName);
        public Publish GetByOne(string userLogin, string? albumName, string imageName);

        public Publish Create(Guid userID, string? albumName, Publish publish);
        public Publish Create(string userLogin, string? albumName, Publish publish);

        public Publish Delete(Guid userId, string? albumName, string imageName);
        public Publish Delete(string userLogin, string? albumName, string imageName);
        public Publish Delete(Guid publishId);

        public Publish Update(Guid publishId, Publish publish);
 
        public bool Move(Guid userId, string? targetAlbumName, Guid publishId);
        public bool Move(string userLogin, string? targetAlbumName, Guid publishId);
    }
}
