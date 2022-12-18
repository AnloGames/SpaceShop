﻿using Microsoft.AspNetCore.Identity;

namespace SpaceShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
        //Если хотим добавить в таблицу столбец, наследуем класс от модели таблицы
}
