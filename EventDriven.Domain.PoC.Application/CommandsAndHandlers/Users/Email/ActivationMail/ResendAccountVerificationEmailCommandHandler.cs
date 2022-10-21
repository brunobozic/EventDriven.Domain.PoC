﻿using AutoMapper;
using EventDriven.Domain.PoC.Application.DomainServices.EmailServices;
using EventDriven.Domain.PoC.Domain.DomainEntities.DomainExceptions;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email.ActivationMail
{
    public class
        ResendAccountVerificationEmailCommandHandler : ICommandHandler<ResendAccountVerificationEmailCommand, bool>
    {
        private readonly IEmailService _emailService;

        private readonly IMyUnitOfWork _unitOfWork;
        private readonly ITrackableRepository<User> _userRepository;
        private MyConfigurationValues _appSettings;
        private IMapper _mapper;
        private ITrackableRepository<Role> _roleRepository;

        public ResendAccountVerificationEmailCommandHandler(
            IMyUnitOfWork unitOfWork,
            ITrackableRepository<User> userRepository,
            ITrackableRepository<Role> roleRepository,
            IMapper mapper,
            IOptions<MyConfigurationValues> appSettings,
            IEmailService emailService
        )
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        /// <summary>
        ///     Will compose and send the account verification email
        /// </summary>
        public async Task<bool> Handle(ResendAccountVerificationEmailCommand command,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository
                .Queryable()
                .Where(u => u.NormalizedEmail == command.Email.Trim().ToUpper() &&
                            u.NormalizedUserName == command.UserName.Trim().ToUpper())
                .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new DomainException("Application user not found by requested Id of: [ " + command.UserId +
                                          " ], e-mail [ " + command.Email + " ], username: [ " + command.UserName +
                                          " ]");
            string message;
            var origin = command.Origin;

            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/user/verify-email?token={user.EmailVerificationToken}";
                message = $@"<p>Please click the below link to activate your account:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message =
                    $@"<p>Please use the below token to activate your account with the <code>/user/verify-email</code> api route:</p>
                             <p><code>{user.EmailVerificationToken}</code></p>";
            }

            var emailSubject = "Sign-up Verification API - Verify Account";
            var completeEmailMessageBody = $@"<h4>Verify Account</h4>
                         <p>Thanks for registering!</p>
                         {message}";
            var emailFrom = "admin.poc@gmail.com";

            try
            {
                _emailService.Send(user.Email, emailSubject, completeEmailMessageBody, emailFrom);

                user.SetAccountActivationMailResent();
            }
            catch (Exception e)
            {
                Log.Fatal("Application exception for requested Id of: [ " + command.UserId +
                          " ], e-mail [ " + command.Email + " ], username: [ " + command.UserName + " ]", e);
                user.AccountActivationMailNotSent(e.Message);
                return false;
            }

            return true;
        }
    }
}