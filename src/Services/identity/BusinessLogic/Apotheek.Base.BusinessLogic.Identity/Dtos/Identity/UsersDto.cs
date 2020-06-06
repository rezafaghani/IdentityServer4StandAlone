﻿using System.Collections.Generic;
using System.Linq;
using Apotheek.Base.BusinessLogic.Identity.Dtos.Identity.Interfaces;

namespace Apotheek.Base.BusinessLogic.Identity.Dtos.Identity
{
    public class UsersDto<TUserDto, TUserDtoKey> : IUsersDto where TUserDto : UserDto<TUserDtoKey>
    {
        public UsersDto()
        {
            Users = new List<TUserDto>();
        }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public List<TUserDto> Users { get; set; }

        List<IUserDto> IUsersDto.Users => Users.Cast<IUserDto>().ToList();
    }
}