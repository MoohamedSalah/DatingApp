using System;
using System.Threading.Tasks;
using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _contect;
        public AuthRepository(DataContext contect)
        {
            _contect = contect;

        }
        public async Task<Users> Login(string username, string Password)
        {
            var user = await _contect.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
                return null;

            if (!VerifyPasswordHash(Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var Computehash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < Computehash.Length; i++)
                {
                    if (Computehash[i] != passwordHash[i]) return false;

                }
            }
            return true;
        }

        public async Task<Users> Register(Users user, string Password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePaswordHash(Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _contect.Users.AddAsync(user);
            await _contect.SaveChangesAsync();
            return user;
        }

        private void CreatePaswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _contect.Users.AnyAsync(x => x.Username == username))
                return true;

            return false;

        }
    }
}