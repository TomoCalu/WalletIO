﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WalletIO.Entities;
using WalletIO.Helpers;

namespace WalletIO.Service
{
    public interface ICategoryService
    {
        Category GetById(int idCategory);
    }

    public class CategoryService : ICategoryService
    {
        private DataContext _context;

        public CategoryService(DataContext context)
        {
            _context = context;
        }

        public Category GetById(int idCategory)
        {
            return _context.Categories.Find(idCategory);            
        }
    }

}
