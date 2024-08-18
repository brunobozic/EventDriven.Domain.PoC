using System;

namespace IdentityService.Application.CQRSBoilerplate;

    public class InboxMessageDto
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }
    }
