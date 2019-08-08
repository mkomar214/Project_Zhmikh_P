using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Users.Models;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Users.Infrastructure
{
    public class CustomUserValidator : IIdentityValidator<AppUser>
    {
        private readonly AppUserManager _manager;

        public CustomUserValidator(AppUserManager manager)
        {
            _manager = manager;
        }

        public async Task<IdentityResult> ValidateAsync(AppUser item)
        {
            List<string> errors = new List<string>();

            if (String.IsNullOrEmpty(item.UserName.Trim()))
                errors.Add("Вы указали пустое имя.");

            if (_manager != null)
            {
                var otherAccountName = await _manager.FindByNameAsync(item.UserName);
                var otherAccountEmail = await _manager.FindByEmailAsync(item.Email);

                if (otherAccountName != null && otherAccountName.Id != item.Id)
                    errors.Add($"Имя {item.UserName} уже используется");

                if (otherAccountEmail != null && otherAccountEmail.Id != item.Id)
                    errors.Add($"Почта {item.Email} уже используется");
            }

            string userNamePattern = @"^[a-zA-Z0-9а-яА-Я]+$";

            if (!Regex.IsMatch(item.UserName, userNamePattern))
                errors.Add("В имени разрешается указывать буквы английского или русского языков, и цифры");

            if (errors.Count > 0)
                return IdentityResult.Failed(errors.ToArray());

            return IdentityResult.Success;
        }
    }
}