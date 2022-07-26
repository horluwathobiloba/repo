using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Application.Common.Interfaces
{
   public interface IPasswordService
    {
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);

    }
}
