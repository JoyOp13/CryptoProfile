using AutoMapper;
using CryptoCurrency.Application.DTO.AuthDTO;
using CryptoCurrency.Application.Interface;
using CryptoCurrency.Domain.Models;
using CryptoCurrency.Infrastructure.Data;
using OtpNet;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace CryptoCurrency.Infrastructure.Services
{
    public class AuthService : IAuthInterface
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly JwtService jwt;
        public AuthService(ApplicationDbContext db, IMapper mapper, JwtService jwt  )
        {
            this.db = db;
            this.mapper = mapper;
            this.jwt = jwt;
        }
        public void Register(RegisterDTO dto)
        {
            var exists = db.Users.Any(x => x.Email == dto.Email);
            if (exists)
                throw new Exception("User already exists");
            var user = new Users
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PassWord = BCrypt.Net.BCrypt.HashPassword(dto.PassWord),
                Role = "User",
                CreatedAt = DateTime.Now
            };

            db.Users.Add(user);
            db.SaveChanges();

            var wallet = new Wallet
            {
                UserId = user.UserId,
                Balance = 0,
                CreatedAt = DateTime.Now,
                CreatedBy = user.UserName
            };

            db.Wallet.Add(wallet);
            db.SaveChanges();
        }
        public LoginResDTO Login(LoginReqDTO dto)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.PassWord, user.PassWord))
                throw new Exception("Invalid email or password");
            bool Is2FASetup = !string.IsNullOrEmpty(user.AuthenticatorKey);
            return new LoginResDTO
            {
                Email = user.Email,
                Role = user.Role,
                IsOtpRequired = Is2FASetup,
                Message = Is2FASetup ? "OTP required to login": "Please setup 2FA first"
            };
        }
        public string GenerateQRCode(string email)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
                throw new Exception("User not found");
            if (user.Is2FAEnabled)
                throw new Exception("2FA already enabled");
            if (string.IsNullOrEmpty(user.AuthenticatorKey))
            {
                var bytes = KeyGeneration.GenerateRandomKey(20);
                user.AuthenticatorKey = Base32Encoding.ToString(bytes);

                db.SaveChanges();
            }
            string appName = "CryptoApp";

            var url = $"otpauth://totp/{appName}:{email}?secret={user.AuthenticatorKey}&issuer={appName}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new Base64QRCode(qrData);

            return qrCode.GetGraphic(20);
        }
        public TokenDTO VerifyOtp(VerifyOTPDTO dto)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == dto.Email);

            if (user == null)
                throw new Exception("User Not Found");

            if (string.IsNullOrEmpty(user.AuthenticatorKey))
                throw new Exception("2FA Not Setup. Please Scan The QR Code first");

            var keyBytes = Base32Encoding.ToBytes(user.AuthenticatorKey);
            var totp = new Totp(keyBytes);

            bool isValid = totp.VerifyTotp(dto.Otp, out _, new VerificationWindow(1, 1));

            if (!isValid)
                throw new Exception("Invalid OTP");
            if (!user.Is2FAEnabled)
            {
                user.Is2FAEnabled = true;
                db.SaveChanges();
            }
            var token = jwt.GenerateToken(user.Email, user.Role, user.UserName);

            return new TokenDTO
            {
                Token = token
            };
        }
    }
}
